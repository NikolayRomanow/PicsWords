using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionCheckerUnity : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;

    private void Awake()
    {
        label.text = $"Версия сборки: {Application.version}";
    }
}
