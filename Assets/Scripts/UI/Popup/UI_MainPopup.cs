//using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Define;


public class UI_MainPopup : UI_Popup
{
	enum Texts
	{
		GameDayText,
		StressBarText,
		HungerBarText,
		MoneyText,
		GameMoneyText,
	}

	enum Buttons
	{
		TemplateButton1,
		TemplateButton2,

		//Template2
		ChoiceButton1,
		ChoiceButton2,
		ChoiceButton3,

		//Template1
		ChoicePanel1,
		ChoicePanel2,
		ChoicePanel3,
		ChoicePanel4,
		ChoicePanel5,

		VisibleButton,

		CharacterInfoButton,
		//TutorialButton,
	}

	enum Images
	{
		StressBarFill,
		HungerBarFill,
		ArrowIcon1,
		ArrowIcon2,
		TemporaryBackground,
	}

	enum GameObjects
	{
		StressBar,
		HungerBar,
		StressBarFill,
		HungerBarFill,
		ChoiceButtons, //Template2
		MainDownPanel, //Template1
		Arrow,
		Template1, //AbilityTab
		ChoicePanelContent, //Content
		Template2,
		ChoiceButtonContent,
	}


	private GameManager _game;
	public float GameDaySpeed = 1f;
	public float lerpSpeed = 0.5f;

	public bool canBroadcast = false;

	//public Action OnClickTemplate1; -> 액션 문서 참고하기
	public bool OnClickTemplate1 = false;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		_game = Managers.Game;

		BindText(typeof(Texts));
		BindButton(typeof(Buttons));
		BindObject(typeof(GameObjects));
		BindImage(typeof(Images));


		GetButton((int)Buttons.CharacterInfoButton).gameObject.BindEvent(OnClickCharacterInfoButton);
		GetButton((int)Buttons.TemplateButton1).gameObject.BindEvent(OnClickTemplateButton1);
		GetButton((int)Buttons.TemplateButton2).gameObject.BindEvent(OnClickTemplateButton2);

		//Template1
		GetButton((int)Buttons.ChoicePanel1).gameObject.BindEvent(OnClickChoicePanel1);
		GetButton((int)Buttons.ChoicePanel2).gameObject.BindEvent(OnClickChoicePanel2);
		GetButton((int)Buttons.ChoicePanel3).gameObject.BindEvent(OnClickChoicePanel3);
		GetButton((int)Buttons.ChoicePanel4).gameObject.BindEvent(OnClickChoicePanel4);
		GetButton((int)Buttons.ChoicePanel5).gameObject.BindEvent(OnClickChoicePanel5);

		//Template2
		GetButton((int)Buttons.ChoiceButton1).gameObject.BindEvent(OnClickChoiceButton1);
		GetButton((int)Buttons.ChoiceButton2).gameObject.BindEvent(OnClickChoiceButton2);
		GetButton((int)Buttons.ChoiceButton3).gameObject.BindEvent(OnClickChoiceButton3);

		GetButton((int)Buttons.VisibleButton).gameObject.BindEvent(OnClickVisibleButton);
		
		//Default
		GetImage((int)Images.TemporaryBackground).gameObject.SetActive(false);


		ChangeTemplate(1); //템플릿 수정 코드, 처음 템플릿은 1부터 보여줌.

		RefreshUI();
		StartCoroutine(CoSaveGame(10.0f));

