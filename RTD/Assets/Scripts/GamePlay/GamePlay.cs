using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ResponseMessage;


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
        BreakTime,
        GameEnd

    }
    STATE state = STATE.StandBy;

    public GameObject IntroPanel = null;
    public GameObject RoundText = null;
    public GameObject BreakTimeText = null;
    public GameObject GameLifeText = null;
    public GameObject GameEndText = null;
    public GameObject LevelUpActiveButton = null;

    WaveSpawner WaveSpawner;
    Coroutine MoveCoroutine;


    const int GameStartBreakTime = 15;
    int GameLife = 10;

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
    public struct Round
    {
        public string addr;
        public int enemyCount;
        public int breakTime;

        public Round(string addr, int enemyCount, int breakTime)
        {
            this.addr = addr;
            this.enemyCount = enemyCount;
            this.breakTime = breakTime;
        }
    }

    private void Awake()
    {
        RoundList.Add(new Round("Character/Enemy/TurtleShell", 11, 25));
        RoundList.Add(new Round("Character/Enemy/TurtleShell", 5, 25));
        RoundList.Add(new Round("Character/Enemy/TurtleShell", 5, 25));
        RoundList.Add(new Round("Character/Enemy/Golem", 5, 25));
        RoundList.Add(new Round("Character/Enemy/Slime", 5, 25));
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
                GetComponent<BossRoundManager>().State = BossRoundManager.STATE.BreakTime;

                break;
            case STATE.RoundStart:
                // Start Attack
                GetComponent<CharacterInfoManager>().CharacterAttackFlagOn();

                // Restrict
                GetComponent<PickController>().SetDisable();
                LevelUpActiveButton.GetComponent<BtnLevelUpActive>().SetDeactive();

                // Update Round
                CurrentRound++;
                GetComponent<BossRoundManager>().Round = CurrentRound;
                GetComponent<BossRoundManager>().State = BossRoundManager.STATE.RoundStart;

                // Update Info
                BreakTimeText.SetActive(false);
                SetRoundText(CurrentRound);

                // Enemy Spawn
                MoveCoroutine = StartCoroutine(
                    WaveSpawner.StartSpawnWaves(
                        RoundList[CurrentRound - 1].addr,
                        RoundList[CurrentRound - 1].enemyCount,   // Enemy Count
                        ()=> {ChangeState(STATE.SpawnEnd); }    // If Spawn Done
                        )
                    );

                break;
            case STATE.SpawnEnd:
                break;
            case STATE.BreakTime:
                // Stop Attack
                GetComponent<CharacterInfoManager>().CharacterAttackFlagOff();

                // Set Boss Round
                GetComponent<BossRoundManager>().State = BossRoundManager.STATE.BreakTime;

                // Receive Money
                if (GetComponent<MoneyManager>().CalculateMoney(MoneyManager.ACTION.Receive, 500, response, "Refresh Card"))
                {
                    // Succeed
                }
                else
                {
                    // Fail
                    Debug.Log(ResponseMessage.Trade.Receive(response));
                }


                // GameOver?
                if (CurrentRound >= RoundList.Count || GetComponent<BossRoundManager>().State == BossRoundManager.STATE.GameOver)
                {
                    ChangeState(STATE.GameEnd); 
                    GetComponent<BossRoundManager>().State = BossRoundManager.STATE.GameOver;
                }
                // Next Round
                else
                {
                    GetComponent<PickController>().SetNormal();
                    LevelUpActiveButton.GetComponent<BtnLevelUpActive>().SetActive();
                    BreakTimeText.SetActive(true);
                    StartCoroutine(CountDownTime(RoundList[CurrentRound - 1].breakTime, () => { ChangeState(STATE.RoundStart); }));
                }

                break;
            case STATE.GameEnd:
                if(GameLife <= 0)
                {
                    GameEndText.GetComponent<TMPro.TextMeshProUGUI>().text = "GAMEOVER";
                }
                else if(GetComponent<BossRoundManager>().State == BossRoundManager.STATE.GameOver)
                {
                    GameEndText.GetComponent<TMPro.TextMeshProUGUI>().text = "Boss is alive";
                }
                else
                {
                    GameEndText.GetComponent<TMPro.TextMeshProUGUI>().text = "CLEAR";
                }
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

}

