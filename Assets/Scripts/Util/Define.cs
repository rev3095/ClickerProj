using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
	public enum UIEvent
	{
		Click,
		Pressed,
		PointerDown,
		PointerUp,
	}

	public enum Scene
	{
		Unknown,
		Game,
	}

	public enum Sound
	{
		Bgm,
		Sfx_Eating,
		Sfx_Playing,
		Sfx_Sleeping,
		Sfx_Broadcast1,
	}

	public enum AnimState
	{
		None,
		Idle,
		Playing,
		Eating,
		Sleeping,
	}

	public enum HouseType
	{
		None = 0,
		OneRoom,
		Apartment,
		Office,
		Building,
	}

	public enum StatType
	{
		Stress,
		Hunger,
	}

	//UI 관련 텍스트
	public const int DataResetConfirm = 1000;
	public const int NameHintText = 1001;

}
