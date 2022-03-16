using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ResetOnRestart : MonoBehaviour
{
    Player player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        player.Restart += ExecuteRestart;
    }

    private void ExecuteRestart()
    {

        player.Die(restart: true);

        if (gameObject.layer == 9)
        {
            player.Restart -= ExecuteRestart;
            Destroy(gameObject);
        }

    }
}
