using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> windows;

    private void Awake()
    {
        LocalizationManager.SetLocalization();
        LocalizationManager.CurrentLanguage = Languages.English;
    }

    public void SetScreen(int index)
    {
        windows[index].transform.SetAsLastSibling();
    }

}
