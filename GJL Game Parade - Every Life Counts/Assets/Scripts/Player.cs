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
    readonly float horizontalAcceleration = 0.5f;
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
    public int deathCounter = 0;

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
        if (gameObj.layer == 12)
        {//this is to make you be able to step over short objects
            StepOver(gameObj);
        }
        if (gameObj.layer == 13)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().EndLevel();
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
        if(OnBlocks("Ground", "Deaths", "Free Blocks")) {
            ownRigidBody.velocity += jumpVelocity;
            CheckVertSpeed(jumpVelocity.y);
        }
    }


    void MoveRight()
    {
        Vector2 tangent = SlopeTangentVec(1, "Ground", "Deaths", "Free Blocks");
        if (tangent != Vector2.zero) ownRigidBody.velocity += tangent * horizontalAcceleration;
        else ownRigidBody.velocity += new Vector2(horizontalAcceleration, 0);

        if(ownRigidBody.velocity.x >= horizontalSpeedcap)
        {
            ownRigidBody.velocity = new Vector2(horizontalSpeedcap, ownRigidBody.velocity.y);
        }
    }

    void MoveLeft()
    {
        Vector2 tangent = SlopeTangentVec(1, "Ground", "Deaths", "Free Blocks");
        if (tangent != Vector2.zero) ownRigidBody.velocity -= tangent * horizontalAcceleration;
        else ownRigidBody.velocity -= new Vector2(horizontalAcceleration, 0);

        if(ownRigidBody.velocity.x <= -horizontalSpeedcap)
        {
            ownRigidBody.velocity = new Vector2(-horizontalSpeedcap, ownRigidBody.velocity.y);
        }
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

    private bool OnBlocks(string str1, string str2 = "", string str3 = "") {
        var left_side = transform.position - new Vector3(transform.localScale.x / 2, 0);
        var right_side = transform.position + new Vector3(transform.localScale.x / 2, 0);

        float extension = 0.42f;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, extension, LayerMask.GetMask(str1, str2, str3));
        RaycastHit2D hit_left = Physics2D.Raycast(left_side, Vector2.down, extension, LayerMask.GetMask(str1, str2, str3));
        RaycastHit2D hit_right = Physics2D.Raycast(right_side, Vector2.down, extension, LayerMask.GetMask(str1, str2, str3));

        if (hit.collider != null || hit_left.collider != null || hit_right.collider != null)
        {
            return true;
        }

        return false;
    }

    //checks if you're standing on a slope that has a slope up to the given max slope and gives you the unit tangent vector
    //returns Vector3.zero if there's no slope found
    private Vector2 SlopeTangentVec(float maxSlope, string str1, string str2 = "", string str3 = "") {
       
        Vector2 tangent = Vector2.zero;
       
        var center = transform.position + new Vector3(0, -transform.localScale.y / 2);
        var left_side = transform.position + new Vector3(-transform.localScale.x / 2, -transform.localScale.y / 2);
        var right_side = transform.position + new Vector3(transform.localScale.x / 2, -transform.localScale.y / 2);

        float corner_extension = 0.05f;
        float extension = maxSlope * transform.localScale.x / 2 + corner_extension;

        RaycastHit2D hit = Physics2D.Raycast(center, Vector2.down, extension, LayerMask.GetMask(str1, str2, str3));
        RaycastHit2D hit_right = Physics2D.Raycast(right_side, Vector2.down, corner_extension, LayerMask.GetMask(str1, str2, str3));
        RaycastHit2D hit_left = Physics2D.Raycast(left_side, Vector2.down, corner_extension, LayerMask.GetMask(str1, str2, str3));

        //slope is positive
        if (hit.collider != null && hit_right.collider != null && hit_left.collider == null)
        {
            tangent = new Vector2(transform.localScale.x / 2, hit.distance - hit_right.distance);
        }
        //slope is negative
        if (hit.collider != null && hit_left.collider != null && hit_right.collider == null)
        {
            tangent = new Vector2(transform.localScale.x / 2, hit_left.distance - hit.distance);
        }

        //normalizing
        if (tangent != Vector2.zero) {
            float mag = Mathf.Sqrt(tangent.x * tangent.x + tangent.y * tangent.y);
            Vector2 unit_tangent = new Vector2(tangent.x / mag, tangent.y / mag);
            return unit_tangent;
        }
        return Vector2.zero;

    }
   
    void StepOver(GameObject gameObj) {
        //this will break if this object didn't start as a normal 1x1 square
        //also I think could implement this by breaking the player down in to multiple colliders
        //cannot step over if already on a free block...
            if(!OnBlocks("Ground", "Deaths")) return;

            Vector3 step = new Vector3(0, 0.13f);
            float x_velocity = GetComponent<Rigidbody2D>().velocity.x;

            Vector3 source = transform.position + new Vector3(0, -transform.localScale.y / 2);
            Vector3 higher_source = transform.position + new Vector3(0, -transform.localScale.y / 2 + 0.1f);
            
            float extension = 0.5f;

            RaycastHit2D freeBlocksLeft = Physics2D.Raycast(source, Vector2.left, extension, LayerMask.GetMask("Free Blocks"));
            RaycastHit2D freeBlocksRight = Physics2D.Raycast(source, Vector2.right, extension, LayerMask.GetMask("Free Blocks"));
            RaycastHit2D higherFreeBlocksLeft = Physics2D.Raycast(higher_source, Vector2.left, extension, LayerMask.GetMask("Free Blocks"));
            RaycastHit2D higherFreeBlocksRight = Physics2D.Raycast(higher_source, Vector2.right, extension, LayerMask.GetMask("Free Blocks"));

            if (freeBlocksLeft.collider != null && x_velocity < 0 || freeBlocksRight.collider != null && x_velocity > 0
            || higherFreeBlocksLeft.collider != null && x_velocity < 0 || higherFreeBlocksRight.collider != null && x_velocity > 0) {
                transform.position += step;
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
        deathCounter++;
    }

    protected virtual void OnRestart()
    {
        Restart?.Invoke();
        deathCounter = 0;
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
