using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyOnUI;
    
    [SerializeField] private int money;

    private void Awake()
    {
        moneyOnUI.text = $"{money}";
    }

    public void AddMoney(int mon)
    {
        money += mon;

        moneyOnUI.text = $"{money}";
    }

    public void RemoveMoney(int mon)
    {
        money -= mon;
        
        moneyOnUI.text = $"{money}";
    }
}
