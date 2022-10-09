using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class TextTrigger : MonoBehaviour
{

    ///<summar> fadeSpeed is the change in alpha (between 0 and 1) per second </summary>
    public float fadeSpeed;
    TMP_Text text;
    bool seenSinceLastRestart = false;

    void Start() {

        text = GetComponentInParent<TMP_Text>();
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.Restart += NotSeenSinceRestart;
        player.Restart += InstantFadeOut;

    }

    public IEnumerator FadeIn() {

        if (seenSinceLastRestart) yield break;

        while (text.alpha <= 1) {

            text.alpha += fadeSpeed * Time.deltaTime;
            yield return null;

        }

    }

    public IEnumerator FadeOut() {

        seenSinceLastRestart = true;
        while (text.alpha >= 0) {

            text.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;

        }

    }

    void InstantFadeOut() {
        text.alpha = 0;
    }

    void NotSeenSinceRestart() {
        seenSinceLastRestart = false;
    }

}
