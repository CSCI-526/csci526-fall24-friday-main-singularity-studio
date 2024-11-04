using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Analytics;
using UnityEngine.SceneManagement;

public class AnalyticsManager : MonoBehaviour
{
    async void Start()
    {
        // if(Application.isEditor) return;
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("AnalyticsManager Initialized");
    }

    public static void trackProgress(int levelCompleted, bool isGameWon, float playTime)
    {
        ProgressTracker progressTracker = new ProgressTracker
        {
            LevelCompleted = levelCompleted,
            IsGameWon = isGameWon,
            PlayTime = playTime
        };
        AnalyticsService.Instance.RecordEvent(progressTracker);
        AnalyticsService.Instance.Flush();
        Debug.Log($"Level: {levelCompleted}, Won: {isGameWon}, Play Time: {playTime} seconds");
    }

}