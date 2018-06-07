using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseUI;

    private CatController controller;
    private bool paused = false;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CatController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePaused();
        }
	}

    public void TogglePaused()
    {
        paused = !paused;
        controller.enabled = !paused;
        pauseUI.SetActive(paused);
    }
}
