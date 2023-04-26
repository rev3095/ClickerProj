using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_TitlePopup : UI_Popup
{
    enum Texts
    {   
        TitleText,
        StartButtonText,
        ContinueButtonText,
        SettingButtonText,
        CollectionButtonText,
    }

    enum Buttons
    {
        StartButton,
        ContinueButton,
        SettingButton,
        CollectionButton,
    
    }

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindText(typeof(Texts));
		BindButton(typeof(Buttons));

		//������ On click Event�� ���ؼ� ����� �Լ��� �����ϴ� ����� �Ϲ���������, �̷��� �ڵ�󿡼� �����ϴ�, ��ũ��Ʈ�� �����ϴ°� �� ȿ������.
		//BindEvent�� Util->Extension���� �̿�.
		GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
		GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
		//GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
		//GetButton((int)Buttons.CollectionButton).gameObject.BindEvent(OnClickCollectionButton);
		return true;
	}

	void OnClickStartButton()
	{
		Debug.Log("OnClickStartButton");

		// ������ �ִ��� Ȯ��, �ε��� �����Ͱ� ���� ��� ���� �����͸� �����ϰ� ���� �������� Ȯ���ϴ� �κ�.
		if (Managers.Game.LoadGame())
		{

			Managers.UI.ShowPopupUI<UI_ConfirmPopup>().SetInfo(() =>
			{
				Managers.Game.Init();
				Managers.Game.SaveGame();

				Managers.UI.ClosePopupUI(this); // UI_TitlePopup
				Managers.UI.ShowPopupUI<UI_NamePopup>();
			}, Managers.GetText(Define.DataResetConfirm));

		}
		else
		{
			Managers.Game.Init();
			Managers.Game.SaveGame();

			Managers.UI.ClosePopupUI(this); // UI_TitlePopup
			Managers.UI.ShowPopupUI<UI_NamePopup>();
		}
	}

	void OnClickContinueButton()
	{
		Debug.Log("OnClickContinueButton");

		Managers.Game.Init();
		Managers.Game.LoadGame();

		Managers.UI.ClosePopupUI(this);
		Managers.UI.ShowPopupUI<UI_MainPopup>();
	}
	/*
	void OnClickSettingButton()
	{
		Managers.Game.Init();
		Managers.Game.LoadGame();

		Managers.UI.SHowPopupUI<UI_SettingPopup>();
	}
	*/

	/*
	void OnClickCollectionButton()
	{
		Managers.Sound.Play(Sound.Effect, ("Sound_FolderItemClick"));
		Managers.Game.Init();
		Managers.Game.LoadGame();

		Debug.Log("OnClickCollectionButton");
		Managers.UI.ShowPopupUI<UI_CollectionPopup>();
	}
	*/
}
