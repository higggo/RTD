using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    uint money;
    bool IsCalculatingMoney;
    uint SerialNumber = 0;
    public TMPro.TextMeshProUGUI GoldText;

    Coroutine CalculateCoroutine;

    public enum ACTION
    {
        Pay,
        Receive,
    }


    private void Awake()
    {
        IsCalculatingMoney = false;
        Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        GoldText.text = money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Init()
    {
        SetMoney(500);
    }

    public float GetMoney()
    {
        return money;
    }

    void SetMoney(uint money)
    {
        this.money = money;
    }

    public bool CalculateMoney(ACTION act, uint money, ref ResponseCode.TRADE respone, string Message)
    {
        bool output = false;
        if (IsCalculatingMoney)
        {
            respone = ResponseCode.TRADE.BUSY;
            return false;
        }

        IsCalculatingMoney = true;

        if (act == ACTION.Pay)
        {
            if (this.money >= money)
            {
                this.money -= money;
                respone = ResponseCode.TRADE.SUCCESS;
            }
            else
            {
                respone = ResponseCode.TRADE.NEEDMOREMONEY;
                output = false;
            }
        }
        else if (act == ACTION.Receive)
        {
            this.money += money;
            respone = ResponseCode.TRADE.SUCCESS;
        }

        //
        if(respone == ResponseCode.TRADE.SUCCESS)
        {
            output = true;
            GameObject obj = Instantiate(Resources.Load("UI/TradeAmount")) as GameObject;
            obj.GetComponent<TradeAmount>().Save(act, money, SerialNumber++, Message);
            obj.transform.parent = gameObject.transform.Find("Account");
        }
        GoldText.text = this.money.ToString();
        IsCalculatingMoney = false;
        return output;
    }
    //public ResponseCode.TRADE PayMoney(float money)
    //{
    //    if(IsCalculatingMoney)
    //    {
    //       return ResponseCode.TRADE.BUSY;
    //    }
    //    else
    //    {
    //        return CalculateMoney(ACTION.Pay, money);
    //    }
    //}

    //public ResponseCode.TRADE ReceiveMoney(float money)
    //{
    //    if (IsCalculatingMoney)
    //    {
    //        return ResponseCode.TRADE.BUSY;
    //    }
    //    else
    //    {
    //        return CalculateMoney(ACTION.Pay, money);
    //    }
    //}
    //IEnumerator CalculateMoney(ACTION act, float money, TradeDelegate done)
    //{
    //    ResponseCode.TRADE respone;
    //    IsCalculatingMoney = true;
    //    GameObject obj = Instantiate(Resources.Load("Resouces/UI/TradeAmount")) as GameObject;
    //    obj.GetComponent<TradeAmount>().Save(act, money, SerialNumber++);
    //    obj.transform.parent = gameObject.transform.Find("Account");

    //    //// Waiting for Server Response ...
    //    //while (IsCalculatingMoney)
    //    //{
    //    //    foreach(Transform child in gameObject.transform.Find("Account"))
    //    //    {
    //    //        child.GetComponent<TradeAmount>.
    //    //    }
    //    //    yield return null;
    //    //}
    //    if (act == ACTION.Pay)
    //    {

    //        if (this.money >= money)
    //        {
    //            this.money -= money; 
    //            respone = ResponseCode.TRADE.SUCCESS;
    //        }
    //        else
    //        {
    //            respone = ResponseCode.TRADE.NEEDMOREMONEY;
    //        }
    //    }
    //    else if(act == ACTION.Receive)
    //    {
    //        this.money += money;
    //            respone = ResponseCode.TRADE.SUCCESS;
    //    }
    //    yield return null;
    //}

}
