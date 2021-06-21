using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationText : MonoBehaviour
{
    [SerializeField] private string key;
    
    private TextMeshProUGUI _textMeshProUGUI;

    private void Start()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        SetText();
    }

    public void SetText()
    {
        _textMeshProUGUI.text = LocalizationManager.ReturnTranslate(key);
    }
}
