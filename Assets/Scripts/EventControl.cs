using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Analytics;


public class EventControl : MonoBehaviour
{
    public GameObject instructionPanel;    // Panel for instructions
    public GameObject mainMenuUI;          // Main menu UI
    public GameObject gameOverPanel;       // Game Over panel
    public GameObject winPanel;             // Win panel
    public GameObject levelsPanel;
    public GameObject Message; 
    public PlayerControl playerMovement;  // Reference to PlayerMovement
    private AnalyticsManager analyticsManager;
    public CameraMovement cameraMovement;

    void Start()
    {
        // Hide all panels initially
        // instructionPanel.SetActive(false);
        // gameOverPanel.SetActive(false);
        // winPanel.SetActive(false);
        if (this.gameObject.name == "EventSystemMenu"){
            mainMenuUI.SetActive(true);
            Time.timeScale = 0f;  // Pause game at start
        }
        else if (this.gameObject.name == "EventSystemTutorial"){
            print("start tutorial");
            Time.timeScale = 1f;  // Make sure time is resumed here

            if (playerMovement != null)
            {
                print("playerMovement != null");
                playerMovement.StartGame();
            }
        }
        else if (this.gameObject.name == "EventSystemGame"){
            analyticsManager = GameObject.Find("AnalyticsManager").GetComponent<AnalyticsManager>();
            // StartGame();
            Time.timeScale = 1f;  // Make sure time is resumed here
            if (playerMovement != null)
            {
                playerMovement.StartGame();
            }
        }
        // mainMenuUI.SetActive(true);
        // Time.timeScale = 0f;  // Pause game at start
        // analyticsManager = GameObject.Find("AnalyticsManager").GetComponent<AnalyticsManager>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        // mainMenuUI.SetActive(false);
        // Time.timeScale = 1f;  // Make sure time is resumed here

        // if (playerMovement != null)
        // {
        //     playerMovement.StartGame();
        // }
    }

    public void ShowTutorial(){
        SceneManager.LoadScene("Tutorial");
        // mainMenuUI.SetActive(false);
        // Time.timeScale = 1f;  // Make sure time is resumed here
        // if (playerMovement != null)
        // {
        //     playerMovement.StartGame();
        // }
    }

    public void ShowInstructions()
    {
        mainMenuUI.SetActive(false);
        instructionPanel.SetActive(true);
    }

    public void ShowLevels()
    {
        mainMenuUI.SetActive(false);
        levelsPanel.SetActive(true);
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


    public void BackToMain(){
        SceneManager.LoadScene("Menu");
    }
    public void RestartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void ShowMessage()
    {
        // winPanel.SetActive(true);
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

    // public void RestartGame()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    // }
    // public void RestartGameBack()
    // {
    //     instructionPanel.SetActive(false);
    //     mainMenuUI.SetActive(true);
    //     gameOverPanel.SetActive(false);
    //     winPanel.SetActive(false);
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    // }

}
