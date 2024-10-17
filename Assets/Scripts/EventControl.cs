using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventControl : MonoBehaviour
{
    public GameObject instructionPanel;    // Panel for instructions
    public GameObject settingsPanel;       // Panel for settings
    public GameObject mainMenuUI;          // Main menu UI
    public GameObject gameOverPanel;       // Game Over panel
    public GameObject winPanel;             // Win panel
    private MonoBehaviour playerMovement;  // Reference to PlayerMovement

    void Start()
    {
        // Hide all panels initially
        instructionPanel.SetActive(false);
        settingsPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        mainMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Pause game at start
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Make sure time is resumed here

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement == null)
            {
                playerMovement = player.GetComponent<PlayerMovementSpeedrun>();
            }

            // Check which type of playerMovement we have and call StartGame on it
            if (playerMovement is PlayerMovement movement)
            {
                movement.StartGame();
            }
            else if (playerMovement is PlayerMovementSpeedrun speedrun)
            {
                speedrun.StartGame();
            }
            else
            {
                Debug.LogError("No valid player movement component found!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
    }

    public void ShowInstructions()
    {
        mainMenuUI.SetActive(false);
        instructionPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        mainMenuUI.SetActive(false);
        settingsPanel.SetActive(true);
        instructionPanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        instructionPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenuUI.SetActive(true);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartGameBack()
    {
        instructionPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenuUI.SetActive(true);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
