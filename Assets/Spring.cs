using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    // Variable to control the force applied by the spring
    public float springForce = 10f;

    // Detect when any object collides with the spring
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Get the Rigidbody2D of the colliding object
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        // If the object has a Rigidbody2D, apply the spring force
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, springForce);
        }
    }
}