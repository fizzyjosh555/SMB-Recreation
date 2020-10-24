﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUI;
    private bool paused = false;

    void Start()
    {
        PauseUI.SetActive(false);
    }


    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;
        }

        if (paused)
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0f;
        }

        if (!paused)
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Resume()
    {
        paused = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
