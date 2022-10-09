using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Enums;

public class Button : MonoBehaviour
{
    
    public int buttonNum;
    public float pressHeight = 0.2f;
    Vector3 pressMotion;
    Transform visualTransform;
    
    void Awake()
    {
        visualTransform = GetComponentInChildren<Transform>().Find("ButtonVisual");

        // uhh neither way is negative lmao
        pressMotion = pressHeight * visualTransform.up;
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.ButtonPressed += GetPressed;
        Thread.Sleep(750);//why??
        player.ButtonUnpressed += GetUnpressed;
    }


    void GetPressed(int id) {

        
        if (buttonNum == id) {
            visualTransform.position -= pressMotion;
        }

    }

    void GetUnpressed(int id) {
        
        if (buttonNum == id) {
            visualTransform.position += pressMotion;
        }

    }

}
