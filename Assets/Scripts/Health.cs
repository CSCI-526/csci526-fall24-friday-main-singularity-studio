using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

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
        StartCoroutine(ShowDamage());
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("YOU LOST!");
            PlayerPrefs.DeleteAll();
            // Time.timeScale = 0f; //Pause the game (for now)

            FindObjectOfType<EventControl>().ShowGameOverPanel();

        }
        UpdateHearts();
    }

    IEnumerator ShowDamage()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        transform.GetChild(0).gameObject.SetActive(false);
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
                hearts[i].GetComponent<RawImage>().color = new Color(1f, 0.4f, 0.7f); 
            else
                hearts[i].GetComponent<RawImage>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
