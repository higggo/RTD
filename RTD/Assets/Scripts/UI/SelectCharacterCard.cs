using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectCharacterCard : MonoBehaviour
{
    public GameObject CharacterPickerUI;
    MoneyManager MoneyManager;
    Transform CharacterSelectArea;
    int  MaxSalesNum = 5;
    ResponseCode Response;
    string[] CardDB = new string[] {
        "UI/EmptyCard",
        "UI/Shyvana",
        "UI/Tristana",
        "UI/Zyra"
    };
    GameObject[] SalesList = new GameObject[5];
    public GameObject Storage;
    ResponseCode.TRADE respone;

    // Start is called before the first frame update
    void Start()
    {
        Response = GetComponent<ResponseCode>();
        MoneyManager = GetComponent<MoneyManager>();
        CharacterSelectArea = CharacterPickerUI.transform.Find("CharacterSelectArea");
        RefreshCardsFree();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RefreshCards()
    {
        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, 50, ref respone, "Refresh Card"))
        {
            for (int i = 0; i < MaxSalesNum; i++)
            {
                int temp = i;
                if (SalesList[i] != null) Destroy(SalesList[i]);
                GameObject obj = Instantiate(
                    Resources.Load(CardDB[Random.Range(1, CardDB.Length)]),
                    CharacterSelectArea
                    ) as GameObject;
                SalesList[i] = obj;
                SalesList[i].GetComponent<Button>().onClick.AddListener(() => BuyCard(temp));
            }
        }
        else
        {
            Debug.Log(Response.TradeMessage(respone));
        }
    }
    void RefreshCardsFree()
    {
        for (int i = 0; i < MaxSalesNum; i++)
        {
            int temp = i;
            if (SalesList[i] != null) Destroy(SalesList[i]);
            GameObject obj = Instantiate(
                Resources.Load(CardDB[Random.Range(1, CardDB.Length)]),
                CharacterSelectArea
                ) as GameObject;
            SalesList[i] = obj;
            SalesList[i].GetComponent<Button>().onClick.AddListener(() => BuyCard(temp));
        }
    }
    public void BuyXP()
    {
        //MoneyManager.PayMoney
        Debug.Log("BuyXP");
    }

    void BuyCard(int i)
    {
        if (Storage.GetComponent<Storage>().IsFull())
        {
            Debug.Log("Storage Is Full !!");
            return;
        }

        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, 100, ref respone, "Buy Card"))
        {
            Storage.GetComponent<Storage>().Push(SalesList[i]);

            if (SalesList[i] != null) Destroy(SalesList[i]);
            GameObject obj = Instantiate(
                    Resources.Load(CardDB[0]),
                    CharacterSelectArea
                    ) as GameObject;
            obj.transform.SetSiblingIndex(i);
            SalesList[i] = obj;
        }
        else
        {
            Debug.Log(Response.TradeMessage(respone));
        }
    }

    //public void BuyResponse(ResponseCode.TRADE response)
    //{
    //    if(response == ResponseCode.TRADE.WAITING)
    //    {
    //        Debug.Log("buy waiting");
    //    }
    //    if (response == ResponseCode.TRADE.SUCCESS)
    //    {
    //        Debug.Log("buy success");
    //    }
    //    if (response == ResponseCode.TRADE.NEEDMOREMONEY)
    //    {
    //        Debug.Log("you need more money");
    //    }
    //    if (response == ResponseCode.TRADE.BUSY)
    //    {
    //        Debug.Log("so Busy");
    //    }
    //}
}
