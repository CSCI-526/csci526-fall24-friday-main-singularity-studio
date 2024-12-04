using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikeCollision : MonoBehaviour
{
    private float stayOnSpikeTime = 0.0f;
    private const float damageCooldown = 0.75f;
    private bool canTakeDamage = true;
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike") && canTakeDamage)
        {
            Die();
            StartCoroutine(DamageCooldown());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            stayOnSpikeTime += Time.deltaTime;
            if (stayOnSpikeTime >= damageCooldown && canTakeDamage)
            {
                Die();
                stayOnSpikeTime = 0.0f;
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            stayOnSpikeTime = 0.0f;
        }
    }

    private void Die()
    {
        StartCoroutine(ShowDamage());
        health.TakeDamage(1, DamageCause.Hazard);
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            AnalyticsManager.trackDamageCause("spike");
        }
        Debug.Log("Lost one heart due to spike collision");
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false; 
        yield return new WaitForSeconds(damageCooldown); 
        canTakeDamage = true;
    }

    IEnumerator ShowDamage()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
