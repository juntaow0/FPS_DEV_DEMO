using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class ClearScore : MonoBehaviour
{
    private Player player;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public void useButton()
    {
        AudioManager.instance.Play("buttons");
        player.clearScore();
        StartCoroutine(scoreRoutine());
    }

    IEnumerator scoreRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        AudioManager.instance.Play("yarimasune");
    }
}
