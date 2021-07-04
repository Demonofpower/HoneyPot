using System;
using System.Collections.Generic;
using HoneyPot.Scene;
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
                
                //var creator = new SceneCreator(debugLog, selectionManager);
                
                //var scenes = GameManager.Data.DialogScenes;

                //var dialogManager = GameManager.System.Dialog;

                //var _definitions = new Dictionary<int, DialogSceneDefinition>();

                //var array = Resources.FindObjectsOfTypeAll(typeof(DialogSceneDefinition)) as DialogSceneDefinition[];
                //for (var i = 0; i < array.Length; i++)
                //{
                //    _definitions.Add(array[i].id, array[i]);
                //}

                //DialogSceneDefinition sceneX = new DialogSceneDefinition();

                //var girlDefinition = allGirls[8];
                //var girlDefinition2 = allGirls[1];


                //GameManager.System.Location.currentLocation = locations[2];
                //GameManager.Stage.background.UpdateLocation();

                //sceneX.steps.Add(creator.ShowAltGirlStep(girlDefinition, girlDefinition.defaultHairstyle, girlDefinition.defaultOutfit));

                //var dic = new Dictionary<string, List<DialogSceneStep>>();
                //dic.Add("Yep", new List<DialogSceneStep>() { creator.DialogLineStep("Ilu <3") });
                //dic.Add("Nope", new List<DialogSceneStep>() { creator.DialogLineStep("I hate you </3") });

                //sceneX.steps.Add(creator.DialogLineStep("rly?"));

                //sceneX.steps.Add(creator.ResponseOptionsStep(dic));

                //sceneX.steps.Add(creator.HideAltGirlStep());

                //sceneX.steps.Add(creator.DialogLineStep("hahaha"));

                //sceneX.steps.Add(creator.BranchDialogStep());

                //sceneX.steps.Add(creator.WaitStep(3));

                //sceneX.steps.Add(creator.ShowAltGirlStep(girlDefinition2, girlDefinition2.defaultHairstyle, girlDefinition2.defaultOutfit));

                //sceneX.steps.Add(creator.WaitStep(1));

                //sceneX.steps.Add(creator.HideAltGirlStep());

                //dialogManager.PlayDialogScene(sceneX);
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