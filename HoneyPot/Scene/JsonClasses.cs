﻿using System.Collections.Generic;

namespace HoneyPot.Scene
{
    public class Step
    {
        public int id { get; set; }

        public StepType type { get; set; }

        public string altGirl { get; set; }

        public int altGirlHairId { get; set; }

        public int altGirlOutfitId { get; set; }

        public string text { get; set; }

        public bool altGirlSpeaks { get; set; }

        public string newLoc { get; set; }

        public List<Response> responses { get; set; }
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
        ShowAltGirl,
        HideAltGirl,
        DialogLine,
        Travel,
        ResponseOptions
    }

    public struct SceneStep
    {
        public DialogSceneDefinition sceneDef;
        public LocationDefinition locDef;
        
        public SceneStep(DialogSceneDefinition sceneDef)
        {
            this.sceneDef = sceneDef;
            this.locDef = null;
        }

        public SceneStep(LocationDefinition locDef)
        {
            this.sceneDef = null;
            this.locDef = locDef;
        }
    }
}