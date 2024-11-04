using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For Image and UI components
using UnityEngine.Analytics;


public class LevelCompletion : MonoBehaviour
{
    private int currentLevel = 0;
    public Image ProgressBarImg;
    private RectTransform rt;
    private float ProgressBarWidth;
    public CameraMovement cameraMovement;
    public SceneRotation sceneRotation;
    
    private float startTime; // Track the game start time
    private bool isGameStarted = false; // Track if the game has started

    private void Start()
    {
        ProgressBarImg = GameObject.Find("Progress").GetComponent<Image>();
        rt = ProgressBarImg.GetComponent<RectTransform>();
        ProgressBarWidth = rt.rect.width;
        rt.anchorMin = new Vector2(0, 0.5f);
        rt.anchorMax = new Vector2(0, 0.5f);
        rt.sizeDelta = new Vector2(1, rt.sizeDelta.y);
    }

    // Method to start tracking playtime
    public void StartGameTimer()
    {
        startTime = Time.time;
        isGameStarted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
        }
        else if (collision.gameObject.CompareTag("LevelTrigger"))
        {
            Debug.Log("Leveled up");
            Destroy(collision.gameObject);
            currentLevel++;
        }
    }

    private void Win()
    {
        FindObjectOfType<EventControl>().ShowWinPanel();
        Debug.Log("Winner Winner Chicken Dinner!");
        
        float playTime = Time.time - startTime; // Calculate the play duration
        AnalyticsManager.trackProgress(currentLevel, true, playTime); // Pass playtime to AnalyticsManager
        
        cameraMovement.StopCamera();
        sceneRotation.StopRotation();
        isGameStarted = false; // Reset the game state
    }
}
