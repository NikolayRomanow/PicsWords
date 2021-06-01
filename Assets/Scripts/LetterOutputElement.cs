using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterOutputElement : MonoBehaviour
{
    public TextMeshProUGUI letter;
    //private Button button;
    private void Awake()
    {
        letter = GetComponentInChildren<TextMeshProUGUI>();
        //button = GetComponent<Button>();
        //button.onClick.AddListener(() => FindObjectOfType<RebusManager>().RemoveLetter(letter.text));
        SetLetter(' ');
    }

    public void SetLetter(char ch)
    {
        letter.text = $"{ch}";
    }
}
