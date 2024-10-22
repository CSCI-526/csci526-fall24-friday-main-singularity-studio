public class ProgressTracker : Unity.Services.Analytics.Event
{
	public ProgressTracker() : base("progressTracker")
	{
	}

	public int LevelCompleted { set { SetParameter("LevelCompleted", value); } }
	public bool IsGameWon { set { SetParameter("IsGameWon", value); } }

}