using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

//enum으로 OnClickEvent하는 방식과 주원이처럼 switch(btn) 으로 case Buttons.SleepButton; 이렇게 하나하나 SetInfo 할 수도 있음.

public class UI_ConfirmPopup : UI_Popup
{
	public enum Texts
	{
		MessageText
	}

	enum Buttons
	{
		YesButton,
		NoButton
	}

	string _text;


	//똑같은 코드임에도 여기 안에 있는 텍스트를 SetInfo 로 바꿔주는 것.
	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindText(typeof(Texts));
		BindButton(typeof(Buttons));

		GetButton((int)Buttons.YesButton).gameObject.BindEvent(OnClickYesButton);
		GetButton((int)Buttons.NoButton).gameObject.BindEvent(OnClickNoButton);
		GetText((int)Texts.MessageText).text = _text;

		RefreshUI();
		return true;
	}

	Action _onClickYesButton;

	public void SetInfo(Action onClickYesButton, string text)
	{
		_onClickYesButton = onClickYesButton;
		_text = text;

		RefreshUI();
	}

	void RefreshUI()
	{
		if (_init == false)
			return;

	}

	void OnClickYesButton()
	{
		Managers.UI.ClosePopupUI(this);
		//Managers.Sound.Play(Sound.Effect, "Sound_CheckButton");
		if (_onClickYesButton != null)
			_onClickYesButton.Invoke();
	}

	void OnClickNoButton()
	{
		//Managers.Sound.Play(Sound.Effect, "Sound_CancelButton");
		OnComplete();
	}


	void OnComplete()
	{
		Managers.UI.ClosePopupUI(this);
	}
}
