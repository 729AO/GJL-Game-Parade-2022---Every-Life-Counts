using System.Collections;
using System.Collections.Generic;
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
        player.ButtonPressed += (int id, ItemType hopefullyNull) => GetPressed(id);
        player.ButtonUnpressed += (int id, ItemType hopefullyNull) => GetUnpressed(id);
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
