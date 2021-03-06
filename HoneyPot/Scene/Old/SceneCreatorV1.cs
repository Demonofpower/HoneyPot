using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using HoneyPot.Debug;
using HoneyPot.DebugUtil;
using HoneyPot.Scene.Helper;

namespace HoneyPot.Scene.Old
{
    class SceneCreatorV1
    {
        private readonly DebugLog debugLog;
        private readonly SelectionManager selectionManager;

        public SceneCreatorV1(DebugLog debugLog, SelectionManager selectionManager)
        {
            this.debugLog = debugLog;
            this.selectionManager = selectionManager;
        }
        
        public void PlayScene(string path)
        {
            var dialogManager = GameManager.System.Dialog;

            var parser = new SceneParserV1(debugLog, selectionManager);

            var helper = new SceneHelper(debugLog);
            
            helper.HideUI();

            activeTravel = false;

            var scene = parser.Parse(path);
            debugLog.AddMessage("Scene parsed");
            debugLog.AddMessage("Start playing..");
            
            var firstSceneStep = scene[0];
            if (firstSceneStep.locDef != null)
            {
                debugLog.AddMessage("TRAVEL");
                GameManager.System.Location.currentLocation = firstSceneStep.locDef;
                GameManager.Stage.background.UpdateLocation();
            }
            else
            {
                debugLog.AddMessage("PLAYSCENE");
                dialogManager.PlayDialogScene(firstSceneStep.sceneDef);
            }

            scene.Remove(firstSceneStep);

            Thread t = new Thread(() =>
            {
                foreach (var sceneStep in scene)
                {
                    while (typeof(DialogManager)
                        .GetField("_activeDialogScene", BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.GetValue(dialogManager) != null || activeTravel)
                    {
                        debugLog.AddMessage("A scene is running..");
                        Thread.Sleep(50);
                    }

                    if (sceneStep.locDef != null)
                    {
                        activeTravel = true;
                        debugLog.AddMessage("TRAVEL");
                        
                        Paranoia.isBlackScreen = true;

                        GameManager.System.Location.currentLocation = sceneStep.locDef;
                        GameManager.Stage.background.UpdateLocation();

                        helper.HideGirlSpeechBubble();
                        helper.HideAltGirlSpeechBubble();
                    }
                    else if(sceneStep.sceneDef != null)
                    {
                        debugLog.AddMessage("PLAYSCENE");
                        dialogManager.PlayDialogScene(sceneStep.sceneDef);
                        typeof(DialogManager).GetField("_altGirlShowing", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(dialogManager, true);

                    }
                    else if(sceneStep.cleanDef)
                    {
                        helper.HideAltGirl();
                        helper.HideGirlSpeechBubble();
                        helper.HideAltGirlSpeechBubble();
                        helper.ShowUI();
                    }
                }
            });
            t.Start();
        }

        public static bool activeTravel;

        public DialogSceneStep ShowGirlStep(GirlDefinition girl, int girlHairId, int girlOutfitId)
        {
            DialogSceneStep step = new DialogSceneStep();
            step.type = DialogSceneStepType.SHOW_GIRL;
            step.preventOptionShuffle = false;
            step.hasBestOption = false;

            step.responseOptions = new List<DialogSceneResponseOption>();

            step.hasBestBranch = false;
            step.showGirlStyles = girlHairId + "," + girlOutfitId;
            step.hideOppositeSpeechBubble = false;

            step.girlDefinition = girl;

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
        
        public DialogSceneStep HideGirlStep()
        {
            DialogSceneStep step = new DialogSceneStep();
            step.type = DialogSceneStepType.HIDE_GIRL;

            return step;
        }

        public DialogSceneStep ShowAltGirlStep(GirlDefinition altGirl, int altGirlHairId, int altGirlOutfitId)
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

        public DialogSceneStep HideAltGirlStep()
        {
            DialogSceneStep step = new DialogSceneStep();
            step.type = DialogSceneStepType.HIDE_ALT_GIRL;

            return step;
        }

        public DialogSceneStep DialogLineStep(string text, bool altGirl = false, List<DialogSceneResponseOption> responseOptions = null)
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

            step.responseOptions = responseOptions ?? new List<DialogSceneResponseOption>();

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

        public DialogSceneStep WaitStep(int waitTime)
        {
            DialogSceneStep step = new DialogSceneStep();
            step.type = DialogSceneStepType.WAIT;

            step.waitTime = waitTime;

            return step;
        }

        public DialogSceneStep BranchDialogStep()
        {
            //TODOOOO

            DialogSceneStep step = new DialogSceneStep();
            step.type = DialogSceneStepType.BRANCH_DIALOG;


            return step;
        }

        public DialogSceneStep ResponseOptionsStep(Dictionary<string, List<DialogSceneStep>> responseOptions)
        {
            DialogSceneStep step = new DialogSceneStep();
            step.type = DialogSceneStepType.RESPONSE_OPTIONS;

            var options = new List<DialogSceneResponseOption>();

            int i = 0;
            foreach (var responseOption in responseOptions)
            {
                var option = new DialogSceneResponseOption();
                option.specialIndex = i;
                option.text = responseOption.Key;
                option.steps = responseOption.Value;

                options.Add(option);

                i += 1;
            }

            step.responseOptions = options;

            return step;
        }

        public DialogSceneStep TravelStep(LocationDefinition newLoc)
        {
            DialogSceneStep step = new DialogSceneStep();
            step.type = DialogSceneStepType.SET_NEXT_LOCATION;

            step.locationDefinition = newLoc;

            return step;
        }
    }
}
