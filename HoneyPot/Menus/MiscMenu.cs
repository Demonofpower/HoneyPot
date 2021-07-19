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

            if (GUILayout.Button("Clear Log"))
            {
                debugLog.Clear();

                debugLog.AddMessage("Log cleared.");
            }
            
            if (GUILayout.Button("Github"))
            {
                System.Diagnostics.Process.Start("https://github.com/Demonofpower/HoneyPot");
            }

            GUI.contentColor = new Color(255, 215, 0);
            if (GUILayout.Button("Support me <3"))
            {
                System.Diagnostics.Process.Start("https://www.buymeacoffee.com/paranoia");

                debugLog.AddMessage("Thank you <3");
            }
            GUI.contentColor = Color.white;
        }
    }
}