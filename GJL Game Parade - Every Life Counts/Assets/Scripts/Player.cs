using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using ObjectInterfaces;

public delegate void MyAction(int id, ItemType type = ItemType.unspecified);
public delegate void RestartAction();

public class Player : MonoBehaviour
{

    public GameObject deadPlayerBlock;

    readonly Vector2 jumpVelocity = new Vector2(0,6.2f);
    readonly Vector2 horizontalAcceleration = new Vector2(0.5f, 0);
    readonly float horizontalSpeedcap = 4.5f;
    readonly float verticalSpeedcap = 10f;
    Rigidbody2D ownRigidBody;

    int spawned_blocks = 0;
    public ItemType currentClothes = ItemType.solid;

    public event MyAction Dead;
    public event MyAction ButtonPressed;
    public event MyAction ButtonUnpressed;

    public event RestartAction Restart;
    int numObjsButtonIsPressedBy = 0;

    public Player() { }

    // Start is called before the first frame update
    void Start()
    {
        ownRigidBody = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate makes the cube not jump good
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);

        if (Input.GetKey("space")) {
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
        
        if (Input.GetKey("d"))
        {
            MoveRight();
        }

        if (Input.GetKey("a"))
        {
            MoveLeft();
        }  
    }

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
            PickupItem(gameObj.GetComponent<Item>());
        }
        if (gameObj.layer == 11)
        {
            PressButton(gameObj.GetComponent<Button>().buttonNum);
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

        if(numObjsButtonIsPressedBy <= 0) Debug.Log("whelp, you've unpressed the unpressed button");
        if(numObjsButtonIsPressedBy == 1) {
            ButtonUnpressed?.Invoke(buttonNum);
        }
        numObjsButtonIsPressedBy--;
        
    }

#endregion

#region movement
    void TryJump()
    {

        var left_side = transform.position - new Vector3(transform.localScale.x / 2, 0);
        var right_side = transform.position + new Vector3(transform.localScale.x / 2, 0);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.4f, LayerMask.GetMask("Ground", "Deaths"));
        RaycastHit2D hit_left = Physics2D.Raycast(left_side, Vector2.down, 0.4f, LayerMask.GetMask("Ground", "Deaths"));
        RaycastHit2D hit_right = Physics2D.Raycast(right_side, Vector2.down, 0.4f, LayerMask.GetMask("Ground", "Deaths"));

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
        OnDeath();
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

    protected virtual void OnDeath()
    {
        Dead?.Invoke(spawned_blocks - 1, currentClothes);
    }

    protected virtual void OnRestart()
    {
        Restart?.Invoke();
    }

#endregion

#region items

    void PickupItem(IConsumable consumable) {

        currentClothes = consumable.consumableObject.GetComponent<Item>().type;
        consumable.consumableObject.transform.position = new Vector2(-500,-500);
        consumable.consumed = true;

    }

#endregion

}
