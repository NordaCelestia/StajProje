using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class isGameOver : MonoBehaviour
{
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] TMP_Text winnerText;

    public static isGameOver instance;
    private string winnerName;
    public bool isPlaying;

    private void Awake()
    {

        isPlaying = true;
        //singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndTheGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameOverScreen.SetActive(true);
        isPlaying = false;
        
    }

    public void EndTheGame(string winnerName)
    {
        
        winnerText.text = "Winner: "+ winnerName;
        Time.timeScale = 0f;
    }

    public void ReMatchButton()
    {
        SceneManager.LoadScene("InGame");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

}