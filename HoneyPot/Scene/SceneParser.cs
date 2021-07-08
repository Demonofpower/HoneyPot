using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Reflection;

namespace HoneyPot.Scene
{
    class SceneParser
    {
        private readonly DebugLog debugLog;
        private readonly SelectionManager selectionManager;

        public SceneParser(DebugLog debugLog, SelectionManager selectionManager)
        {
            this.debugLog = debugLog;
            this.selectionManager = selectionManager;
        }

        public List<SceneStep> Parse(string path)
        {
            debugLog.AddMessage("---SceneLoader---");

            string text = File.ReadAllText(path);

            var parsedText = new JsonParser().Parse(text);

            Dictionary<string, object> parseDictionary = (Dictionary<string, object>) parsedText;

            Scene scene = (Scene)ConvertToClass(parseDictionary, typeof(Scene));

            if (scene == null)
            {
                debugLog.AddMessage("Scene file is corrupted");
                throw new Exception("Scene file is corrupted");
            }

            debugLog.AddMessage("Name: " + scene.name);
            debugLog.AddMessage("Author: " + scene.author);

            var scenes = new List<SceneStep>();

            for (int i = 0; i < scene.steps.Count; i++)
            {
                var currStep = GetStepWithId(scene.steps, i);

                DialogSceneStep step = null;
                switch (currStep.type)
                {
                    case StepType.ShowGirl:
                    case StepType.HideGirl:
                    case StepType.ShowAltGirl:
                    case StepType.HideAltGirl:
                    case StepType.DialogLine:
                    case StepType.ResponseOptions:
                        step = CreateStep(currStep);
                        break;
                    case StepType.Travel:
                        var locDef = GetLocationByName(currStep.newLoc);
                        scenes.Add(new SceneStep(locDef));

                        var dialogDef = new DialogSceneDefinition();
                        dialogDef.steps = new List<DialogSceneStep>();
                        dialogDef.steps.Add(new SceneCreator(debugLog, selectionManager).WaitStep(2));
                        scenes.Add(new SceneStep(dialogDef));
                        break;
                    default:
                        debugLog.AddMessage("Unknown step type at step " + currStep.id);
                        throw new Exception("Unknown step type at step " + currStep.id);
                }

                if (step != null)
                {
                    if (scenes.Count == 0)
                    {
                        var dialogDef = new DialogSceneDefinition();
                        dialogDef.steps = new List<DialogSceneStep>();
                        scenes.Add(new SceneStep(dialogDef));
                    }
                    
                    var currSceneDef = scenes[scenes.Count - 1].sceneDef;
                    currSceneDef.steps.Add(step);
                    currSceneDef.steps.Add(new SceneCreator(debugLog, selectionManager).WaitStep(1));
                }
            }
            
            scenes.Add(new SceneStep(true));

            return scenes;
        }

        private DialogSceneStep CreateStep(Step currStep)
        {
            var creator = new SceneCreator(debugLog, selectionManager);

            DialogSceneStep step = null;

            switch (currStep.type)
            {
                case StepType.ShowGirl:
                    step = creator.ShowGirlStep(GetGirlByName(currStep.girl), currStep.girlHairId,
                        currStep.girlOutfitId);
                    break;
                case StepType.HideGirl:
                    step = creator.HideGirlStep();
                    break;
                case StepType.ShowAltGirl:
                    step = creator.ShowAltGirlStep(GetGirlByName(currStep.altGirl), currStep.altGirlHairId,
                        currStep.altGirlOutfitId);
                    break;
                case StepType.HideAltGirl:
                    step = creator.HideAltGirlStep();
                    break;
                case StepType.DialogLine:
                    step = creator.DialogLineStep(currStep.text, currStep.altGirlSpeaks);
                    break;
                case StepType.ResponseOptions:
                    Dictionary<string, List<DialogSceneStep>> responseOptions = new Dictionary<string, List<DialogSceneStep>>();
                    foreach (var currStepResponse in currStep.responses)
                    {
                        responseOptions.Add(currStepResponse.text, CreateSteps(currStepResponse.steps));
                    }
                    step = creator.ResponseOptionsStep(responseOptions);
                    break;
                case StepType.Travel:
                    debugLog.AddMessage("Travel step is currently not supported as sub-step");
                    break;
                default:
                    debugLog.AddMessage("Unknown step type at step " + currStep.id);
                    break;
            }

            return step;
        }

        private List<DialogSceneStep> CreateSteps(List<Step> steps)
        {
            var dialogSteps = new List<DialogSceneStep>();

            dialogSteps.Add(new SceneCreator(debugLog, selectionManager).WaitStep(1));
            foreach (var step in steps)
            {
                dialogSteps.Add(CreateStep(step));
                dialogSteps.Add(new SceneCreator(debugLog, selectionManager).WaitStep(1));
            }

            return dialogSteps;
        }

        private Step GetStepWithId(List<Step> steps, int id)
        {
            foreach (var step in steps)
            {
                if (step.id == id)
                {
                    return step;
                }
            }

            return null;
        }

        private LocationDefinition GetLocationByName(string name)
        {
            LocationDefinition[] locations =
                Resources.FindObjectsOfTypeAll(typeof(LocationDefinition)) as LocationDefinition[];
            
            foreach (var location in locations)
            {
                if (location.fullName == name)
                {
                    return location;
                }
            }

            return null;
        }

        private GirlDefinition GetGirlByName(string name)
        {
            GirlDefinition[] girls =
                Resources.FindObjectsOfTypeAll(typeof(GirlDefinition)) as GirlDefinition[];

            foreach (var girl in girls)
            {
                if (girl.firstName == name)
                {
                    return girl;
                }
            }

            return null;
        }

        private object ConvertToClass(Dictionary<string, object> dic, Type classToUse)
        {
            Type type = classToUse;
            var obj = Activator.CreateInstance(type);

            foreach (var item in dic)
            {
                var property = type.GetProperty(item.Key);
                if (property is null)
                {
                    continue;
                }

                var value = item.Value;
                if (value is Dictionary<string, object> && !property.PropertyType.FullName.Contains("Generic.IList"))
                {
                    property.SetValue(obj, ConvertToClass((Dictionary<string, object>)(item.Value), property.PropertyType), index: null);
                    continue;
                }
                if (property.PropertyType.FullName.Contains("Generic.List"))
                {
                    var subClassTouse = property.PropertyType.GetGenericArguments()[0];

                    Type genericListType = typeof(List<>);
                    Type concreteListType = genericListType.MakeGenericType(subClassTouse);
                    var list = (IList)Activator.CreateInstance(concreteListType, new object[] { });

                    var values = (List<object>)dic[item.Key];

                    foreach (var itemClass in values)
                    {
                        list.Add(ConvertToClass((Dictionary<string, object>)itemClass, subClassTouse));
                    }
                    property.SetValue(obj, list, index: null);
                    continue;
                }

                if (property.Name.Contains("type"))
                {
                    StepType stepType = (StepType)Enum.Parse(typeof(StepType), (string)item.Value);
                    property.SetValue(obj, stepType, index: null);
                    continue;
                }

                property.SetValue(obj, item.Value, index: null);
            }

            return obj;
        }
    }
}
