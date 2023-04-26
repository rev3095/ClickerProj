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

		//원래는 On click Event를 통해서 실행될 함수를 연결하는 방식이 일반적이지만, 이렇게 코드상에서 조립하는, 스크립트로 관리하는게 더 효과적임.
		//BindEvent는 Util->Extension에서 이용.
		GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
		GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
		//GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
		//GetButton((int)Buttons.CollectionButton).gameObject.BindEvent(OnClickCollectionButton);
		return true;
	}

	void OnClickStartButton()
	{
		Debug.Log("OnClickStartButton");

		// 데이터 있는지 확인, 로드할 데이터가 있을 경우 현재 데이터를 삭제하고 새로 시작할지 확인하는 부분.
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
