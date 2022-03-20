using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausable : MonoBehaviour
{

    Transform savedTransform = null;
    Vector3 savedVelociy = Vector3.zero;
    float savedAngularVelocity = 0;
    float savedGravity = 2;
    float pauseTime;

    void Awake() {

        Manager manager = GameObject.Find("Manager").GetComponent<Manager>();
        manager.Pause += Pause;
        manager.Unpause += Unpause;

    }

    public void Pause() {

        Rigidbody2D body = GetComponent<Rigidbody2D>();

        savedTransform = transform;
        savedVelociy = body.velocity;
        savedAngularVelocity = body.angularVelocity;
        savedGravity = body.gravityScale;

        body.velocity = Vector3.zero;
        body.angularVelocity = 0;
        body.gravityScale = 0;

        pauseTime = Time.time;

    }

    public void Unpause() {
        
        if (savedTransform != null) {
            transform.position = savedTransform.position;
            transform.rotation = savedTransform.rotation;
        }
        if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic) {
            GetComponent<Rigidbody2D>().velocity = savedVelociy;
            GetComponent<Rigidbody2D>().angularDrag = savedAngularVelocity;
            GetComponent<Rigidbody2D>().gravityScale = savedGravity;
        }

        float timeElapsed = Time.time - pauseTime;
        if(GetComponent<Projectile>() != null) {
            GetComponent<Projectile>().spawn_time += timeElapsed;
        }

    }

}