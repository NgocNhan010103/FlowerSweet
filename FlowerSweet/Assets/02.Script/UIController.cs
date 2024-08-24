using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
    public static UIController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    
    public AudioClip soundClickBTN;


    public void GameOver()
    {
        SyntheticScore.Instance.Synthetic();
        MenuManager.Instance.OpenMenu("GameOver");
        Time.timeScale = 0;
    }


    public void PlaySoundClick()
    {
        SoundFXManager.Instance.PlaySoundFXClip(soundClickBTN, 3f);
    }

    public void OnPauseMenu()
    {
        Time.timeScale = 0f;
        GameController.instance.ChangeGameState(GameState.Pause);
    }

    public void Replay()
    {
        StartCoroutine(LoadScene(1));
        Time.timeScale = 1f;
    }

    public void Resume()
    {
        GameController.instance.ChangeGameState(GameState.Playing);
        Time.timeScale = 1f;
    }

    public void Home()
    {
        StartCoroutine(LoadScene(0));
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    IEnumerator LoadScene(int id)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(id);
    }
}
