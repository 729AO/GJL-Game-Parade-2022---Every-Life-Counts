using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

using ObjectInterfaces;

public class Item : MonoBehaviour, IConsumable
{
    public ItemType type;
    public Vector2 starting_velocity { get; set; }
    public Vector2 starting_position { get; set; }
    public GameObject consumableObject { get; set; }
    public bool consumed { get; set; }

    private void Start()
    {

        starting_velocity = new Vector2(2, 0);
        starting_position = transform.position;
        consumableObject = gameObject;

        if (gameObject.name == "lvl1Item")
        {
            GetComponent<Rigidbody2D>().velocity = starting_velocity;
        }

        GetComponent<ResetOnDeath>().Reset += ResetHandler;
    }

    private void ResetHandler()
    {
        if (!consumed)
        {
            transform.position = starting_position;
        }
        GetComponent<Rigidbody2D>().velocity = starting_velocity;
    }

}
