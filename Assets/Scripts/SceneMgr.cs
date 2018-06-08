using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour {
    public static int startLevel = 1;
    public LevelManager cat;

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel(int level)
    {
        startLevel = level;
        SceneManager.LoadScene(0);
    }
}
