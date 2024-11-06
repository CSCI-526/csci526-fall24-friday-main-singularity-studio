using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    private float stayOnWallTime = 0.0f;
    private bool isWallDamage = false;
    private const float damageCooldown = 0.5f;
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            stayOnWallTime += Time.deltaTime;

            if (stayOnWallTime < damageCooldown && !isWallDamage)
            {
                Die();
                isWallDamage = true;
            }
            else if (stayOnWallTime >= damageCooldown)
            {
                stayOnWallTime = 0.0f;
                isWallDamage = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            stayOnWallTime = 0.0f;
        }
    }

    private void Die()
    {
        StartCoroutine(ShowDamage());

        health.TakeDamage(1);
        AnalyticsManager.trackDamageCause("wall");
        Debug.Log("Lost one heart due to wall collision");
    }

    IEnumerator ShowDamage()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
