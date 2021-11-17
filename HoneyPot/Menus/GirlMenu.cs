using System.Collections.Generic;
using HoneyPot.Debug;
using HoneyPot.DebugUtil;
using HoneyPot.Helper;
using UnityEngine;

namespace HoneyPot.Menus
{
    class GirlMenu
    {
        private readonly DebugLog debugLog;
        private readonly SelectionManager selectionManager;

        public GirlMenu(DebugLog debugLog, SelectionManager selectionManager)
        {
            this.debugLog = debugLog;
            this.selectionManager = selectionManager;
        }

        public void DoGirl(int windowId)
        {
            var allGirls = GameManager.Data.Girls.GetAll();

            var girl = GameManager.Stage.girl;
            var girlDefinition = girl.definition;
            var girlPlayerData = GameManager.System.Player.GetGirlData(girlDefinition);

            if (GUILayout.Button("WithBra"))
            {
                GirlHelper.WithBra();

                debugLog.AddMessage("Girl has now  bra hoho");
            }
            if (GUILayout.Button("Underwear"))
            {
                GirlHelper.Underwear();

                debugLog.AddMessage("Girl has now underwear xaxa");
            }
            if (GUILayout.Button("OnlyBra"))
            {
                GirlHelper.OnlyBra();

                debugLog.AddMessage("Girl has now only bra haha");
            }
            if (GUILayout.Button("OnlyPanties"))
            {
                GirlHelper.OnlyPanties();

                debugLog.AddMessage("Girl has now only panties hihi");
            }
            if (GUILayout.Button("Nude"))
            {
                GirlHelper.Nude();

                debugLog.AddMessage("Girl is now naked hehe");
            }

            if (GUILayout.Button("Hairstyle"))
            {
                var hairstyleNames = new List<string>();
                foreach (var girlDefinitionHairstyle in girlDefinition.hairstyles)
                    hairstyleNames.Add(girlDefinitionHairstyle.styleName);

                selectionManager.NewSelection(hairstyleNames, 3, () =>
                {
                    GirlHelper.ChangeHairstyle(selectionManager.SelectionId, girl, girlDefinition);
                    debugLog.AddMessage("Changed curr girl hairstyle to: " +
                                        hairstyleNames[selectionManager.SelectionId]);
                });
            }

            if (GUILayout.Button("Outfit"))
            {
                var outfitNames = new List<string>();
                foreach (var girlDefinitionOutfit in girlDefinition.outfits)
                    outfitNames.Add(girlDefinitionOutfit.styleName);

                selectionManager.NewSelection(outfitNames, 3, () =>
                {
                    GirlHelper.ChangeOutfit(selectionManager.SelectionId, girl, girlDefinition);
                    debugLog.AddMessage("Changed curr girl outfit to: " + outfitNames[selectionManager.SelectionId]);
                });
            }

            if (GUILayout.Button("ChangeGirl"))
            {
                var girlNames = new List<string>();
                foreach (var thisGirl in allGirls)
                {
                    girlNames.Add(thisGirl.firstName);
                }

                selectionManager.NewSelection(girlNames, 3, () =>
                {
                    GirlHelper.ChangeGirl(selectionManager.SelectionId + 1);
                    debugLog.AddMessage("Changed girl to: " + girlNames[selectionManager.SelectionId]);
                });
            }

            if (GUILayout.Button("HumanKyu"))
            {

                var kyu = new GirlDefinition();
                foreach (var thisGirl in allGirls)
                {
                    if (thisGirl.firstName == "Kyu")
                    {
                        kyu = thisGirl;
                    }
                }

                GirlHelper.ChangeGirl(kyu.id);

                foreach (var displayObject in GameManager.Stage.girl.extraOne.GetChildren())
                {
                    debugLog.AddMessage(displayObject.name);
                }
                debugLog.AddMessage("....");
                foreach (var displayObject in GameManager.Stage.girl.extraTwo.GetChildren())
                {
                    debugLog.AddMessage(displayObject.name);
                }
                

                var frontHair = girl.definition.pieces[girl.definition.pieces.Count - 2];
                var backHair = girl.definition.pieces[girl.definition.pieces.Count - 1];
                GirlHelper.ChangePiece(frontHair.primaryArt, girl.fronthair, girl);
                GirlHelper.ChangePiece(backHair.primaryArt, girl.backhair, girl);
                
                //var wings = girl.definition.pieces[girl.definition.pieces.Count - 3];
                //ChangePiece(wings.primaryArt, girl.extraOne, girl);
                
                girl.extraOne.RemoveAllChildren(true);

                debugLog.AddMessage("Kyu is now human-like");
            }
        }
    }
}