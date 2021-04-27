using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;
using UnityEngine.Events;

public abstract class MissionCategory
{
    public bool Succeed = false;
    public virtual void Init(GameObject GameManager)
    {

    }
    public abstract bool Verify();
    public abstract string Message();
}
public class MissionKillMonster : MissionCategory
{
    public struct Kit
    {
        public CharacterKit.GRADE grade;
        public CharacterKit.UNION union;
        public int num;
        public string msg;

        public Kit(GRADE grade, UNION union, int num, string msg)
        {
            this.grade = grade;
            this.union = union;
            this.num = num;
            this.msg = msg;
        }
    }

    public List<Kit> KitList = new List<Kit>();
    List<int> CheckCnt = new List<int>();

    GamePlay GamePlay = null;
    WaveSpawner WaveSpawner = null;

    GameObject LastObj;

    public override void Init(GameObject GameManager)
    {
        base.Init(GameManager);
        GamePlay = GameManager.GetComponent<GamePlay>();
        WaveSpawner = GameManager.GetComponent<WaveSpawner>();
        WaveSpawner.SpawnDelegate += SpawnCharacter;
    }

    public void PushMonsters(CharacterKit.GRADE grade, CharacterKit.UNION union, int num, string msg)
    {
        KitList.Add(new Kit(grade, union, num, msg));
        CheckCnt.Add(0);
    }

    public override bool Verify()
    {
        bool Succeed = true;
        for (int i = 0; i < KitList.Count; i++)
        {
            if (KitList[i].num > CheckCnt[i])
            {
                Succeed = false;
            }
        }
        this.Succeed = Succeed;

        return this.Succeed;
    }
    public override string Message()
    {
        string str = "처치미션\n";
        for (int i = 0; i < KitList.Count; i++)
        {
            str += KitList[i].msg + CheckCnt[i] + "/" + KitList[i].num.ToString() + " ";
        }
        return str;
    }
    public void CountDead()
    {
        for (int i = 0; i < KitList.Count; i++)
        {
            if (KitList[i].grade == LastObj.GetComponent<CharController>().statInfo.grade
                && KitList[i].union == LastObj.GetComponent<CharController>().statInfo.union)
            {
                CheckCnt[i]++;
            }
        }
    }
    public void SpawnCharacter(GameObject obj)
    {
        obj.GetComponent<Damageable>().onDeadDel += CountDead;
        LastObj = obj;
    }
}

public class MissionGetCharacter : MissionCategory
{
    public struct Kit
    {
        public CharacterKit.GRADE grade;
        public CharacterKit.UNION union;
        public int num;
        public string msg;

        public Kit(GRADE grade, UNION union, int num, string msg)
        {
            this.grade = grade;
            this.union = union;
            this.num = num;
            this.msg = msg;
        }
    }

    public List<Kit> KitList = new List<Kit>();
    List<int> CheckCnt = new List<int>();

    PickController PickController = null;
    CharacterInfoManager CharacterInfoManager = null;

    public override void Init(GameObject GameManager)
    {
        base.Init(GameManager);
        PickController = GameManager.GetComponent<PickController>();
        CharacterInfoManager = GameManager.GetComponent<CharacterInfoManager>();
    }

    public void PushCharacters(CharacterKit.GRADE grade, CharacterKit.UNION union, int num, string msg)
    {
        KitList.Add(new Kit(grade, union, num, msg));
        CheckCnt.Add(0);
    }

    public override bool Verify()
    {
        bool Succeed = true;
        for (int i = 0; i < KitList.Count; i++)
        {
            int cnt = CharacterInfoManager.GetAllCharacters(KitList[i].grade, KitList[i].union);
            CheckCnt[i] = cnt;
            if (cnt < KitList[i].num)
            {
                Succeed = false;
                break;
            }
        }
        this.Succeed = Succeed;

        return this.Succeed;
    }
    public override string Message()
    {
        string str = "수집미션\n";
        for (int i = 0; i < KitList.Count; i++)
        {
            str += KitList[i].msg + CheckCnt[i] + "/" + KitList[i].num.ToString() + " ";
        }
        return str;
    }

}
