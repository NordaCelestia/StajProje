using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject SelectScreen;
    public float snowballSpeed, leadFactor;



    private void Start()
    {
        
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
        snowballSpeed = 80f;
        leadFactor = 0.4f;
        SaveSettings();
        SceneManager.LoadScene("InGame");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SnowballSpeed", snowballSpeed);
        PlayerPrefs.SetFloat("LeadFactor", leadFactor);
        PlayerPrefs.Save(); 
    }
}
