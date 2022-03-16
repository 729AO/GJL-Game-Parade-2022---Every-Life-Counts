using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class DeadPlayer : MonoBehaviour
{

    public int ID;
    Player player;

    void Awake() {   
        player = GameObject.Find("Player").GetComponent<Player>();
        player.Dead += (int id, ItemType type) => SetParameters(type, id);

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void SetParameters(ItemType type, int target_id) {

        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        if (target_id != ID)
        {
            return;
        }

        switch(type) {

            case ItemType.solid:
                PhysicsMaterial2D mSolid = Resources.Load<PhysicsMaterial2D>("Physics Materials/Solid");
                collider.sharedMaterial = mSolid;
                break;
            case ItemType.bouncy:
                PhysicsMaterial2D mBouncy = Resources.Load<PhysicsMaterial2D>("Physics Materials/Bouncy");
                collider.sharedMaterial = mBouncy;
                break;
            default:
                break;

        }
    }

}
