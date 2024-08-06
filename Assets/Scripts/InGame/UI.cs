using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private List<GameObject> countdownImages; 
    [SerializeField] private float displayTime = 1f; 

    private void Start()
    {
        Time.timeScale = 0; 
        StartCoroutine(DisplayCountdownImages());
    }

    private IEnumerator DisplayCountdownImages()
    {
        foreach (GameObject image in countdownImages)
        {
            image.SetActive(true); 
            yield return new WaitForSecondsRealtime(displayTime);
            image.SetActive(false); 
        }

        Time.timeScale = 1; // Oyunu tekrar baþlat
    }
}
