using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyOnUI;
    
    [SerializeField] private int money;

    public UnityAction Action;
    
    private void Awake()
    {
        moneyOnUI.text = $"{money}";
    }

    public void AddMoney(int mon)
    {
        money += mon;

        moneyOnUI.text = $"{money}";
        
        Action.Invoke();
    }

    public void RemoveMoney(int mon)
    {
        money -= mon;
        
        moneyOnUI.text = $"{money}";
        
        Action.Invoke();
    }

    public bool ValidateMoney(int mon, Button eventButton)
    {
        Debug.Log("Valid");
        
        bool isValid = mon <= money ? true : false;

        eventButton.interactable = isValid;
        return isValid;
    }
}
