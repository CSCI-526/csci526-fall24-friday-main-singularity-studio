using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ⁠ https://www.youtube.com/watch?v=mldjoVDhKc4 ⁠ Reference
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public SceneRotation sceneRotation;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForceLandscape = 6.0f; 
    [SerializeField] private float jumpForcePortrait = 4.0f; 
    [SerializeField] private float fallSpeedLandscape = 0.09f; 
    [SerializeField] private float normalFallSpeed = 0.05f; 
    private Rigidbody2D rb;
    public CameraMovement cameraMovement;
    private Health health;
    private int healthDamage = 1;
    private Vector3 playerOriginalPosition;
    public GameObject cam; 
    public Vector3 CameraOriginalPosition;
    public GameObject scene;
    public Vector3 sceneOriginalPosition; 

    private bool isGameStated = false;
    private float startTime; // Track the game start time

    private HashSet<GameObject> damagedSpikes = new HashSet<GameObject>();
    private float stayOnWallTime = 0.0f;
    private float stayOnSpikeTime = 0.0f;
    private bool isWallDamage = false;
    private float damageCoolDown = 0.5f;

    public Image ProgressBarImg;
    private float ProgressBarWidth;
    private RectTransform rt;

    // Analytics
    private int currentLevel = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();

        ProgressBarImg = GameObject.Find("Progress").GetComponent<Image>();
        rt = ProgressBarImg.GetComponent<RectTransform>();

        rt.anchorMin = new Vector2(0, 0.5f);
        rt.anchorMax = new Vector2(0, 0.5f);

        ProgressBarWidth = rt.rect.width;
        rt.sizeDelta = new Vector2(1, rt.sizeDelta.y);
    }

    void Update()
    {
        if (isGameStated)
        {
            float moveLR = Input.GetAxis("Horizontal");
            Vector2 vel = new Vector2(moveLR * speed, rb.velocity.y);
            rb.velocity = vel;

            if (sceneRotation.isVertical)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForcePortrait);
                }
                if (rb.velocity.y < 0)
                {
                    rb.velocity += new Vector2(0, -normalFallSpeed);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForceLandscape);
                }
                if (rb.velocity.y < 0)
                {
                    rb.velocity += new Vector2(0, -fallSpeedLandscape);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike") && !sceneRotation.isRotating)
        {
            if (!damagedSpikes.Contains(collision.gameObject))
            {
                damagedSpikes.Add(collision.gameObject);
                playerOriginalPosition = gameObject.transform.position;

                cam = GameObject.Find("Main Camera");
                CameraOriginalPosition = cam.transform.position;

                scene = GameObject.Find("GameView");
                sceneOriginalPosition = scene.transform.position;
                Die();
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            stayOnSpikeTime += Time.deltaTime;
            if (stayOnSpikeTime >= damageCoolDown)
            {
                Die();
                stayOnSpikeTime = 0.0f;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            damagedSpikes.Remove(collision.gameObject);
            stayOnSpikeTime = 0.0f;
        }
    }

    public void StartGame()
    {
        startTime = Time.time; // Initialize start time
        isGameStated = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "EndPhase1")
        {
            rt.sizeDelta = new Vector2(ProgressBarWidth / 3, rt.sizeDelta.y);
        }
        else if (collision.gameObject.name == "EndPhase2")
        {
            rt.sizeDelta = new Vector2(2 * ProgressBarWidth / 3, rt.sizeDelta.y);
        }
        else if (collision.gameObject.CompareTag("WinTrigger"))
        {
            rt.sizeDelta = new Vector2(ProgressBarWidth, rt.sizeDelta.y);
            Win();
            cameraMovement.StopCamera();
            sceneRotation.StopRotation();
        }
        else if (collision.gameObject.CompareTag("LevelTrigger"))
        {
            Debug.Log("Leveled up");
            Destroy(collision.gameObject);
            currentLevel++;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            stayOnWallTime += Time.deltaTime;
            if (stayOnWallTime < damageCoolDown && !isWallDamage)
            {
                Die();
                isWallDamage = true;
            }
            else if (stayOnWallTime >= damageCoolDown)
            {
                stayOnWallTime = 0.0f;
                isWallDamage = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            stayOnWallTime = 0.0f;
        }
    }

    void Die()
    {
        Debug.Log("Lost one heart");
        health.TakeDamage(healthDamage);
        if (health.currentHealth <= 0)
        {
            float playTime = Time.time - startTime; // Calculate play time
            AnalyticsManager.trackProgress(currentLevel, false, playTime);
            Debug.Log("You died at level " + currentLevel);
        }
    }

    void Win()
    {
        float playTime = Time.time - startTime; // Calculate play time
        FindObjectOfType<EventControl>().ShowWinPanel();
        Debug.Log("Winner Winner Chicken Dinner!");
        AnalyticsManager.trackProgress(currentLevel, true, playTime);
    }
}
