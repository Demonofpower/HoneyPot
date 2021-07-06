using Holoville.HOTween;
using UnityEngine;

namespace HoneyPot.Scene
{
    class SceneHelper
    {
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
    }
}
