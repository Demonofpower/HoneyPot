using System.Collections.Generic;
using HoneyPot.Debug;
using HoneyPot.DebugUtil;
using HoneyPot.Scene.Helper;
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

        public DialogStep CreateDialogStep(string text, bool altGirlSpeaks, GirlExpressionType expressionType, bool closeEyes, string audioName)
        {
            return new DialogStep(text, altGirlSpeaks, expressionType, closeEyes, audioName);
        }

        public ShowGirlStep CreateShowGirlStep(string girlName, int altGirlHairId, int altGirlOutfitId)
        {
            return new ShowGirlStep(GetGirlByName(girlName), altGirlHairId, altGirlOutfitId);
        }
        public HideGirlStep CreateHideGirlStep()
        {
            return new HideGirlStep();
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

        public ResponseOptionsStep CreateResponseOptionsStep(List<Response> responseOptions)
        {
            return new ResponseOptionsStep(responseOptions);
        }
        public ExistingDialogStep CreateExistingDialogStep(int currStepDialogId, bool currStepAltGirlSpeaks)
        {
            return new ExistingDialogStep(currStepDialogId, currStepAltGirlSpeaks);
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
                if (location.name == name)
                {
                    return location;
                }
                if(name == "DPark" && location.fullName == "Dawnwood Park")
                {
                    return location;
                }
                if (name == "WPark" && location.fullName == "Water Park")
                {
                    return location;
                }
            }

            return null;
        }
    }
}
