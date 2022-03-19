using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Projectile : MonoBehaviour
{
    float spawn_time;
    public float lifespan;
    float hit_time = -1;
    float death_delay = 0.05f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit_time = Time.time;
    }

    private void Awake()
    {
        spawn_time = Time.time;
    }

    private void Update()
    {
        if ((Time.time - spawn_time) >= lifespan) { 
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