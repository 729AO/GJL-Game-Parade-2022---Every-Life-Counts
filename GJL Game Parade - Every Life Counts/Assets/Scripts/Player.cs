using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    readonly Vector3 startPos = new Vector3(0.75f,9.75f,-5);
    readonly Vector3 jump = new Vector3(0,1);
    Rigidbody2D ownRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        Transform parentTransform = GameObject.Find("RefFrame").GetComponent<Transform>();
        transform.position = parentTransform.position + startPos;
        ownRigidBody = GetComponent<Rigidbody2D>();
        ownRigidBody.gravityScale = 0.1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown("space") == true) {
            transform.position += jump;
        }
    }

    void OnCollisionEnter(Collision col) {

        
        ownRigidBody.velocity = Vector2.zero;

    }

}
