using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHandler : MonoBehaviour
{
    [SerializeField] private float jumpForceLandscape = 6.0f;
    [SerializeField] private float jumpForcePortrait = 10.0f;
    [SerializeField] private float jumpForceLowHealth = 1.0f; // New variable for low health jump force
    [SerializeField] private float normalFallSpeed = 0.05f;
    [SerializeField] private float fallSpeedLandscape = 0.09f;
    private Rigidbody2D rb;
    public SceneRotation sceneRotation;
    private Health playerHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = FindObjectOfType<Health>(); // Reference to the Health script
    }

    public void HandleJump()
    {
        if (playerHealth != null && playerHealth.currentHealth == 1) // Check if health is 1
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForceLowHealth); // Apply low-health jump force
            }
        }
        else
        {
            if (sceneRotation.isVertical && Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForcePortrait);
            }
            else if (!sceneRotation.isVertical && Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForceLandscape);
            }
        }
    }

    private void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += sceneRotation.isVertical 
                ? new Vector2(0, -normalFallSpeed) 
                : new Vector2(0, -fallSpeedLandscape);
        }

        HandleJump(); // Ensure jump logic is checked in every frame
    }
}
