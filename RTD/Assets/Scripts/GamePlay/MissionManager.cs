using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterKit;
using UnityEngine.Events;


/**
 * 미션
 * 1. 수집
 * Ex)N법사7
 * Ex)M법사3 R법사3
 * 
 * 2. 몬스터사냥
 * Ex)헤비유닛 10마리 처치
 * Ex)몬스터 15마리 잡기
 * 
 * 
 * 
 * 
 * */


public class MissionManager : MonoBehaviour
{
    public static List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }

        return _list;
    }

    public List<Mission> MissionList = new List<Mission>();
    public List<Mission> CurrentMissions = new List<Mission>();
    ResponseMessage.Trade.CODE response;
    public GameObject MissionsVertical = null;

    private void Start()
    {
        if(MissionsVertical == null) MissionsVertical = GameObject.Find("Missions");
        
        MissionList.Add(new MissionA());
        MissionList.Add(new MissionB());

        foreach (Mission mission in MissionList)
        {
            mission.Init(gameObject);
        }
        // shuffle
        MissionList = Shuffle<Mission>(MissionList);
    }

    private void Update()
    {
        if(MissionsVertical.transform.childCount > 0)
        {
            foreach (Transform info in MissionsVertical.transform)
            {
                Mission mission = info.GetChild(0).GetComponent<MissionDeleteButton>().LinkedObj;
                mission.UpdateVerify();
                if (mission.Succeed && !mission.Finished)
                {
                    MissionReward(mission.Reward());
                    mission.MissionFinished();
                }
                info.GetComponent<TMPro.TextMeshProUGUI>().text = mission.UpdateMessage();
            }
        }
    }

    public void PushNewMission()
    {
        if(MissionsVertical.transform.childCount >= 2)
        {
            Debug.Log("미션이 가득 찼습니다.");
        }
        else
        {
            foreach(Mission mission in MissionList)
            {
                if(!mission.isPicking && !mission.Finished)
                {
                    // 미션정보, 버튼 생성
                    GameObject MissionInfoUI = Instantiate(Resources.Load("UI/MissionInfo")) as GameObject;
                    MissionInfoUI.transform.SetParent(MissionsVertical.transform);

                    mission.isPicking = true;

                    MissionInfoUI.transform.Find("Button").GetComponent<MissionDeleteButton>().LinkedObj = mission;
                    MissionInfoUI.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => {
                        mission.isPicking = false;
                        Destroy(MissionInfoUI);
                    });
                    break;
                }
            }
        }
    }

    public void MissionReward(uint gold)
    {
        GetComponent<MoneyManager>().CalculateMoney(MoneyManager.ACTION.Receive, gold, response, "Reward");
    }
    
}
