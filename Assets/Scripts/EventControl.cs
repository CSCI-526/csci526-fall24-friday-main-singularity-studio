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
    public GameObject Message; 
    public PlayerControl playerMovement;   // Reference to PlayerMovement
    public CameraMovement cameraMovement;
    
    // Pause-related UI elements
    public Button pauseButton;
    public Button resumeButton;
    public float pauseSpeed = 2.0f;
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    private bool isPulsing = false;
    
    private AnalyticsManager analyticsManager;

    void Start()
    {
        // Hide all panels initially
        if (this.gameObject.name == "EventSystemMenu")
        {
            mainMenuUI.SetActive(true);
            Time.timeScale = 0f;  // Pause game at start
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
            analyticsManager = GameObject.Find("AnalyticsManager").GetComponent<AnalyticsManager>();
            Time.timeScale = 1f;  // Make sure time is resumed here

            if (playerMovement != null)
            {
                playerMovement.StartGame();
            }
        }

        // Initialize pause button setup
        pauseButton.onClick.AddListener(StartPause);
        resumeButton.onClick.AddListener(StopPause);
        resumeButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPulsing)
        {
            float scale = Mathf.PingPong(Time.unscaledTime * pauseSpeed, maxScale - minScale) + minScale;
            transform.localScale = new Vector3(scale, scale, 1);
        }
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
}

