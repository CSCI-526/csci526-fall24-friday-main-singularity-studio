using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Include this to manage UI elements like buttons

using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Analytics;

public class EventControl : MonoBehaviour
{
    public GameObject instructionPanel;    // Panel for instructions
    public GameObject mainMenuUI;          // Main menu UI
    public GameObject gameOverPanel;       // Game Over panel
    public GameObject winPanel;            // Win panel
    public GameObject levelsPanel;
    public GameObject Message; 
    public PlayerControl playerMovement;   // Reference to PlayerMovement
    public CameraMovement cameraMovement;
    
    // Pause-related UI elements
    public Button pauseButton;
    public Button resumeButton;
    public Button quitButton;
    public float pauseSpeed = 2.0f;
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    private bool isPulsing = false;

    private float menuStartTime; // To record when the main menu is loaded
    private bool isTimerRunning=false; // To track if the timer is active

    private AnalyticsManager analyticsManager;

    void Start()
    {
        // Hide all panels initially
        if (this.gameObject.name == "EventSystemMenu")
        {
            mainMenuUI.SetActive(true);
            Time.timeScale = 0f;

            if (!isTimerRunning)
            {
                analyticsManager = GameObject.FindObjectOfType<AnalyticsManager>();
                menuStartTime = Time.time;
                isTimerRunning = true;
                Debug.Log($"Timer started at: {menuStartTime} seconds since game launch");
            }
            else
            {
                Debug.Log("Timer is already running, keeping the original start time.");
            }
        }
        else if (this.gameObject.name == "EventSystemTutorial")
        {
            print("start tutorial");
            Time.timeScale = 1f;  // Make sure time is resumed here

            if (playerMovement != null)
            {
                print("playerMovement != null");
                playerMovement.StartGame();
            }
        }
        else if (this.gameObject.name == "EventSystemGame")
        {
            analyticsManager = GameObject.FindObjectOfType<AnalyticsManager>();
            Time.timeScale = 1f;  // Make sure time is resumed here

            if (playerMovement != null)
            {
                playerMovement.StartGame();
            }
        }

        // Initialize pause button setup
        if(SceneManager.GetActiveScene().name != "Menu"){
            pauseButton.onClick.AddListener(StartPause);
            resumeButton.onClick.AddListener(StopPause);
            resumeButton.gameObject.SetActive(false);
        }
        if (quitButton != null)
        {
            if (this.gameObject.name == "WinPanel")
            {
                quitButton.onClick.AddListener(ExitGameFinished); // For the win panel
            }
            else if (this.gameObject.name == "RestartPanel")
            {
                quitButton.onClick.AddListener(ExitGameUnfinished); // For the restart panel
            }
        }
    }

    void Update()
    {
        if (isPulsing)
        {
            float scale = Mathf.PingPong(Time.unscaledTime * pauseSpeed, maxScale - minScale) + minScale;
            transform.localScale = new Vector3(scale, scale, 1);
        }
        isTimerRunning = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ShowTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void ShowInstructions()
    {
        mainMenuUI.SetActive(false);
        instructionPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        mainMenuUI.SetActive(false);
        instructionPanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        instructionPanel.SetActive(false);
        mainMenuUI.SetActive(true);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        Message.SetActive(false);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("Menu");
    }

    public void RestartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void ShowMessage()
    {
        Message.SetActive(true);
        cameraMovement.StopCamera();
    }

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowGameOverPanel()
    {
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Pause and Resume Functionality
    public void StartPause()
    {
        isPulsing = true;
        Time.timeScale = 0f;
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
    }

    public void StopPause()
    {
        isPulsing = false;
        Time.timeScale = 1f;
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        transform.localScale = Vector3.one;
    }

    public void ShowLevels()
    {
        mainMenuUI.SetActive(false);
        levelsPanel.SetActive(true);
    }

    public void ExitGameUnfinished()
    {
        Debug.Log("ExitGameUnfinished called");
        print(isTimerRunning);
        if (isTimerRunning)
        {
            float sessionDuration = Time.time - menuStartTime; // Calculate session duration
            AnalyticsManager.UploadSessionData(sessionDuration, "Failed"); // Upload session data
            isTimerRunning = false; // Stop the timer
        }
        #if UNITY_WEBGL
                Application.Quit();
        #elif UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void ExitGameFinished()
    {
        Debug.Log("ExitGameFinished called");
        print(isTimerRunning);
        if (isTimerRunning)
        {
            float sessionDuration = Time.time - menuStartTime; // Calculate session duration
            AnalyticsManager.UploadSessionData(sessionDuration, "Complete"); // Upload session data
            isTimerRunning = false; // Stop the timer
        }

        #if UNITY_WEBGL
                Application.Quit();
        #elif UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

