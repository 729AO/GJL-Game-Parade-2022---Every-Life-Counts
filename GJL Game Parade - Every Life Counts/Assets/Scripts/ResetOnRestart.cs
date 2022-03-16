using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

using ObjectInterfaces;

public class ResetOnRestart : MonoBehaviour
{
    Player player;
    List<IConsumable> consumables;

    void Start()
    {
        consumables = new List<IConsumable>();
        player = GameObject.Find("Player").GetComponent<Player>();
        player.Restart += ExecuteRestart;
        FindAllIConsumables();
    }

    private void FindAllIConsumables()
    {
        GameObject[] raw_consumables = GameObject.FindGameObjectsWithTag("Consumable");
        
        foreach (GameObject item in raw_consumables)
        {
            if(item.GetComponent<Item>() != null)
            {
                consumables.Add(item.GetComponent<Item>());
            }
        }
    }

    private void ExecuteRestart()
    {

        player.Die(restart: true);

        if (gameObject.layer == 9)
        {
            player.Restart -= ExecuteRestart;
            Destroy(gameObject);
        }

        ResetConsumables();

    }

    private void ResetConsumables()
    {
        foreach(IConsumable consumable in consumables)
        {
            consumable.consumableObject.GetComponent<Rigidbody2D>().velocity = consumable.starting_velocity;
            consumable.consumableObject.GetComponent<Transform>().position = consumable.starting_position;
        }
    }
}
