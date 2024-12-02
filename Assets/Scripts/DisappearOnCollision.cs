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

    private void Start()
    {
        Player = GameObject.Find("Player");
        health = Player.GetComponent<Health>();
        if (Player == null)
        {
            Debug.LogError("Player  not found in the scene!");
        }

        // Collider2D thisCollider = GetComponent<Collider2D>(); // Get the current object's 2D collider
        // GameObject[] spikes = GameObject.FindGameObjectsWithTag("Wall");
        // print(spikes.Length);

        // // Loop through each spike and ignore collision with the current object
        // foreach (GameObject spike in spikes)
        // {
        //     Collider2D spikeCollider = spike.GetComponent<Collider2D>();

        //     if (spikeCollider != null)
        //     {
        //         Physics2D.IgnoreCollision(thisCollider, spikeCollider);
        //         Debug.Log($"Ignoring collision between {this.gameObject.name} and {spike.name}");
        //     }
        // }

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
        // if (goodHearts.Count > 0 && poisonHearts.Count > 0)
        // {
        //     SetGoodHeartsActive(true);
        //     SetPoisonHeartsActive(false);
        //     StartCoroutine(ChangeColorAfterDelay());
        // }

        if(gameObject.name == "fortuneHeart"){
            StartCoroutine(ChangeColorAfterDelay());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Player){
            if (this.gameObject.name == "Platform Trap" || this.gameObject.name == "Platform Trap Art")
            {
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
        yield return new WaitForSeconds(.5f); 
        Destroy(gameObject); 
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

        // foreach (Transform child in transform){
        //     Color newColor = isOriginalColor ? healColor : damageColor;

        //     SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

        //     if (spriteRenderer != null)
        //     {
        //         spriteRenderer.color = newColor;
        //     }

        //     Renderer renderer = child.GetComponent<Renderer>();

        //     if (renderer != null)
        //     {
        //         renderer.material.color = newColor;
        //     }
        // }
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
