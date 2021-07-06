using System;
using System.Collections.Generic;
using Holoville.HOTween;
using HoneyPot.Scene;
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
                    new SceneHelper().ShowUI();
                }
                else
                {
                    new SceneHelper().HideUI();
                }

                hideUI = !hideUI;
            }
            if (GUILayout.Button("xxx"))
            {
                try
                {
                    new SceneCreator(debugLog, selectionManager).PlayScene(@"A:\Daten\testScene2.txt");
                }
                catch (Exception e)
                {
                    debugLog.AddMessage(e.Message);
                    debugLog.AddMessage(e.StackTrace);
                    debugLog.AddMessage(e.ToString());
                }
            }
        }

        private void TalkTest()
        {
            var dialogLine = new DialogLine();
            dialogLine.text = "i love you <3";
            dialogLine.secondary = false;
            dialogLine.secondaryText = "";
            var dialogLineExp = new DialogLineExpression();
            dialogLineExp.changeEyes = false;
            dialogLineExp.changeMouth = false;
            dialogLineExp.closeEyes = false;
            dialogLineExp.expression = GirlExpressionType.HORNY;
            dialogLineExp.startAtCharIndex = 0;
            dialogLine.startExpression = dialogLineExp;
            GameManager.Stage.girl.ReadDialogLine(dialogLine, false, false, false, false);
        }
    }
}