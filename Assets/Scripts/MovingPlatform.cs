using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2.0f; // Speed for movement
    public bool moveHorizontally = true; // Control horizontal/vertical movement
    public float moveDistance = 3.0f; // Distance to move before changing direction
    private Vector3 initialPosition;
    private bool movingForward = true;
    private Rigidbody2D rb; // Reference to Rigidbody2D

    void Start()
    {
        // Store the initial position of the platform
        initialPosition = transform.position;

        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing on the platform");
        }

        // Make sure the Rigidbody is set to Kinematic (in case it's not set in the inspector)
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (moveHorizontally)
        {
            MoveHorizontally();
        }
        else
        {
            MoveVertically();
        }
    }

    void MoveHorizontally()
    {
        float movement = speed * Time.deltaTime;

        if (movingForward)
        {
            rb.MovePosition(rb.position + new Vector2(movement, 0));
            if (rb.position.x >= initialPosition.x + moveDistance)
            {
                movingForward = false;
            }
        }
        else
        {
            rb.MovePosition(rb.position + new Vector2(-movement, 0));
            if (rb.position.x <= initialPosition.x - moveDistance)
            {
                movingForward = true;
            }
        }
    }

    void MoveVertically()
    {
        float movement = speed * Time.deltaTime;

        if (movingForward)
        {
            rb.MovePosition(rb.position + new Vector2(0, movement));
            if (rb.position.y >= initialPosition.y + moveDistance)
            {
                movingForward = false;
            }
        }
        else
        {
            rb.MovePosition(rb.position + new Vector2(0, -movement));
            if (rb.position.y <= initialPosition.y - moveDistance)
            {
                movingForward = true;
            }
        }
    }
}