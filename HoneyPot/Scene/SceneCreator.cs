﻿using HoneyPot.Debug;
using HoneyPot.Scene.Steps;
using UnityEngine;

namespace HoneyPot.Scene
{
    class SceneCreator
    {
        private readonly DebugLog debugLog;

        public SceneCreator(DebugLog debugLog)
        {
            this.debugLog = debugLog;
        }

        public DialogStep CreateDialogStep(string text, bool altGirlSpeaks)
        {
            return new DialogStep(text, altGirlSpeaks);
        }

        public ShowAltGirlStep CreateShowAltGirlStep(string girlName, int altGirlHairId, int altGirlOutfitId)
        {
            return new ShowAltGirlStep(GetGirlByName(girlName), altGirlHairId, altGirlOutfitId);
        }

        public HideAltGirlStep CreateHideAltGirlStep()
        {
            return new HideAltGirlStep();
        }
        
        public TravelStep CreateTravelStep(string name)
        {
            return new TravelStep(GetLocationByName(name));
        }

        private GirlDefinition GetGirlByName(string name)
        {
            GirlDefinition[] girls = Resources.FindObjectsOfTypeAll(typeof(GirlDefinition)) as GirlDefinition[];

            foreach (var girl in girls)
            {
                if (girl.firstName == name)
                {
                    return girl;
                }
            }

            return null;
        }

        private LocationDefinition GetLocationByName(string name)
        {
            LocationDefinition[] locations =
                Resources.FindObjectsOfTypeAll(typeof(LocationDefinition)) as LocationDefinition[];

            foreach (var location in locations)
            {
                if (location.fullName == name)
                {
                    return location;
                }
            }

            return null;
        }
    }
}