		return true;
	}


	private void Update()
	{
		if (Managers.UI.PeekPopupUI<UI_MainPopup>() != this)
			return;

		// 1/현재fps가 deltaTime이므로, PlayTime은 현재 시간 1초를 기준으로 흘러가도록 설정됨.
		// GameDay는 GameManager에서 5초 간격으로 하루가 흘러가도록 설정됨. ( 5초 간격에서 5를 결정하는 건, 변수로 따로 GameDayRatio로 뺄 예정 )
		_game.PlayTime += Time.deltaTime ;
		RefreshTime();

		int gameDay = _game.GameDay;
		// 체력 감소
		if (_game.LastHungerDecreaseDay < gameDay)
		{
			/*
			float HungerDecreaseDay = 7.5f;
			int diffHp = (int)(_game.MaxHunger * HungerDecreaseDay / 100);
			_game.Hunger = Math.Max(0, _game.Hunger - diffHp);
			_game.LastHungerDecreaseDay = gameDay;
			RefreshHungerBar();
			*/
			_game.LastHungerDecreaseDay = gameDay;
			

		}

		// 스트레스 증가
		if (_game.LastStressIncreaseDay < gameDay)
		{
			_game.LastStressIncreaseDay = gameDay;
			
		}

		//Money, GameMoney 획득량
		_game.Money += Time.deltaTime;
		_game.GameMoney += Time.deltaTime * 2; //deltaTime 붙이지 않으면, 프레임 단위로 실행되기 때문에 순식간에 촤라락 돈이 증가된다.

		//Stress,Hunger 중첩량
		_game.Stress += Time.deltaTime * Managers.Game.increaseStress;
		_game.Hunger += Time.deltaTime * Managers.Game.increaseHunger;

		RefreshUI(); //Money,Time 재설정
	}

	
	public void RefreshUI()
	{
		if (_init == false)
			return;

		RefreshMoney();
		RefreshGameMoney();
		RefreshTime();

		RefreshHungerBar();
		RefreshStressBar();
	}

	public void RefreshTime()
	{
		int gameday = _game.GameDay;
		GetText((int)Texts.GameDayText).text = $"{gameday}";
	}

	public void RefreshHungerBar()
	{
		float hungerPercent = _game.HungerPercent;
		Image a = GetImage((int)Images.HungerBarFill);
		//a.fillAmount = hungerPercent / 100; // 보간값에 Time.deltaTime * lerpSpeed 또는 lerpSpeed를 넣어도 작동하긴 함.
		a.fillAmount = Mathf.Lerp(_game.Hunger / _game.MaxHunger, a.fillAmount, Time.deltaTime * 20);

		// -> 추가 : a.fillAmount을 Horizontal Left, 1로 설정했으므로 0에서 1로 왼->오른쪽으로 증가할 것, 이때 Hunger 0일때 0/MaxHunger = 0이므로 0부터 시작하여, 점차 증가되는 방식으로 진행됨.

		GetText((int)Texts.HungerBarText).text = $"Hunger : {(int)hungerPercent}%";
	}

	public void RefreshStressBar()
	{
		float stressPercent = _game.StressPercent;
		GetImage((int)Images.StressBarFill).fillAmount = Mathf.Lerp(_game.Stress / _game.MaxStress, GetImage((int)Images.StressBarFill).fillAmount, Time.deltaTime * 20);
		GetText((int)Texts.StressBarText).text = $"Stress : {(int)stressPercent}%";
	}

	public void RefreshMoney()
	{
		if (GetText((int)Texts.MoneyText).text != Utils.GetMoneyString((int)_game.Money,"Money"))
		{
			
			GetText((int)Texts.MoneyText).text = Utils.GetMoneyString((int)_game.Money, "Money");
		}
	}

	public void RefreshGameMoney()
    {
		if(GetText((int)Texts.GameMoneyText).text != Utils.GetMoneyString((int)_game.GameMoney, "GameMoney"))
        {
			GetText((int)Texts.GameMoneyText).text = Utils.GetMoneyString((int)_game.GameMoney, "GameMoney");

        }
    }
    void OnClickCharacterInfoButton()
	{
		Debug.Log("OnClickPlayerInfoButton");
		//Managers.Sound.Play(Sound.Effect, "Sound_FolderItemClick");
		Managers.UI.ShowPopupUI<UI_CharacterInfoPopup>();
	}

	public void OnClickTemplateButton1()
    {
		OnClickTemplate1 = true;
		GetObject((int)GameObjects.Template1).GetComponent<ScrollRect>().ResetVertical();
		ChangeTemplate(1);
		
    }

	public void OnClickTemplateButton2()
	{
		OnClickTemplate1 = false;
		GetObject((int)GameObjects.Template2).GetComponent<ScrollRect>().ResetHorizontal();
		ChangeTemplate(2);
	}

	public void ChangeTemplate(int value)
    {
		GetObject((int)GameObjects.Template1).gameObject.SetActive(false);
		GetObject((int)GameObjects.Template2).gameObject.SetActive(false);

		
		GetObject((int)GameObjects.Template1 - 1 + value).gameObject.SetActive(true);
	}

    #region Template1 Buttons

	public void OnClickChoicePanel1()
    {
		_game.Stress -= 50;
		_game.Hunger += 10;
		RefreshUI();
    }

	public void OnClickChoicePanel2()
    {
		_game.Stress += 10;
		_game.Hunger -= 50;
		RefreshUI();
    }

	public void OnClickChoicePanel3()
    {
		Managers.UI.ShowPopupUI<UI_ComputerPopup>();
    }

	public void OnClickChoicePanel4()
    {
		if(canBroadcast ==false)
        {
			Debug.Log("Can't Broadcast");
        }
        else
        {
			//Managers.UI.ShowPopup<UI_BroadCast>();
        }
    }

	public void OnClickChoicePanel5()
    {
		Debug.Log("Dummy Button");

	}

    #endregion

    #region Template2 Buttons
	
	public void OnClickChoiceButton1()
    {

    }
	public void OnClickChoiceButton2()
	{

	}
	public void OnClickChoiceButton3()
	{

	}



	#endregion

	public void OnClickVisibleButton()
    {
		if(OnClickTemplate1 == true)
        {
			Debug.Log("Plz Click Template2");
        }
        else
        {
			GetImage((int)Images.TemporaryBackground).gameObject.SetActive(true);
		}

		
    }
	IEnumerator CoSaveGame(float interval)
	{
		while (true)
		{
			yield return new WaitForSeconds(interval);
			Managers.Game.SaveGame();
		}
	}
}
