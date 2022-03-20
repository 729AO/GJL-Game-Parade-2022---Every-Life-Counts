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
    
    void Awake()
    {
        // uhh neither way is negative lmao
        pressMotion = pressHeight * transform.up;
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.ButtonPressed += GetPressed;
        Thread.Sleep(750);
        player.ButtonUnpressed += GetUnpressed;
    }


    void GetPressed(int id) {

        
        if (buttonNum == id) {
            transform.position -= pressMotion;
        }

    }

    void GetUnpressed(int id) {
        
        if (buttonNum == id) {
            transform.position += pressMotion;
        }

    }

}
