using System;

namespace HoneyPot.Scene.Steps
{
    public delegate void StepFinishedEventHandler();
    
    public interface IStep
    {
        event StepFinishedEventHandler StepFinished;

        void InvokeStep();
    }
}
