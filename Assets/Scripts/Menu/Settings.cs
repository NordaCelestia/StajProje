using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] private CharacterDataScriptableObject characterData;
    [SerializeField] GameObject SelectScreen;
    

    private void Start()
    {
       
    }

    public void Play()
    {
        SceneManager.LoadScene("InGame");
    }

    public void selectDifficultyScreen()
    {
        SelectScreen.SetActive(true);
    }

    public void SelectDifficulty(float snowballSpeed, float leadFactor)
    {
        if (characterData != null)
        {
            characterData.snowballSpeed = snowballSpeed;
            characterData.leadFactor = leadFactor;
        }
    }
}
