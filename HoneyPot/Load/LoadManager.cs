using System;
using System.Collections.Generic;
using System.Threading;
using HoneyPot.DebugUtil;

namespace HoneyPot.Load
{
    class LoadManager
    {
        private readonly DebugLog debugLog;
        
        private readonly List<ILoader> toLoad;

        public LoadManager(DebugLog debugLog)
        {
            this.debugLog = debugLog;

            toLoad = new List<ILoader>();
        }

        public void LoadAll()
        {
            Init();

            var t = new Thread(LoadNext);
            t.Start();
        }

        public void LoadNext()
        {
            try
            {
                if (toLoad.Count == 0)
                {
                    return;
                }

                toLoad[0].LoadFinished += LoaderOnLoadFinished;
                toLoad[0].Load();
            }
            catch (Exception e)
            {
                debugLog.AddError(e.Message);
            }
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