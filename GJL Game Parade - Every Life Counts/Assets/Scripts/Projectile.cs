using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Projectile : MonoBehaviour
{
    public float spawn_time;
    public float lifespan;
    float hit_time = -1;
    float death_delay = 0.05f;
    Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit_time = Time.time;
    }

    private void Awake()
    {
        spawn_time = Time.time;
        Player player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (player == null) player = GameObject.Find("Player").GetComponent<Player>();

        if ((Time.time - spawn_time) >= lifespan && !player.isPaused) { 
            SelfDestruct();
        }
        if (hit_time != -1 && (Time.time - hit_time >= death_delay)) {
            SelfDestruct();
        }
    }

    public void SelfDestruct() {

        if (GetComponent<ResetOnDeath>() != null) {
            GetComponent<ResetOnDeath>().UnsubscribeFromDeath();
        }
        Destroy(gameObject);

    }

}