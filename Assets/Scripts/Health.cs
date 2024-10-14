using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public static int currentHealth;

    public int numOfHearts;
    public GameObject[] hearts;
    public GameObject fullHeart;
    public GameObject emptyHeart;

    private void Start()
    {
        if (currentHealth == 0)
        {
            ResetHealth();
            print("We either just started/restarted the game");
        }
        UpdateHearts();
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("YOU LOST!");
            PlayerPrefs.DeleteAll();
            Time.timeScale = 0f; //Pause the game (for now)
        }
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        //Ensure the player has the maximum of (3) hearts
        if(maxHealth > numOfHearts){
            maxHealth = numOfHearts;
        }

        //Update heart UI upon collision
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].GetComponent<RawImage>().color = new Color(255, 0, 0 );
            else
                hearts[i].GetComponent<RawImage>().color = new Color(0, 0, 0 );
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
