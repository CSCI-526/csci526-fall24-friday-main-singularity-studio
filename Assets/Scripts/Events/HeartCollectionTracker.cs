public class HeartCollectionTracker : Unity.Services.Analytics.Event
{
    public HeartCollectionTracker() : base("HeartCollected")
    {
    }

    public int HeartID
    {
        set { SetParameter("HeartID", value); }
    }

    public string HeartName
    {
        set { SetParameter("HeartName", value); }
    }

    public bool GainedHealth
    {
        set { SetParameter("GainedHealth", value); }
    }

    public bool lostHealth
    {
        set { SetParameter("lostHealth", value); }
    }
}