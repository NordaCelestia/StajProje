using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterData : MonoBehaviour
{
    [SerializeField] private CharacterDataScriptableObject characterData;
    [SerializeField] Image hpBar;
    [SerializeField] GameObject healthBar;
    [SerializeField] UnityEvent isDead;

    float healthPercentage;
    private int currentHealth;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
       
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
    }

    public void TakeDamage()
    {
        currentHealth -= 1;
        Debug.Log(characterData.characterName + " takes damage! remaining health: " + currentHealth);
        UpdateUI();
        if (currentHealth <= 0)
        {
            isDead.Invoke();
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
