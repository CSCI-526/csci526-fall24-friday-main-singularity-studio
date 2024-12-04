using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    private float lastZRotation; 
    public bool isSurprise = false;
    private bool isTouchingPlatform;
    private SpriteRenderer spriteRenderer;
    private Sprite notRollingSprite;
    public Sprite supriseSprite;
    public Sprite rollingSprite;
    [SerializeField] private AudioClip rollingSound; 
    private AudioSource audioSource;

    private bool isRolling = false; 
    private float movementThreshold = 0.1f;
    private Rigidbody2D playerRigidbody;  

    // Start is called before the first frame update
    void Start()
    {
        lastZRotation = transform.rotation.eulerAngles.z;
        spriteRenderer = GetComponent<SpriteRenderer>();
        notRollingSprite = spriteRenderer.sprite;

        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = rollingSound;
        audioSource.loop = true;
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float currentZRotation = transform.rotation.eulerAngles.z;
        if(isSurprise){
            transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.sprite = supriseSprite;
        }
        else if (isTouchingPlatform) {
            spriteRenderer.sprite = rollingSprite;
        }else{
            transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.sprite = notRollingSprite;
        }
        lastZRotation = currentZRotation;
        
        if (isTouchingPlatform && IsPlayerMoving())
        {
            StartRollingSound();
        }
        else
        {
            StopRollingSound();
        }
    }

    private void StartRollingSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void StopRollingSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private bool IsPlayerMoving()
    {
        // Check if the player's velocity is above the movement threshold
        return playerRigidbody.velocity.magnitude > movementThreshold;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Square" || collision.gameObject.name == "Platform Trap Art")
        {
            isTouchingPlatform = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Square" || collision.gameObject.name == "Platform Trap Art")
        {
            isTouchingPlatform = false;
        }
    }

}
