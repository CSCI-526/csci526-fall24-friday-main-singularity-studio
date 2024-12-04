using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHandler : MonoBehaviour
{
    [SerializeField] private float jumpForceLandscape = 6.0f;
    [SerializeField] private float jumpForcePortrait = 10.0f;
    [SerializeField] private float normalFallSpeed = 0.05f;
    [SerializeField] private float fallSpeedLandscape = 0.09f;
    [SerializeField] private AudioClip jumpSound; // Add a reference for the jump sound effect
    private AudioSource audioSource;             // Reference to the AudioSource component
    private Rigidbody2D rb;
    public SceneRotation sceneRotation;
    private GameObject fireLeft;
    private GameObject fireRight;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fireLeft = GameObject.Find("Fire Left");
        fireRight = GameObject.Find("Fire Right");

        if (fireLeft != null && fireRight != null)
        {
            fireLeft.SetActive(false);
            fireRight.SetActive(false);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initialize the AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume= 0.3f;
    }

    public void HandleJump()
    {
        if (sceneRotation.isVertical && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForcePortrait);
            PlayJumpSound(); // Play the jump sound
            if (fireLeft != null && fireRight != null && spriteRenderer.sprite.name != "Player_Head")
            {
                StartCoroutine(DelayStopTime());
            }
        }
        else if (!sceneRotation.isVertical && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForceLandscape);
            PlayJumpSound(); // Play the jump sound
            if (fireLeft != null && fireRight != null && spriteRenderer.sprite.name != "Player_Head")
            {
                StartCoroutine(DelayStopTime());
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
    }

    IEnumerator DelayStopTime()
    {
        fireLeft.SetActive(true);
        fireRight.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fireLeft.SetActive(false);
        fireRight.SetActive(false);
    }

    private void PlayJumpSound()
    {
        if (jumpSound != null && audioSource != null && spriteRenderer.sprite.name != "Player_Head")
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }
}
