using System.Collections.Generic;

namespace HoneyPot.Scene.Helper
{
    public class Step
    {
        public int id { get; set; }

        public StepType type { get; set; }

        public string girl { get; set; }

        public int girlHairId { get; set; }

        public int girlOutfitId { get; set; }
        
        public string altGirl { get; set; }

        public int altGirlHairId { get; set; }

        public int altGirlOutfitId { get; set; }

        public string text { get; set; }

        public bool altGirlSpeaks { get; set; }
        
        public bool closeEyes { get; set; }
        
        public string audio { get; set; }
        
        public GirlExpressionType expression { get; set; }

        public string newLoc { get; set; }

        public List<Response> responses { get; set; }
        
        public int dialogId { get; set; }
    }

    public class Response
    {
        public string text { get; set; }

        public List<Step> steps { get; set; }
    }

    public class Scene
    {
        public string name { get; set; }

        public string author { get; set; }

        public List<Step> steps { get; set; }
    }

    public enum StepType
    {
        ShowGirl,
        HideGirl,
        ShowAltGirl,
        HideAltGirl,
        DialogLine,
        Travel,
        ResponseOptions,
        ExistingDialogLine
    }

    public struct SceneStepV1
    {
        public DialogSceneDefinition sceneDef;
        public LocationDefinition locDef;
        public bool cleanDef;
        
        public SceneStepV1(DialogSceneDefinition sceneDef)
        {
            this.sceneDef = sceneDef;
            this.locDef = null;
            cleanDef = false;
        }

        public SceneStepV1(LocationDefinition locDef)
        {
            this.sceneDef = null;
            this.locDef = locDef;
            cleanDef = false;
        }

        public SceneStepV1(bool cleanDef)
        {
            this.sceneDef = null;
            this.locDef = null;
            this.cleanDef = cleanDef;
        }
    }
}
