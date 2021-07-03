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

                DialogSceneDefinition sceneX = new DialogSceneDefinition();

                var girlDefinition = allGirls[8];
                var girlDefinition2 = allGirls[1];


                sceneX.steps.Add(ShowAltGirlStep(girlDefinition, girlDefinition.defaultHairstyle, girlDefinition.defaultOutfit));
                
                sceneX.steps.Add(HideAltGirlStep());

                sceneX.steps.Add(DialogLineStep("hahaha"));
                
                sceneX.steps.Add(BranchDialogStep());

                sceneX.steps.Add(WaitStep(5));

                sceneX.steps.Add(ShowAltGirlStep(girlDefinition2, girlDefinition2.defaultHairstyle, girlDefinition2.defaultOutfit));

                dialogManager.PlayDialogScene(sceneX);
            }
        }

        private DialogSceneStep ShowAltGirlStep(GirlDefinition altGirl, int altGirlHairId, int altGirlOutfitId)
        {
            DialogSceneStep step = new DialogSceneStep();
            step.type = DialogSceneStepType.SHOW_ALT_GIRL;
            step.preventOptionShuffle = false;
            step.hasBestOption = false;

            step.responseOptions = new List<DialogSceneResponseOption>();

            step.hasBestBranch = false;
            step.showGirlStyles = altGirlHairId + "," + altGirlOutfitId;
            step.hideOppositeSpeechBubble = false;

            step.altGirl = altGirl;

            step.waitTime = 0;
            step.metStatus = GirlMetStatus.MET;
            step.dialogTriggerIndex = 0;
            step.girlDetailType = GirlDetailType.LAST_NAME;
            step.stepBackSteps = 0;
            step.toEquipment = false;
            step.wrapped = false;
            step.tokenCount = 0;
            step.xPos = 0;
            step.yPos = 0;

            return step;
        }

        private DialogSceneStep HideAltGirlStep()
        {
            DialogSceneStep step = new DialogSceneStep();
            step.type = DialogSceneStepType.HIDE_ALT_GIRL;

            return step;
        }

        private DialogSceneStep DialogLineStep(string text, bool altGirl = false)
        {
            DialogSceneStep step = new DialogSceneStep();
            step.type = DialogSceneStepType.DIALOG_LINE;

            var sceneLine = new DialogSceneLine();
            var dialogLine = new DialogLine();
            dialogLine.text = text;
            dialogLine.secondary = false;
            dialogLine.secondaryText = "";
            var dialogLineExp = new DialogLineExpression();
            dialogLineExp.changeEyes = false;
            dialogLineExp.changeMouth = false;
            dialogLineExp.closeEyes = false;
            dialogLineExp.expression = GirlExpressionType.HORNY;
            dialogLineExp.startAtCharIndex = 0;
            dialogLine.startExpression = dialogLineExp;
            sceneLine.dialogLine = dialogLine;
            sceneLine.altGirl = altGirl;
            step.sceneLine = sceneLine;

            step.preventOptionShuffle = false;
            step.hasBestOption = false;

            step.responseOptions = new List<DialogSceneResponseOption>();

            step.hasBestBranch = false;
            step.showGirlStyles = "";
            step.hideOppositeSpeechBubble = false;

            step.waitTime = 0;
            step.metStatus = GirlMetStatus.MET;
            step.dialogTriggerIndex = 0;
            step.girlDetailType = GirlDetailType.LAST_NAME;
            step.stepBackSteps = 0;
            step.toEquipment = false;
            step.wrapped = false;
            step.tokenCount = 0;
            step.xPos = 0;
            step.yPos = 0;

            return step;
        }

        private DialogSceneStep WaitStep(int waitTime)
        {
            DialogSceneStep step = new DialogSceneStep();
            step.type = DialogSceneStepType.WAIT;

            step.waitTime = waitTime;

            return step;
        }

        private DialogSceneStep BranchDialogStep()
        {
            //TODOOOO
            
            DialogSceneStep step = new DialogSceneStep();
            step.type = DialogSceneStepType.BRANCH_DIALOG;
            

            return step;
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