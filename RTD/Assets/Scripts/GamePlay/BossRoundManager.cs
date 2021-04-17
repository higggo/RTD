using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class BossRound
{
    public string addr;     // Prefab Address
    public int spawnRound;  // Spawn Round
    public int endRound;    // Boss is Alive at EndRound, GameOver
    public GameObject boss; // Instantiate obj
    public bool bossClear;  // flag Boss is Dead
    public BossRound(string addr, int spawnRound, int endRound)
    {
        this.addr = addr;
        this.spawnRound = spawnRound;
        this.endRound = endRound;
        boss = null;
        bossClear = false;
    }
}

public class BossRoundManager : MonoBehaviour
{
    public int Round { get; set; }

    List<BossRound> BossRoundList = new List<BossRound>();

    bool AllClear = true;

    public enum STATE
    {
        StandBy,
        GameStart,
        BossSpawn,
        RoundStart,
        BreakTime,
        BossClear,
        GameOver
    }
    public STATE state = STATE.StandBy;
    public STATE State
    {
        get { return state; }
        set { ChangeState(value); }
    }

    // Start is called before the first frame update
    void Start()
    {
        Round = 0;

        BossRoundList.Add(new BossRound(GetComponent<GameDB>().Boss[0], 2, 3));

        ChangeState(STATE.GameStart);
    }

    public void Init()
    {
        Round = 0;
        for (int i = 0; i < BossRoundList.Count; i++)
        {
            if (BossRoundList[i].boss != null)
            {
                Destroy(BossRoundList[i].boss);
            }
            BossRoundList[i].boss = null;
            BossRoundList[i].bossClear = false;
        }
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

        switch (state)
        {
            case STATE.StandBy:
                break;
            case STATE.GameStart:
                break;
            case STATE.BossSpawn:
                break;
            case STATE.RoundStart:
                for (int i = 0; i < BossRoundList.Count; i++)
                {
                    if(BossRoundList[i].boss != null)
                        BossRoundList[i].boss.GetComponent<BossController>().SetCanAction(true);
                }
                break;
            case STATE.BreakTime:
                for (int i = 0; i < BossRoundList.Count; i++)
                {
                    // Boss Spawn
                    if (BossRoundList[i].spawnRound == Round+1)
                    {
                        Transform spawnSpot = GameObject.Find("BossSpawn").transform;
                        BossRoundList[i].boss = Instantiate(Resources.Load(GetComponent<GameDB>().Boss[0])) as GameObject;
                        BossRoundList[i].boss.transform.SetParent(spawnSpot.parent);
                        BossRoundList[i].boss.transform.position = spawnSpot.position;
                    }

                    // Stop Boss Attack
                    if (BossRoundList[i].boss != null)
                        BossRoundList[i].boss.GetComponent<BossController>().SetCanAction(false);

                    //
                    if (BossRoundList[i].endRound == Round)
                    {
                        // GameOver
                        ChangeState(STATE.GameOver);
                    }
                }
                break;
            case STATE.BossClear:
                break;
            case STATE.GameOver:
                break;
        }
    }

     void StateProcess()
    {
        switch (state)
        {
            case STATE.StandBy:
                break;
            case STATE.GameStart:
                break;
            case STATE.BossSpawn:
                break;
            case STATE.RoundStart:
                {
                    // Boss Dead
                    for (int i = 0; i < BossRoundList.Count; i++)
                    {
                        if(BossRoundList[i].boss != null)
                        {
                            if (BossRoundList[i].boss.GetComponent<BossController>().isDead &&
                                !BossRoundList[i].bossClear)
                            {
                                // [i] Boss Clear
                                BossRoundList[i].bossClear = true;
                            }
                        }
                        if (!BossRoundList[i].bossClear)
                            AllClear = false;
                    }
                    if(AllClear)
                        ChangeState(STATE.BossClear);
                }
                break;
            case STATE.BreakTime:
                break;
            case STATE.BossClear:
                break;
            case STATE.GameOver:
                break;
        }
    }

}
