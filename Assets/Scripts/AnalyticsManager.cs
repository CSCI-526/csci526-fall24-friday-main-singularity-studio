using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Analytics;
using UnityEngine.SceneManagement;
using System;

public class AnalyticsManager : MonoBehaviour
{
    async void Start()
    {
        // if(Application.isEditor) return;
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("AnalyticsManager Initialized");
    }

    private static string CategorizeDuration(float sessionDuration)
    {
        if (sessionDuration <= 40) return "0-40s";
        else if (sessionDuration <= 80) return "40-80s";
        else if (sessionDuration <= 120) return "80-120s";
        else if (sessionDuration <= 160) return "120-160s";
        else if (sessionDuration <= 200) return "160-200s";
        else return "200s+";
    }

    public static void UploadSessionData(float sessionDuration, string exitReason)
    {
        string durationCategory = CategorizeDuration(sessionDuration);
        SessionDataTracker sessionData = new SessionDataTracker
        {
            SessionDuration = sessionDuration,
            ExitReason = exitReason,
            TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            DurationCategory = durationCategory
        };

        Debug.Log("SessionDataTracker created. Preparing to record event...");

        try
        {
            AnalyticsService.Instance.RecordEvent(sessionData);
            Debug.Log("RecordEvent called successfully.");

            AnalyticsService.Instance.Flush(); // Flush the analytics data
            Debug.Log("Analytics data flushed successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during analytics upload: {ex.Message}");
        }

        Debug.Log($"Session Data Recorded: Duration={sessionDuration}s, ExitReason={exitReason}, DurationCat={durationCategory}");
    }


    public static void trackProgress(int levelCompleted, bool isGameWon, float playTime, string timeCategory)
    {

        ProgressTracker progressTracker = new ProgressTracker
        {
            LevelCompleted = levelCompleted,
            IsGameWon = isGameWon,
            PlayTime = playTime,
            TimeCategory = timeCategory
        };
        AnalyticsService.Instance.RecordEvent(progressTracker);
        AnalyticsService.Instance.Flush();
        Debug.Log($"Level: {levelCompleted}, Won: {isGameWon}, Play Time: {playTime} seconds, Time Category: {timeCategory}");
    }

    public static void trackDamageCause(string obstacle)
    {
        DamageCauseTracker damageCauseTracker = new DamageCauseTracker
        {
            Obstacle = obstacle
        };
        AnalyticsService.Instance.RecordEvent(damageCauseTracker);
        AnalyticsService.Instance.Flush();
    }
    
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