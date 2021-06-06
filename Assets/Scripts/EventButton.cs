using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventButton : MonoBehaviour
{
    [SerializeField] private int coast;
    
    private Button _button;
    private MoneyManager _moneyManager;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _moneyManager = FindObjectOfType<MoneyManager>();
        
        //_button.onClick.AddListener(() => _moneyManager.ValidateMoney(coast, _button));
        _moneyManager.Action += () => { _moneyManager.ValidateMoney(coast, _button); };
    }
}
