using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenu;
    public MouseLook mouseLook;
    public GameObject textHolder;
    public GameObject inputField;

    private void Awake()
    {
        mouseLook = FindObjectOfType<MouseLook>();
        textHolder.GetComponent<Text>().text = mouseLook.getSensitivity().ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mouseLook.enabled = true;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        mouseLook.enabled = false;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        // mouse cursor
        GameIsPaused = true;
    } 

    public void titleScreen()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("StartScreen");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Apply()
    {
        float sensitivity = mouseLook.getSensitivity();
        try {
            sensitivity = float.Parse(inputField.GetComponent<Text>().text);
        }
        catch (FormatException)
        {

        }
        catch (ArgumentNullException)
        {

        }
        mouseLook.setSensitivity(sensitivity);
        textHolder.GetComponent<Text>().text = sensitivity.ToString();
    }
}
