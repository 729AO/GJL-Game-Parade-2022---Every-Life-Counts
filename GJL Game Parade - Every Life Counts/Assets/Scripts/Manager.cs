using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{

    List<string> levelOrder;
    public GameObject popupWindow;

    private void Start()
    {
        levelOrder = new List<string>() {
            "staircase",
            "kaylaDrop",
            "crazyPhysicsShit" 
        };
    }

    public void EndLevel() {
        
        //this should be an instantiation but then I don't know how to make everything work with the canvas and all the children stuff
        popupWindow.GetComponent<LevelEndPopup>().DisplayText();

    }

    public void LoadNextLevel() {
        Scene current_scene = SceneManager.GetActiveScene();
        string current_scene_name = current_scene.name;
        int current_number = levelOrder.IndexOf(current_scene_name);
        SceneManager.LoadScene(levelOrder[current_number + 1]);
    }

}
