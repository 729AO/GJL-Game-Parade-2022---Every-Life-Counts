using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class DeadPlayer : MonoBehaviour
{
    
    ItemType type;

    void Awake() {
        Player player = GameObject.Find("Player").GetComponent<Player>();        
        player.Dead += (ItemType type) => SetParameters(type);
    }

    void SetParameters(ItemType type) {

        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        switch(type) {

            case ItemType.solid:
                collider.sharedMaterial = (PhysicsMaterial2D)Resources.FindObjectsOfTypeAll(typeof(PhysicsMaterial2D))[1];
                break;
            case ItemType.bouncy:
            //WE NEED TO MAKE THIS SEARCH FOR THE APPROPRIATE MATERIAL USING BINARYSEARCH(ARRAY, OBJECT, ICOMPARER), BUT THAT REQUIRES A WHOLE COMPARATOR
                collider.sharedMaterial = (PhysicsMaterial2D)Resources.FindObjectsOfTypeAll(typeof(PhysicsMaterial2D))[0];
                break;
            default:
                break;

        }

    }

}
