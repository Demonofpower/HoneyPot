using HoneyPot.DebugUtil;
using HoneyPot.Load;

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
            var dialogLine = ExistingDialogLoader.LoadedDialogs[DialogId];
           
            if (AltGirlSpeaks)
            {
                GameManager.Stage.altGirl.DialogLineReadEvent += AltGirlOnDialogLineReadEvent;
                GameManager.Stage.altGirl.ReadDialogLine(dialogLine, false, false, false, false);
            }
            else
            {
                GameManager.Stage.girl.DialogLineReadEvent += GirlOnDialogLineReadEvent;
                GameManager.Stage.girl.ReadDialogLine(dialogLine, false, false, false, false);
            }
        }

        private void GirlOnDialogLineReadEvent()
        {
            GameManager.Stage.girl.DialogLineReadEvent -= GirlOnDialogLineReadEvent;
            Paranoia.ActivateIsSpeaking(150, StepFinished);
        }

        private void AltGirlOnDialogLineReadEvent()
        {
            GameManager.Stage.altGirl.DialogLineReadEvent -= AltGirlOnDialogLineReadEvent;
            Paranoia.ActivateIsSpeaking(150, StepFinished);
        }
    }
}
