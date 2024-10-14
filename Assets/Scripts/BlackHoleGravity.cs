using UnityEngine;

public class BlackHoleGravity : MonoBehaviour
{
    public float gravityStrength = 5f;        // Gravitational strength for natural floating effect
    public Transform targetTransform;         // Reference to the target (player) transform
    public float gravityFieldRadius = 10f;    // Range for the gravitational field
    public float maxGravityForce = 150f;      // Maximum gravitational force applied

    private Rigidbody2D targetRb;             // Reference to the target's Rigidbody2D

    void Start()
    {
        if (targetTransform != null)
        {
            targetRb = targetTransform.GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        
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