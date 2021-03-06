using HoneyPot.Load;
using HoneyPot.Scene.Helper;

namespace HoneyPot.Scene.Steps
{
    public class DialogStep : IStep
    {
        public event StepFinishedEventHandler StepFinished;

        public string Text { get; }

        public bool AltGirlSpeaks { get; }

        public GirlExpressionType ExpressionType { get; }

        public bool CloseEyes { get; }
        
        public string AudioName { get; }

        public DialogStep(string text, bool altGirlSpeaks, GirlExpressionType expressionType, bool closeEyes, string audioName)
        {
            Text = text;
            AltGirlSpeaks = altGirlSpeaks;
            ExpressionType = expressionType;
            CloseEyes = closeEyes;
            AudioName = audioName;
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

            // ReSharper disable once ReplaceWithStringIsNullOrEmpty
            if (AudioName != null && AudioName != "")
            {
                var myClip = SceneAudioLoader.LoadedClips[AudioName];
                dialogLine.audioDefinition = new AudioDefinition() { clip = myClip };
            }

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