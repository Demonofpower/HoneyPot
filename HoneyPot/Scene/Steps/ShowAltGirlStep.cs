using Holoville.HOTween;
using Holoville.HOTween.Core;
using HoneyPot.Debug;
using UnityEngine;

namespace HoneyPot.Scene.Steps
{
    class ShowAltGirlStep : IStep
    {
        public event StepFinishedEventHandler StepFinished;

        public GirlDefinition AltGirlDefinition { get; }

        public int AltGirlHairId { get; }

        public int AltGirlOutfitId { get; }

        public ShowAltGirlStep(GirlDefinition altGirlDefinition, int altGirlHairId, int altGirlOutfitId)
        {
            AltGirlDefinition = altGirlDefinition;
            AltGirlHairId = altGirlHairId;
            AltGirlOutfitId = altGirlOutfitId;
        }

        public void InvokeStep()
        {
            GameManager.Stage.altGirl.girlPieceContainers.localX = -600f;
            GameManager.Stage.altGirl.ShowGirl(AltGirlDefinition);

            var styles = AltGirlHairId + "," + AltGirlOutfitId;

            if (!StringUtils.IsEmpty(styles))
            {
                string[] array = styles.Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    GameManager.Stage.altGirl.ChangeStyle(StringUtils.ParseIntValue(array[i]));
                }
            }

            var sequence = new Sequence(new SequenceParms().OnComplete(this.OnAltGirlShown, true, styles));
            var p_time = 0.5f;
            sequence.Insert(p_time, HOTween.To(GameManager.Stage.altGirl.girlPieceContainers, 1f, new TweenParms().Prop("localX", 0).Ease(EaseType.EaseOutCubic)));
            
            sequence.Play();
        }

        private void OnAltGirlShown(TweenEvent __notUsed)
        {
            StepFinished?.Invoke();
        }
    }
}