using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public delegate void ResetAction();

public class ResetOnDeath : MonoBehaviour
{

    public Vector3 startingPosition;
    public Quaternion startingRotation;
    public Vector3 startingVelocity;
    public float startingAngularVelocity;
    DeathAction playerDiedOverload;
    Player player;
    

    private void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        if (GetComponent<Rigidbody2D>() != null && GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic) {
            Rigidbody2D body = GetComponent<Rigidbody2D>();
            startingVelocity = body.velocity;
            startingAngularVelocity = body.angularVelocity;
        }
        

        player = GameObject.Find("Player").GetComponent<Player>();
        playerDiedOverload = (int id, ItemType type, bool restart) => PlayerDied();
        player.Dead += playerDiedOverload;
    }

    public void UnsubscribeFromDeath() {
    //no one cares but im gonna write it cause im excited
    //I couldn't say player.Dead += (int id, ItemType type) => PlayerDied();
    //and then player.Dead -= (int id, ItemType type) => PlayerDied();
    //because "(int id, ItemType type) => PlayerDied()" defines an overload-like ANONYMOUS method for PlayerDied()
    //since otherwise PlayerDied() doesn't have the right parameters to be encapsulated by a MyAction delegate...
    //but anonymous methods are nearly if not actually impossible to unsubscribe since there's no way to reference them
        player.Dead -= playerDiedOverload;

    }

    private void PlayerDied()
    {
        //try
        //{

            //if it's supposed to be 'destroyed' it won't get reset, even though it didn't get physically destroyed
            if (gameObject.layer == 10 && gameObject.GetComponent<Item>() != null && gameObject.GetComponent<Item>().isDestroyed) return;

            transform.position = startingPosition;
            transform.rotation = startingRotation;

            if (GetComponent<Rigidbody2D>() != null && GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic) {
                Rigidbody2D body = GetComponent<Rigidbody2D>();
                body.velocity = startingVelocity;
                body.angularVelocity = startingAngularVelocity;
            }

            if (GetComponent<Projectile>() != null) {
                gameObject.GetComponent<Projectile>().SelfDestruct();
            }
            


            OnReset();//no understanding of why this is a thing
        //}
        // catch (UnityEngine.MissingReferenceException)
        // { Debug.Log("ig it didn't reset"); }
    }

    protected virtual void OnReset()
    {
        Reset?.Invoke();
    }

    public event ResetAction Reset;

}