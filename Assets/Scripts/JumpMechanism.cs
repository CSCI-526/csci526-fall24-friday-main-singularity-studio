using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMechanism : MonoBehaviour
{
    public float springForce = 8f;  // You can adjust the spring force in the Inspector

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the Rigidbody2D component of the object that collided with the spring
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        // If the object has a Rigidbody2D, apply an upward force
        if (rb != null)
        {
            // Set the Y velocity to a positive value (springForce) to push the player upwards
            Vector2 velocity = rb.velocity;
            velocity.y = springForce;
            rb.velocity = velocity;
        }
    }
}
