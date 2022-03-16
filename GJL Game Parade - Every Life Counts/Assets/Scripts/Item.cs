using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Item : MonoBehaviour
{
    public ItemType type;

    private void Start()
    {
        if (gameObject.name == "lvl1Item")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(2, 0);
        }
    }

}
