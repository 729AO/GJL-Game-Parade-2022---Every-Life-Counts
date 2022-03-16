using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float spawn_time;
    float lifespan = 10;
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
        if ((Time.time - spawn_time) >= lifespan)
        {
            Destroy(gameObject);
        }
        if (hit_time != -1 && (Time.time - hit_time >= death_delay)) {
            Destroy(gameObject);
        }
    }
}