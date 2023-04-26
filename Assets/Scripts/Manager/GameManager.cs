using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Define;

public enum CollectionState
{
	None,
	Uncheck,
	Done
}

[Serializable]
public class GameData
{
	//�⺻ Stat
	public string Name;
	public float Stress;
	public float Hunger;

	public int MaxStress;
	public int MaxHunger;

	//�⺻ Stat ��ȭ�� -> �̰� ���ӵ����Ϳ� ���� �ʿ� ����, GameManager �κп��� ������ �ٷ�°� ������. SaveData�� ���Խ�ų �ʿ䰡 ����.

	//��ȭ
	public float Money;
	public float GameMoney;

	//��Ÿ�� -> ���߿� ��Ÿ�� ���ҷ��� ���� �� �ְ� ����ٸ� ���⼭ ������ �ִ°� ����.

	//�ð� -> �������� PlayTime �ϳ��� ������ ó���� ���� �ְ�����, ���ϰ� GameDay�� �ϳ� �� ������ ����.
	public float PlayTime;
	public int GameDay;
	public int MaxGameDay;

	public int SecondPerGameDay;
	public int LastStressIncreaseDay;
	public int LastHungerDecreaseDay;

	public float increaseHunger;
	public float increaseStress;

	public float HungerPercent;
	public float StressPercent;
}



//
public class GameManager
{
	GameData _gameData = new GameData();
	public GameData SaveData { get { return _gameData; } set { _gameData = value; } }

	#region Stats
	public string Name
	{
		get { return _gameData.Name; }
		set { _gameData.Name = value; }
	}

	public Action OnStressChanged;
	public float Stress
	{
		get { return _gameData.Stress; }
		set { _gameData.Stress = Mathf.Clamp(value, 0, MaxStress); OnStressChanged?.Invoke(); }
	}

	public Action OnHungerChanged;
	public float Hunger
	{
		get { return _gameData.Hunger; }
		set { _gameData.Hunger = Mathf.Clamp(value, 0, MaxStress); OnStressChanged?.Invoke(); ; }
	}
	public int MaxHunger
	{
		get { return _gameData.MaxHunger; }
		set { _gameData.MaxHunger = value; }
	}
	public int MaxStress
	{
		get { return _gameData.MaxStress; }
		set { _gameData.MaxStress = value; }
	}

	public float HungerPercent
    {
		get { return Hunger * 100 / (float)MaxHunger; }
		set { _gameData.HungerPercent = value; }
    }
	public float StressPercent
	{
		get { return Stress * 100 / (float)MaxStress; }
		set { _gameData.StressPercent = value; }
	}

	public float Money
	{
		get { return _gameData.Money; }
		set { _gameData.Money = value; }
	}

	public float GameMoney
    {
		get { return _gameData.GameMoney; }
		set { _gameData.GameMoney = value; }
    }

	//PlayTime���� ���� �ϴ°� �ƴ϶� �� ��¥ 5�� ��� ī�������ִ� ������. ��������
	//5 ������ ���� ���� �̰� �� �ʸ��� Day 1�� ������ų�� int �� ���°� ������ϴ°� �ξ� ������ 
	public float PlayTime
	{
		get { return _gameData.PlayTime; }
		set { if (value > 5) { value = value - value; GameDay++; }  _gameData.PlayTime = value; }
	}

	/*
	public int GameDay
	{
		get
		{
			int gameDays = (int)(PlayTime / SecondPerGameDay);
			return Mathf.Min(gameDays, MaxGameDay);
		}
        set { _gameData.GameDay = value; }
	}
	*/
	public int GameDay
	{
		get { return Mathf.Min(_gameData.GameDay,_gameData.MaxGameDay); }
		set { _gameData.GameDay = value; }
	}

	public int MaxGameDay
    {
		get { return _gameData.MaxGameDay; }
		set { _gameData.MaxGameDay = value; }
    }
	public int SecondPerGameDay
	{
		get { return _gameData.SecondPerGameDay; }
		set { _gameData.SecondPerGameDay = value; }
	}
	public int LastStressIncreaseDay
	{
		get { return _gameData.LastStressIncreaseDay; }
		set { _gameData.LastStressIncreaseDay = value; }
	}
	public int LastHungerDecreaseDay
	{
		get { return _gameData.LastHungerDecreaseDay; }
		set { _gameData.LastHungerDecreaseDay = value; }
	}
	public float increaseHunger
	{
		get { return _gameData.increaseHunger; }
		set { _gameData.increaseHunger = value; }
	}

	public float increaseStress
	{
		get { return _gameData.increaseStress; }
		set { _gameData.increaseStress = value; }
	}


	#endregion

	//��¥�� �������� ���� �߻��ϴ� ���� ��ȭ �̺�Ʈ -> Extension���� Random �����ؼ� ���� �ʾҳ�?





	//�ʱ�ȭ �κ��̹Ƿ�, �Ź� Init�������� �ʴ´�.
	public void Init()
	{
		//���� StartData�� ������ �ʰ�, Init�� �ٷ� �����൵ ���� ������?
		//���߿� ��ȭ�� Stat��ó�� �ʱⰪ�鸸 StartData�� ���°� ���� ����.
		StartData data = Managers.Data.Start;

		Name = "NoName";
		Stress = 0;
		Hunger = 0;
		MaxStress = 100;
		MaxHunger = 100;

		Money = 0;
		GameMoney = 0;

		PlayTime = 0.0f;
		GameDay = 1;
		MaxGameDay = 365;
		SecondPerGameDay = 1;
		LastStressIncreaseDay = 0;
		LastHungerDecreaseDay = 0;

		HungerPercent = 2;
		StressPercent = 3;

		increaseHunger = 5.0f;
		increaseStress = 4.0f;


	}

	#region Save & Load	
	public string _path = Application.persistentDataPath + "/SaveData.json";

	public void SaveGame()
	{
		string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
		File.WriteAllText(_path, jsonStr);
		Debug.Log($"Save Game Completed : {_path}");
	}

	public bool LoadGame()
	{
		if (File.Exists(_path) == false)
			return false;

		string fileStr = File.ReadAllText(_path);
		GameData data = JsonUtility.FromJson<GameData>(fileStr);
		if (data != null)
		{
			Managers.Game.SaveData = data;
		}

		Debug.Log($"Save Game Loaded : {_path}");
		return true;
	}
    #endregion


}
