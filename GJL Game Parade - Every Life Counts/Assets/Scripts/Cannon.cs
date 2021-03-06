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
    public int fixed_frames_per_fire;
    public GameObject projectile;
    Player player;

    int frame = 0;

    // Start is called before the first frame update
    void Awake()
    {
        SetRotation();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per set time interval
    void FixedUpdate()
    {
        TryFireProjectile();
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
        if (player.isPaused) {
            return;
        } else if (fixed_frames_per_fire == 0) {
            Debug.Log("set the frames_per_fire to something other than zero, probably also give them speed if you haven't you dingbat");
        } else if(frame % fixed_frames_per_fire == 0)
        {
            FireProjectile();
        }
        frame += 1;
    }

    void FireProjectile()
    {
        GameObject proj = Instantiate(projectile, transform.position + (transform.up * 0.5f), transform.rotation);
        proj.GetComponent<Rigidbody2D>().velocity = transform.up * projectile_speed;
    }
}
