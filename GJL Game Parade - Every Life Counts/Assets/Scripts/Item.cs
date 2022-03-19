using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

using ObjectInterfaces;

public class Item : MonoBehaviour
{
    public ItemType type;
    public Vector2 starting_velocity { get; set; }
    public Vector2 starting_position { get; set; }
    public bool isDestroyed = false;

    private void Awake()
    {
        starting_velocity = Vector2.zero;
        starting_position = transform.position;
        
        if (gameObject.name == "lvl1Item")
        {
            starting_velocity = new Vector2(2,0);
            GetComponent<Rigidbody2D>().velocity = starting_velocity;
        }

        GetComponent<ResetOnDeath>().Reset += AddStartingVelocity;
    }

    private void AddStartingVelocity()
    {
        GetComponent<Rigidbody2D>().velocity = starting_velocity;
    }

}
