using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ComputerPopup : UI_Popup
{
    public void Start()
    {
        StartCoroutine(delayTime(4.5f));
        Managers.UI.ClosePopupUI(this);
    }

    IEnumerator delayTime(float value)
    {
        yield return new WaitForSeconds(value);
    }

}
