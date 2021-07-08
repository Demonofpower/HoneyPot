using System;
using System.Collections.Generic;
using Holoville.HOTween;
using HoneyPot.Debug;
using HoneyPot.Scene;
using HoneyPot.Scene.Helper;
using HoneyPot.Scene.Old;
using UnityEngine;

namespace HoneyPot.Menus
{
    class SceneMenu
    {
        private readonly DebugLog debugLog;
        private readonly SelectionManager selectionManager;

        private bool hideUI;

        public SceneMenu(DebugLog debugLog, SelectionManager selectionManager)
        {
            this.debugLog = debugLog;
            this.selectionManager = selectionManager;

            hideUI = false;
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
            if (GUILayout.Button("xxx"))
            {
                try
                {
                    new ScenePlayer(debugLog).Play(@"A:\Daten\testScene2.txt");
                }
                catch (Exception e)
                {
                    debugLog.AddMessage(e.Message);
                    debugLog.AddMessage(e.StackTrace);
                    debugLog.AddMessage(e.ToString());
                }
            }
            if (GUILayout.Button("yyy"))
            {
                
            }
        }
    }
}