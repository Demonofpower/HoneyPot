using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using HoneyPot.Debug;
using HoneyPot.DebugUtil;
using UnityEngine;

namespace HoneyPot.Scene.Helper
{
    class SceneAudioLoader
    {
        public static Dictionary<string, AudioClip> LoadedClips;
        
        private static readonly string AudiosPath = Environment.CurrentDirectory + @"\Scenes\Audio";

        private readonly DebugLog debugLog;

        public SceneAudioLoader(DebugLog debugLog)
        {
            this.debugLog = debugLog;
            LoadedClips = new Dictionary<string, AudioClip>();

            Directory.CreateDirectory(AudiosPath);
        }

        public void LoadClips()
        {
            debugLog.AddMessage("Loading audio..");

            var filesArray = Directory.GetFiles(AudiosPath);

            foreach (var audioPath in filesArray)
            {
                var name = Path.GetFileName(audioPath);
                
                debugLog.AddMessage("Now loading: " + name);

                var extension = Path.GetExtension(audioPath);
                if (extension != ".wav")
                {
                    debugLog.AddError("File type not supported! Use .wav");
                    continue;
                }

                var clip = LoadClip("file:///" + audioPath);
                LoadedClips.Add(name, clip);
            }

            debugLog.AddMessage("Audio loaded.");
        }

        private AudioClip LoadClip(string path)
        {
            var w = new WWW(path);
            while (!w.isDone)
            {
                Thread.Sleep(10);
            }

            return w.audioClip;
        }
    }
}