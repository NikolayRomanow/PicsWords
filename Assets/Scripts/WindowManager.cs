using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> windows;

    public void SetScreen(int index)
    {
        
        
        windows[index].transform.SetAsLastSibling();
    }

}
