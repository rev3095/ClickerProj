using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
	//IPointerUpHandler : void OnPointerUp 함수를 사용할 수 있게 해줌. > 버튼을 클릭/터치"하는 순간" 실행됨
	//IPointerClickHandler : void OnPointerClick 함수를 사용할 수 있게 해줌. > 클릭/터치를 감지하는 이벤트가 실행됨
	//OnPressedHandler: 누르거나 터치하고 있는 상태
	//IPointerDownHandler : void OnPointerDown 함수를 사용할 수 있게 해줌. > 버튼을 클릭/터치를 뗄 때 실행됨
	public Action OnClickHandler = null;
	public Action OnPressedHandler = null;
	public Action OnPointerDownHandler = null;
	public Action OnPointerUpHandler = null;

	bool _pressed = false;

	private void Update()
	{
		if (_pressed)
			OnPressedHandler?.Invoke();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClickHandler?.Invoke();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		//버튼 pressed됨 -> true;
		_pressed = true;
		OnPointerDownHandler?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//버튼 pressed 해제됨 -> false;
		_pressed = false;
		OnPointerUpHandler?.Invoke();
	}
}
