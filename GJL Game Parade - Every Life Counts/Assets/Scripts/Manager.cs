using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public delegate void PauseAction();

public class Manager : MonoBehaviour
{

    List<string> levelOrder;
    public GameObject popupWindow;
    public GameObject pausePopup;
    public PauseAction Pause;
    public PauseAction Unpause;

    private void Start()
    {
        levelOrder = new List<string>() {
            "combinationLock",
            "theBasics",
            "staircase",
            "kaylaDrop",
            "crazyPhysicsShit"
        };
    }

    public void PauseLevel() {
        pausePopup.GetComponent<PausePopup>().Show();
        Pause?.Invoke();
    }

    public void UnpauseLevel() {
        pausePopup.GetComponent<PausePopup>().Hide();
        Unpause?.Invoke();
    }

    public void EndLevel() {
        
        //this should be an instantiation but then I don't know how to make everything work with the canvas and all the children stuff
        popupWindow.GetComponent<LevelEndPopup>().Show();

    }

    public void LoadNextLevel() {
        Debug.Log("the fuck?");
        Scene current_scene = SceneManager.GetActiveScene();
        string current_scene_name = current_scene.name;
        int current_number = levelOrder.IndexOf(current_scene_name);
        SceneManager.LoadScene(levelOrder[current_number + 1]);
    }

}
