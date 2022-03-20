using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class LevelEndPopup : AbstractPopup
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

    readonly string[] text = {"Contratulations! You've completed the level in ", " deaths!\r\nYou've achieved a ", " Medal!", "\r\nDie no more than ", " times for ", " Medal."};
    public int silverDeathGoal;
    public int goldDeathGoal;

    public override void Show() {

        transform.position = position;
        Debug.Log(position);

        int deaths = GameObject.Find("Player").GetComponent<Player>().deathCounter;
        MedalInfo medalInfo = GetMedalInfo(deaths);
        popupText.text = text[0] + deaths + text[1] + medalInfo.medal + text[2];

        if (medalInfo.nextMedal != null) {
            popupText.text += text[3] + medalInfo.nextDeathGoal + text[4] + medalInfo.nextMedal + text[5];
        }

    }

    private MedalInfo GetMedalInfo(int deaths) {

        if (deaths <= goldDeathGoal) {
            SaveMedal("gold");
            return new MedalInfo("gold", null, -1); }
        if (deaths <= silverDeathGoal) {
            SaveMedal("silver");
            return new MedalInfo("silver", "gold", goldDeathGoal); }
        SaveMedal("bronze");
        return new MedalInfo("bronze", "silver", silverDeathGoal);

    }
    
    private void SaveMedal(string medal_type)
    {
        string scene_name = SceneManager.GetActiveScene().name;
        string save_pref = "your mom";
        int save_value = 69420;

        switch (scene_name)
        {
            case "movingAndEnemies":
                save_pref = "lvl1";
                break;
            case "theBasics":
                save_pref = "lvl2";
                break;
            case "staircase":
                save_pref = "lvl3";
                break;
            case "kaylaDrop":
                save_pref = "lvl4";
                break;
            case "crazyPhysicsShit":
                save_pref = "lvl5";
                break;
        }

        switch (medal_type)
        {
            case "gold":
                save_value = 3;
                break;
            case "silver":
                save_value = 2;
                break;
            case "bronze":
                save_value = 1;
                break;
        }

        print(save_pref);
        print(save_value);
        PlayerPrefs.SetInt(save_pref, save_value);

    }

}
