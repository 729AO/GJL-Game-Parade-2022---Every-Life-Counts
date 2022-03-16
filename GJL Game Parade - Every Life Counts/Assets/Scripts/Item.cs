using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Item : MonoBehaviour
{
    public ItemType type;
    public Vector3 startVelocity;

    private void Start()
    {
        if (gameObject.name == "lvl1Item")
        {
            GetComponent<Rigidbody2D>().velocity = startVelocity;
        }

        GetComponent<ResetOnDeath>().Reset += AddStartingVelocity;
    }

    private void AddStartingVelocity()
    {
        GetComponent<Rigidbody2D>().velocity = startVelocity;
    }

}
