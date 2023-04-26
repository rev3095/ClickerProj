using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오류날 경우, 한 줄 짜리 SingleJson으로 변환시켜주기.
[Serializable]
public class StartData
{
    public int ID;

    public float difHunger;
    public float difStress;

    public float cooltimePercent;
}

