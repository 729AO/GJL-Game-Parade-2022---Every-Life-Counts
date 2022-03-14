using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    readonly Vector3 startPos = new Vector3(0.75f,9.75f,-5);
    readonly Vector2 jumpVelocity = new Vector2(0,5);
    readonly Vector2 horizontalAcceleration = new Vector2(0.15f, 0);
    readonly float horizontalSpeedcap = 5f;
    Rigidbody2D ownRigidBody;


    // Start is called before the first frame update
    void Start()
    {
        Transform parentTransform = GameObject.Find("RefFrame").GetComponent<Transform>();
        transform.position = parentTransform.position + startPos;
        ownRigidBody = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate makes the cube not jump good
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            TryJump();
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
}
