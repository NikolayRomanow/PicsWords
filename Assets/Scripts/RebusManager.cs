using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI labelURL;
    [SerializeField] private RebusDataBaseAsset rebusDataBaseAsset;
    [SerializeField] private string hiddenWord;

    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private WindowManager windowManager;
    [SerializeField] private Image zoomCanvas;
    [SerializeField] private GameObject winPanel;

    [Header("Chars")]
    [SerializeField] private GameObject letterOutput;
    [SerializeField] private GameObject letterInput;
    [SerializeField] private GameObject emptyButton;
    [SerializeField] private Transform rootLettersOutput;
    [SerializeField] private Transform rootLettersInput;
    [SerializeField] private int sizeLettersOutput;
    [SerializeField] private int sizeLettersInput;

    private List<LetterOutputElement> letterOutputElements = new List<LetterOutputElement>();

    private List<char> charsArray = new List<char>();
    [SerializeField] private int _outputCounter;

    [SerializeField] private List<char> bufferWord;

    private bool expensiveLettersIsShow;
    
    private void SetWinPanel()
    {
        winPanel.SetActive(true);
    }
    
    public void NextRebus()
    {
        addressableRef[indexOfRebus].LoadAssetAsync().Completed += handle =>
        {
            rebusDataBaseAsset = handle.Result;
            indexOfRebus++;

            moneyManager.AddMoney(20);
                
            SetRebus();
                
            //Addressables.Release(handle);
        };
    }
    
    public void ShowFirstLetter()
    {
        var count = Random.Range(1, hiddenWord.Length - 1);

        var bufferOutputCounter = _outputCounter;
        _outputCounter = count;

        if (letterOutputElements[count].letter.text != " ")
        {
            count = letterOutputElements.IndexOf(letterOutputElements.Find(item => item.letter.text == " "));
            _outputCounter = count;
            Debug.Log(count);
        }
        
        SetLetter(hiddenWord[count].ToString(), null, -1);
        moneyManager.RemoveMoney(20);

        _outputCounter = bufferOutputCounter;

        windowManager.SetScreen(1);
    }
    
    public void RemoveUnnecessaryLetters()
    {
        if (expensiveLettersIsShow)
            return;

        expensiveLettersIsShow = true;
        for (int i = 0; i < rootLettersInput.childCount; i++)
        {
            if (!hiddenWord.Contains(rootLettersInput.GetChild(i).GetComponent<LetterInputElement>().letter.text))
            {
                //Destroy(rootLettersInput.GetChild(i).gameObject);
                rootLettersInput.GetChild(i).gameObject.SetActive(false);
            }
        }
        
        moneyManager.RemoveMoney(30);
        
        windowManager.SetScreen(1);
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

    private void ZoomImage(Image image)
    {
        zoomCanvas.gameObject.SetActive(true);
        zoomCanvas.sprite = image.sprite;
    }
    
    public void LoadYetImage(int index)
    {
        if (images[index].sprite != null)
        {
            ZoomImage(images[index]);
            labelURL.text = rebusDataBaseAsset.labelsURL[index];
            return;
        }
        
        images[index].sprite = rebusDataBaseAsset.images[index];
        images[index].color = new Color(255, 255, 255, 255);

        moneyManager.RemoveMoney(10);
    }
    
    private void SetRebus()
    {

        expensiveLettersIsShow = false;
        
        winPanel.SetActive(false);
        
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

        bufferWord = new List<char>();
        bufferWord = hiddenWord.ToList();

        for (int i = 0; i < bufferWord.Count; i++)
        {
            bufferWord[i] = '\0';
        }
        
        for (int i = 0; i < sizeLettersOutput; i++)
        {
            var letter = Instantiate(letterOutput, rootLettersOutput);
            
            var lttr =  letter.GetComponent<LetterOutputElement>();
            
            lttr.buttonIndex = i;
            lttr.GetComponent<Button>().onClick.AddListener(() => {RemoveThisLetter(lttr.buttonIndex);});
            
            letterOutputElements.Add(lttr);
        }

        for (int i = 0; i < sizeLettersInput; i++)
        {
            var letter = Instantiate(letterInput, rootLettersInput);
            var letterInputElement = letter.GetComponent<LetterInputElement>();

            var ch = charsArray[Random.Range(0, charsArray.Count - 1)];
            letterInputElement.SetLetter(ch, i);

            charsArray.Remove(ch);
        }
    }

    public void SetLetter(string ch, GameObject obj, int index)
    {
        // if (letterOutputElements[_outputCounter + 1].siblingIndex == -1)
        // {
        //     _outputCounter++;
        //     SetLetter(ch, obj, index);
        //     return;
        // }
        
        if (_outputCounter >= letterOutputElements.Count)
            return;

        if (letterOutputElements[_outputCounter].siblingIndex == -1)
        {
            while (letterOutputElements[_outputCounter].siblingIndex == -1)
            {
                _outputCounter++;
            }
        }
        
        letterOutputElements[_outputCounter].SetLetter(ch[0], index);
        bufferWord[_outputCounter] = ch[0];
        _outputCounter++;

        var msg = string.Join("", bufferWord);

        if (msg == hiddenWord)
            SetWinPanel();

        if (_outputCounter < letterOutputElements.Count)
        {
            if (letterOutputElements[_outputCounter].siblingIndex == -1)
            {
                while (letterOutputElements[_outputCounter].siblingIndex == -1)
                {
                    _outputCounter++;
                }
            }
        }

        Destroy(obj);
        if (obj != null)
            Instantiate(emptyButton, rootLettersInput).transform.SetSiblingIndex(index);
    }

    public void RemovingLetter()
    {
        if (_outputCounter <= 0)
            return;

        var lt = letterOutputElements[_outputCounter - 1].letter.text[0];

        if (letterOutputElements[_outputCounter - 1].siblingIndex == -1)
        {
            //Destroy(rootLettersInput.GetChild(letterOutputElements[_outputCounter - 1].siblingIndex).gameObject);
            _outputCounter--;
            RemovingLetter();
            return;
        }
        
        letterOutputElements[_outputCounter - 1].SetLetter('\0');

        Destroy(rootLettersInput.GetChild(letterOutputElements[_outputCounter - 1].siblingIndex).gameObject);
        
        var letter = Instantiate(letterInput, rootLettersInput);
        
        letter.transform.SetSiblingIndex(letterOutputElements[_outputCounter - 1].siblingIndex);
        
        var letterInputElement = letter.GetComponent<LetterInputElement>();

        bufferWord[_outputCounter - 1] = '\0';
        
        letterInputElement.SetLetter(lt, letterOutputElements[_outputCounter - 1].siblingIndex);
        
        letterOutputElements[_outputCounter - 1].siblingIndex = 0;
        
        _outputCounter--;
    }

    public void RemoveThisLetter(int index)
    {
        var lt = letterOutputElements[index].letter.text[0];

        if (letterOutputElements[index].siblingIndex == -1)
        {
            return;
        }
        
        letterOutputElements[index].SetLetter('\0');

        Destroy(rootLettersInput.GetChild(letterOutputElements[index].siblingIndex).gameObject);
        
        var letter = Instantiate(letterInput, rootLettersInput);
        
        letter.transform.SetSiblingIndex(letterOutputElements[index].siblingIndex);
        
        var letterInputElement = letter.GetComponent<LetterInputElement>();

        bufferWord[index] = '\0';
        
        letterInputElement.SetLetter(lt, letterOutputElements[index].siblingIndex);
        
        letterOutputElements[index].siblingIndex = 0;

        _outputCounter = index;
    }
    
    public void RemoveLetter()
    {
        StartCoroutine(RemoveLettersNum());
    }

    IEnumerator RemoveLettersNum()
    {
        while (_outputCounter > 0)
        {
            yield return new WaitForSeconds(0.01f);
            RemovingLetter();
        }
    }
}

public static class Alphabet
{
    public const string Russian = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
}
