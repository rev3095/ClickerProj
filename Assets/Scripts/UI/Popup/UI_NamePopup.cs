using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_NamePopup : UI_Popup
{
	enum GameObjects
	{
		NameInputField,
	}

	enum Texts
	{
		ConfirmButtonText,
		HintText,
		ValueText
	}

	enum Buttons
	{
		ConfirmButton
	}

	TMP_InputField _inputField;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindObject(typeof(GameObjects));
		BindText(typeof(Texts));
		BindButton(typeof(Buttons));

		GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

		_inputField = GetObject((int)GameObjects.NameInputField).gameObject.GetComponent<TMP_InputField>();
		_inputField.text = "";

		RefreshUI();

		return true;
	}

	//�ؽ�Ʈ �����ϴ� �κ�
	void RefreshUI()
	{
		GetText((int)Texts.HintText).text = Managers.GetText(Define.NameHintText);
	}
	void OnClickConfirmButton()
	{
		//Managers.Sound.Play(Sound.Effect, ("Sound_Checkbutton"));
		Debug.Log("OnClickConfirmButton");
		Debug.Log($"Input ID {_inputField.text}");

		Managers.Game.Name = _inputField.text;

		// UI_NamePopup �ݱ�
		Managers.UI.ClosePopupUI(this);

		/*
		Managers.UI.ShowPopupUI<UI_IntroPopup>().SetInfo((int)UI_IntroPopup.GameObjects.Intro1, (int)UI_IntroPopup.GameObjects.Intro3, () =>
		{
			Managers.UI.ShowPopupUI<UI_PlayPopup>();
		});
		*/
		Managers.UI.ShowPopupUI<UI_MainPopup>();
	}
}
