using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private DatabaseReference reference;
    private string userID;
    private Dictionary<string, int> reachCounts = new Dictionary<string, int>(); // Track reaches per phase
    private Dictionary<string, int> deathCounts = new Dictionary<string, int>(); // Track deaths per phase
    private int completionCount = 0; // Track level completions

    void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                userID = Guid.NewGuid().ToString(); // Unique user ID for this session
                InitializeCounts(); // Initialize reach and death counts for each phase
                Debug.Log("Firebase initialized successfully.");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    // Initialize reach and death counts for each phase
    private void InitializeCounts()
    {
        reachCounts["Phase0"] = 0; // Before reaching EndPhase1
        reachCounts["EndPhase1"] = 0;
        reachCounts["EndPhase2"] = 0;

        deathCounts["Phase0"] = 0; // Deaths in Phase0
        deathCounts["EndPhase1"] = 0; // Deaths in EndPhase1
        deathCounts["EndPhase2"] = 0; // Deaths in EndPhase2
    }

    // Log when the player reaches a specific phase
    public void LogReach(string phaseName)
    {
        if (!reachCounts.ContainsKey(phaseName))
        {
            Debug.LogError($"Phase {phaseName} not recognized.");
            return;
        }

        // Increment the reach count for the specified phase
        reachCounts[phaseName]++;

        // Create JSON structure for reach data
        var reachData = new Dictionary<string, object>
        {
            { "userID", userID },
            { "phase", phaseName },
            { "reachCount", reachCounts[phaseName] }
        };

        // Update Firebase with the new reach count
        reference.Child("gameplay").Child(userID).Child("reaches").Child(phaseName)
            .SetValueAsync(reachData).ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    Debug.Log($"Reach count for {phaseName} updated successfully: {reachCounts[phaseName]} times.");
                }
                else
                {
                    Debug.LogError($"Failed to update reach count for {phaseName}: " + task.Exception);
                }
            });
    }

    // Log a death in a specific phase
    public void LogDeath(string phaseName)
    {
        if (!deathCounts.ContainsKey(phaseName))
        {
            Debug.LogError($"Phase {phaseName} not recognized.");
            return;
        }

        // Increment the death count for the specified phase
        deathCounts[phaseName]++;

        // Create JSON structure for death data
        var deathData = new Dictionary<string, object>
        {
            { "userID", userID },
            { "phase", phaseName },
            { "deathCount", deathCounts[phaseName] }
        };

        // Update Firebase with the new death count for the phase
        reference.Child("gameplay").Child(userID).Child("deaths").Child(phaseName)
            .SetValueAsync(deathData).ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    Debug.Log($"Death count for {phaseName} updated successfully: {deathCounts[phaseName]} deaths.");
                }
                else
                {
                    Debug.LogError($"Failed to update death count for {phaseName}: " + task.Exception);
                }
            });
    }

    // Log when the player completes the level by touching the finish line
    public void LogLevelCompletion()
    {
        // Increment the level completion count
        completionCount++;

        // Create JSON structure for completion data
        var completionData = new Dictionary<string, object>
        {
            { "userID", userID },
            { "completionCount", completionCount }
        };

        // Update Firebase with the new level completion count
        reference.Child("gameplay").Child(userID).Child("level_completions")
            .SetValueAsync(completionData).ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    Debug.Log($"Level completion count updated successfully: {completionCount} completions.");
                }
                else
                {
                    Debug.LogError("Failed to update level completion count: " + task.Exception);
                }
            });
    }
}
