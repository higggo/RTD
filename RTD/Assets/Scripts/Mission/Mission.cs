using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterKit;
using UnityEngine.Events;

public abstract class Mission
{
    public List<MissionCategory> Categories = new List<MissionCategory>();
    public bool Succeed = false;    // 미션 성공시 true
    public bool Finished = false;   // 보상제공 후 true
    public bool isPicking = false;  // 진행중인 미션이면 true
    protected uint reward = 0;
    
    public virtual void Init(GameObject GameManager)
    {
        foreach (MissionCategory category in Categories)
        {
            category.Init(GameManager);
        }
        UpdateMessage();
        UpdateVerify();
    }
    public virtual void UpdateVerify()
    {
        bool Succeed = true;
        foreach (MissionCategory category in Categories)
        {
            category.Verify();
            if (!category.Succeed)
            {
                Succeed = false;
            }
        }
        this.Succeed = Succeed;
    }
    public string UpdateMessage()
    {
        string str = "";
        for(int i = 0; i < Categories.Count; i++)
        {
            str += Categories[i].Message();
            if(i != Categories.Count-1)
                str += ", ";

        }
        if (Finished)
            str += " 완료";
        return str;
    }
    public uint Reward()
    {
        return reward;
    }
    public void MissionFinished()
    {
        Finished = true;
    }
}
public class MissionA : Mission
{
    //MissionKillMonster kill = new MissionKillMonster();
    MissionGetCharacter chracter = new MissionGetCharacter();

    public MissionA()
    {
        reward = 500;

        //kill.PushMonsters(GRADE.NORMAL, UNION.UNION001, 3, "노멀 마법사 3마리 잡기");
        //kill.PushMonsters(GRADE.MAGIC, UNION.UNION002, 1, "매직 전사 1마리 잡기");
        chracter.PushCharacters(GRADE.NORMAL, UNION.MAGE, 3, "N법사");
        chracter.PushCharacters(GRADE.MAGIC, UNION.WARRIOR, 1, "M전사");
        chracter.PushCharacters(GRADE.MAGIC, UNION.ARCHER, 1, "M아처");
        //Categories.Add(kill);
        Categories.Add(chracter);
    }
}

public class MissionB : Mission
{
    MissionKillMonster kill = new MissionKillMonster();

    public MissionB()
    {
        reward = 500;

        kill.PushMonsters(SIMBOL_ARMOR.HEAVY, 3, "해비타입 몬스터 ");
        Categories.Add(kill);
    }
}
