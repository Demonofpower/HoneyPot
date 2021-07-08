using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace HoneyPot.Debug
{
    public class DebugLog
    {
        public static DebugLog Instance;
        
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
            File.AppendAllText(Environment.CurrentDirectory + @"\log.txt", this.number + ": " + message + Environment.NewLine);
        }
        
        public List<string> PrintLastMessages()
        {
            return this.messages.Skip(Math.Max(0, this.messages.Count<string>() - 15)).ToList<string>();
        }

        public void DoDebugLog(int windowID)
        {
            foreach (var text in PrintLastMessages())
            {
                GUILayout.Label(text);
            }
        }

        private List<string> messages;
        
        private int number;
    }
}