namespace HoneyPot.Helper
{
    public static class GirlHelper
    {
        public static void WithBra()
        {
            var girl = GameManager.Stage.girl;
            var girlDefinition = girl.definition;
            
            ChangePiece(girlDefinition.braPiece, girl.bra, girl);
        }

        public static void Underwear()
        {
            var girl = GameManager.Stage.girl;
            var girlDefinition = girl.definition;
            
            girl.outfit.RemoveAllChildren(true);
            ChangePiece(girlDefinition.pantiesPiece, girl.panties, girl);
            ChangePiece(girlDefinition.braPiece, girl.bra, girl);
        }

        public static void OnlyBra()
        {
            var girl = GameManager.Stage.girl;
            var girlDefinition = girl.definition;

            girl.outfit.RemoveAllChildren(true);
            ChangePiece(girlDefinition.braPiece, girl.bra, girl);
            girl.panties.RemoveAllChildren(true);
        }

        public static void OnlyPanties()
        {
            var girl = GameManager.Stage.girl;
            var girlDefinition = girl.definition;

            girl.outfit.RemoveAllChildren(true);
            girl.bra.RemoveAllChildren(true);
            ChangePiece(girlDefinition.pantiesPiece, girl.panties, girl);
        }

        public static void Nude()
        {
            var girl = GameManager.Stage.girl;
            
            girl.outfit.RemoveAllChildren(true);
            girl.bra.RemoveAllChildren(true);
            girl.panties.RemoveAllChildren(true);
        }
        
        public static void ChangePiece(GirlPieceArt pieceArt, DisplayObject container, Girl currGirl)
        {
            container.RemoveAllChildren(true);

            var spriteObject =
                DisplayUtils.CreateSpriteObject(currGirl.spriteCollection, pieceArt.spriteName);
            container.AddChild(spriteObject);

            if (currGirl.flip)
            {
                spriteObject.sprite.FlipX = true;
                spriteObject.SetLocalPosition(1200 - pieceArt.x, -(float)pieceArt.y);
            }
            else
            {
                spriteObject.SetLocalPosition(pieceArt.x, -(float)pieceArt.y);
            }
        }

        public static void ChangeHairstyle(int id, Girl currGirl, GirlDefinition currGirlDef)
        {
            var currGirlPiece = currGirlDef.pieces[currGirlDef.hairstyles[id].artIndex];

            ChangePiece(currGirlPiece.primaryArt, currGirl.fronthair, currGirl);
            ChangePiece(currGirlPiece.secondaryArt, currGirl.backhair, currGirl);
        }

        public static void ChangeOutfit(int id, Girl currGirl, GirlDefinition currGirlDef)
        {
            var currGirlPiece = currGirlDef.pieces[currGirlDef.outfits[id].artIndex];

            ChangePiece(currGirlPiece.primaryArt, currGirl.outfit, currGirl);
            //this.AddGirlPiece(this.definition.pieces[18]);
        }

        public static void ChangeGirl(int id)
        {
            var girlDefinition = GameManager.Data.Girls.Get(id);

            var girl = GameManager.Stage.girl;
            girl.ShowGirl(girlDefinition);
        }
    }
}
