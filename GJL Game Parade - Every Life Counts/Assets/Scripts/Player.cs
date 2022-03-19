using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public delegate void DeathAction(int id, ItemType type, bool restart = false);
public delegate void ButtonAction(int id);
public delegate void RestartAction();

public class Player : MonoBehaviour
{

#region instance_vars_and_events
    public GameObject deadPlayerBlock;

    public readonly Vector2 jumpVelocity = new Vector2(0,9f);
    readonly Vector2 horizontalAcceleration = new Vector2(0.5f, 0);
    readonly float horizontalSpeedcap = 5f;
    readonly float verticalSpeedcap = 14f;
    Rigidbody2D ownRigidBody;

    int spawned_blocks = 0;
    public ItemType currentClothes = ItemType.solid;

    public event DeathAction Dead;
    public event ButtonAction ButtonPressed;
    public event ButtonAction ButtonUnpressed;

    public event RestartAction Restart;
    int numObjsButtonIsPressedBy = 0;

    string rightKey = "d";
    string leftKey = "a";

#endregion

#region inputs_and_basics
    public Player() { }

    // Start is called before the first frame update
    void Start()
    {
        ownRigidBody = GetComponent<Rigidbody2D>();
        Restart += () => Die(restart: true);
    }

    // FixedUpdate makes the cube not jump good
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);

        if (Input.GetKeyDown("space")) {
            TryJump();
        }
        CheckVertSpeed(verticalSpeedcap);
        
        if (Input.GetKeyDown("backspace"))
        {
            OnRestart();
        }
    }
    
    // Otherwise movement would be tied to frame rate
    void FixedUpdate()
    {
        
        if (Input.GetKey(rightKey))
        {
            MoveRight();
        }

        if (Input.GetKey(leftKey))
        {
            MoveLeft();
        }  
    }
#endregion

#region triggers and button stuff
    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject gameObj = collider.gameObject;
        if (gameObj.layer == 8)
        {
            Die();
        }
        if (gameObj.layer == 10)
        {
            PickupItem(gameObj);           
        }
        if (gameObj.layer == 11)
        {
            PressButton(gameObj.GetComponent<Button>().buttonNum);
        }
        if (gameObj.layer == 12)//this will break if this object didn't start as a normal 1x1 square
        {//this is to make you be able to step over short objects
            Vector3 step = new Vector3(0, 0.13f);

            Vector3 source = transform.position + new Vector3(0, -transform.localScale.y / 2);
            float extension = 0.4f;
            RaycastHit2D freeBlocksLeft = Physics2D.Raycast(source, Vector2.left, extension, LayerMask.GetMask("Free Blocks"));
            RaycastHit2D freeBlocksRight = Physics2D.Raycast(source, Vector2.right, extension, LayerMask.GetMask("Free Blocks"));
            if (freeBlocksLeft.collider != null && Input.GetKey(leftKey) || freeBlocksRight.collider != null && Input.GetKey(rightKey)) {
                transform.position += step;
            }
            
        }

    }

    void OnTriggerExit2D(Collider2D collider) {

        GameObject gameObj = collider.gameObject;
        if (gameObj.layer == 11)
        {
            UnpressButton(gameObj.GetComponent<Button>().buttonNum);
        }

    }

    public void PressButton(int buttonNum) {

        if(numObjsButtonIsPressedBy == 0) {
            ButtonPressed?.Invoke(buttonNum);
        }
        numObjsButtonIsPressedBy++;

    }

    public void UnpressButton(int buttonNum) {

        if(numObjsButtonIsPressedBy <= 0) Debug.Log("whelp, you've unpressed the unpressed button, but if you just restarted that's ok");
        if(numObjsButtonIsPressedBy == 1) {
            ButtonUnpressed?.Invoke(buttonNum);
            numObjsButtonIsPressedBy--;
        }
        
    }

    public void FullyUnpressButton(int buttonNum) {

        if(numObjsButtonIsPressedBy >= 1) {
            numObjsButtonIsPressedBy = 1;
            UnpressButton(buttonNum);
        }

    }

#endregion

#region movement
    void TryJump()
    {
        var left_side = transform.position - new Vector3(transform.localScale.x / 2, 0);
        var right_side = transform.position + new Vector3(transform.localScale.x / 2, 0);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.4f, LayerMask.GetMask("Ground", "Deaths","Free Blocks"));
        RaycastHit2D hit_left = Physics2D.Raycast(left_side, Vector2.down, 0.4f, LayerMask.GetMask("Ground", "Deaths","Free Blocks"));
        RaycastHit2D hit_right = Physics2D.Raycast(right_side, Vector2.down, 0.4f, LayerMask.GetMask("Ground", "Deaths","Free Blocks"));

        if (hit.collider != null || hit_left.collider != null || hit_right.collider != null)
        {
            ownRigidBody.velocity += jumpVelocity;
            CheckVertSpeed(jumpVelocity.y);
        }
    }

    void MoveRight()
    {
        if((ownRigidBody.velocity + horizontalAcceleration).x >= horizontalSpeedcap)
        {
            ownRigidBody.velocity.Set(horizontalSpeedcap, ownRigidBody.velocity.y);
        }
        else { ownRigidBody.velocity += horizontalAcceleration; }
    }

    void MoveLeft()
    {
        if ((ownRigidBody.velocity + horizontalAcceleration).x <= -horizontalSpeedcap)
        {
            ownRigidBody.velocity.Set(-horizontalSpeedcap, ownRigidBody.velocity.y);
        }
        else { ownRigidBody.velocity -= horizontalAcceleration; }
    }

    void CheckVertSpeed(float vertSpeedcap) {
            
        if(ownRigidBody.velocity.y > vertSpeedcap)
        {
            ownRigidBody.velocity = new Vector2(ownRigidBody.velocity.x, vertSpeedcap);
        }
        else if (ownRigidBody.velocity.y < -vertSpeedcap)
        {
            ownRigidBody.velocity = new Vector2(ownRigidBody.velocity.x, -vertSpeedcap);
        }
    }

    #endregion

#region death
    public void Die(bool restart = false)
    {
        if (!restart)
        {
            SpawnDeadPlayerBlock();
        }
        OnDeath(restart);
        currentClothes = ItemType.solid;
    }

    void SpawnDeadPlayerBlock()
    {
        if (transform.position != GetComponent<ResetOnDeath>().startingPosition)
        {
            GameObject death_block = Instantiate(deadPlayerBlock, transform.position, transform.rotation);
            death_block.GetComponent<DeadPlayer>().ID = spawned_blocks;
            spawned_blocks++;
        }
    }

    protected virtual void OnDeath(bool restart = false)
    {
        Dead?.Invoke(spawned_blocks - 1, currentClothes, restart);
    }

    protected virtual void OnRestart()
    {
        Restart?.Invoke();
    }

#endregion

#region items

    void PickupItem(GameObject gameObj) {

        currentClothes = gameObj.GetComponent<Item>().type;
        gameObj.transform.position = new Vector3(-500,-500);
        if (gameObj.GetComponent<Item>() != null) {
            gameObj.GetComponent<Item>().isDestroyed = true;
        } else {
            Debug.Log("uhhh... what the hell did you just pickup?");
        }

    }

#endregion

}
