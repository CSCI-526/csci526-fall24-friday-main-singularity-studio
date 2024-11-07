using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCollision : MonoBehaviour
{
    private HashSet<GameObject> damagedSpikes = new HashSet<GameObject>();
    private float stayOnSpikeTime = 0.0f;
    private const float damageCooldown = 0.5f;
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            if (!damagedSpikes.Contains(collision.gameObject))
            {
                damagedSpikes.Add(collision.gameObject);
                Die();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            stayOnSpikeTime += Time.deltaTime;
            if (stayOnSpikeTime >= damageCooldown)
            {
                Die();
                stayOnSpikeTime = 0.0f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            damagedSpikes.Remove(collision.gameObject);
            stayOnSpikeTime = 0.0f;
        }
    }

    private void Die()
    {
        StartCoroutine(ShowDamage());
        health.TakeDamage(1, DamageCause.Hazard);
        AnalyticsManager.trackDamageCause("spike");
        Debug.Log("Lost one heart due to spike collision");
    }

    IEnumerator ShowDamage()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
