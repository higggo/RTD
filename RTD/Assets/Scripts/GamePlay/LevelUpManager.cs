using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;
public class LevelUpManager : MonoBehaviour
{
    CharacterInfoManager CharInfoManager;
    MoneyManager MoneyManager;

    public BtnLevelUpMage MAGE = null;
    public BtnLevelUpWarrior WARRIOR = null;
    public BtnLevelUpArcher ARCHER = null;

    ResponseMessage.Trade.CODE response;
    // Start is called before the first frame update
    void Start()
    {
        // Find Reference
        if (MAGE == null) MAGE = GameObject.Find("Mage").GetComponent<BtnLevelUpMage>();
        if (WARRIOR == null) WARRIOR = GameObject.Find("Warrior").GetComponent<BtnLevelUpWarrior>();
        if (ARCHER == null) ARCHER = GameObject.Find("Archer").GetComponent<BtnLevelUpArcher>();

        CharInfoManager = GetComponent<CharacterInfoManager>();
        MoneyManager = GetComponent<MoneyManager>();

        MAGE.OnclickDelegate += LevelUpMage;
        WARRIOR.OnclickDelegate += LevelUpWarrior;
        ARCHER.OnclickDelegate += LevelUpArcher;

        GameObject.Find("Storage").GetComponent<Storage>().CreateCharacterDelegate += UpdateCharacterLevel;
    }
    void LevelUpMage()
    {
        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, MAGE.Price, response, "MAGE Level Up"))
        {
            BtnLevelUpMage.Level += 1;
            CharUtils.UpdateSpecificUnion(CharacterKit.UNION.MAGE, BtnLevelUpMage.Level);
        }
        else
        {
            Debug.Log(ResponseMessage.Trade.Receive(response));
        }
    }

    void LevelUpWarrior()
    {
        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, WARRIOR.Price, response, "WARRIOR Level Up"))
        {
            BtnLevelUpWarrior.Level += 1;
            CharUtils.UpdateSpecificUnion(CharacterKit.UNION.WARRIOR, BtnLevelUpWarrior.Level);
        }
        else
        {
            Debug.Log(ResponseMessage.Trade.Receive(response));
        }
    }

    void LevelUpArcher()
    {
        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, ARCHER.Price, response, "ARCHER Level Up"))
        {
            BtnLevelUpArcher.Level += 1;
            CharUtils.UpdateSpecificUnion(CharacterKit.UNION.ARCHER, BtnLevelUpArcher.Level);
        }
        else
        {
            Debug.Log(ResponseMessage.Trade.Receive(response));
        }
    }

    public void UpdateCharacterLevel(GameObject character)
    {
        CharacterKit.UNION union = character.GetComponent<CharacterStat>().union;
        character.GetComponent<CharacterStat>().UpdateStat(GetLevel(union));
        //CharUtils.UpdateSpecificUnion(union, GetLevel(union));
    }

    int GetLevel(CharacterKit.UNION union)
    {
        int level = 0;
        switch(union)
        {
            case CharacterKit.UNION.MAGE:
                level = BtnLevelUpMage.Level;
                break;
            case CharacterKit.UNION.WARRIOR:
                level = BtnLevelUpWarrior.Level;
                break;
            case CharacterKit.UNION.ARCHER:
                level = BtnLevelUpArcher.Level;
                break;
        }
        return level;
    }

    public void Init()
    {
        MAGE.Init();
        WARRIOR.Init();
        ARCHER.Init();
    }
}
