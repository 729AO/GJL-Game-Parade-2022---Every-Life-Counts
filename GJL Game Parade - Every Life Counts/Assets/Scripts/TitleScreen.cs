using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public static bool prefs_loaded = false;

    private void Start()
    {
        if (!prefs_loaded)
        {
            PlayerPrefs.SetInt("lvl1", 0);
            PlayerPrefs.SetInt("lvl2", 0);
            PlayerPrefs.SetInt("lvl3", 0);
            PlayerPrefs.SetInt("lvl4", 0);
            PlayerPrefs.SetInt("lvl5", 0);
        }
        prefs_loaded = true;
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void LoadNextLevel()
    {
        if(PlayerPrefs.GetInt("lvl1") == 0) { SceneManager.LoadScene(Manager.levelOrder[0]); }
        else if (PlayerPrefs.GetInt("lvl2") == 0) { SceneManager.LoadScene(Manager.levelOrder[1]); }
        else if (PlayerPrefs.GetInt("lvl3") == 0) { SceneManager.LoadScene(Manager.levelOrder[2]); }
        else if (PlayerPrefs.GetInt("lvl4") == 0) { SceneManager.LoadScene(Manager.levelOrder[3]); }
        else if (PlayerPrefs.GetInt("lvl5") == 0) { SceneManager.LoadScene(Manager.levelOrder[4]); }
        else{ SceneManager.LoadScene(Manager.levelOrder[0]); }
    }

}
