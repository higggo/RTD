using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseCode : MonoBehaviour
{
    public enum TRADE
    {
        SUCCESS,
        WAITING,        // my task calculating
        NEEDMOREMONEY,
        BUSY
    };

    public string TradeMessage(TRADE Trade)
    {
        string output = "";
            switch (Trade)
            {
                case TRADE.SUCCESS:
                    output = "Success";
                    break;
                case TRADE.WAITING:
                    output = "Waiting";
                    break;
                case TRADE.NEEDMOREMONEY:
                    output = "NeedMoreMoney";
                    break;
                case TRADE.BUSY:
                    output = "Busy";
                    break;
        }
        return output;
    }
}
