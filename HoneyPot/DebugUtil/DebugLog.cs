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

        private static readonly string Path = Environment.CurrentDirectory + @"\log.txt";

        private List<string> messages;

        private int number;

        public DebugLog()
        {
            Instance = this;

            this.number = 0;
            this.messages = new List<string>();
        }

        public void AddMessage(string message, bool withNumber = true)
        {
            if (withNumber)
            {
                number++;
                messages.Add(number + ": " + message);
                File.AppendAllText(Path, number + ": " + message + Environment.NewLine);
            }
            else
            {
                messages.Add(message);
                File.AppendAllText(Path, message + Environment.NewLine);
            }
            
        }

        public void AddError(string error)
        {
            this.number++;
            this.messages.Add(number + " ERROR: " + error);
            File.AppendAllText(Path, number + ": " + error + Environment.NewLine);
        }

        public void Clear()
        {
            try
            {
                File.Delete(Path);
                messages = new List<string>();
                number = 0;
            }
            catch (Exception e)
            {
                AddError("File deletion failed.");
                AddError(e.Message);
            }
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