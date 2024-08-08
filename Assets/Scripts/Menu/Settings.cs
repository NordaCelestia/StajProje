using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject SelectScreen;
    public float snowballSpeed, leadFactor;
    public bool isPaused = false;
    [SerializeField] Animator pauseAnimator;

    private void Update()
    {
        PauseGame();
    }

    public void selectDifficultyScreen()
    {
        SelectScreen.SetActive(true);
    }

    public void setEasy()
    {
        snowballSpeed = 50f;
        leadFactor = 0.001f;
        SaveSettings();
        SceneManager.LoadScene("InGame");
    }

    public void setNormal()
    {
        snowballSpeed = 60f;
        leadFactor = 0.5f;
        SaveSettings();
        SceneManager.LoadScene("InGame");
    }

    public void setHard()
    {
        snowballSpeed = 100f;
        leadFactor = 0.2f;
        SaveSettings();
        SceneManager.LoadScene("InGame");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SnowballSpeed", snowballSpeed);
        PlayerPrefs.SetFloat("LeadFactor", leadFactor);
        PlayerPrefs.Save(); 
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isPaused)
            {
                StartCoroutine(waitForTabOpen());
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                pauseAnimator.SetTrigger("closeTab");
                isPaused = false;
                
            }
        }
    }

    IEnumerator waitForTabOpen()
    {
        pauseAnimator.SetTrigger("openTab");
        yield return new WaitForSeconds(0.8f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
        Time.timeScale = 0f;
    }
}
