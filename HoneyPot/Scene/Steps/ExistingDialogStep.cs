namespace HoneyPot.Scene.Steps
{
    class ExistingDialogStep : IStep
    {
        public event StepFinishedEventHandler StepFinished;

        public int DialogId { get; }
        
        public bool AltGirlSpeaks { get; }

        public ExistingDialogStep(int dialogId, bool altGirlSpeaks)
        {
            DialogId = dialogId;
            AltGirlSpeaks = altGirlSpeaks;
        }
        
        public void InvokeStep()
        {
            throw new System.NotImplementedException();
        }
    }
}
