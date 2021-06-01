using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class AssetReferenceRB : AssetReferenceT<RebusDataBaseAsset>
{
    public AssetReferenceRB(string guid) : base(guid) { }
}

public class RebusManager : MonoBehaviour
{
    [SerializeField] private int indexOfRebus;
    [SerializeField] private List<AssetReferenceRB> addressableRef;
    
    [SerializeField] private List<Image> images;
    [SerializeField] private RebusDataBaseAsset rebusDataBaseAsset;
    [SerializeField] private string hiddenWord;

    [SerializeField] private MoneyManager moneyManager;

    [Header("Chars")]
    [SerializeField] private GameObject letterOutput;
    [SerializeField] private GameObject letterInput;
    [SerializeField] private Transform rootLettersOutput;
    [SerializeField] private Transform rootLettersInput;
    [SerializeField] private int sizeLettersOutput;
    [SerializeField] private int sizeLettersInput;

    private List<LetterOutputElement> letterOutputElements = new List<LetterOutputElement>();

    private List<char> charsArray = new List<char>();
    private int _outputCounter;

    [SerializeField] private string bufferWord;

    public void ShowFirstLetter()
    {
        if (_outputCounter == 0)
        {
            SetLetter(hiddenWord[0].ToString(), null);
        }
        
        moneyManager.RemoveMoney(20);
    }
    
    public void RemoveUnnecessaryLetters()
    {
        for (int i = 0; i < rootLettersInput.childCount; i++)
        {
            if (!hiddenWord.Contains(rootLettersInput.GetChild(i).GetComponent<LetterInputElement>().letter.text))
            {
                Destroy(rootLettersInput.GetChild(i).gameObject);
            }
        }
        
        moneyManager.RemoveMoney(30);
    }
    
    private void Awake()
    {
        addressableRef[indexOfRebus].LoadAssetAsync().Completed += handle =>
        {
            rebusDataBaseAsset = handle.Result;
            indexOfRebus++;

            SetRebus();
            //Addressables.Release(handle);
        };
    }

    public void LoadYetImage(int index)
    {
        images[index].sprite = rebusDataBaseAsset.images[index];
        images[index].color = new Color(255, 255, 255, 255);
        moneyManager.RemoveMoney(10);
    }
    
    public void SetRebus()
    {
        _outputCounter = 0;

        letterOutputElements = new List<LetterOutputElement>();

        foreach (var image in images)
        {
            image.sprite = null;
            image.color = new Color(255, 255, 255, 0);
        }
        
        images[0].sprite = rebusDataBaseAsset.images[0];
        images[0].color = new Color(255, 255, 255, 255);

        hiddenWord = rebusDataBaseAsset.word;
        sizeLettersOutput = hiddenWord.Length;

        string _charsArray = hiddenWord;

        int counter = sizeLettersInput - _charsArray.Length;
        
        for (int i = 0; i < counter; i++)
        {
            _charsArray += Alphabet.Russian[Random.Range(0, Alphabet.Russian.Length - 1)];
        }

        for (int i = 0; i < _charsArray.Length; i++)
        {
            char ch = _charsArray[i];
            charsArray.Add(ch);
        }

        for (int i = 0; i < rootLettersOutput.childCount; i++)
        {
            Destroy(rootLettersOutput.GetChild(i).gameObject);
        }
        
        for (int i = 0; i < rootLettersInput.childCount; i++)
        {
            Destroy(rootLettersInput.GetChild(i).gameObject);
        }

        bufferWord = String.Empty;
        
        for (int i = 0; i < sizeLettersOutput; i++)
        {
            var letter = Instantiate(letterOutput, rootLettersOutput);
            letterOutputElements.Add(letter.GetComponent<LetterOutputElement>());
        }

        for (int i = 0; i < sizeLettersInput; i++)
        {
            var letter = Instantiate(letterInput, rootLettersInput);
            var letterInputElement = letter.GetComponent<LetterInputElement>();

            var ch = charsArray[Random.Range(0, charsArray.Count - 1)];
            letterInputElement.SetLetter(ch);
            charsArray.Remove(ch);
        }
    }

    public void SetLetter(string ch, GameObject obj)
    {
        if (_outputCounter >= letterOutputElements.Count)
            return;
        letterOutputElements[_outputCounter].SetLetter(ch[0]);
        _outputCounter++;

        bufferWord += ch[0];

        if (bufferWord == hiddenWord)
            addressableRef[indexOfRebus].LoadAssetAsync().Completed += handle =>
            {
                rebusDataBaseAsset = handle.Result;
                indexOfRebus++;

                moneyManager.AddMoney(20);
                
                SetRebus();
                
                //Addressables.Release(handle);
            };
            
        Destroy(obj);
    }

    public void RemoveLetter(string ch)
    {
        if (_outputCounter <= 0)
            return;

        var lt = letterOutputElements[_outputCounter - 1].letter.text[0];
        letterOutputElements[_outputCounter - 1].SetLetter('\0');
        var letter = Instantiate(letterInput, rootLettersInput);
        var letterInputElement = letter.GetComponent<LetterInputElement>();
        
        bufferWord = bufferWord.Remove(_outputCounter - 1);
        
        letterInputElement.SetLetter(lt);
        
        _outputCounter--;
    }
}

public static class Alphabet
{
    public const string Russian = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
}
