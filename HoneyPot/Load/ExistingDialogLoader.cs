using HoneyPot.DebugUtil;

namespace HoneyPot.Load
{
    class ExistingDialogLoader : ILoader
    {
        private readonly DebugLog debugLog;

        public ExistingDialogLoader(DebugLog debugLog)
        {
            this.debugLog = debugLog;
        }

        public event LoadFinishedEventHandler LoadFinished;

        public void Load()
        {
            debugLog.AddError("works!");
            LoadFinished?.Invoke(this);
        }
    }
}
