using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
	//IPointerUpHandler : void OnPointerUp �Լ��� ����� �� �ְ� ����. > ��ư�� Ŭ��/��ġ"�ϴ� ����" �����
	//IPointerClickHandler : void OnPointerClick �Լ��� ����� �� �ְ� ����. > Ŭ��/��ġ�� �����ϴ� �̺�Ʈ�� �����
	//OnPressedHandler: �����ų� ��ġ�ϰ� �ִ� ����
	//IPointerDownHandler : void OnPointerDown �Լ��� ����� �� �ְ� ����. > ��ư�� Ŭ��/��ġ�� �� �� �����
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
		//��ư pressed�� -> true;
		_pressed = true;
		OnPointerDownHandler?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//��ư pressed ������ -> false;
		_pressed = false;
		OnPointerUpHandler?.Invoke();
	}
}
