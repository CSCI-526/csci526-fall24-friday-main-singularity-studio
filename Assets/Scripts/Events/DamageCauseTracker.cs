public class DamageCauseTracker : Unity.Services.Analytics.Event
{
	public DamageCauseTracker() : base("damageCauseTracker")
	{
	}

	public string Obstacle { set { SetParameter("Obstacle", value.FirstCharToUpper()); } }
}

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }
}
