using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterCard : MonoBehaviour
{
    public Transform CharacterSelectArea;
    int  MaxSalesNum = 5;
    string[] CardDB = new string[] {
        "UI/EmptyCard",
        "UI/Shyvana",
        "UI/Tristana",
        "UI/Zyra"
    };
    GameObject[] SalesList = new GameObject[5];
    public GameObject Storage;
    // Start is called before the first frame update
    void Start()
    {
        RefreshCards();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RefreshCards()
    {
        for (int i = 0; i < MaxSalesNum; i++)
        {
            int temp = i;
            if(SalesList[i] != null) Destroy(SalesList[i]);
            GameObject obj = Instantiate(
                Resources.Load(CardDB[Random.Range(1, CardDB.Length)]),
                CharacterSelectArea
                ) as GameObject;
            SalesList[i] = obj;
            SalesList[i].GetComponent<Button>().onClick.AddListener(() => SelectCard(temp));
        }
    }

    public void BuyXP()
    {
        Debug.Log("BuyXP");
    }

    public void SelectCard(int i)
    {
        if(Storage.GetComponent<Storage>().IsFull())
        {
            Debug.Log("Storage Is Full !!");
            return;
        }
        Storage.GetComponent<Storage>().Push(SalesList[i]);

        if(SalesList[i] != null) Destroy(SalesList[i]);
        GameObject obj = Instantiate(
                Resources.Load(CardDB[0]),
                CharacterSelectArea
                ) as GameObject;
        obj.transform.SetSiblingIndex(i);
        SalesList[i] = obj;
    }
}
