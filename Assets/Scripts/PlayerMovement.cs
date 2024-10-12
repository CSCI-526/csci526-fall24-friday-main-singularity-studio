using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ⁠ https://www.youtube.com/watch?v=mldjoVDhKc4 ⁠ Reference
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private float speed = 5f;
    private float jumpForceLandscape = 6.0f; 
    private float fallSpeedLandscape = 0.09f; 
    private float jetpackForce = 3.0f; 
    private float normalFallSpeed = 0.05f; 
    private Rigidbody2D rb;
    private bool useJet;

    private bool isGameStarted = false;

    public Transform initialCheckpoint;
    private Vector3 checkpointPosition; 
    public CameraMovement cameraMovement;
    public GameObject gameOverPanel;
    public SceneRotation sceneRotation;

    public GameObject mainMenuUI;
    public GameObject winPanel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //set initial checkpoint
        isGameStarted = false;
        if (initialCheckpoint != null)
        {
            checkpointPosition = initialCheckpoint.position;
        }
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isGameStarted)
        {
            HandleMovement();
        }
    }
    void HandleMovement()
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
        if (collision.gameObject.CompareTag("CheckPoint"))
        {
            SetCheckpoint(collision.transform);  // Update the checkpoint
        }
    }

    void Die()
    {
        Debug.Log("Player has died :(");

        if (cameraMovement != null)
        {
            cameraMovement.StopCamera();
        }
        Time.timeScale = 0f;

        gameOverPanel.SetActive(true);

    }

    public void SetCheckpoint(Transform newCheckpoint)
    {
        checkpointPosition = newCheckpoint.position;
        Debug.Log("Checkpoint updated to: " + checkpointPosition);
    }


    public void RestartFromCheckpoint()
    {
        Debug.Log("Player has died and the game will restart.");

        gameOverPanel.SetActive(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }


    void Win()
    {
        Debug.Log("Player has won the game!");

        // Show the Win Panel
        MainMenu mainMenu = FindObjectOfType<MainMenu>();
        if (mainMenu != null)
        {
            mainMenu.ShowWinPanel();
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
        Debug.Log("Player can now move!");
    }
}
