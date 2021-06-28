using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RebusDB", menuName = "RebusDB")]
public class RebusDataBaseAsset : ScriptableObject
{
    public List<Sprite> images = new List<Sprite>(4);
    public List<string> labelsURL = new List<string>(4);
    public string rusWord;
    public string engWord;
    public string espWord;
}
