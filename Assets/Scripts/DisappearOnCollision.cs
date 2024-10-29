using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DisappearOnCollision : MonoBehaviour
{
    private GameObject Player;
    private float objectID;
    private Health health;
    private Color healColor = Color.red;  
    private Color damageColor = Color.magenta; 
    private bool isOriginalColor = true; 

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
        if(gameObject.name == "fortuneHeart"){
            StartCoroutine(ChangeColorAfterDelay());
        }
    }

    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Player){
            if (this.gameObject.name == "Platform Trap")
            {
                StartCoroutine(DestroyAfterDelay());
            }
            else if (this.gameObject.name == "fortuneHeart")
            {
                if (isOriginalColor){
                    print("heal");
                    health.Heal(1);
                }else{
                    print("damage");
                    health.TakeDamage(1);
                }
                Destroy(gameObject);
            }
            else if(this.gameObject.name == "heart"){
                print("fortune");
                health.Heal(1);
                Destroy(gameObject);
            }
        }
        // else{
        //     if (this.gameObject.name == "heart-test"){
        //         print(other.gameObject.name);
        //     }
            
        // }
    }
    

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1f); 
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
        foreach (Transform child in transform){
            Color newColor = isOriginalColor ? healColor : damageColor;

            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.color = newColor;
            }

            Renderer renderer = child.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material.color = newColor;
            }
        }
    }
}
