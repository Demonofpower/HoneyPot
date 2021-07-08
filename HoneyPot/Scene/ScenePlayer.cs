using System.Collections.Generic;
using HoneyPot.Debug;
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
            
            remainingSteps = steps;
            
            active = true;
            inStep = false;
        }

        private void PlayStep()
        {
            if (remainingSteps.Count == 0)
            {
                new SceneHelper(debugLog).ShowUI();
                active = false;
                Instance = null;
                return;
            }

            inStep = true;

            var currStep = remainingSteps[0];
            

            currStep.StepFinished += CurrStepOnStepFinished;
            
            remainingSteps.Remove(currStep);
            
            currStep.InvokeStep();
        }

        private void CurrStepOnStepFinished()
        {
            inStep = false;
        }
    }
}