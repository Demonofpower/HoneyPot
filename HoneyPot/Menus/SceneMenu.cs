using System;
using System.Collections.Generic;
using HoneyPot.Debug;
using UnityEngine;

namespace HoneyPot.Menus
{
    class SceneMenu
    {
        private readonly DebugLog debugLog;
        private readonly SelectionManager selectionManager;

        public SceneMenu(DebugLog debugLog, SelectionManager selectionManager)
        {
            this.debugLog = debugLog;
            this.selectionManager = selectionManager;
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

            if (GUILayout.Button("xxx"))
            {
                var scenes = GameManager.Data.DialogScenes;

                var dialogManager = GameManager.System.Dialog;

                var _definitions = new Dictionary<int, DialogSceneDefinition>();

                var array = Resources.FindObjectsOfTypeAll(typeof(DialogSceneDefinition)) as DialogSceneDefinition[];
                for (var i = 0; i < array.Length; i++)
                {
                    _definitions.Add(array[i].id, array[i]);
                }

                DialogSceneDefinition sceneX = null;

                foreach (var scene in _definitions.Values)
                {
                    if (scene.id == 4)
                    {
                        sceneX = scene;
                        break;
                    }
                }

                new Dump(debugLog).SceneStepsDump(sceneX);
                
                dialogManager.PlayDialogScene(sceneX);

                //DialogSceneDefinition sceneX = new DialogSceneDefinition();

                //DialogSceneStep stepX = new DialogSceneStep();

                //stepX.type = DialogSceneStepType.SEND_MESSAGE;

                //stepX.locationDefinition = GameManager.System.Location.currentLocation;

                //stepX.girlDefinition = currGirl;

                //stepX.soundEffect = null; //TODO

                //DialogSceneLine sceneLineX = new DialogSceneLine();
                //sceneLineX.altGirl = false;
                //DialogLine dialogLineX = new DialogLine();
                //dialogLineX.text = "i love you";
                //sceneLineX.dialogLine = dialogLineX;
                //stepX.sceneLine = sceneLineX;




                //sceneX.steps.Add(stepX);

                //dialogManager.PlayDialogScene(sceneX);
            }
        }
    }
}
