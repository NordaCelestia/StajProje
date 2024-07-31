using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CharacterData : MonoBehaviour
{
    [SerializeField] private CharacterDataScriptableObject characterData;
    [SerializeField] Image hpBar;
    [SerializeField] GameObject healthBar,GameOverManagaer, AudioManager;

    isGameOver isGameOver;
    float healthPercentage;
    private int currentHealth;
    SFX sfxManager;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        sfxManager = AudioManager.GetComponent<SFX>();
        isGameOver = GameOverManagaer.GetComponent<isGameOver>();
        currentHealth = characterData.maxHealth;
    }

    private void Update()
    {
        
        Vector3 newPosition = healthBar.transform.position;
        newPosition.x = this.gameObject.transform.position.x;
        healthBar.transform.position = newPosition;

        if (Input.GetKeyDown(KeyCode.O)) //test damage
        {
            TakeDamage();
        }

        if (isGameOver.isPlaying == false && currentHealth > 0)
        {
            isGameOver.EndTheGame(characterData.characterName);
        }
    }

    public void TakeDamage()
    {
        sfxManager.PlaySound(3);
        currentHealth -= 1;
        Debug.Log(characterData.characterName + " takes damage! remaining health: " + currentHealth);
        UpdateUI();

        if (currentHealth <= 0)
        {
            isGameOver.EndTheGame();
        }


        
       
        
    }

    public void UpdateUI()
    {
        healthPercentage = (float)currentHealth / (float)characterData.maxHealth;
        hpBar.fillAmount = healthPercentage;
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("snowball"))
        {
            TakeDamage();
            collision.gameObject.SetActive(false);
        }
    }
}
