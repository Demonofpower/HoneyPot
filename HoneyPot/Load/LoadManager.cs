using System.Collections.Generic;
using System.Threading;
using HoneyPot.DebugUtil;

namespace HoneyPot.Load
{
    class LoadManager
    {
        private readonly DebugLog debugLog;
        
        private List<ILoader> toLoad;

        public LoadManager(DebugLog debugLog)
        {
            this.debugLog = debugLog;

            toLoad = new List<ILoader>();
        }

        public void LoadAll()
        {
            Init();

            LoadNext();
        }

        public void LoadNext()
        {
            if (toLoad.Count == 0)
            {
                return;
            }

            toLoad[0].LoadFinished += LoaderOnLoadFinished;
            toLoad[0].Load();
        }

        private void Init()
        {
            toLoad.Add(new ExistingDialogLoader(debugLog));
            toLoad.Add(new SceneAudioLoader(debugLog));
        }

        private void LoaderOnLoadFinished(ILoader self)
        {
            toLoad.Remove(self);
            
            LoadNext();
        }
    }
}