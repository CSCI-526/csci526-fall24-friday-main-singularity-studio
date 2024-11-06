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

    public static void trackProgress(int levelCompleted, bool isGameWon)
    {
        ProgressTracker progressTracker = new ProgressTracker
        {
            LevelCompleted = levelCompleted,
            IsGameWon = isGameWon
        };
        AnalyticsService.Instance.RecordEvent(progressTracker);
        AnalyticsService.Instance.Flush();
    }
    // Method to track heart collection events
    public static void TrackHeartCollection(int heartID, string heartName, bool gainedHealth, bool lostHealth)
    {
        HeartCollectionTracker heartCollection = new HeartCollectionTracker
        {
            HeartID = heartID,
            HeartName = heartName,
            GainedHealth = gainedHealth,
            lostHealth = lostHealth
        };

        AnalyticsService.Instance.RecordEvent(heartCollection);
        AnalyticsService.Instance.Flush();
    }
}
