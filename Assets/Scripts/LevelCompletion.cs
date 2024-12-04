using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.Analytics;

public class LevelCompletion : MonoBehaviour
{
    private int currentLevel = 0;
    public Image ProgressBarImg;
    private RectTransform rt;
    private float ProgressBarWidth;
    public CameraMovement cameraMovement;
    public SceneRotation sceneRotation;
    public ParticleSystem confetti1;
    public ParticleSystem confetti2;
    public ParticleSystem confetti3;
    private Vector2 mapPosition;
    private Vector2 mapScale;
    private Vector2 topLeftCorner;
    public float distanceFromStart;
    private float mapLength;
    private Scene currentScene;

    private float startTime; // Track the game start time
    private bool isGameStarted = false; // Track if the game has started

    public Health playerHealth;
    public PlayerControl play;
    public SceneRotation scene;
    public Button pauseButton;
    public Button resumeButton;
    public Button quitButton;
    public CameraMovement cam;
    public GameObject checkpointMessage;

    [SerializeField] private AudioClip checkpointSound; // Checkpoint sound
    [SerializeField] private AudioClip winningSound; // Checkpoint sound
    private AudioSource audioSource;                   // Reference to AudioSource

    private void Start()
    {
        ProgressBarImg = GameObject.Find("Progress").GetComponent<Image>();
        rt = ProgressBarImg.GetComponent<RectTransform>();
        ProgressBarWidth = rt.rect.width;
        rt.anchorMin = new Vector2(0, 0.5f);
        rt.anchorMax = new Vector2(0, 0.5f);
        rt.sizeDelta = new Vector2(1, rt.sizeDelta.y);

        mapPosition = sceneRotation.transform.position;
        mapScale = sceneRotation.transform.localScale;

        currentScene = SceneManager.GetActiveScene();

        if (currentScene.name != "Tutorial")
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
                playerHealth.OnPlayerDied += HandlePlayerDeath; // Subscribe to the event
            }
            else
            {
                Debug.LogError("Player object not found. Make sure the player is tagged 'Player' and has a Health component.");
            }
            mapLength = 380f; // 387
            confetti1.Stop();
            confetti2.Stop();
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
            playerHealth.OnPlayerDied += HandlePlayerDeath; // Subscribe to the event
            topLeftCorner = mapPosition + new Vector2(mapScale.x / 2 - 28, mapScale.y / 2);
        }
        else
        {
            mapLength = 180f;
            topLeftCorner = mapPosition + new Vector2(mapScale.x / 2, mapScale.y / 2);
        }

        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        // Calculate the distance from the player to the top-left corner
        distanceFromStart = Vector2.Distance(transform.position, topLeftCorner);
        float progress = (mapLength - distanceFromStart) * 100 / mapLength;
        rt.sizeDelta = new Vector2(ProgressBarWidth * (100 - progress) / 100, rt.sizeDelta.y);
    }

    // Method to start tracking playtime
    public void StartGameTimer()
    {
        startTime = Time.time;
        isGameStarted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "EndPhase1" && currentScene.name != "Tutorial")
        {
            confetti1.Play();
            StartCoroutine(DisplayMessage());
            PlayCheckpointSound(); // Play checkpoint sound
            Debug.Log("Level 2");
            playerHealth.Heal(3);
            cam.speed = 4f;
            scene.rotationPeriod = 8.75f;
        }
        else if (collision.gameObject.name == "EndPhase2" && currentScene.name != "Tutorial")
        {
            confetti2.Play();
            StartCoroutine(DisplayMessage());
            PlayCheckpointSound(); // Play checkpoint sound
            Debug.Log("Level 3");
            playerHealth.Heal(3);
            play.speed = 8.5f;
            cam.speed = 4.5f;
            scene.rotationPeriod = 7.78f;
        }
        else if (collision.gameObject.CompareTag("WinTrigger"))
        {
            confetti3.Play();
            // PlayCheckpointSound(); // Play checkpoint sound
            PlayWinningSound();
            Win();
        }
        else if (collision.gameObject.CompareTag("LevelTrigger"))
        {
            Debug.Log("Leveled up");
            Destroy(collision.gameObject);
            currentLevel++;
        }
    }

    private IEnumerator DisplayMessage()
    {
        checkpointMessage.SetActive(true);
        yield return new WaitForSeconds(3f); 
        checkpointMessage.SetActive(false);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WinTrigger"))
        {
            GameObject wallDamage = GameObject.Find("Wall Damage Trigger");
            if (wallDamage != null)
            {
                wallDamage.SetActive(false);
            }
        }
    }

    private void PlayCheckpointSound()
    {
        if (checkpointSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(checkpointSound);
        }
    }

    private void PlayWinningSound()
    {
        if (winningSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(winningSound);
        }
    }
    private void HandlePlayerDeath()
    {
        Debug.Log("Player has died. Game Over.");
        UploadDeathData(); // Custom method to upload data
        var eventControl = FindObjectOfType<EventControl>();
        if (eventControl != null)
        {
            eventControl.ShowGameOverPanel();
        }
        else
        {
            Debug.LogError("EventControl component is missing.");
        }

        if (cameraMovement != null)
        {
            cameraMovement.StopCamera();
        }
        else
        {
            Debug.LogError("CameraMovement is missing.");
        }

        if (sceneRotation != null)
        {
            sceneRotation.StopRotation();
        }
        else
        {
            Debug.LogError("SceneRotation is missing.");
        }
    }

    private static string CategorizePlayTime(float playTime)
    {
        if (playTime <= 10) return "Very Short";
        else if (playTime <= 30) return "Short";
        else if (playTime <= 60) return "Moderate";
        else if (playTime <= 120) return "Long";
        else return "Very Long";
    }

    private void UploadDeathData()
    {
        float playTime = Time.time - startTime; // Calculate time spent in level
        string timeCategory = CategorizePlayTime(playTime);
        AnalyticsManager.trackProgress(currentLevel, false, playTime, timeCategory); // Upload data indicating game over
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent potential memory leaks
        playerHealth.OnPlayerDied -= HandlePlayerDeath;
    }

    private void Win()
    {
        Debug.Log("Win method called");
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;
        if (sceneName == "Tutorial")
        {
            StartCoroutine(PauseForAnimationTutorial());
        }
        else
        {
            float playTime = Time.time - startTime; // Calculate the play duration
            string timeCategory = CategorizePlayTime(playTime);
            AnalyticsManager.trackProgress(currentLevel, true, playTime, timeCategory); // Pass playtime to AnalyticsManager

            StartCoroutine(PauseForAnimationGame());
        }
    }

    private IEnumerator PauseForAnimationTutorial()
    {
        yield return new WaitForSeconds(2.5f);
        cameraMovement.StopCamera();
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        FindObjectOfType<EventControl>().ShowWinPanel();
        quitButton.gameObject.SetActive(false);
    }

    private IEnumerator PauseForAnimationGame()
    {
        yield return new WaitForSeconds(1.5f);
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        FindObjectOfType<EventControl>().ShowWinPanel();
        cameraMovement.StopCamera();
        sceneRotation.StopRotation();
        isGameStarted = false; // Reset the game state
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1.5f);
        FindObjectOfType<EventControl>().StartGame();
    }
}
