using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class MovableBar : MonoBehaviour
{
    public int associatedButton;
    public Vector3 movement;

    void Awake()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.ButtonPressed += (int id, ItemType hopefullyNull) => Move(id);
        player.ButtonUnpressed += (int id, ItemType hopefullyNull) => MoveBack(id);
    }

    void Move(int id) {

        if (associatedButton == id) {
            transform.position += movement;
        }

    }

    void MoveBack(int id) {

        if (associatedButton == id) {
            transform.position -= movement;
        }

    }

}
