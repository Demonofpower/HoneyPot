using HoneyPot.Debug;
using HoneyPot.Scene.Helper;

namespace HoneyPot.Scene.Steps
{
    class TravelStep : IStep
    {
        public event StepFinishedEventHandler StepFinished;
        
        public LocationDefinition NewLocationDefinition { get; }

        public TravelStep(LocationDefinition newLocationDefinition)
        {
            NewLocationDefinition = newLocationDefinition;
        }

        public void InvokeStep()
        {
            new SceneHelper(DebugLog.Instance).HideGirlSpeechBubble();
            new SceneHelper(DebugLog.Instance).HideAltGirlSpeechBubble();
            
            Paranoia.ActivateBlackScreen(300, StepFinished);

            GameManager.System.Location.currentLocation = NewLocationDefinition;
            GameManager.Stage.background.UpdateLocation();
        }
    }
}
