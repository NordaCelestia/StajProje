using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class isGameOver : MonoBehaviour
{
    [SerializeField] GameObject GameOverScreen;

    public void EndTheGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReMatchButton()
    {
        SceneManager.LoadScene("InGame");
    }

    public void MainMenuButton()
    {

    }

}