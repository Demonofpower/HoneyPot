using System;
using System.Collections.Generic;
using HoneyPot.DebugUtil;
using UnityEngine;

namespace HoneyPot.Load
{
    class ExistingDialogLoader : ILoader
    {
        public static Dictionary<int, DialogLine> LoadedDialogs;

        private readonly DebugLog debugLog;

        public ExistingDialogLoader(DebugLog debugLog)
        {
            this.debugLog = debugLog;
            LoadedDialogs = new Dictionary<int, DialogLine>();
        }

        private void LoadDialogs()
        {
            debugLog.AddMessage("Loading dialogs..");

            var dialogScenes = Resources.FindObjectsOfTypeAll(typeof(DialogSceneDefinition)) as DialogSceneDefinition[];

            if (dialogScenes == null)
            {
                return;
            }

            foreach (var dialogSceneDefinition in dialogScenes)
            {
                foreach (var step in dialogSceneDefinition.steps)
                {
                    if (step.type == DialogSceneStepType.DIALOG_LINE)
                    {
                        if (step.sceneLine?.dialogLine?.text == null)
                        {
                            continue;
                        }

                        var hashCode = step.sceneLine.dialogLine.text.GetHashCode();

                        //Same text twice for that hash so we have to change hash of one
                        if (hashCode == -1647543241)
                        {
                            if (LoadedDialogs.ContainsKey(hashCode))
                            {
                                hashCode += 1;
                            }
                        }

                        debugLog.AddMessage("Now loading: " + hashCode);

                        var dialogLine = step.sceneLine.dialogLine;

                        LoadedDialogs.Add(hashCode, dialogLine);
                    }
                }
            }

            debugLog.AddMessage("Dialogs loaded.");
        }

        public event LoadFinishedEventHandler LoadFinished;

        public void Load()
        {
            LoadDialogs();
            LoadFinished?.Invoke(this);
        }
    }
}