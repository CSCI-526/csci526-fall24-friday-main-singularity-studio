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

        StartCoroutine(ChangeColorAfterDelay());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Player){
            if (this.gameObject.name == "Platform Trap")
            {
                StartCoroutine(DestroyAfterDelay());
            }
            else if (this.gameObject.name == "heart")
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
        }
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
