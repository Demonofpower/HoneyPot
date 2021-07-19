using System.Collections.Generic;
using HoneyPot.Debug;
using HoneyPot.DebugUtil;
using HoneyPot.Scene.Helper;
using HoneyPot.Scene.Steps;

namespace HoneyPot.Scene
{
    class ScenePlayer
    {
        public static ScenePlayer Instance;
        
        private readonly DebugLog debugLog;

        private List<IStep> remainingSteps;
        private bool active;
        private bool inStep;

        public ScenePlayer(DebugLog debugLog)
        {
            Instance = this;
            
            this.debugLog = debugLog;

            remainingSteps = new List<IStep>();
            active = false;
        }

        public void Update()
        {
            if (active && !inStep)
            {
                PlayStep();
            }
        }

        public void Play(string path)
        {
            var helper = new SceneHelper(debugLog);
            var parser = new SceneParser(debugLog);
            var steps = parser.Parse(path);

            helper.HideUI();
            helper.HideGirlSpeechBubble();
            helper.HideAltGirlSpeechBubble();
            
            remainingSteps = steps;
            
            active = true;
            inStep = false;
        }

        private void PlayStep()
        {
            if (remainingSteps.Count == 0)
            {
                Clear();
                return;
            }

            inStep = true;

            var currStep = remainingSteps[0];
            

            currStep.StepFinished += CurrStepOnStepFinished;
            
            remainingSteps.Remove(currStep);
            
            currStep.InvokeStep();
        }

        private void Clear()
        {
            var helper = new SceneHelper(debugLog);
            
            helper.HideGirlSpeechBubble();
            helper.HideAltGirlSpeechBubble();
            helper.HideAltGirl();
            helper.ShowUI();
            
            active = false;
            Instance = null;
        }

        private void CurrStepOnStepFinished()
        {
            inStep = false;
        }

        public void AddScenesAtFirst(List<Step> steps)
        {
            var newSteps = new List<IStep>();
            newSteps.AddRange(new SceneParser(debugLog).CreateIStepsFromSteps(steps));
            newSteps.AddRange(remainingSteps);

            remainingSteps = newSteps;
        }
    }
}