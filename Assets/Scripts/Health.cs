using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum DamageCause
{
    Hazard,
    FortuneHeart
}

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public int numOfHearts;
    public GameObject[] hearts;
    public GameObject fullHeart;
    public GameObject emptyHeart;

    //Analytic HeartCollection
    private HeartTracker currentHeart; 
    private int previousHealth; 
    private bool isCollectingHeart = false;

    public delegate void PlayerDiedEventHandler();
    public event PlayerDiedEventHandler OnPlayerDied;

    private void Start()
    {
        if (currentHealth == 0)
        {
            ResetHealth();
            print("We either just started/restarted the game");
        }
        UpdateHearts();
    }
    public void CollectHeart(HeartTracker heart)
    {
        currentHeart = heart;
        previousHealth = currentHealth;
        isCollectingHeart = true;
    }


    public void Heal(int healAmount)
    {
        // Log healing if a heart was collected
        if (isCollectingHeart && currentHeart != null)
        {
            Debug.Log($"Healing from Collected Heart - ID: {currentHeart.heartID}, Name: {currentHeart.heartName}");
        }
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHearts();
    }

    public void TakeDamage(int damage, DamageCause cause)
    {
        // Log the damage cause for debugging
        if (cause == DamageCause.FortuneHeart && currentHeart != null)
        {
            Debug.Log($"Lost health due to fortune heart - ID: {currentHeart.heartID}, Name: {currentHeart.heartName}");
        }
        StartCoroutine(ShowDamage());
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnPlayerDied?.Invoke();

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
        // Log heart collection if a heart was collected
        if (isCollectingHeart && currentHeart != null)
        {
            bool gainedHealth = currentHealth > previousHealth;  // Determine if health increased
            bool lostHealth = currentHealth < previousHealth;    // Determine if health decreased

            AnalyticsManager.TrackHeartCollection(currentHeart.heartID, currentHeart.heartName, gainedHealth, lostHealth);

            // Reset currentHeart and flag after logging
            currentHeart = null;
            isCollectingHeart = false;
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
