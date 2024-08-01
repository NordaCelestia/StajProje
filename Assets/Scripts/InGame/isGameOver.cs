using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class isGameOver : MonoBehaviour
{
    [SerializeField] GameObject GameOverScreen, AudioManager;
    [SerializeField] TMP_Text winnerText;

    public static isGameOver instance;
    private string winnerName;
    public bool isPlaying;
    SFX sfxManager;
    private bool gameEnded; // Oyun bitip bitmediðini kontrol etmek için bayrak

    private List<CharacterData> characters = new List<CharacterData>();

    private void Awake()
    {
        isPlaying = true;
        gameEnded = false; // Baþlangýçta oyun bitmemiþ
        // Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sfxManager = AudioManager.GetComponent<SFX>();
    }

    public void RegisterCharacter(CharacterData character)
    {
        characters.Add(character);
    }

    public void CharacterDied(CharacterData deadCharacter)
    {
        characters.Remove(deadCharacter);
        if (characters.Count == 1)
        {
            EndTheGame(characters[0].GetCharacterName());
        }
        else if (characters.Count == 0)
        {
            EndTheGame("No winner");
        }
    }

    public void EndTheGame(string winnerName)
    {
        if (gameEnded) return; // Eðer oyun zaten bitmiþse, metodu çalýþtýrma
        gameEnded = true; // Oyun bitti olarak iþaretle

        sfxManager.PlaySound(4);
        sfxManager.PlaySound(5);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameOverScreen.SetActive(true);
        isPlaying = false;

        winnerText.text = "Winner: " + winnerName;
        Time.timeScale = 0f; // Zamaný durdur
    }

    public void ReMatchButton()
    {
        Time.timeScale = 1f; // Zamaný yeniden baþlat
        SceneManager.LoadScene("InGame");
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f; // Zamaný yeniden baþlat
        SceneManager.LoadScene("MainMenu");
    }
}
