using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ⁠ https://www.youtube.com/watch?v=mldjoVDhKc4 ⁠ Reference
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public SceneRotation sceneRotation;
    private float speed = 5f;
    private float jumpForceLandscape = 6.0f; 
    private float fallSpeedLandscape = 0.09f; 
    private float jetpackForce = 3.0f; 
    private float normalFallSpeed = 0.05f; 
    private Rigidbody2D rb;
    private bool useJet;
    public CameraMovement cameraMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveLR = Input.GetAxis("Horizontal"); // Left/Righ Movement
        Vector2 vel= new Vector2(moveLR * speed, rb.velocity.y);
        rb.velocity = vel;

        if (sceneRotation.isVertical) //Check vert
        {
            if (Input.GetKey(KeyCode.Space)) // disable jump, enable jet
            {
                useJet = true;
                rb.velocity = new Vector2(rb.velocity.x, jetpackForce);
            }
            else
            {
                useJet = false;
            }

            if (!useJet && rb.velocity.y < 0) // Use normal gravity in vertical mode
            {
                rb.velocity += new Vector2(0, -normalFallSpeed); // Normal falling speed
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space)) // input spaceBar
            {
                Vector2 spac = new Vector2(rb.velocity.x, jumpForceLandscape);
                rb.velocity = spac;
            }

            if (rb.velocity.y < 0) // Slow landscape fall
            {
                Vector2 slo = new Vector2(0, -fallSpeedLandscape);
                rb.velocity += slo;
            }

            useJet = false;
        }
    }

    
    void OnCollisionEnter2D(Collision2D collision) // If collided with spike
    {
        if (collision.gameObject.CompareTag("Spike")) //check if it is spike
        {
            Die();
        }
    }

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WinTrigger")) // If it is winTrigger
        {
            Win();
            cameraMovement.StopCamera();
            sceneRotation.StopRotation();
        }
    }

    void Die()
    {
        Debug.Log("Player has died :("); //print death
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void Win()
    {
        Debug.Log("Winner Winner Chicken Dinner!"); //print win
    }
}
