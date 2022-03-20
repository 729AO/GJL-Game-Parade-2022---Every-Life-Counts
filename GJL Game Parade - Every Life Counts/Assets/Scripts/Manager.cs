using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public delegate void PauseAction();

public class Manager : MonoBehaviour
{

    static public List<string> levelOrder = new List<string>() {
            "movingAndEnemies",
            "theBasics",
            "staircase",
            "kaylaDrop",
            "crazyPhysicsShit"
        };

    public GameObject popupWindow;
    public GameObject pausePopup;
    public PauseAction Pause;
    public PauseAction Unpause;


    public void PauseLevel() {
        Debug.Log("pausing");
        if (!GameObject.Find("Player").GetComponent<Player>().isPaused) {
            pausePopup.GetComponent<PausePopup>().Show();
            Pause?.Invoke();
        }
    }

    public void UnpauseLevel() {
        pausePopup.GetComponent<PausePopup>().Hide();
        Unpause?.Invoke();
    }

    public void EndLevel() {
        
        //this should be an instantiation but then I don't know how to make everything work with the canvas and all the children stuff
        popupWindow.GetComponent<LevelEndPopup>().Show();
        Pause?.Invoke();//otherwise the pause button's still clickable and you can still move


    }

    public void LoadNextLevel() {
        Scene current_scene = SceneManager.GetActiveScene();
        string current_scene_name = current_scene.name;
        int current_number = levelOrder.IndexOf(current_scene_name);
        SceneManager.LoadScene(levelOrder[current_number + 1]);
    }

}
