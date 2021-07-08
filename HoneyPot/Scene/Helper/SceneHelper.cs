using Holoville.HOTween;
using HoneyPot.Debug;
using UnityEngine;

namespace HoneyPot.Scene.Helper
{
    class SceneHelper
    {
        private readonly DebugLog debugLog;
        
        public SceneHelper(DebugLog debugLog)
        {
            this.debugLog = debugLog;
        }
        
        public void HideGirl()
        {
            var dialogSceneSequence = new Sequence(new SequenceParms());
            dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlPieceContainers, 1f, new TweenParms().Prop("localX", -600).Ease(EaseType.EaseInCubic)));

            dialogSceneSequence.Play();
        }
        
        public void HideAltGirl()
        {
            var dialogSceneSequence = new Sequence(new SequenceParms());
            dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.altGirl.girlPieceContainers, 1f, new TweenParms().Prop("localX", -600).Ease(EaseType.EaseInCubic)));

            dialogSceneSequence.Play();
        }

        public void HideGirlSpeechBubble()
        {
            var dialogSceneSequence = new Sequence(new SequenceParms());
            dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlSpeechBubble.gameObj.transform, 0f, new TweenParms().Prop("localScale", new Vector3(0.8f, 0.8f, 1f)).Ease(EaseType.EaseInBack)));
            dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlSpeechBubble, 0f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseInBack)));

            dialogSceneSequence.Play();
        }

        public void HideAltGirlSpeechBubble()
        {
            var dialogSceneSequence = new Sequence(new SequenceParms());
            dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.altGirl.girlSpeechBubble.gameObj.transform, 0f, new TweenParms().Prop("localScale", new Vector3(0.8f, 0.8f, 1f)).Ease(EaseType.EaseInBack)));
            dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.altGirl.girlSpeechBubble, 0f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseInBack)));

            dialogSceneSequence.Play();
        }

        public void ShowUI()
        {
            GameManager.Stage.uiGirl.transform.localScale = new Vector3(1, 1, 1);
            GameManager.Stage.uiTop.transform.localScale = new Vector3(1, 1, 1);

            ActionMenuWindow[] actionMenuWindow =
                Resources.FindObjectsOfTypeAll(typeof(ActionMenuWindow)) as ActionMenuWindow[];
            foreach (var actionWindow in actionMenuWindow)
            {
                actionWindow.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public void HideUI()
        {
            GameManager.Stage.uiGirl.transform.localScale = new Vector3(0, 0, 0);
            GameManager.Stage.uiTop.transform.localScale = new Vector3(0, 0, 0);

            ActionMenuWindow[] actionMenuWindow =
                Resources.FindObjectsOfTypeAll(typeof(ActionMenuWindow)) as ActionMenuWindow[];
            foreach (var actionWindow in actionMenuWindow)
            {
                actionWindow.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }
}
