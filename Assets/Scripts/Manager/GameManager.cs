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
	//기본 Stat
	public string Name;
	public float Stress;
	public float Hunger;

	public int MaxStress;
	public int MaxHunger;

	//기본 Stat 변화량 -> 이걸 게임데이터에 가질 필요 없이, GameManager 부분에서 변수로 다루는게 맞을듯. SaveData에 포함시킬 필요가 없음.

	//재화
	public float Money;
	public float GameMoney;

	//쿨타임 -> 나중에 쿨타임 감소량도 줄일 수 있게 만든다면 여기서 가지고 있는게 맞음.

	//시간 -> 계산식으로 PlayTime 하나만 가지고 처리할 수도 있겠지만, 편하게 GameDay를 하나 더 가지고 있자.
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

	//PlayTime으로 무언가 하는게 아니라 얜 진짜 5의 배수 카운팅해주는 변수네. ㅇㅋㅇㅋ
	//5 변수를 따로 만들어서 이걸 몇 초마다 Day 1씩 증가시킬지 int 로 빼는게 디버깅하는게 훨씬 좋을듯 
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

	//날짜가 지나감에 따라 발생하는 랜덤 대화 이벤트 -> Extension에서 Random 랩핑해서 쓰지 않았나?





	//초기화 부분이므로, 매번 Init시켜주지 않는다.
	public void Init()
	{
		//굳이 StartData를 가지지 않고, Init에 바로 적어줘도 되지 않을까?
		//나중에 변화할 Stat들처럼 초기값들만 StartData로 빼는게 좋아 보임.
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
