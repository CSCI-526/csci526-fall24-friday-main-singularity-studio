using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DisappearOnCollision : MonoBehaviour
{
    private GameObject Player;
    private float objectID;
    private Health health;

    private void Start()
    {
        Player = GameObject.Find("Player");
        health = Player.GetComponent<Health>();
        if (Player == null)
        {
            Debug.LogError("Player  not found in the scene!");
        }
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
                health.Heal(1);
                Destroy(gameObject);
            }
        }
    }
    

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1f); 
        Destroy(gameObject); 
    }
}
