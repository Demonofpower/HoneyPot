using System;
using System.Collections.Generic;
using System.IO;
using HoneyPot.Debug;
using HoneyPot.Scene;
using HoneyPot.Scene.Helper;
using UnityEngine;

namespace HoneyPot.Menus
{
    class SceneMenu
    {
        private static readonly string ScenesPath = Environment.CurrentDirectory + @"\Scenes";

        private readonly DebugLog debugLog;
        private readonly SelectionManager selectionManager;

        private bool hideUI;

        public SceneMenu(DebugLog debugLog, SelectionManager selectionManager)
        {
            this.debugLog = debugLog;
            this.selectionManager = selectionManager;

            hideUI = false;

            Directory.CreateDirectory(ScenesPath);
        }

        public void DoScene(int windowId)
        {
            var locationManager = GameManager.System.Location;
            var LocationDefinitions = new List<LocationDefinition>();

            LocationDefinition[] locations =
                Resources.FindObjectsOfTypeAll(typeof(LocationDefinition)) as LocationDefinition[];
            for (int i = 0; i < locations.Length; i++)
            {
                LocationDefinitions.Add(locations[i]);
            }

            var allGirls = GameManager.Data.Girls.GetAll();
            var currGirl = GameManager.Stage.girl.definition;

            if (GUILayout.Button("Travel"))
            {
                var sceneNames = new List<string>();
                foreach (var locationDefinition in LocationDefinitions)
                {
                    if (locationDefinition.type == LocationType.NORMAL)
                    {
                        sceneNames.Add(locationDefinition.fullName);
                    }
                }

                selectionManager.NewSelection(sceneNames, 3, () =>
                {
                    var name = sceneNames[selectionManager.SelectionId];
                    foreach (var locationDefinition in LocationDefinitions)
                    {
                        if (locationDefinition.fullName == name)
                        {
                            locationManager.TravelTo(locationDefinition, currGirl);
                            break;
                        }
                    }


                    debugLog.AddMessage("Traveled to " + name);
                });
            }

            if (GUILayout.Button("Background"))
            {
                var sceneNames = new List<string>();
                foreach (var locationDefinition in LocationDefinitions)
                {
                    sceneNames.Add(locationDefinition.fullName);
                }

                selectionManager.NewSelection(sceneNames, 3, () =>
                {
                    var name = sceneNames[selectionManager.SelectionId];
                    foreach (var locationDefinition in LocationDefinitions)
                    {
                        if (locationDefinition.fullName == name)
                        {
                            GameManager.System.Location.currentLocation = locationDefinition;
                            GameManager.Stage.background.UpdateLocation();
                            break;
                        }
                    }


                    debugLog.AddMessage("Scene changed to " + name);
                });
            }
            if (GUILayout.Button("ToggleUI"))
            {
                if (hideUI)
                {
                    new SceneHelper(debugLog).ShowUI();
                }
                else
                {
                    new SceneHelper(debugLog).HideUI();
                }

                hideUI = !hideUI;
            }
            if (GUILayout.Button("PlayScene"))
            {
                try
                {
                    var filesArray = Directory.GetFiles(ScenesPath);
                    var fileNames = new List<string>();
                    foreach (var file in filesArray)
                    {
                        fileNames.Add(Path.GetFileName(file));
                    }
                    
                    if (filesArray.Length == 0)
                    {
                        debugLog.AddMessage("Scenes folder is empty! " + ScenesPath);
                        return;
                    }
                  
                    selectionManager.NewSelection(fileNames, 1, () =>
                    {
                        var name = filesArray[selectionManager.SelectionId];
                        new ScenePlayer(debugLog).Play(name);
                    });
                }
                catch (Exception e)
                {
                    debugLog.AddMessage(e.Message);
                    debugLog.AddMessage(e.StackTrace);
                    debugLog.AddMessage(e.ToString());
                }
            }
        }
    }
}