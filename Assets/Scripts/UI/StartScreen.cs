using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public AudioClip click;
    public void Quit()
    {
        StartCoroutine(quitRoutine());
    }

    IEnumerator quitRoutine()
    {
        AudioSource.PlayClipAtPoint(click, Vector3.zero);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
    public void StartGame()
    {
        StartCoroutine(startRoutine());
    }

    IEnumerator startRoutine()
    {
        AudioSource.PlayClipAtPoint(click, Vector3.zero);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync("Playground");
    }
}
