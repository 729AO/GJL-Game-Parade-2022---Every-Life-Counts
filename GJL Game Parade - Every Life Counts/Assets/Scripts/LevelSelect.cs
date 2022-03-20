using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{

    private void Start()
    {

    }

    public void OnButtonClick(int level)
    {

        SceneManager.LoadScene(Manager.levelOrder[level - 1]);

    }

}
