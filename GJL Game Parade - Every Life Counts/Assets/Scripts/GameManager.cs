using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    List<string> levelOrder;

    private void Start()
    {
        levelOrder = new List<string>() {
            "staircase",
            "kaylaDrop",
            "crazyPhysicsShit" 
        };
    }

    public void EndLevel() {

        Scene current_scene = SceneManager.GetActiveScene();
        string current_scene_name = current_scene.name;
        int current_number = levelOrder.IndexOf(current_scene_name);
        SceneManager.LoadScene(levelOrder[current_number + 1]);
    }

}
