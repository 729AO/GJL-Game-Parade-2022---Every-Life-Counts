using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

//this and the levelend popup should have a shared interface or parent class
public abstract class AbstractPopup : MonoBehaviour
{

    public TMP_Text popupText;
    readonly string[] text;
    public Vector3 position;
    public Vector3 hiddenPos = new Vector3(0,10);

    public abstract void Show();

    public void Hide() {

        transform.position = hiddenPos;

    }

}