using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Storage : MonoBehaviour
{
    GameObject obj;
    int SpaceNum;

    private void Awake()
    {
        SpaceNum = GetAllSpaceCount();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Push(GameObject CharacterInfo)
    {
        foreach (Transform child in transform.Find("Space"))
        {
            if(child.childCount <= 0)
            {
                obj = Instantiate(Resources.Load("UI/DummyCharacter")) as GameObject;
                obj.transform.parent = child;
                obj.transform.localPosition = Vector3.zero;
                obj.GetComponentInChildren<TMP_Text>().text 
                    = CharacterInfo.GetComponentInChildren<TMP_Text>().text;
                break;
            }
        }
    }

    int GetAllSpaceCount()
    {
        int num = 0;
        foreach (Transform child in transform.Find("Space"))
        {
                num++;
        }
        return num;
    }

    int GetEmptySpaceCount()
    {
        int num = 0;
        foreach (Transform child in transform.Find("Space"))
        {
            if (child.childCount == 0)
            {
                num++;
            }
        }
        return Mathf.Clamp(num, 0, SpaceNum);
    }

    public bool IsFull()
    {
        if (GetEmptySpaceCount() > 0)
            return false;
        else
            return true;
    }
}
