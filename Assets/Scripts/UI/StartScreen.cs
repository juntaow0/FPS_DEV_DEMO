using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public AudioClip click;
    public AudioClip quitExtra;
    public AudioClip startExtra;
    public void Quit()
    {
        StartCoroutine(quitRoutine());
    }

    IEnumerator quitRoutine()
    {
        AudioSource.PlayClipAtPoint(click, Vector3.zero);
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(quitExtra, Vector3.zero);
        yield return new WaitForSeconds(2.1f);
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
        AudioSource.PlayClipAtPoint(startExtra, Vector3.zero);
        yield return new WaitForSeconds(2.8f);
        SceneManager.LoadSceneAsync("Playground");
    }
}
