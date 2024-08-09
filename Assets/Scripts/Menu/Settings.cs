using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject SelectScreen,LoadingScreen;
    [SerializeField] Slider slider;
    public float snowballSpeed, leadFactor;
    public bool isPaused = false;
    [SerializeField] Animator pauseAnimator;

    private void Update()
    {
        PauseGame();
    }

    IEnumerator LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;

            yield return null;
        }
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
        StartCoroutine(LoadAsynchronously());
    }

    public void setNormal()
    {
        snowballSpeed = 60f;
        leadFactor = 0.5f;
        SaveSettings();
        StartCoroutine(LoadAsynchronously());
    }

    public void setHard()
    {
        snowballSpeed = 100f;
        leadFactor = 0.2f;
        SaveSettings();
        StartCoroutine(LoadAsynchronously());
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
