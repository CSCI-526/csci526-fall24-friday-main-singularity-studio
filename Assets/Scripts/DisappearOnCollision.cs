using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DisappearOnCollision : MonoBehaviour
{
    private GameObject Player;
    private float objectID;
    private Health health;
    // private Color healColor = Color.red;  
    // private Color damageColor = Color.magenta; 
    private Color healColor = new Color(1f, 0.6f, 0.9f);  
    private Color damageColor = Color.yellow; 

    private bool isOriginalColor = true; 

    private List<GameObject> goodHearts = new List<GameObject>();
    private List<GameObject> poisonHearts = new List<GameObject>();

    public AudioClip glassBreakingSound;
    public AudioSource audioSource;

    private void Start()
    {
        Player = GameObject.Find("Player");
        health = Player.GetComponent<Health>();
        if (Player == null)
        {
            Debug.LogError("Player  not found in the scene!");
        }

        foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "Good Heart")
            {
                goodHearts.Add(child.gameObject);
            }
            else if (child.name == "Poison Heart")
            {
                poisonHearts.Add(child.gameObject);
            }
        }

        if(gameObject.name == "fortuneHeart"){
            StartCoroutine(ChangeColorAfterDelay());
        }
        if(gameObject.name == "Platform Trap Art"){
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.volume= 0.05f;
        }
    }

    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Player){
            if (this.gameObject.name == "Platform Trap" || this.gameObject.name == "Platform Trap Art")
            {
                PlayAudio();
                StartCoroutine(DestroyAfterDelay());
            }
            else if (this.gameObject.name == "fortuneHeart")
            {
                HeartTracker heart = GetComponent<HeartTracker>();
                if (heart != null || SceneManager.GetActiveScene().name == "Tutorial")
                {
                    if (SceneManager.GetActiveScene().name != "Tutorial")
                    {
                        health.CollectHeart(heart);
                    }
                    if (isOriginalColor)
                    {
                        health.Heal(1);
                    }
                    else
                    {
                        health.TakeDamage(1, DamageCause.FortuneHeart);
                        if (SceneManager.GetActiveScene().name != "Tutorial")
                        {
                            AnalyticsManager.trackDamageCause("FortuneHeart");
                        }
                    }
                }
                Destroy(gameObject);
            }
            else if(this.gameObject.name == "heart"){
                HeartTracker heart = GetComponent<HeartTracker>();
                if (heart != null || SceneManager.GetActiveScene().name == "Tutorial")
                {
                    if (SceneManager.GetActiveScene().name != "Tutorial")
                    {
                        health.CollectHeart(heart);
                    }
                    health.Heal(1);
                    Destroy(gameObject);
                }
            }
            else if(this.gameObject.name == "poison heart"){
                health.TakeDamage(1, DamageCause.Hazard);
                Destroy(gameObject);
            }
        }
    }
    

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.75f); 
        // yield return new WaitForSeconds(2f); 
        Destroy(gameObject);
    }
    private void PlayAudio()
    {
        print("play glass breaking sound");
        if (glassBreakingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(glassBreakingSound);
        }
    }

    private IEnumerator ChangeColorAfterDelay(){
        while (true)
        {
            changeHeartColor();
            yield return new WaitForSeconds(2f);
            isOriginalColor = !isOriginalColor;
        }
    }
    private void changeHeartColor(){
        if(isOriginalColor){
            SetGoodHeartsActive(true);
            SetPoisonHeartsActive(false);
            
        }
        else{
            SetGoodHeartsActive(false);
            SetPoisonHeartsActive(true);
        }
    }

    private void SetGoodHeartsActive(bool isActive)
    {
        foreach (GameObject heart in goodHearts)
        {
            heart.SetActive(isActive);
        }
    }

    private void SetPoisonHeartsActive(bool isActive)
    {
        foreach (GameObject heart in poisonHearts)
        {
            heart.SetActive(isActive);
        }
    }
}
