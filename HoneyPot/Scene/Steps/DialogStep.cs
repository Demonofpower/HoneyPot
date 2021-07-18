using System;
using HoneyPot.Debug;

namespace HoneyPot.Scene.Steps
{
    public class DialogStep : IStep
    {
        public event StepFinishedEventHandler StepFinished;
        
        public string Text { get; }

        public bool AltGirlSpeaks { get; }

        public GirlExpressionType ExpressionType { get; }
        
        public bool CloseEyes { get; }

        public DialogStep(string text, bool altGirlSpeaks, GirlExpressionType expressionType, bool closeEyes)
        {
            Text = text;
            AltGirlSpeaks = altGirlSpeaks;
            ExpressionType = expressionType;
            CloseEyes = closeEyes;
        }

        public void InvokeStep()
        {
            var dialogLine = new DialogLine();
            dialogLine.text = Text;
            var dialogLineExp = new DialogLineExpression();
            dialogLineExp.changeEyes = true;
            dialogLineExp.changeMouth = true;
            dialogLineExp.closeEyes = CloseEyes;
            dialogLineExp.expression = ExpressionType;
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
            Paranoia.ActivateIsSpeaking(150, StepFinished);
        }
        
        private void AltGirlOnDialogLineReadEvent()
        {
            GameManager.Stage.altGirl.DialogLineReadEvent -= AltGirlOnDialogLineReadEvent;
            Paranoia.ActivateIsSpeaking(150, StepFinished);
        }
    }
}