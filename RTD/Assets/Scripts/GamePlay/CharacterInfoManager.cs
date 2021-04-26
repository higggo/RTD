using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoManager : MonoBehaviour
{
    public Transform GroundSpace = null;
    public Transform StorageSpace = null;
    public Transform BossSpace = null;

    private void Start()
    {
        if (GroundSpace == null)
            GroundSpace = GameObject.Find("Ground").transform.Find("Space").transform;
        if (StorageSpace == null)
            StorageSpace = GameObject.Find("Storage").transform.Find("Space").transform;
        if (BossSpace == null)
            BossSpace = GameObject.Find("FieldMap").transform.Find("Space").transform;
    }

    public GameObject GetUpgradeTarget(GameObject obj)
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

        //if (target != null)
        //{
        //    string[] charList = GetGradeCharacterList(GetNextGrade(grade));
        //    upgradeCharacter = Instantiate(Resources.Load(charList[Random.Range(0, charList.Length)])) as GameObject;
        //    upgradeCharacter.transform.parent = obj.transform.parent;
        //    upgradeCharacter.transform.localPosition = Vector3.zero;
        //    //UpdateCharacterField(upgradeCharacter);
        //    Destroy(obj);
        //    Destroy(target);
        //}
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
            case "FieldMap":
                map = GamePlay.MAP.Boss;
                break;
        }

        return map;
    }
    public void CharacterAttackFlagOn()
    {
        for (int i = 0; i < GroundSpace.childCount; i++)
        {
            if (GroundSpace.GetChild(i).childCount > 0)
            {
                GameObject character = GroundSpace.GetChild(i).GetChild(0).gameObject;
                character.GetComponent<CharController>().isInField = true;
                //if (GetFieldLocated(character) == GamePlay.MAP.Ground)
                //{
                //    character.GetComponent<CharController>().isInField = true;
                //}
                //else
                //{
                //    character.GetComponent<CharController>().isInField = false;
                //}
            }
        }

        for (int i = 0; i < StorageSpace.childCount; i++)
        {
            if (StorageSpace.GetChild(i).childCount > 0)
            {
                GameObject character = StorageSpace.GetChild(i).GetChild(0).gameObject;
                character.GetComponent<CharController>().isInField = false;
                //if (GetFieldLocated(character) == GamePlay.MAP.Ground)
                //{
                //    character.GetComponent<CharController>().isInField = true;
                //}
                //else
                //{
                //    character.GetComponent<CharController>().isInField = false;
                //}
            }
        }
        for (int i = 0; i < BossSpace.childCount; i++)
        {
            if (BossSpace.GetChild(i).childCount > 0)
            {
                GameObject character = BossSpace.GetChild(i).GetChild(0).gameObject;
                character.GetComponent<CharController>().isInField = true;
            }
        }
    }
    public void CharacterAttackFlagOff()
    {
        for (int i = 0; i < GroundSpace.childCount; i++)
        {
            if (GroundSpace.GetChild(i).childCount > 0)
            {
                GameObject character = GroundSpace.GetChild(i).GetChild(0).gameObject;
                character.GetComponent<CharController>().isInField = false;
            }
        }

        for (int i = 0; i < StorageSpace.childCount; i++)
        {
            if (StorageSpace.GetChild(i).childCount > 0)
            {
                GameObject character = StorageSpace.GetChild(i).GetChild(0).gameObject;
                character.GetComponent<CharController>().isInField = false;
            }
        }
        for (int i = 0; i < BossSpace.childCount; i++)
        {
            if (BossSpace.GetChild(i).childCount > 0)
            {
                GameObject character = BossSpace.GetChild(i).GetChild(0).gameObject;
                character.GetComponent<CharController>().isInField = false;
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

    public List<GameObject> GetCharacters(CharacterKit.UNION union)
    {
        List<GameObject> characters = new List<GameObject>();

        for (int i = 0; i < GroundSpace.childCount; i++)
        {
            if (GroundSpace.GetChild(i).childCount > 0)
            {
                if (GroundSpace.GetChild(i).GetChild(0).gameObject.GetComponent<CharController>().statInfo.union == union)
                {
                    characters.Add(GroundSpace.GetChild(i).GetChild(0).gameObject);
                }
            }
        }

        for (int i = 0; i < StorageSpace.childCount; i++)
        {
            if (StorageSpace.GetChild(i).childCount > 0)
            {
                if (StorageSpace.GetChild(i).GetChild(0).gameObject.GetComponent<CharController>().statInfo.union == union)
                {
                    characters.Add(StorageSpace.GetChild(i).GetChild(0).gameObject);
                }
            }
        }

        return characters;
    }

}
