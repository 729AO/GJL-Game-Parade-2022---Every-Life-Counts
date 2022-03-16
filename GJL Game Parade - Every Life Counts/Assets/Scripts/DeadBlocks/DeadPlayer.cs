using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class DeadPlayer : MonoBehaviour
{

    public int ID;
    Player player;
    public GameObject deadCannonBlock;

    void Awake() {   
        player = GameObject.Find("Player").GetComponent<Player>();
        player.Dead += (int id, ItemType type) => SetParameters(type, id);

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void SetParameters(ItemType type, int target_id) {

        BoxCollider2D collider;

        try
        {
            collider = GetComponent<BoxCollider2D>();
        }
        catch (MissingReferenceException)
        {
            return;
        }
        

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
            case ItemType.cannon:
                if(!(this is DeadCannon)) {
                    Instantiate(deadCannonBlock, transform.position, deadCannonBlock.transform.rotation);
                    Destroy(gameObject);
                }
                break;
            default:
                break;

        }

        CheckIfOnButton();
    }

    void CheckIfOnButton() {

        var left_side = transform.position - new Vector3(0.375f, 0);
        var right_side = transform.position - new Vector3(0.375f, 0);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.4f, LayerMask.GetMask("Buttons"));
        RaycastHit2D hit_left = Physics2D.Raycast(left_side, Vector2.down, 0.4f, LayerMask.GetMask("Buttons"));
        RaycastHit2D hit_right = Physics2D.Raycast(right_side, Vector2.down, 0.4f, LayerMask.GetMask("Buttons"));

        if (hit.collider != null) {

            if (hit.collider.gameObject.GetComponent<Button>() != null) {
                player.PressButton(hit.collider.gameObject.GetComponent<Button>().buttonNum);

            } else if (hit_left.collider.gameObject.GetComponent<Button>() != null) {
                player.PressButton(hit_left.collider.gameObject.GetComponent<Button>().buttonNum);

            } else if (hit_right.collider.gameObject.GetComponent<Button>() != null) {
                player.PressButton(hit_right.collider.gameObject.GetComponent<Button>().buttonNum);
            }

        }


    }

}
