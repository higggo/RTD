using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ResponseMessage;

public class SelectCharacterCard : MonoBehaviour
{
    public GameObject CharacterPickerUI;
    MoneyManager MoneyManager;
    Transform CharacterSelectArea;
    int  MaxSalesNum = 5;
    GameDB GameDB;
    GameObject[] SalesList = new GameObject[5];
    public GameObject Storage;
    ResponseMessage.Trade.CODE response;

    // Start is called before the first frame update
    void Start()
    {
        MoneyManager = GetComponent<MoneyManager>();
        GameDB = GetComponent<GameDB>();
        CharacterSelectArea = CharacterPickerUI.transform.Find("CharacterSelectArea");
        Init();
    }
    public void Init()
    {
        RefreshCardsFree();
    }
    public void RefreshCards()
    {
        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, 50, response, "Refresh Card"))
        {
            for (int i = 0; i < MaxSalesNum; i++)
            {
                int temp = i;
                if (SalesList[i] != null) Destroy(SalesList[i]);
                GameObject obj = Instantiate(
                    Resources.Load(GameDB.Card[Random.Range(1, GameDB.Card.Length)]),
                    CharacterSelectArea
                    ) as GameObject;
                SalesList[i] = obj;
                SalesList[i].GetComponent<Button>().onClick.AddListener(() => BuyCard(temp));
            }
        }
        else
        {
            Debug.Log(ResponseMessage.Trade.Receive(response));
        }
    }
    void RefreshCardsFree()
    {
        for (int i = 0; i < MaxSalesNum; i++)
        {
            int temp = i;
            if (SalesList[i] != null) Destroy(SalesList[i]);
            GameObject obj = Instantiate(
                Resources.Load(GameDB.Card[Random.Range(1, GameDB.Card.Length)]),
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

        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, 100, response, "Buy Card"))
        {
            Storage.GetComponent<Storage>().Push(SalesList[i].GetComponent<CardInfo>().CharacterPrefab);

            // Destry Select Card
            if (SalesList[i] != null) Destroy(SalesList[i]);

            // Instantiate Empty Card
            GameObject obj = Instantiate(Resources.Load(GameDB.Card[0]), CharacterSelectArea) as GameObject;
            obj.transform.SetSiblingIndex(i);
            SalesList[i] = obj;
        }
        else
        {
            Debug.Log(ResponseMessage.Trade.Receive(response));
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
