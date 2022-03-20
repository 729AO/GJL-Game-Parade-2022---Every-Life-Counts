using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class LevelEndPopup : MonoBehaviour
{

    private struct MedalInfo {

        public string medal;
        public string nextMedal;
        public int nextDeathGoal;

        public MedalInfo(string medal, string nextMedal, int nextDeathGoal) {
            this.medal = medal;
            this.nextMedal = nextMedal;
            this.nextDeathGoal = nextDeathGoal;
        }

    }

    public TMP_Text popupText;
    readonly string[] text = {"Contratulations! You've completed the level in ", " deaths!\r\nYou've achieved a ", " Medal!", "\r\nDie no more than ", " times for ", " Medal."};
    public int silverDeathGoal;
    public int goldDeathGoal;
    public Vector3 position;

    public void DisplayText() {

        transform.position = position;

        int deaths = GameObject.Find("Player").GetComponent<Player>().deathCounter;
        MedalInfo medalInfo = GetMedalInfo(deaths);
        popupText.text = text[0] + deaths + text[1] + medalInfo.medal + text[2];

        if (medalInfo.nextMedal != null) {
            popupText.text += text[3] + medalInfo.nextDeathGoal + text[4] + medalInfo.nextMedal + text[5];
        }

    }

    private MedalInfo GetMedalInfo(int deaths) {

        if (deaths <= goldDeathGoal) return new MedalInfo("gold", null, -1);
        if (deaths <= silverDeathGoal) return new MedalInfo("silver", "gold", goldDeathGoal);
        return new MedalInfo("bronze", "silver", silverDeathGoal);

    }
    
}
