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
    bool setupComplete = false;
    

    void Awake()//between awake and start? bc we need defaults set beforehand to read them
    //but the projectiles have to have this subscription before start
//ok splitting it up into awake and return works but what's a better strat?
//I don't see how to reformulate my overall strategy either...?
    {
        if (GetComponent<Projectile>() != null) {
            player = GameObject.Find("Player").GetComponent<Player>();
            playerDiedOverload = (int id, ItemType type, bool restart) => PlayerDied();
            player.Dead += playerDiedOverload;
            setupComplete = true;
        }

    }

    void Update() {

        if (setupComplete) return;

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
        
        setupComplete = true;

    }

    //also unsubscribe from pause/unpause
    public void UnsubscribeFromDeath() {
    //no one cares but im gonna write it cause im excited
    //I couldn't say player.Dead += (int id, ItemType type) => PlayerDied();
    //and then player.Dead -= (int id, ItemType type) => PlayerDied();
    //because "(int id, ItemType type) => PlayerDied()" defines an overload-like ANONYMOUS method for PlayerDied()
    //since otherwise PlayerDied() doesn't have the right parameters to be encapsulated by a MyAction delegate...
    //but anonymous methods are nearly if not actually impossible to unsubscribe since there's no way to reference them
        player.Dead -= playerDiedOverload;
        Manager manager = GameObject.Find("Manager").GetComponent<Manager>();
        Pausable pausable = GetComponent<Pausable>();
        manager.Pause -= pausable.Pause;
        manager.Unpause -= pausable.Unpause;

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

            OnReset();//no understanding of why this is a thing, doesn't it invoke it a bunch of times?
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