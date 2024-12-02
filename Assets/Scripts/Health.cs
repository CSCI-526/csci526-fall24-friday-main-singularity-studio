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
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // Sound Effects
    [SerializeField] private AudioClip damageSound; // Sound when taking damage
    [SerializeField] private AudioClip healSound;   // Sound when healing
    [SerializeField] private AudioClip deathSound;  // Sound when dying
    private AudioSource audioSource;

    // Analytic HeartCollection
    private HeartTracker currentHeart; 
    private int previousHealth; 
    private bool isCollectingHeart = false;

    public delegate void PlayerDiedEventHandler();
    public event PlayerDiedEventHandler OnPlayerDied;

    private void Start()
    {
        // Initialize health
        if (currentHealth == 0)
        {
            ResetHealth();
            print("We either just started/restarted the game");
        }
        UpdateHearts();

        // Initialize audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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
        PlayHealSound(); // Play the heal sound
    }

    public void TakeDamage(int damage, DamageCause cause)
    {
        var eventControl = FindObjectOfType<EventControl>();

        // Log the damage cause for debugging
        if (cause == DamageCause.FortuneHeart && currentHeart != null)
        {
            Debug.Log($"Lost health due to fortune heart - ID: {currentHeart.heartID}, Name: {currentHeart.heartName}");
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            UpdateHearts();

            // If player dies, play death sound and invoke death event
            PlayDeathSound();
            OnPlayerDied?.Invoke();

            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                eventControl.ShowGameOverPanel();
            }

            return; // Exit early to prevent playing damage sound
        }

        UpdateHearts();
        StartCoroutine(ShowDamage());
        PlayDamageSound(); // Play the damage sound
    }

    IEnumerator ShowDamage()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void UpdateHearts()
    {
        // Ensure the player has the maximum of (3) hearts
        if (maxHealth > numOfHearts)
        {
            maxHealth = numOfHearts;
        }

        // Update heart UI upon collision
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
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

    private void PlayDamageSound()
    {
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }

    private void PlayHealSound()
    {
        if (healSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(healSound);
        }
    }

    private void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}
