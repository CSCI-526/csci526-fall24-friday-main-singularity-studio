using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventControl : MonoBehaviour
{
    public GameObject instructionPanel;    // Panel for instructions
    public GameObject mainMenuUI;          // Main menu UI
    public GameObject gameOverPanel;       // Game Over panel
    public GameObject winPanel;             // Win panel
    public PlayerMovement playerMovement;  // Reference to PlayerMovement

    void Start()
    {
        // Hide all panels initially
        instructionPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        mainMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Pause game at start
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Make sure time is resumed here

        if (playerMovement != null)
        {
            playerMovement.StartGame();
        }
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
        mainMenuUI.SetActive(true);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
