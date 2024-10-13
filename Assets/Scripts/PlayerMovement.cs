using System.Collections;
using UnityEngine;

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

    private bool isGameStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGameStarted = false;
    }

    void Update()
    {
        if (isGameStarted)
        {
            float moveLR = Input.GetAxis("Horizontal");  // Left/Right Movement
            Vector2 vel = new Vector2(moveLR * speed, rb.velocity.y);
            rb.velocity = vel;

            if (sceneRotation.isVertical)  // Check vertical mode
            {
                if (Input.GetKey(KeyCode.Space))  // Jetpack
                {
                    useJet = true;
                    rb.velocity = new Vector2(rb.velocity.x, jetpackForce);  // Apply jetpack force
                }
                else
                {
                    useJet = false;
                }

                if (!useJet && rb.velocity.y < 0)  // Apply gravity in vertical mode
                {
                    rb.velocity += new Vector2(0, -normalFallSpeed);
                }
            }
            else  // Landscape mode (horizontal)
            {
                if (Input.GetKeyDown(KeyCode.Space))  // Jump
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForceLandscape);
                }

                if (rb.velocity.y < 0)  // Apply slow fall
                {
                    rb.velocity += new Vector2(0, -fallSpeedLandscape);
                }

                useJet = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WinTrigger"))
        {
            Win();
            cameraMovement.StopCamera();
            sceneRotation.StopRotation();
        }
    }

    public void StartGame()  // Call this from anywhere to start player movement
    {
        isGameStarted = true;
        Debug.Log("Player can now move!");
    }

    void Die()
    {
        Debug.Log("Player has died :(");
        FindObjectOfType<EventControl>().ShowGameOverPanel();  // Notify EventControl
    }

    void Win()
    {
        Debug.Log("Winner Winner Chicken Dinner!");
        FindObjectOfType<EventControl>().ShowWinPanel();  // Notify EventControl
    }
}
