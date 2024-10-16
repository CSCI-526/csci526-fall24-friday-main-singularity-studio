using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxLeftRight : MonoBehaviour
{
    public float speed = 2f; // Speed of movement, changeable in the Unity Editor
    public float distance = 3f; // Distance to move, changeable in the Unity Editor

    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        startPosition = transform.position; // Record the starting position
    }

    void Update()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= startPosition.x + distance)
            {
                movingRight = false; // Change direction when it reaches the set distance to the right
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= startPosition.x - distance)
            {
                movingRight = true; // Change direction when it reaches the set distance to the left
            }
        }
    }
}

