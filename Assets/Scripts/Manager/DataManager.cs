using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public StartData Start { get; private set; }
    public Dictionary<int, Texts> TextDict { get; private set; }
 
    public void Init()
    {
        Start = LoadSingleJson<StartData>("StartData");

        TextDict = LoadJson<TextData,int,Texts>("TextData").MakeDict();
    }
    public T LoadSingleJson<T>(string path)
    {
        TextAsset textAsset = Utils.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<T>(textAsset.text);
    }

    public Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

}

