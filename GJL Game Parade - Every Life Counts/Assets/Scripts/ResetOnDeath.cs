using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class ResetOnDeath : MonoBehaviour
{

    public Vector3 startingPosition;
    Player player;

    private void Start()
    {
        startingPosition = transform.position;

        player = GameObject.Find("Player").GetComponent<Player>();
        player.Dead += (int id, ItemType type) => PlayerDied();
    }

    private void PlayerDied()
    {
        try
        {
            transform.position = startingPosition;
            if (GetComponent<Rigidbody2D>() != null)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        catch (UnityEngine.MissingReferenceException)
        { return; }
    }
}