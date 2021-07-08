using System;
using HoneyPot.Scene.Helper;

namespace HoneyPot.Scene.Steps
{
    class ShowAltGirlStep : IStep
    {
        public GirlDefinition AltGirlDefinition { get; }
        
        public int AltGirlHairId { get; }
        
        public int AltGirlOutfitId { get; }

        public ShowAltGirlStep(GirlDefinition altGirlDefinition, int altGirlHairId, int altGirlOutfitId)
        {
            AltGirlDefinition = altGirlDefinition;
            AltGirlHairId = altGirlHairId;
            AltGirlOutfitId = altGirlOutfitId;
        }

        public event StepFinishedEventHandler StepFinished;

        public void InvokeStep()
        {
            throw new System.NotImplementedException();
        }
    }
}
