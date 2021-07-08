using System;
using System.Collections.Generic;

namespace HoneyPot.Debug
{
    class Dump
    {
        private readonly DebugLog debugLog;

        public Dump(DebugLog debugLog)
        {
            this.debugLog = debugLog;
        }

        public void LocationsDump(IEnumerable<LocationDefinition> locationDefinitions)
        {
            try
            {
                debugLog.AddMessage("-------------");
                debugLog.AddMessage("LOCATIONS DUMP");
                debugLog.AddMessage("-------------");
                foreach (var locationDefinition in locationDefinitions)
                {
                    debugLog.AddMessage("-------------");
                    debugLog.AddMessage("LOCATION");
                    debugLog.AddMessage("-------------");
                    debugLog.AddMessage(locationDefinition.name);
                    debugLog.AddMessage(locationDefinition.fullName);
                    debugLog.AddMessage(locationDefinition.type.ToString());
                }
            }
            catch (Exception e)
            {
                debugLog.AddMessage("Dump failed!: " + e.Message);
            }
            finally
            {
                debugLog.AddMessage("-------------");
                debugLog.AddMessage("-------------");
                debugLog.AddMessage("-------------");
            }
        }

        public void GirlsDump(IEnumerable<GirlDefinition> girlsDefinitions)
        {
            try
            {
                debugLog.AddMessage("-------------");
                debugLog.AddMessage("GIRLS DUMP");
                debugLog.AddMessage("-------------");
                foreach (var girlDefinition in girlsDefinitions)
                {
                    debugLog.AddMessage("-------------");
                    debugLog.AddMessage("GIRL");
                    debugLog.AddMessage("-------------");
                    debugLog.AddMessage(girlDefinition.name);
                    debugLog.AddMessage(girlDefinition.firstName);
                }
            }
            catch (Exception e)
            {
                debugLog.AddMessage("Dump failed!: " + e.Message);
            }
            finally
            {
                debugLog.AddMessage("-------------");
                debugLog.AddMessage("-------------");
                debugLog.AddMessage("-------------");
            }
        }

        public void ScenesDump(IEnumerable<DialogSceneDefinition> dialogSceneDefinitions)
        {
            try
            {
                debugLog.AddMessage("-------------");
                debugLog.AddMessage("SCENES DUMP");
                debugLog.AddMessage("-------------");
                foreach (var scene in dialogSceneDefinitions)
                {
                    debugLog.AddMessage("-------------");
                    debugLog.AddMessage("SCENE");
                    debugLog.AddMessage("-------------");
                    debugLog.AddMessage(scene.id.ToString());
                    debugLog.AddMessage(scene.name);
                    debugLog.AddMessage(scene.editorFromJsonString);
                }
            }
            catch (Exception e)
            {
                debugLog.AddMessage("Dump failed!: " + e.Message);
            }
            finally
            {
                debugLog.AddMessage("-------------");
                debugLog.AddMessage("-------------");
                debugLog.AddMessage("-------------");
            }
        }

        public void SceneStepsDump(DialogSceneDefinition dialogSceneDefinition)
        {
            try
            {
                debugLog.AddMessage("-------------");
                debugLog.AddMessage("SCENE-DEF DUMP");
                debugLog.AddMessage("-------------");

                foreach (var dialogSceneStep in dialogSceneDefinition.steps)
                {
                    debugLog.AddMessage("-------------");
                    debugLog.AddMessage("STEP");
                    debugLog.AddMessage("-------------");

                    debugLog.AddMessage("Type: " + dialogSceneStep.type);
                    debugLog.AddMessage("LocName: " + dialogSceneStep.locationDefinition?.fullName);
                    debugLog.AddMessage("GirlName: " + dialogSceneStep.girlDefinition?.firstName);
                    debugLog.AddMessage("SoundEffect: " + dialogSceneStep.soundEffect);

                    debugLog.AddMessage("DialogLine: " + dialogSceneStep.sceneLine?.dialogLine?.GetText());
                    debugLog.AddMessage("DialogAltGirlSpeaks: " + dialogSceneStep.sceneLine?.altGirl);

                    debugLog.AddMessage("preventOptionShuffle: " + dialogSceneStep.preventOptionShuffle);
                    debugLog.AddMessage("hasBestOption: " + dialogSceneStep.hasBestOption);

                    debugLog.AddMessage("responseOptions: " + dialogSceneStep.responseOptions);
                    debugLog.AddMessage("responseOptionsCount: " + dialogSceneStep.responseOptions.Count);

                    debugLog.AddMessage("hasBestBranch: " + dialogSceneStep.hasBestBranch);
                    debugLog.AddMessage("showGirlStyles: " + dialogSceneStep.showGirlStyles);
                    debugLog.AddMessage("hideOppositeSpeechBubble: " + dialogSceneStep.hideOppositeSpeechBubble);
                    debugLog.AddMessage("altGirlName: " + dialogSceneStep.altGirl?.firstName);

                    debugLog.AddMessage("insertScene: " + dialogSceneStep.insertScene);

                    debugLog.AddMessage("waitTime: " + dialogSceneStep.waitTime);
                    debugLog.AddMessage("metStatus: " + dialogSceneStep.metStatus);
                    debugLog.AddMessage("dialogTrigger: " + dialogSceneStep.dialogTrigger?.name);
                    debugLog.AddMessage("dialogTriggerIndex: " + dialogSceneStep.dialogTriggerIndex);
                    debugLog.AddMessage("girlDetailType: " + dialogSceneStep.girlDetailType);
                    debugLog.AddMessage("stepBackSteps: " + dialogSceneStep.stepBackSteps);
                    debugLog.AddMessage("itemDefinition: " + dialogSceneStep.itemDefinition?.name);
                    debugLog.AddMessage("toEquipment: " + dialogSceneStep.toEquipment);
                    debugLog.AddMessage("wrapped: " + dialogSceneStep.wrapped);
                    debugLog.AddMessage("tokenDefinition: " + dialogSceneStep.tokenDefinition?.name);
                    debugLog.AddMessage("gridKey: " + dialogSceneStep.gridKey);
                    debugLog.AddMessage("tokenCount: " + dialogSceneStep.tokenCount);
                    debugLog.AddMessage("particleEmitterDefinition: " + dialogSceneStep.particleEmitterDefinition?.name);
                    debugLog.AddMessage("spriteGroupDefinition: " + dialogSceneStep.spriteGroupDefinition?.name);
                    debugLog.AddMessage("xPos: " + dialogSceneStep.xPos);
                    debugLog.AddMessage("yPos: " + dialogSceneStep.yPos);
                    debugLog.AddMessage("messageDef: " + dialogSceneStep.messageDef?.messageText);
                }
            }
            catch (Exception e)
            {
                debugLog.AddMessage("Dump failed!: " + e.Message);
            }
            finally
            {
                debugLog.AddMessage("-------------");
                debugLog.AddMessage("-------------");
                debugLog.AddMessage("-------------");
            }
        }
    }
}