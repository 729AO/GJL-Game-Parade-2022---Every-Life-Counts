using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float spawn_time;
    float lifespan = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
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
    }
}