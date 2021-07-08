using System;
using HoneyPot.Debug;

namespace HoneyPot.Scene.Steps
{
    public class DialogStep : IStep
    {
        public event StepFinishedEventHandler StepFinished;
        
        public string Text { get; }

        public bool AltGirlSpeaks { get; }

        public DialogStep(string text, bool altGirlSpeaks)
        {
            AltGirlSpeaks = altGirlSpeaks;
            Text = text;
        }

        public void InvokeStep()
        {
            var dialogLine = new DialogLine();
            dialogLine.text = Text;
            var dialogLineExp = new DialogLineExpression();
            dialogLineExp.changeEyes = false;
            dialogLineExp.changeMouth = false;
            dialogLineExp.closeEyes = false;
            dialogLineExp.expression = GirlExpressionType.HORNY;
            dialogLineExp.startAtCharIndex = 0;
            dialogLine.startExpression = dialogLineExp;

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
            StepFinished?.Invoke();
        }
        
        private void AltGirlOnDialogLineReadEvent()
        {
            GameManager.Stage.altGirl.DialogLineReadEvent -= AltGirlOnDialogLineReadEvent;
            StepFinished?.Invoke();
        }
    }
}