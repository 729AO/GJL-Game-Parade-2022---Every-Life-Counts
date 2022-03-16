using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPressButton : MonoBehaviour
{
    
    Player player;

    void Awake() {   
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {

        GameObject gameObj = collider.gameObject;
        if (gameObj.layer == 11)
        {
            player.PressButton(gameObj.GetComponent<Button>().buttonNum);
        }

    }

    void OnTriggerExit2D(Collider2D collider) {

        GameObject gameObj = collider.gameObject;
        if (gameObj.layer == 11)
        {
            player.UnpressButton(gameObj.GetComponent<Button>().buttonNum);
        }

    }
}
