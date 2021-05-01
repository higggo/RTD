using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ResponseMessage;

public class Round
{
    public enum Type
    {
        Mob,
        Boss
    }

    public string addr;
    public int enemyCount;
    public int breakTime;
    public uint reward;

    public Type battle;
    public bool clear;

    public Round(string addr, int enemyCount, int breakTime, uint reward)
    {
        this.addr = addr;
        this.enemyCount = enemyCount;
        this.breakTime = breakTime;
        this.reward = reward;
    }
}

public class MobRound : Round 
{
    MobRound(string addr, int enemyCount, int breakTime, uint reward)
        : base(addr, enemyCount, breakTime, reward)
    {
        battle = Type.Mob;
    }
}

public class GamePlay : MonoBehaviour
{
    //public UnityAction RoundStartDelegate;
    //public UnityAction RoundEndDelegate;
    public enum STATE
    {
        StandBy,
        GameStart,
        RoundStart,
        SpawnEnd,
        BossApearance,
        BossRoundStart,
        BossRoundEnd,
        BreakTime,
        GameEnd

    }
    STATE state = STATE.StandBy;

    Coroutine MoveCoroutine;

    public GameObject IntroPanel = null;
    public GameObject RoundText = null;
    public GameObject BreakTimeText = null;
    public GameObject GameLifeText = null;
    public GameObject GameEndText = null;
    public GameObject LevelUpActiveButton = null;

    public GameObject CurrentBoss;

    WaveSpawner WaveSpawner;
    Coroutine DirectionCameraFunc;

    bool bViewtoGround = true;

    // 처음 시작시간
    const int GameStartBreakTime = 10;

    // 라이프
    int GameLife = 100;

    int CurrentRound = 0;
    public List<Round> RoundList = new List<Round>();

    ResponseMessage.Trade.CODE response;


    public STATE State
    {
        get
        {
            return state;
        }
    }
    private void Awake()
    {
        // "애들 경로", 갯수, 브레이크타임, 보상금액
        // 1 ~ 4 Round
        RoundList.Add(new Round("Character/Enemy/TurtleShell", 15, 25, 350));
        RoundList.Add(new Round("Character/Enemy/RatDragon/RatDragon Blue", 15, 25, 350));
        RoundList.Add(new Round("Character/Enemy/Creatures/Creature Blue", 15, 25, 350));
        RoundList.Add(new Round("Character/Enemy/Salamander/Salamander Blue", 15, 25, 350));
        //
        // Boss Level1
        RoundList.Add(new BossRound("Character/Boss/Dragon Level1", 1, 25, 1000));
        //

        // 6 ~ 9 Round
        RoundList.Add(new Round("Character/Enemy/Slime", 20, 25, 500));
        RoundList.Add(new Round("Character/Enemy/RatDragon/RatDragon Green", 20, 25, 500));
        RoundList.Add(new Round("Character/Enemy/Creatures/Creature Green", 20, 25, 500));
        RoundList.Add(new Round("Character/Enemy/Salamander/Salamander Green", 20, 25, 500));

        //
        // Boss Level2
        RoundList.Add(new BossRound("Character/Boss/Dragon Level2", 1, 25, 1500));
        //

        // 11 ~ 14 Round
        RoundList.Add(new Round("Character/Enemy/Golem", 25, 25, 750));
        RoundList.Add(new Round("Character/Enemy/RatDragon/RatDragon Red", 25, 25, 750));
        RoundList.Add(new Round("Character/Enemy/Creatures/Creature Grey", 25, 25, 750));
        RoundList.Add(new Round("Character/Enemy/Salamander/Salamander Red", 25, 25, 750));

        //
        // Final Boss
        RoundList.Add(new BossRound("Character/Boss/Dragon Level3", 1, 25, 2000));
        //
    }

    public enum MAP
    { 
        Ground,
        Storage,
        Boss
    }

