using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterData : MonoBehaviour
{
    [SerializeField] private CharacterDataScriptableObject characterData;
    [SerializeField] Image hpBar;
    [SerializeField] GameObject healthBar, GameOverManager, AudioManager, damageVFXPrefab;

    private string teamTag;
    isGameOver isGameOver;
    float healthPercentage;
    private int currentHealth;
    SFX sfxManager;

    private void Awake()
    {
        
        Time.timeScale = 1f;
        teamTag = this.gameObject.tag;
    }

    private void Start()
    {
        sfxManager = AudioManager.GetComponent<SFX>();
        isGameOver = GameOverManager.GetComponent<isGameOver>();
        isGameOver.RegisterCharacter(this); // Karakteri kaydet
        currentHealth = characterData.maxHealth;
    }

    public string GetTeamTag()
    {
        return teamTag;
    }

    private void Update()
    {
        Vector3 newPosition = healthBar.transform.position;
        newPosition.x = this.gameObject.transform.position.x;
        healthBar.transform.position = newPosition;

        if (Input.GetKeyDown(KeyCode.O)) // Test damage
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        sfxManager.PlaySound(3);
        currentHealth -= 1;
        Debug.Log(characterData.characterName + " takes damage! remaining health: " + currentHealth);
        UpdateUI();

        Instantiate(damageVFXPrefab, this.gameObject.transform.position, Quaternion.identity); //damage VFX

        if (currentHealth <= 0)
        {
            isGameOver.CharacterDied(this); // Karakter öldüðünde bildir
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

    public string GetCharacterName()
    {
        return characterData.characterName;
    }
}
