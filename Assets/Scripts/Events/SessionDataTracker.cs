public class SessionDataTracker : Unity.Services.Analytics.Event
{
    public SessionDataTracker() : base("sessionData") // Event name in Unity Analytics
    {
    }

    public float SessionDuration
    {
        set { SetParameter("SessionDuration", value); } // Logs the session duration in seconds
    }

    public string ExitReason
    {
        set { SetParameter("ExitReason", value); } // Logs why the player exited (e.g., "Player clicked exit")
    }

    public string TimeStamp
    {
        set { SetParameter("TimeStamp", value); } // Logs the timestamp of when the session ended
    }
    public string DurationCategory
    {
        set { SetParameter("DurationCategory", value); }
    }
}
