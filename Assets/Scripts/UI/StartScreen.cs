using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public void Quit()
    {
        // play qz audio
        Application.Quit();
    }

    public void StartGame()
    {
        // play qz audio
        SceneManager.LoadSceneAsync("Playground");
    }
}
