using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ���, �� �� ¥�� SingleJson���� ��ȯ�����ֱ�.
[Serializable]
public class Texts
{
	public int ID;
	public string kor;
}

[Serializable]
public class TextData : ILoader<int, Texts>
{
	public List<Texts> texts = new List<Texts>();
	public Dictionary<int, Texts> MakeDict()
    {
		Dictionary<int, Texts> dict = new Dictionary<int, Texts>();
		foreach(Texts texts in texts)
        {
			dict.Add(texts.ID, texts);
        }
		return dict;
    }
}
