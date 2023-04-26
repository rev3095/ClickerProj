using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using static Define;

public class Utils
{
	public static T ParseEnum<T>(string value, bool ignoreCase = true)
	{
		return (T)System.Enum.Parse(typeof(T), value, ignoreCase);
	}

	public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
	{
		T component = go.GetComponent<T>();
		if (component == null)
			component = go.AddComponent<T>();
		return component;
	}

	public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
	{
		if (go == null)
			return null;

		if (recursive == false)
		{
			Transform transform = go.transform.Find(name);
			if (transform != null)
				return transform.GetComponent<T>();
		}
		else
		{
			foreach (T component in go.GetComponentsInChildren<T>())
			{
				if (string.IsNullOrEmpty(name) || component.name == name)
					return component;
			}
		}

		return null;
	}

	public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
	{
		Transform transform = FindChild<Transform>(go, name, recursive);
		if (transform != null)
			return transform.gameObject;
		return null;
	}

	public static T Load<T>(string path) where T : Object
	{
		return Resources.Load<T>(path);
	}

	public static GameObject Instantiate(string path, Transform parent = null)
	{
		GameObject prefab = Load<GameObject>($"Prefabs/{path}");
		if (prefab == null)
		{
			Debug.Log($"Filed to load prefab : {path}");
			return null;
		}

		return GameObject.Instantiate(prefab, parent);
	}

	public static void Destroy(GameObject go)
	{
		if (go == null)
			return;

		GameObject.Destroy(go);
	}

	//지금보니까 이 Get으로 가져올 수 있는건 GetText를 이용하여 한 줄 가져올 수 있음 TextData의 Value를!

	public static string GetMoneyString(int value, string MoneyType)
	{
		if(MoneyType == "Money")
        {
			int money = value;
			return $"{money}$";
			//return string.Format("{0:0.0}만", value / 10000.0f);

		}
		else if(MoneyType == "GameMoney")
        {
			int money = value;
			return $"{money}M";

		}
        else
        {
			return null;
        }
	}
}
