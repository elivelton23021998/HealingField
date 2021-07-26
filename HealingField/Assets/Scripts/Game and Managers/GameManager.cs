using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void Continue()
    {
        SceneManager.LoadScene("PreparationPhase");
    }
    public void NovoJogo()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("PreparationPhase");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Game()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Mute()
    {
        AudioListener.pause=true;
    }
    public void UnMute()
    {
        AudioListener.pause = false;
    }
}
