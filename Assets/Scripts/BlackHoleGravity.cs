using UnityEngine;
using System.Collections;

public class BlackHoleGravity : MonoBehaviour
{
    public float gravityStrength = 5f;        // Gravitational strength for natural floating effect
    public Transform targetTransform;         // Reference to the target (player) transform
    public float gravityFieldRadius = 10f;    // Range for the gravitational field
    public float maxGravityForce = 150f;      // Maximum gravitational force applied

    private Rigidbody2D targetRb;             // Reference to the target's Rigidbody2D
    private Transform player;       // Reference to the player's Transform
    // public Transform targetObject; // Reference to the target object's Transform
    private float previousDistance;
    private float affectiveDistance = 13f;
    public  AudioClip blackholeSound;
    private AudioSource audioSource;
    public float fadeDuration = 3f; 
    private bool isPlaying = false;

    void Start()
    {
        if (targetTransform != null)
        {
            targetRb = targetTransform.GetComponent<Rigidbody2D>();
        }
        player = GameObject.FindWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = blackholeSound;
        audioSource.loop = true; 
        audioSource.volume = 0f;

    }

    void Update()
    {
        // Calculate the current distance to player
        float currentDistance = Vector3.Distance(player.position, this.transform.position);

        // Check if the player is approaching
        if (currentDistance < affectiveDistance && !isPlaying)
        {
            Debug.Log("Player is approaching the target.");
            // audioSource.Play();
            StartCoroutine(FadeIn());
            isPlaying = true;
        }
        else if (currentDistance > affectiveDistance)
        {
            Debug.Log("Player is moving away from the target.");
            // audioSource.Stop();
            StartCoroutine(FadeOut());
            isPlaying = false;
        }  
    }

    private IEnumerator FadeIn()
    {
        float startVolume = 0.8f;
        audioSource.Play();
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
    }

    void FixedUpdate()
    {
        if (targetTransform != null)
        {
            // Calculate direction and distance from target to the black hole
            Vector2 directionToBlackHole = (Vector2)transform.position - (Vector2)targetTransform.position;
            float distanceToBlackHole = directionToBlackHole.magnitude;

            if (distanceToBlackHole < gravityFieldRadius)  // Apply gravity within the field radius
            {
                // Normalize the direction for consistent force application
                directionToBlackHole.Normalize();

                // Calculate the gravitational force based on the inverse square law
                float gravityForceMagnitude = Mathf.Min(gravityStrength / Mathf.Pow(distanceToBlackHole, 2), maxGravityForce);
                Vector2 gravityForce = directionToBlackHole * gravityForceMagnitude;

                // Apply the gravitational force to the player for natural floating
                targetRb.AddForce(gravityForce);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the gravitational field radius in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, gravityFieldRadius);
    }
}