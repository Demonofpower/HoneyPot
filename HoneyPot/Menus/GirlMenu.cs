using System.Collections.Generic;
using HoneyPot.Debug;
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

            if (GUILayout.Button("Bra"))
            {
                Naked(girl, girlDefinition);

                debugLog.AddMessage("Girl has now bra haha");
            }
            if (GUILayout.Button("NoBra"))
            {
                Naked(girl, girlDefinition);

                debugLog.AddMessage("Girl is now braless hihi");
            }
            if (GUILayout.Button("Nude"))
            {
                ChangePiece(null, girl.outfit, girl);

                debugLog.AddMessage("Girl is now naked hehe");
            }

            if (GUILayout.Button("Hairstyle"))
            {
                var hairstyleNames = new List<string>();
                foreach (var girlDefinitionHairstyle in girlDefinition.hairstyles)
                    hairstyleNames.Add(girlDefinitionHairstyle.styleName);

                selectionManager.NewSelection(hairstyleNames, 3, () =>
                {
                    ChangeHairstyle(selectionManager.SelectionId, girl, girlDefinition);
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
                    ChangeOutfit(selectionManager.SelectionId, girl, girlDefinition);
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
                    ChangeGirl(selectionManager.SelectionId + 1);
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

                ChangeGirl(kyu.id);

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
                ChangePiece(frontHair.primaryArt, girl.fronthair, girl);
                ChangePiece(backHair.primaryArt, girl.backhair, girl);
                
                //var wings = girl.definition.pieces[girl.definition.pieces.Count - 3];
                //ChangePiece(wings.primaryArt, girl.extraOne, girl);
                
                girl.extraOne.RemoveAllChildren(true);

                debugLog.AddMessage("Kyu is now human-like");
            }
        }

        private void ChangePiece(GirlPieceArt pieceArt, DisplayObject container, Girl currGirl)
        {
            container.RemoveAllChildren(true);

            var fronthairSpriteObject =
                DisplayUtils.CreateSpriteObject(currGirl.spriteCollection, pieceArt.spriteName);
            container.AddChild(fronthairSpriteObject);

            if (currGirl.flip)
            {
                fronthairSpriteObject.sprite.FlipX = true;
                fronthairSpriteObject.SetLocalPosition(1200 - pieceArt.x, -(float) pieceArt.y);
            }
            else
            {
                fronthairSpriteObject.SetLocalPosition(pieceArt.x, -(float) pieceArt.y);
            }
        }

        private void Naked(Girl currGirl, GirlDefinition currGirlDef)
        {
            //Save and change vars
            var oldLocType = GameManager.System.Location.currentLocation.type;
            GameManager.System.Location.currentLocation.type = LocationType.DATE;
            var oldIsBonusRoundloc = GameManager.System.Location.currentLocation.bonusRoundLocation;
            GameManager.System.Location.currentLocation.bonusRoundLocation = true;

            //DO
            currGirl.ShowGirl(currGirlDef);
            GameManager.Stage.girl.HideBra();
            GameManager.Stage.girl.ChangeExpression(GirlExpressionType.HORNY, true, true, true, 0f);

            //Reset old vars
            GameManager.System.Location.currentLocation.type = oldLocType;
            GameManager.System.Location.currentLocation.bonusRoundLocation = oldIsBonusRoundloc;
        }

        private void ChangeHairstyle(int id, Girl currGirl, GirlDefinition currGirlDef)
        {
            var currGirlPiece = currGirlDef.pieces[currGirlDef.hairstyles[id].artIndex];

            ChangePiece(currGirlPiece.primaryArt, currGirl.fronthair, currGirl);
            ChangePiece(currGirlPiece.secondaryArt, currGirl.backhair, currGirl);
        }

        private void ChangeOutfit(int id, Girl currGirl, GirlDefinition currGirlDef)
        {
            var currGirlPiece = currGirlDef.pieces[currGirlDef.outfits[id].artIndex];

            ChangePiece(currGirlPiece.primaryArt, currGirl.outfit, currGirl);
            //this.AddGirlPiece(this.definition.pieces[18]);
        }

        private void ChangeGirl(int id)
        {
            var girlDefinition = GameManager.Data.Girls.Get(id);

            var girl = GameManager.Stage.girl;
            girl.ShowGirl(girlDefinition);
        }
    }
}