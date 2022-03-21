using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

using ObjectInterfaces;

/// <summary> used to restart any objects that have restart rules different from death rules </summary>
public class ResetOnRestart : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        player.Restart += ExecuteRestart;
    }

    private void ExecuteRestart()
    {

        if (gameObject.layer == 9)
        {
            player.Restart -= ExecuteRestart;
            Destroy(gameObject);
        }

        if (gameObject.layer == 10) {
            if (GetComponent<Rigidbody2D>() == null || GetComponent<Item>() == null) Debug.Log("your item only partly exists");
            GetComponent<Rigidbody2D>().velocity = GetComponent<Item>().starting_velocity;
            transform.position = GetComponent<Item>().starting_position;
            GetComponent<Item>().isDestroyed = false;
        }

        if (gameObject.layer == 11) {
            if (GetComponentInParent<Button>() == null) Debug.Log("another case of partial existence...");
            player.FullyUnpressButton(gameObject.GetComponentInParent<Button>().buttonNum);
        }

    }

}