    // Start is called before the first frame update
    void Start()
    {
        // Find Reference
        if (IntroPanel == null) IntroPanel = GameObject.Find("IntroPanel");
        if (RoundText == null)  RoundText = GameObject.Find("Round");
        if (BreakTimeText == null) BreakTimeText = GameObject.Find("BreakTime");
        if (GameLifeText == null) GameLifeText = GameObject.Find("GameLife");
        if (GameEndText == null) GameEndText = GameObject.Find("GameEnd");
        if (LevelUpActiveButton == null) LevelUpActiveButton = GameObject.Find("ActiveBtn");

        GameLifeText.GetComponent<TMPro.TextMeshProUGUI>().text = "Life " + GameLife.ToString();

        WaveSpawner = GetComponent<WaveSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;
        switch(state)
        {
            case STATE.StandBy:
                IntroPanel.SetActive(true);
                break;
            case STATE.GameStart:
                Init();
                SetRoundText(CurrentRound);
                StartCoroutine(CountDownTime(GameStartBreakTime, ()=> {ChangeState(STATE.RoundStart); RoundText.SetActive(true); }));

                break;
            case STATE.RoundStart:
                // Start Attack
                GetComponent<CharacterInfoManager>().CharacterAttackFlagOn();

                //  Skill Cool
                CharacterKit.CharUtils.SetSkillCoolDownTrigger(true);

                // Restrict
                GetComponent<PickController>().SetDisable();
                LevelUpActiveButton.GetComponent<BtnLevelUpActive>().SetDeactive();
                GameObject.Find("Canvas").transform.Find("CharacterPickerUI").gameObject.SetActive(false);

                // Update Round
                CurrentRound++;

                // Update Info
                SetRoundText(CurrentRound);

                // Enemy Spawn
                MoveCoroutine = StartCoroutine(
                    WaveSpawner.StartSpawnWaves(
                        RoundList[CurrentRound - 1].addr,
                        RoundList[CurrentRound - 1].enemyCount,
                        RoundList[CurrentRound - 1].battle == Round.Type.Boss? true : false,
                        () => { 
                            if(RoundList[CurrentRound - 1].battle == Round.Type.Boss)
                                ChangeState(STATE.BossApearance);
                            else
                                ChangeState(STATE.SpawnEnd); 
                        }
                    )
                );
                break;
            case STATE.SpawnEnd:
                break;
            case STATE.BossApearance:
                {
                    CurrentBoss = GetComponent<WaveSpawner>().EnemyPoket.GetChild(0).gameObject;
                    if (DirectionCameraFunc != null)
                        StopCoroutine(DirectionCameraFunc);
                    StartCoroutine(GetComponent<CameraManager>().LookAroundBoss(CurrentBoss.transform));
                    GetComponent<CameraManager>().BossMainCamera();
                    // Update Round
                    List<GameObject> characters = GetComponent<TileManager>().GetGroundCharacters();
                    for(int i=0; i< characters.Count; i++)
                    {
                        characters[i].transform.SetParent(GetComponent<TileManager>().GetEmptyBossTile());
                        characters[i].transform.localPosition = Vector3.zero;
                        characters[i].GetComponent<CharController>().isInField = false;
                    }
                    StartCoroutine(CountDownTime(5, () => { ChangeState(STATE.BossRoundStart); }));
                }
                break;
            case STATE.BossRoundStart:
                {
                    GetComponent<CameraManager>().BossMainCamera();
                    CurrentBoss.GetComponent<BossController>().canAction = true;
                    CurrentBoss.GetComponent<CharacterKit.Damageable>().onDeadDel += () => { RoundList[CurrentRound - 1].clear = true; Destroy(CurrentBoss); };


                    List<GameObject> characters = GetComponent<TileManager>().GetBossCharacters();
                    foreach (GameObject character in characters)
                    {
                        character.GetComponent<CharController>().InBossRoom(true);
                    }
                }
                break;
            case STATE.BossRoundEnd:
                {
                    StartCoroutine(CountDownTime(3, () => { ChangeState(STATE.BreakTime);
                        List<GameObject> characters = GetComponent<TileManager>().GetBossCharacters();
                        for (int i = 0; i < characters.Count; i++)
                        {
                            characters[i].GetComponent<CharController>().InBossRoom(false);
                            //characters[i].GetComponent<CharController>().isInBossRoom = false;
                            characters[i].transform.SetParent(GetComponent<TileManager>().GetEmptyGroundTile());
                            characters[i].transform.localPosition = Vector3.zero;

                        }
                        if (CurrentBoss != null)
                            Destroy(CurrentBoss);
                    }));
                }
                break;
            case STATE.BreakTime:
                //  Skill Cool
                CharacterKit.CharUtils.SetSkillCoolDownTrigger(false);

                // Camera Control
                GetComponent<CameraManager>().StopDirectionCamera();
                GetComponent<CameraManager>().GroundMainCamera();

                // Stop Attack
                GetComponent<CharacterInfoManager>().CharacterAttackFlagOff();

                //
                GameObject.Find("Canvas").transform.Find("CharacterPickerUI").gameObject.SetActive(true);

                // Receive Money
                // LJH : Add receiveMoney
                //uint receiveMoney;
                //if (CurrentRound < 5)
                //    receiveMoney = 350;
                //else if (CurrentRound < 10)
                //    receiveMoney = 500;
                //else
                //    receiveMoney = 750;

                GetComponent<MoneyManager>().CalculateMoney(MoneyManager.ACTION.Receive, RoundList[CurrentRound - 1].reward, response, CurrentRound + "Round Clear");

                // GameOver?
                if (CurrentRound >= RoundList.Count)
                {
                    ChangeState(STATE.GameEnd); 
                }
                // Next Round
                else
                {
                    // LJH : Free RefreshCard 추가
                    //       SelectCharacterCard.cs 수정 => private void RefreshCardFree() ==> public void RefreshCardFree()
                    GetComponent<PickController>().SetNormal();
                    GetComponent<SelectCharacterCard>().RefreshCardsFree();
                    LevelUpActiveButton.GetComponent<BtnLevelUpActive>().SetActive();
                    StartCoroutine(CountDownTime(RoundList[CurrentRound - 1].breakTime, () => {
                            ChangeState(STATE.RoundStart);
                    }));
                }

                break;
            case STATE.GameEnd:
                if(GameLife <= 0)
                {
                    GameEndText.GetComponent<TMPro.TextMeshProUGUI>().text = "GAMEOVER";
                }
                else
                {
                    GameEndText.GetComponent<TMPro.TextMeshProUGUI>().text = "CLEAR";
                }
                BreakTimeText.SetActive(false);
                GameEndText.SetActive(true);
                break;
        }    
    }
    void StateProcess()
    {
        switch (state)
        {
            case STATE.StandBy:
                if (!IntroPanel.activeSelf)
                    ChangeState(STATE.GameStart);
                break;
            case STATE.GameStart:
                break;
            case STATE.RoundStart:
                    break;
            case STATE.SpawnEnd:
                // 맵에 적군이 모두 없어질때까지 대기
                if(WaveSpawner.GetCountEnemies() <= 0)
                {
                    ChangeState(STATE.BreakTime);
                }
                break;
            case STATE.BossApearance:
                break;
            case STATE.BossRoundStart:
                {
                    if(RoundList[CurrentRound - 1].clear)
                    {
                        // Boss Clear
                        GetComponent<MoneyManager>().CalculateMoney(MoneyManager.ACTION.Receive, RoundList[CurrentRound - 1].reward, response, CurrentRound + "Round Boss Clear");
                        ChangeState(STATE.BossRoundEnd);
                    }
                    else
                    {
                        bool alldie = true;
                        List<GameObject> characters = GetComponent<TileManager>().GetBossCharacters();
                        foreach (GameObject character in characters)
                        {
                            if (!character.GetComponent<CharacterKit.Damageable>().IsDead)
                            {
                                alldie = false;
                                break;
                            }
                        }
                        if(alldie)
                        {
                            // Characters all die
                            ChangeState(STATE.BossRoundEnd);
                        }
                    }
                }
                break;
            case STATE.BossRoundEnd:
                break;
            case STATE.BreakTime:
                break;
            case STATE.GameEnd:
                if(Input.GetMouseButtonDown(0))
                {
                    IntroPanel.SetActive(true);
                    ChangeState(STATE.StandBy);
                }
                break;
        }
    }
    IEnumerator CountDownTime(int startTime, UnityAction Done)
    {
        BreakTimeText.SetActive(true);
        float curTime = startTime;
        int recordTime = startTime;
        SetBreakTimeText((int)recordTime);

        while (curTime > 0f)
        {
            curTime -= Time.deltaTime;
            if ((float)(recordTime-1) >= curTime)
            {
                recordTime = (int)curTime + 1;
                SetBreakTimeText(recordTime);
            }
            yield return null;
        }
        BreakTimeText.SetActive(false);
        Done?.Invoke();
    }
    void Init()
    {
        CurrentRound = 0;
        GameLife = 10;
        RoundText.SetActive(false);
        GameEndText.SetActive(false); 
        BreakTimeText.SetActive(true);
        GameLifeText.GetComponent<TMPro.TextMeshProUGUI>().text = "Life " + GameLife.ToString();
        if(MoveCoroutine != null)
            StopCoroutine(MoveCoroutine);
        GetComponent<TileManager>().Init();
        GetComponent<MoneyManager>().Init();
        GetComponent<WaveSpawner>().Init();
        GetComponent<SelectCharacterCard>().Init();
        GetComponent<PickController>().Init();
        GetComponent<LevelUpManager>().Init();
        GetComponent<BossRoundManager>().Init();
        LevelUpActiveButton.GetComponent<BtnLevelUpActive>().Init();
        GameObject.Find("Canvas").transform.Find("CharacterPickerUI").gameObject.SetActive(true);
    }
    void SetRoundText(int round)
    {
        RoundText.GetComponent<TMPro.TextMeshProUGUI>().text = "Round " + round;
    }
    void SetBreakTimeText(int time)
    {
        int min = 0;
        int sec = 0;
        min = time / 60;
        sec = time % 60;
        string str = "";
        if (min < 10) str = "0" + min.ToString();
        else str = min.ToString();
        str += " : ";
        if (sec < 10) str += "0" + sec.ToString();
        else str += sec.ToString();

        BreakTimeText.GetComponent<TMPro.TextMeshProUGUI>().text = str;
    }

    public void MinusLife()
    {
        GameLife--;
        if (GameLife <= 0)
        {
            GameLife = 0;
            ChangeState(STATE.GameEnd);
        }
        GameLifeText.GetComponent<TMPro.TextMeshProUGUI>().text = "Life " + GameLife.ToString();

    }

    public void GetMission()
    {
        Debug.Log("get mission !!");
        GetComponent<MissionManager>().PushNewMission();
    }
   
    public int GetRound()
    {
        return CurrentRound;
    }
}

