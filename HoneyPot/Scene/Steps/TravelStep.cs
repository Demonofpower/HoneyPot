using System;

namespace HoneyPot.Scene.Steps
{
    class TravelStep : IStep
    {
        public LocationDefinition NewLocationDefinition { get; }

        public TravelStep(LocationDefinition newLocationDefinition)
        {
            NewLocationDefinition = newLocationDefinition;
        }

        public event StepFinishedEventHandler StepFinished;

        public void InvokeStep()
        {
            throw new System.NotImplementedException();
        }
    }
}
