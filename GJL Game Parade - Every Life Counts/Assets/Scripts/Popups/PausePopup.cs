using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

//this and the levelend popup should have a shared interface or parent class
public class PausePopup : AbstractPopup
{
    readonly string[] text = {"You've paused the level"};

    public override void Show() {

        transform.position = position;

        popupText.text = text[0];

    }

}