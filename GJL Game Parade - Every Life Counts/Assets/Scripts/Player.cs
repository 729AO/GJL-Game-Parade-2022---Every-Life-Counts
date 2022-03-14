using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Notify();

public class Player : MonoBehaviour
{

    readonly Vector2 jumpVelocity = new Vector2(0,5);
    readonly Vector2 horizontalAcceleration = new Vector2(0.15f, 0);
    readonly float horizontalSpeedcap = 5f;
    Rigidbody2D ownRigidBody;

    public event Notify Dead;

    public Player() { }

    // Start is called before the first frame update
    void Start()
    {
        ownRigidBody = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate makes the cube not jump good
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            TryJump();
        }

        if (Input.GetKeyDown("p"))
        {
            Die();
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

    void TryJump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), 0.3f, LayerMask.GetMask("Ground"));
        if(hit.collider != null)
        {
            ownRigidBody.velocity += jumpVelocity;
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
        if ((ownRigidBody.velocity - horizontalAcceleration).x <= -horizontalSpeedcap)
        {
            ownRigidBody.velocity.Set(-horizontalSpeedcap, ownRigidBody.velocity.y);
        }
        else { ownRigidBody.velocity -= horizontalAcceleration; }
    }

    public void Die()
    {
        OnDeath();
    }

    protected virtual void OnDeath()
    {
        Dead?.Invoke();
    }

}
