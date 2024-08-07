using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq; 

public class isGameOver : MonoBehaviour
{
    [SerializeField] GameObject GameOverScreen, AudioManager;
    [SerializeField] TMP_Text winnerText;

    public static isGameOver instance;
    private string winnerName;
    public bool isPlaying;
    SFX sfxManager;
    private bool gameEnded; 

    private List<CharacterData> characters = new List<CharacterData>();

    private void Awake()
    {
        isPlaying = true;
        gameEnded = false; 
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

        //hedef belirlemek i�in target belirleme
        var team1 = characters.Where(c => c.GetTeamTag() == "Team1" || c.GetTeamTag() == "Player").ToList();
        var team2 = characters.Where(c => c.GetTeamTag() == "Team2").ToList();

        if (team1.Count == 0)
        {
            EndTheGame("Team 2");
        }
        else if (team2.Count == 0)
        {
            EndTheGame("Team 1");
        }
        else if (characters.Count == 0)
        {
            EndTheGame("No winner");
        }
    }

    public void EndTheGame(string winnerName)
    {
        if (gameEnded) return; 
        gameEnded = true; 

        sfxManager.PlaySound(4);
        sfxManager.PlaySound(5);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameOverScreen.SetActive(true);
        isPlaying = false;

        winnerText.text = "Winner: " + winnerName;
        Time.timeScale = 0f; 
    }

    public void ReMatchButton()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("InGame");
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
    }
}
