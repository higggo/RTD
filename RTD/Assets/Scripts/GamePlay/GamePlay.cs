using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ResponseMessage;


public class GamePlay : MonoBehaviour
{
    public UnityAction RoundStartDelegate;
    public UnityAction RoundEndDelegate;
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

    public GameObject IntroPanel;
    public GameObject RoundText;
    public GameObject BreakTimeText;
    public GameObject GameLifeText;
    public GameObject GameEndText;

    WaveSpawner WaveSpawner;
    Coroutine MoveCoroutine;


    const int GameStartBreakTime = 30;
    int CurBreakTime = 0;
    int GameLife = 10;

    int CurrentRound = 0;
    const int RoundMaxCount = 2;
    public Round[] RoundList = new Round[RoundMaxCount];

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
    }

    private void Awake()
    {
        RoundList[0].addr = "Character/Enemy";
        RoundList[0].enemyCount = 15;
        RoundList[0].breakTime = 15;
        RoundList[1].addr = "Character/Enemy";
        RoundList[1].enemyCount = 15;
        RoundList[1].breakTime = 15;
        //RoundList[2].addr = "Character/Enemy";
        //RoundList[2].enemyCount = 15;
        //RoundList[2].breakTime = 15;
        //RoundList[3].addr = "Character/Enemy";
        //RoundList[3].enemyCount = 15;
        //RoundList[4].breakTime = 15;
        //RoundList[4].addr = "Character/Enemy";
        //RoundList[4].enemyCount = 15;
        //RoundList[5].breakTime = 15;
        //RoundList[5].addr = "Character/Enemy";
        //RoundList[5].enemyCount = 15;
        //RoundList[6].breakTime = 15;
        //RoundList[6].addr = "Character/Enemy";
        //RoundList[6].enemyCount = 15;
        //RoundList[6].breakTime = 15;
        //RoundList[7].addr = "Character/Enemy";
        //RoundList[7].enemyCount = 15;
        //RoundList[7].breakTime = 15;
        //RoundList[8].addr = "Character/Enemy";
        //RoundList[8].enemyCount = 15;
        //RoundList[8].breakTime = 15;
        //RoundList[9].addr = "Character/Enemy";
        //RoundList[9].enemyCount = 15;
        //RoundList[9].breakTime = 15;
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
                init();
                SetRoundText(CurrentRound);
                StartCoroutine(CountDownTime(GameStartBreakTime, ()=> {ChangeState(STATE.RoundStart); RoundText.SetActive(true); }));
                break;
            case STATE.RoundStart:
                GetComponent<CharacterInfoManager>().UpdateCharacterField();

                RoundStartDelegate?.Invoke();
                BreakTimeText.SetActive(false);
                CurrentRound++;
                SetRoundText(CurrentRound);
                MoveCoroutine = StartCoroutine(WaveSpawner.StartSpawnWaves(RoundList[CurrentRound-1].enemyCount, ()=> {ChangeState(STATE.SpawnEnd);}));
                break;
            case STATE.SpawnEnd:
                break;
            case STATE.BreakTime:
                if (GetComponent<MoneyManager>().CalculateMoney(MoneyManager.ACTION.Receive, 500, response, "Refresh Card"))
                {

                }
                else
                {
                    Debug.Log(ResponseMessage.Trade.Receive(response));
                }

                if (CurrentRound >= RoundList.Length)
                {
                    ChangeState(STATE.GameEnd);
                }
                else
                {
                    RoundEndDelegate?.Invoke();
                    BreakTimeText.SetActive(true);
                    StartCoroutine(CountDownTime(RoundList[CurrentRound - 1].breakTime, () => { ChangeState(STATE.RoundStart); }));
                }
                break;
            case STATE.GameEnd:
                if(GameLife <= 0)
                {
                    GameEndText.GetComponent<TMPro.TextMeshProUGUI>().text = "Game Over";
                }
                else
                {
                    GameEndText.GetComponent<TMPro.TextMeshProUGUI>().text = "Clear";
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
    void init()
    {
        CurrentRound = 0;
        CurBreakTime = 0;
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
        GetComponent<MoveCharacterUI>().Init();
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

