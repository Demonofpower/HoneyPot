using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace HoneyPot.DebugUtil
{
    public class DebugLog
    {
        public static DebugLog Instance;

        private readonly List<string> messages;

        private int number;

        public DebugLog()
        {
            Instance = this;

            this.number = 0;
            this.messages = new List<string>();
        }

        public void AddMessage(string message)
        {
            this.number++;
            this.messages.Add(this.number + ": " + message);
            File.AppendAllText(Environment.CurrentDirectory + @"\log.txt",
                this.number + ": " + message + Environment.NewLine);
        }

        public void AddError(string error)
        {
            this.number++;
            this.messages.Add(this.number + " ERROR: " + error);
            File.AppendAllText(Environment.CurrentDirectory + @"\log.txt",
                this.number + ": " + error + Environment.NewLine);
        }

        public void DoDebugLog(int windowID)
        {
            foreach (var text in PrintLastMessages())
            {
                if (text.Contains("ERROR"))
                {
                    GUI.contentColor = Color.red;
                    GUILayout.Label(text);
                }
                else
                {
                    GUI.contentColor = Color.white;
                    GUILayout.Label(text);
                }
            }

            GUI.contentColor = Color.white;
        }

        private List<string> PrintLastMessages()
        {
            return this.messages.Skip(Math.Max(0, this.messages.Count<string>() - 15)).ToList<string>();
        }
    }
}