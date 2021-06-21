using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public enum Languages
{
    Russian,
    English
}
public static class LocalizationManager
{
    public static Languages CurrentLanguage;
    public static LocalizationStorage LocalizationStorage;

    public static void SetLocalization()
    {
        var textAsset = Resources.Load<TextAsset>("Localization");
        LocalizationStorage = JsonConvert.DeserializeObject<LocalizationStorage>(textAsset.text);
    }

    public static string ReturnTranslate(string key)
    {
        var translate = String.Empty;
        var storageElement = LocalizationStorage.ReturnStorageElement(key);

        switch (CurrentLanguage)
        {
            case Languages.Russian:
                translate = storageElement.russian;
                break;
            case Languages.English:
                translate = storageElement.english;
                break;
        }

        return translate;
    }
}

[Serializable]
public class LocalizationStorage
{
    [JsonProperty("container")] private Dictionary<string, LocalizationStorageElement> _container;
    
    public LocalizationStorage(Dictionary<string, LocalizationStorageElement> container)
    {
        _container = container;
    }

    public LocalizationStorageElement ReturnStorageElement(string key)
    {
        _container.TryGetValue(key, out LocalizationStorageElement storageElement);
        return storageElement;
    }
}

[Serializable]
public class LocalizationStorageElement
{
    [JsonProperty("ru")] public string russian;
    [JsonProperty("en")] public string english;

    public LocalizationStorageElement(string rus, string eng)
    {
        russian = rus;
        english = eng;
    }
}
