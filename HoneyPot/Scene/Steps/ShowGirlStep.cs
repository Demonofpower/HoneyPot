using Holoville.HOTween;

namespace HoneyPot.Scene.Steps
{
    class ShowGirlStep : IStep
    {
        public event StepFinishedEventHandler StepFinished;
        
        public GirlDefinition GirlDefinition { get; }

        public int GirlHairId { get; }

        public int GirlOutfitId { get; }

        public ShowGirlStep(GirlDefinition girlDefinition, int girlHairId, int girlOutfitId)
        {
            GirlDefinition = girlDefinition;
            GirlHairId = girlHairId;
            GirlOutfitId = girlOutfitId;
        }

        public void InvokeStep()
        {
            GameManager.Stage.girl.girlPieceContainers.localX = 600f;
            GameManager.Stage.girl.ShowGirl(GirlDefinition);

            var styles = GirlHairId + "," + GirlOutfitId;

            if (!StringUtils.IsEmpty(styles))
            {
                string[] array = styles.Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    GameManager.Stage.girl.ChangeStyle(StringUtils.ParseIntValue(array[i]));
                }
            }
            
            var sequence = new Sequence(new SequenceParms().OnComplete(this.OnMainGirlShown));
            sequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlPieceContainers, 1f, new TweenParms().Prop("localX", 0).Ease(EaseType.EaseOutCubic)));
            sequence.Play();
        }

        private void OnMainGirlShown()
        {
            StepFinished?.Invoke();
        }
    }
}