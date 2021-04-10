using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoManager : MonoBehaviour
{
    public Transform GroundSpace;
    public Transform StorageSpace;

    GameObject upgradeCharacter;

    public GameObject UpgradeCharacter(GameObject obj)
    {
        GameObject target = null;
        CharacterKit.GRADE grade = obj.GetComponent<CharController>().statInfo.grade;
        CharacterKit.ID id = obj.GetComponent<CharController>().statInfo.id;
        if (!HasNextGrade(grade))
            return null;

        for (int i = 0; i < GroundSpace.childCount; i++)
        {
            if (GroundSpace.GetChild(i).childCount > 0)
            {
                GameObject character = GroundSpace.GetChild(i).GetChild(0).gameObject;
                if (character.GetComponent<CharController>().statInfo.grade == grade &&
                    character.GetComponent<CharController>().statInfo.id == id &&
                    target == null &&
                    obj != character)
                {
                    target = character;
                    break;
                }
            }
        }
        for (int i = 0; i < StorageSpace.childCount; i++)
        {
            if (StorageSpace.GetChild(i).childCount > 0)
            {
                GameObject character = StorageSpace.GetChild(i).GetChild(0).gameObject;
                if (character.GetComponent<CharController>().statInfo.grade == grade &&
                    character.GetComponent<CharController>().statInfo.id == id &&
                    target == null &&
                    obj != character)
                {
                    target = character;
                    break;
                }
            }
        }

        if (target != null)
        {
            string[] charList = GetGradeCharacterList(GetNextGrade(grade));
            upgradeCharacter = Instantiate(Resources.Load(charList[Random.Range(0, charList.Length)])) as GameObject;
            upgradeCharacter.transform.parent = obj.transform.parent;
            upgradeCharacter.transform.localPosition = Vector3.zero;
            //UpdateCharacterField(upgradeCharacter);
            Destroy(obj);
            Destroy(target);
        }
        return target;
    }
    GamePlay.MAP GetFieldLocated(GameObject obj)
    {
        GamePlay.MAP map = GamePlay.MAP.Storage;
        switch(obj.transform.parent.parent.parent.name)
        {
            case "Ground":
                map = GamePlay.MAP.Ground;
                break;
            case "Storage":
                map = GamePlay.MAP.Storage;
                break;
            case "Boss":
                map = GamePlay.MAP.Boss;
                break;
        }

        return map;
    }
    public void UpdateCharacterField()
    {
        for (int i = 0; i < GroundSpace.childCount; i++)
        {
            if (GroundSpace.GetChild(i).childCount > 0)
            {
                GameObject character = GroundSpace.GetChild(i).GetChild(0).gameObject;
                if (GetFieldLocated(character) == GamePlay.MAP.Ground)
                {
                    character.GetComponent<CharController>().isInField = true;
                }
                else
                {
                    character.GetComponent<CharController>().isInField = false;
                }
            }
        }

        for (int i = 0; i < StorageSpace.childCount; i++)
        {
            if (StorageSpace.GetChild(i).childCount > 0)
            {
                GameObject character = StorageSpace.GetChild(i).GetChild(0).gameObject;
                if (GetFieldLocated(character) == GamePlay.MAP.Ground)
                {
                    character.GetComponent<CharController>().isInField = true;
                }
                else
                {
                    character.GetComponent<CharController>().isInField = false;
                }
            }
        }
    }

    public bool HasNextGrade(CharacterKit.GRADE grade)
    {
        if (grade == CharacterKit.GRADE.UNIQUE)
            return false;
        return true;
    }
    public CharacterKit.GRADE GetNextGrade(CharacterKit.GRADE grade)
    {
        if (grade == CharacterKit.GRADE.NORMAL)
            return CharacterKit.GRADE.MAGIC;
        else if (grade == CharacterKit.GRADE.MAGIC)
            return CharacterKit.GRADE.RARE;
        else if (grade == CharacterKit.GRADE.RARE)
            return CharacterKit.GRADE.UNIQUE;

        return CharacterKit.GRADE.NORMAL;
    }
    public string[] GetGradeCharacterList(CharacterKit.GRADE grade)
    {
        string[] strList = null;
        switch (grade)
        {
            case CharacterKit.GRADE.NORMAL:
                strList = GetComponent<GameDB>().CharacterNoramlClassAddr;
                break;
            case CharacterKit.GRADE.MAGIC:
                strList = GetComponent<GameDB>().CharacterMagicClassAddr;
                break;
            case CharacterKit.GRADE.RARE:
                strList = GetComponent<GameDB>().CharacterRareClassAddr;
                break;
            case CharacterKit.GRADE.UNIQUE:
                strList = GetComponent<GameDB>().CharacterUniqueClassAddr;
                break;
        }
        return strList;
    }
}
