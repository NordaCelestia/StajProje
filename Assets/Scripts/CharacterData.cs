using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterData : MonoBehaviour
{
    [SerializeField] private CharacterDataScriptableObject characterData;
    [SerializeField] Image hpBar;
    [SerializeField] GameObject healthBar;

    float healthPercentage;
    private int currentHealth;

    private void Start()
    {
        currentHealth = characterData.maxHealth;
    }

    private void Update()
    {
        // healthBar GameObject'inin sadece x pozisyonunu this.gameObject'in x pozisyonuna eþitleme
        Vector3 newPosition = healthBar.transform.position;
        newPosition.x = this.gameObject.transform.position.x;
        healthBar.transform.position = newPosition;

        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        currentHealth -= 1;
        Debug.Log(characterData.characterName + " takes damage! remaining health: " + currentHealth);
        UpdateUI();
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
