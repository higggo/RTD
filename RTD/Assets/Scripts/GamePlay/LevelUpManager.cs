using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;
public class LevelUpManager : MonoBehaviour
{
    CharacterInfoManager CharInfoManager;
    MoneyManager MoneyManager;

    public BtnLevelUpUnion Union = null;
    public BtnLevelUpDemic Demic = null;
    public BtnLevelUpExis Exis = null;

    ResponseMessage.Trade.CODE response;
    // Start is called before the first frame update
    void Start()
    {
        // Find Reference
        if (Union == null) Union = GameObject.Find("Union").GetComponent<BtnLevelUpUnion>();
        if (Demic == null) Demic = GameObject.Find("Union").GetComponent<BtnLevelUpDemic>();
        if (Exis == null) Exis = GameObject.Find("Union").GetComponent<BtnLevelUpExis>();

        CharInfoManager = GetComponent<CharacterInfoManager>();
        MoneyManager = GetComponent<MoneyManager>();

        Union.OnclickDelegate += LevelUpUnion;
        Demic.OnclickDelegate += LevelUpDemic;
        Exis.OnclickDelegate += LevelUpExis;

        GameObject.Find("Storage").GetComponent<Storage>().CreateCharacterDelegate += UpdateCharacterLevel;
    }
    void LevelUpUnion()
    {
        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, Union.Price, response, "Union Level Up"))
        {
            Union.Level += 1;
            CharUtils.UpdateSpecificUnion(CharacterKit.UNION.MAGE, Union.Level);
        }
        else
        {
            Debug.Log(ResponseMessage.Trade.Receive(response));
        }
    }

    void LevelUpDemic()
    {
        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, Demic.Price, response, "Demic Level Up"))
        {
            Demic.Level += 1;
            CharUtils.UpdateSpecificUnion(CharacterKit.UNION.WARRIOR, Demic.Level);
        }
        else
        {
            Debug.Log(ResponseMessage.Trade.Receive(response));
        }
    }

    void LevelUpExis()
    {
        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, Exis.Price, response, "Exis Level Up"))
        {
            Exis.Level += 1;
            CharUtils.UpdateSpecificUnion(CharacterKit.UNION.ARCHER, Exis.Level);
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
                level = Union.Level;
                break;
            case CharacterKit.UNION.WARRIOR:
                level = Demic.Level;
                break;
            case CharacterKit.UNION.ARCHER:
                level = Exis.Level;
                break;
        }
        return level;
    }

    public void Init()
    {
        Union.Init();
        Demic.Init();
        Exis.Init();
    }
}
