using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceePoint : MonoBehaviour
{
    public int serviceAmount = 10;

    public void GiveReward()
    {
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.AddMoney(serviceAmount);  
        }
    }
}
