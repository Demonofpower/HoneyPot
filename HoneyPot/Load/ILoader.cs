namespace HoneyPot.Load
{
    public delegate void LoadFinishedEventHandler(ILoader self);

    public interface ILoader
    {
        event LoadFinishedEventHandler LoadFinished;
        void Load();
    }
}
