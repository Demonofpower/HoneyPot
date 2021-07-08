using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HoneyPot.Debug;
using HoneyPot.Scene.Helper;
using HoneyPot.Scene.Steps;

namespace HoneyPot.Scene
{
    class SceneParser
    {
        private readonly DebugLog debugLog;

        public SceneParser(DebugLog debugLog)
        {
            this.debugLog = debugLog;
        }

        public List<IStep> Parse(string path)
        {
            debugLog.AddMessage("---SceneLoader---");

            string text = File.ReadAllText(path);

            var parsedText = new JsonParser().Parse(text);

            Dictionary<string, object> parseDictionary = (Dictionary<string, object>)parsedText;

            Helper.Scene scene = (Helper.Scene)ConvertToClass(parseDictionary, typeof(Helper.Scene));
            
            if (scene == null)
            {
                debugLog.AddMessage("Scene file is corrupted");
                throw new Exception("Scene file is corrupted");
            }

            debugLog.AddMessage("Name: " + scene.name);
            debugLog.AddMessage("Author: " + scene.author);
            
            return CreateIStepsFromSteps(scene.steps);
        }

        public List<IStep> CreateIStepsFromSteps(List<Step> sceneSteps)
        {
            var creator = new SceneCreator(debugLog);

            var steps = new List<IStep>();
            
            for (int i = 0; i < sceneSteps.Count; i++)
            {
                var currStep = GetStepWithId(sceneSteps, i);

                switch (currStep.type)
                {
                    case StepType.ShowGirl:
                        steps.Add(creator.CreateShowGirlStep(currStep.girl, currStep.girlHairId, currStep.girlOutfitId));
                        break;
                    case StepType.HideGirl:
                        steps.Add(creator.CreateHideGirlStep());
                        break;
                    case StepType.ShowAltGirl:
                        steps.Add(creator.CreateShowAltGirlStep(currStep.altGirl, currStep.altGirlHairId, currStep.altGirlOutfitId));
                        break;
                    case StepType.HideAltGirl:
                        steps.Add(creator.CreateHideAltGirlStep());
                        break;
                    case StepType.DialogLine:
                        steps.Add(creator.CreateDialogStep(currStep.text, currStep.altGirlSpeaks));
                        break;
                    case StepType.ResponseOptions:
                        steps.Add(creator.CreateResponseOptionsStep(currStep.responses));
                        break;
                    case StepType.Travel:
                        steps.Add(creator.CreateTravelStep(currStep.newLoc));
                        break;
                    default:
                        debugLog.AddMessage("Unknown step type at step " + currStep.id);
                        throw new Exception("Unknown step type at step " + currStep.id);
                }
            }

            return steps;
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
