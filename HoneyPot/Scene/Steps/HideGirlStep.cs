using Holoville.HOTween;
using UnityEngine;

namespace HoneyPot.Scene.Steps
{
    class HideGirlStep : IStep
    {
        public event StepFinishedEventHandler StepFinished;
        
        public void InvokeStep()
        {
            var sequence = new Sequence(new SequenceParms().OnComplete(this.OnGirlHidden));

            sequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlSpeechBubble.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(0.8f, 0.8f, 1f)).Ease(EaseType.EaseInBack)));
            sequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlSpeechBubble, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseInBack)));
            sequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlPieceContainers, 1f, new TweenParms().Prop("localX", 600).Ease(EaseType.EaseInCubic)));
            sequence.Play();
        }

        private void OnGirlHidden()
        {
            StepFinished?.Invoke();
        }
    }
}
