using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    public enum Direction
    {
        unspecified = default,
        up,
        upRight,
        Right,
        downRight,
        down,
        downLeft,
        Left,
        upLeft, 
    }

    public Direction orientation;
    public float projectile_speed;
    public int frames_per_fire;
    public GameObject projectile;

    int frame = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetRotation();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TryFireProjectile();
        frame += 1;
    }

    void SetRotation()
    {
        switch (orientation)
        {
            case Direction.up:
                transform.rotation = Quaternion.identity;
                break;
            case Direction.upRight:
                transform.rotation = Quaternion.Euler(0, 0, -45);
                break;
            case Direction.Right:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case Direction.downRight:
                transform.rotation = Quaternion.Euler(0, 0, -135);
                break;
            case Direction.down:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.downLeft:
                transform.rotation = Quaternion.Euler(0, 0, 135);
                break;
            case Direction.Left:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.upLeft:
                transform.rotation = Quaternion.Euler(0, 0, 45);
                break;
            case Direction.unspecified:
                Debug.LogError("Cannon orientation wasn't specified.");
                break;
        }
    }

    void TryFireProjectile()
    {
        if(frame % frames_per_fire == 0)
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        GameObject proj = Instantiate(projectile, transform.position, transform.rotation);
        proj.GetComponent<Rigidbody2D>().velocity = transform.up * projectile_speed;
    }
}
