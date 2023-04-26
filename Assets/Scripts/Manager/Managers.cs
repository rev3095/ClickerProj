using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class Managers : MonoBehaviour
{
    public static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    private static GameManager s_gameManager = new GameManager();
    private static UIManager s_uiManager = new UIManager();
    private static ResourceManager s_resourceManager = new ResourceManager();
    private static DataManager s_dataManager = new DataManager();

    public static GameManager Game { get { Init(); return s_gameManager; } }
    public static UIManager UI { get { Init(); return s_uiManager; } }
    public static ResourceManager Resource { get { Init(); return s_resourceManager; } }
    public static DataManager Data {  get { Init(); return s_dataManager; } }

    public static string GetText(int id)
    {
        if (Managers.Data.TextDict.TryGetValue(id, out Texts value) == false)
            return "";

        return value.kor.Replace("{userName}", Managers.Game.Name);
    }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if(s_instance == null)
        {
            //go 오브젝트를 @Managers 이름으로 DontDestroyOnLoad static 관리
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
                go = new GameObject { name = "@Managers" };

            //GetOrAdd : GetComponent -> 없다면 Add
            s_instance = Utils.GetOrAddComponent<Managers>(go);
            DontDestroyOnLoad(go);

            s_resourceManager.Init();
            s_dataManager.Init();

            Application.targetFrameRate = 60;
        }
    }
}
