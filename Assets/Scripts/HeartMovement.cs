using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartMovement : MonoBehaviour
{
    public Transform player;  // The player's transform
    public float runAwaySpeed = 2f;  // How fast the object will run away
    public float detectionRange = 5f;  // How close the player needs to be before the object starts running
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        Collider2D thisCollider = GetComponent<Collider2D>();

        if (thisCollider == null)
        {
            Debug.LogError("No Collider2D found on this object!");
            return;
        }

        GameObject[] spikes = GameObject.FindGameObjectsWithTag("Wall");

        if (spikes.Length == 0)
        {
            Debug.LogWarning("No objects with the tag 'Spike' found!");
        }
        foreach (GameObject spike in spikes)
        {
            Collider2D spikeCollider = spike.GetComponent<Collider2D>();

            if (spikeCollider != null)
            {
                Physics2D.IgnoreCollision(thisCollider, spikeCollider);
                // Debug.Log($"Ignoring collision between {this.gameObject.name} and {spike.name}");
            }
            else
            {
                // Debug.LogWarning($"{spike.name} does not have a Collider2D.");
            }
        }
    }


    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            // Calculate the direction away from the player
            Vector2 directionAwayFromPlayer = (Vector2)(transform.position - player.position);
            directionAwayFromPlayer.Normalize();

            // Move the object away from the player
            rb2D.velocity = directionAwayFromPlayer * runAwaySpeed;
        }
        else
        {
            // Stop the object when out of range
            rb2D.velocity = Vector2.zero;
        }
    }
}
