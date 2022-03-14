using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnDeath : MonoBehaviour
{

    Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;

        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.Dead += PlayerDied;
    }

    private void PlayerDied()
    {
        transform.position = startingPosition;
        if(GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

}