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
    [SerializeField] private AudioClip jumpSound; // Add reference for the jump sound
    private Rigidbody2D rb;
    private AudioSource audioSource; // Audio source for playing sounds
    public SceneRotation sceneRotation;
    private Health playerHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        playerHealth = FindObjectOfType<Health>(); // Reference to the Health script
    }

    public void HandleJump()
    {
        if (playerHealth != null && playerHealth.currentHealth == 1) // Check if health is 1
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForceLowHealth); // Apply low-health jump force
                PlayJumpSound(); // Play jump sound
            }
        }
        else
        {
            if (sceneRotation.isVertical && Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForcePortrait);
                PlayJumpSound(); // Play jump sound
            }
            else if (!sceneRotation.isVertical && Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForceLandscape);
                PlayJumpSound(); // Play jump sound
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

    private void PlayJumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound); // Play the jump sound effect
        }
    }
}
