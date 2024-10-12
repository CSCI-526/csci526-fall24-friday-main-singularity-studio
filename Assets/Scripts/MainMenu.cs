using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject instructionPanel;    // Panel for instructions
    public GameObject settingsPanel;       // Panel for settings
    public GameObject mainMenuUI;          // Main menu UI
    public PlayerMovement playerMovement;  // Reference to PlayerMovement
    public GameObject gameOverPanel;       // Reference to Game Over panel
    public GameObject winPanel;            // Reference to Win panel

    private bool gameWonOrReset = false;   // Tracks whether the game was won or reset

    void Start()
    {
        // Initial setup
        instructionPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Pause game at start

        // Ensure panels are hidden at the start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    // Start the game when the Play button is clicked
    public void StartGame()
    {
        if (gameWonOrReset)
        {
            // Reload the scene if the game was won or reset
            gameWonOrReset = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Normal game start
            mainMenuUI.SetActive(false);
            Time.timeScale = 1f;

            if (playerMovement != null)
            {
                playerMovement.StartGame();
            }
        }
    }

    // Show the Instructions panel
    public void ShowInstructions()
    {
        mainMenuUI.SetActive(false);
        instructionPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    // Show the Settings panel
    public void ShowSettings()
    {
        mainMenuUI.SetActive(false);
        settingsPanel.SetActive(true);
        instructionPanel.SetActive(false);
    }

    // Return to the main menu from Instructions or Settings
    public void BackToMainMenu()
    {
        instructionPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenuUI.SetActive(true);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
    }

    // Show the win panel when the player wins
    public void ShowWinPanel()
    {
        gameWonOrReset = true; 
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f; 
        }
    }

    // Restart from the last checkpoint when clicked in the Game Over UI
    public void RestartFromCheckpoint()
    {
        Debug.Log("Restarting from checkpoint...");
        if (playerMovement != null)
        {
            playerMovement.RestartFromCheckpoint();
        }

        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Restart the game completely
    public void RestartGame()
    {
        gameWonOrReset = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Go back to the main menu and reset everything to the start
    public void BackToMainMenuAndReset()
    {
        gameWonOrReset = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
