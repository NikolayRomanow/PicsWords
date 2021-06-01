using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterInputElement : MonoBehaviour
{
    public TextMeshProUGUI letter;
    private Button button;

    private void Awake()
    {
        letter = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => FindObjectOfType<RebusManager>().SetLetter(letter.text, gameObject));
    }
    
    private void OnButton()
    {
        Debug.Log("pressed");
    }

    public void SetLetter(char ch)
    {
        letter.text = $"{ch}";
    }
}