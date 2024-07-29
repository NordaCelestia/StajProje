using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterData : MonoBehaviour
{
    
    [SerializeField] private CharacterDataScriptableObject characterData;

    private int currentHealth;

    private void Start()
    {
        currentHealth = characterData.maxHealth;
    }

    

    public void takeDamage()
    {
        currentHealth -= 1;
        Debug.Log(characterData.characterName + " takes damage! remaining health: " + currentHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("snowball"))
        {
            takeDamage();
        }
    }

}
