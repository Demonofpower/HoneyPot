using HoneyPot.DebugUtil;
using UnityEngine;

namespace HoneyPot.Menus
{
    class MiscMenu
    {
        private readonly DebugLog debugLog;

        public MiscMenu(DebugLog debugLog)
        {
            this.debugLog = debugLog;
        }

        public void DoMisc(int windowID)
        {
            if (GUILayout.Button("Exit/Unload"))
            {
                debugLog.AddMessage("Unloading..");
                Loader.Unload();
            }
        }
    }
}