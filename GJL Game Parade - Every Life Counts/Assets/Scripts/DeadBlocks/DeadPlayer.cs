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
        player.Dead += (int id, ItemType type, bool restart) => {
            if (restart == false) SetParameters(type, id);
        };

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
                PhysicsMaterial2D mSolid = Resources.Load<PhysicsMaterial2D>("Physics Materials/Much Friction");
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



    // //is there a way to just ask like if I'm in a trigger?
    // void CheckIfOnButton() {

    //     var left_side = transform.position - new Vector3(transform.localScale.x / 2, 0);
    //     var right_side = transform.position + new Vector3(transform.localScale.x / 2, 0);

    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.4f, LayerMask.GetMask("Buttons"));
    //     RaycastHit2D hit_left = Physics2D.Raycast(left_side, Vector2.down, 0.4f, LayerMask.GetMask("Buttons"));
    //     RaycastHit2D hit_right = Physics2D.Raycast(right_side, Vector2.down, 0.4f, LayerMask.GetMask("Buttons"));

    //     //if for some reason something is in the "Button" layer without a Button component there will be an error
    //     if (hit.collider != null) {
    //         player.PressButton(hit.collider.gameObject.GetComponent<Button>().buttonNum);

    //     } else if (hit_left.collider != null) {
    //         player.PressButton(hit_left.collider.gameObject.GetComponent<Button>().buttonNum);

    //     } else if (hit_right.collider != null) {
    //         player.PressButton(hit_right.collider.gameObject.GetComponent<Button>().buttonNum);
    //     }

    // }


    //this might be good to change in case the button is thinner than the block
    //also if we want to make some moving death-blocks, we can use OnTriggerStay2D (with an extra bool so it doesn't continuously unpress a button, but like so this is not an expensive calculation)
    //and OnTriggerExit/Enter2D and change the rigidbody to kinematic
    //we just have to freeze the x and y positions for the normal blocks
    void CheckIfOnButton() {

        float side_length_x = transform.localScale.x;
        float side_length_y = transform.localScale.y;

        float extension = 0.38f;

        var up = transform.position + new Vector3(0, side_length_y / 2);
        var right = transform.position + new Vector3(side_length_x / 2, 0);
        var down = transform.position + new Vector3(0, -side_length_y / 2);
        var left = transform.position + new Vector3(-side_length_x / 2, 0);

        RaycastHit2D upLeft = Physics2D.Raycast(up, Vector2.left, extension, LayerMask.GetMask("Buttons"));
        RaycastHit2D upRight = Physics2D.Raycast(up, Vector2.right, extension, LayerMask.GetMask("Buttons"));
        RaycastHit2D rightUp = Physics2D.Raycast(right, Vector2.up, extension, LayerMask.GetMask("Buttons"));
        RaycastHit2D rightDown = Physics2D.Raycast(right, Vector2.down, extension, LayerMask.GetMask("Buttons"));

        RaycastHit2D downRight = Physics2D.Raycast(down, Vector2.right, extension, LayerMask.GetMask("Buttons"));
        RaycastHit2D downLeft = Physics2D.Raycast(down, Vector2.left, extension, LayerMask.GetMask("Buttons"));
        RaycastHit2D leftDown = Physics2D.Raycast(left, Vector2.down, extension, LayerMask.GetMask("Buttons"));
        RaycastHit2D leftUp = Physics2D.Raycast(left, Vector2.up, extension, LayerMask.GetMask("Buttons"));

        //if for some reason something is in the "Button" layer without a Button component there will be an error
        if (upLeft.collider != null) player.PressButton(upLeft.collider.gameObject.GetComponent<Button>().buttonNum);
        else if (upRight.collider != null) player.PressButton(upRight.collider.gameObject.GetComponent<Button>().buttonNum);
        else if (rightUp.collider != null) player.PressButton(rightUp.collider.gameObject.GetComponent<Button>().buttonNum);
        else if (rightDown.collider != null) player.PressButton(rightDown.collider.gameObject.GetComponent<Button>().buttonNum);

        else if (downRight.collider != null) player.PressButton(downRight.collider.gameObject.GetComponent<Button>().buttonNum);
        else if (downLeft.collider != null) player.PressButton(downLeft.collider.gameObject.GetComponent<Button>().buttonNum);
        else if (leftDown.collider != null) player.PressButton(leftDown.collider.gameObject.GetComponent<Button>().buttonNum);
        else if (leftUp.collider != null) player.PressButton(leftUp.collider.gameObject.GetComponent<Button>().buttonNum);

    }

}
