using UnityEngine;

public class BlackHoleGravity : MonoBehaviour
{
    public float gravityStrength = 5f;        // Gravitational strength for natural floating effect
    public Transform targetTransform;         // Reference to the target (player) transform
    public float gravityFieldRadius = 10f;    // Range for the gravitational field
    public float maxGravityForce = 150f;      // Maximum gravitational force applied
    private float jumpForce = 6.0f;             // Force applied when the player jumps to escape

    private Rigidbody2D targetRb;             // Reference to the target's Rigidbody2D
    private bool isJumping = false;           // To track whether the player is jumping

    void Start()
    {
        if (targetTransform != null)
        {
            targetRb = targetTransform.GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        // Check if the player presses the spacebar to jump
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            // Apply an upward force to escape the black hole
            targetRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            // Once the player releases the jump key, stop jumping
            isJumping = false;
        }
    }

    void FixedUpdate()
    {
        if (targetTransform != null && !isJumping) // Only apply gravity when not jumping
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