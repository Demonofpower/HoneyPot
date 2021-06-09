using System;
using System.Reflection;
using UnityEngine;

namespace HoneyPot
{
    public class Paranoia : MonoBehaviour
    {
        private DebugLog debugLog;

        private bool isMenuOpen;
        private bool isDebugOpen;
        private bool isPlayerOpen;
        private bool isPuzzleOpen;
        private bool isGirlOpen;

        private string newMoney;
        private string newHunie;

        private string newMoves;
        private string newAffection;
        private string newPassion;
        private string newSentiment;

        public static bool noDrain;

        // Token: 0x06000BC1 RID: 3009 RVA: 0x0004D52C File Offset: 0x0004B72C
        public void Start()
        {
            this.debugLog = new DebugLog();
            this.newMoney = "0";
            this.newHunie = "0";
            this.newMoves = "0";
            this.newAffection = "0";
            this.newPassion = "0";
            this.newSentiment = "0";
            Paranoia.noDrain = false;
        }

        // Token: 0x06000BC2 RID: 3010 RVA: 0x0004D58C File Offset: 0x0004B78C
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                this.isMenuOpen = !this.isMenuOpen;
                this.isPlayerOpen = false;
                this.isPuzzleOpen = false;
                isGirlOpen = false;
                this.debugLog.AddMessage("Menu opened/closed");
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                GameManager.Stage.girl.HideBra();
                GameManager.Stage.girl.ChangeExpression(GirlExpressionType.HORNY, true, true, true, 0f);
            }
        }

        // Token: 0x06000BC3 RID: 3011 RVA: 0x0004D608 File Offset: 0x0004B808
        public void OnGUI()
        {
            GUI.contentColor = Color.magenta;
            GUI.Label(new Rect(0f, 30f, 150f, 50f), "HoneyPot");
            GUI.contentColor = Color.white;
            GUI.Label(new Rect(0f, 50f, 150f, 50f), "Press F1 for Menu");
            GUI.contentColor = Color.red;
            GUI.Label(new Rect(0f, 70f, 150f, 50f), "By Paranoia with <3");
            if (this.isMenuOpen)
            {
                this.OpenMenu();
            }

            if (this.isDebugOpen)
            {
                this.OpenDebug();
            }

            if (this.isPlayerOpen)
            {
                this.OpenPlayer();
            }

            if (this.isPuzzleOpen)
            {
                this.OpenPuzzle();
            }

            if (this.isGirlOpen)
            {
                this.OpenGirl();
            }
        }

        // Token: 0x06000BC5 RID: 3013 RVA: 0x0004D6D4 File Offset: 0x0004B8D4
        private void OpenMenu()
        {
            Rect clientRect = new Rect(120f, 20f, 120f, 120f);
            GUI.Window(0, clientRect, new GUI.WindowFunction(this.DoMyWindow), "Cool Menu");
        }

        // Token: 0x06000BC6 RID: 3014 RVA: 0x0004D718 File Offset: 0x0004B918
        private void DoMyWindow(int windowID)
        {
            if (GUILayout.Button("debug log", new GUILayoutOption[0]))
            {
                this.isDebugOpen = !this.isDebugOpen;
                this.debugLog.AddMessage("Debug opened/closed");
            }

            if (GUILayout.Button("Player Menu", new GUILayoutOption[0]))
            {
                this.isPlayerOpen = !this.isPlayerOpen;
                this.debugLog.AddMessage("Player opened/closed");
            }

            if (GUILayout.Button("Puzzle Menu", new GUILayoutOption[0]))
            {
                this.isPuzzleOpen = !this.isPuzzleOpen;
                this.debugLog.AddMessage("Puzzle opened/closed");
            }

            if (GUILayout.Button("Girl Menu", new GUILayoutOption[0]))
            {
                this.isGirlOpen = !this.isGirlOpen;
                this.debugLog.AddMessage("Girl opened/closed");
            }
        }

        // Token: 0x06000BC7 RID: 3015 RVA: 0x0004D7B8 File Offset: 0x0004B9B8
        private void OpenDebug()
        {
            Rect clientRect = new Rect(640f, 20f, 500f, 400f);
            GUI.Window(1, clientRect, new GUI.WindowFunction(this.DoDebugLog), "Debug log");
        }

        // Token: 0x06000BC8 RID: 3016 RVA: 0x0004D7FC File Offset: 0x0004B9FC
        private void DoDebugLog(int windowID)
        {
            foreach (string text in this.debugLog.PrintLastMessages())
            {
                GUILayout.Label(text, new GUILayoutOption[0]);
            }
        }

        // Token: 0x06000BC9 RID: 3017 RVA: 0x0004D858 File Offset: 0x0004BA58
        private void OpenPlayer()
        {
            Rect clientRect = new Rect(240f, 20f, 200f, 400f);
            GUI.Window(2, clientRect, new GUI.WindowFunction(this.DoPlayer), "Player menu");
        }

        // Token: 0x06000BCA RID: 3018 RVA: 0x0004D89C File Offset: 0x0004BA9C
        private void DoPlayer(int windowID)
        {
            GUILayout.BeginHorizontal("money", new GUILayoutOption[0]);
            this.newMoney = GUILayout.TextField(this.newMoney, 10, new GUILayoutOption[0]);
            if (GUILayout.Button("ChangeMoney", new GUILayoutOption[0]))
            {
                GameManager.System.Player.money = int.Parse(this.newMoney);
                this.debugLog.AddMessage("Money changed to: " + this.newMoney);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("hunie", new GUILayoutOption[0]);
            this.newHunie = GUILayout.TextField(this.newHunie, 10, new GUILayoutOption[0]);
            if (GUILayout.Button("ChangeHunie", new GUILayoutOption[0]))
            {
                GameManager.System.Player.hunie = int.Parse(this.newHunie);
                this.debugLog.AddMessage("Hunie changed to: " + this.newHunie);
            }

            GUILayout.EndHorizontal();
            if (GUILayout.Button("UnlockAll", new GUILayoutOption[0]))
            {
                this.UnlockAll();
                this.debugLog.AddMessage("All unlocked");
            }

            if (GUILayout.Button("UnlockAllAchivements", new GUILayoutOption[0]))
            {
                this.UnlockAllAchivements();
                this.debugLog.AddMessage("All achivements unlocked");
            }
        }

        // Token: 0x06000BCB RID: 3019 RVA: 0x0004D9F0 File Offset: 0x0004BBF0
        private void UnlockAllAchivements()
        {
            SteamUtils.UnlockAchievement("alpha", true);
            SteamUtils.UnlockAchievement("alpha_champ", true);
            SteamUtils.UnlockAchievement("alpha_master", true);
            SteamUtils.UnlockAchievement("ayy_lmao", true);
            SteamUtils.UnlockAchievement("beastiality", true);
            SteamUtils.UnlockAchievement("buying_love", true);
            SteamUtils.UnlockAchievement("date_the_fairy", true);
            SteamUtils.UnlockAchievement("heavenly_body", true);
            SteamUtils.UnlockAchievement("lucky_loser", true);
            SteamUtils.UnlockAchievement("make_the_move", true);
            SteamUtils.UnlockAchievement("money_bags", true);
            SteamUtils.UnlockAchievement("pickup_artist", true);
            SteamUtils.UnlockAchievement("pickup_legend", true);
            SteamUtils.UnlockAchievement("quickie", true);
            SteamUtils.UnlockAchievement("smooth_as_fuck", true);
            SteamUtils.UnlockAchievement("sobriety", true);
            SteamUtils.UnlockAchievement("there_may_be_hope", true);
            SteamUtils.UnlockAchievement("truest_player", true);
            SteamUtils.UnlockAchievement("vcard_revoked", true);
            SteamUtils.UnlockAchievement("zero_life", true);
        }

        // Token: 0x06000BCC RID: 3020 RVA: 0x0004DADC File Offset: 0x0004BCDC
        private void UnlockAll()
        {
            GameManager.System.Player.hunie = 99999;
            GameManager.System.Player.money = 99999;
            foreach (GirlDefinition girlDef in GameManager.Data.Girls.GetAll())
            {
                GirlPlayerData girlData = GameManager.System.Player.GetGirlData(girlDef);
                girlData.metStatus = GirlMetStatus.MET;
                girlData.relationshipLevel = 5;
                girlData.appetite = 12;
                girlData.inebriation = 12;
                girlData.dayDated = false;
                girlData.AddPhotoEarned(0);
                girlData.AddPhotoEarned(1);
                girlData.AddPhotoEarned(2);
                girlData.AddPhotoEarned(3);
                foreach (ItemDefinition item in girlData.GetGirlDefinition().uniqueGiftList)
                {
                    girlData.AddItemToUniqueGifts(item);
                }

                foreach (ItemDefinition item2 in girlData.GetGirlDefinition().collection)
                {
                    girlData.AddItemToCollection(item2);
                }

                for (int i = 0; i < girlData.GetGirlDefinition().hairstyles.Count; i++)
                {
                    girlData.UnlockHairstyle(i);
                }

                for (int j = 0; j < girlData.GetGirlDefinition().outfits.Count; j++)
                {
                    girlData.UnlockOutfit(j);
                }

                foreach (object obj in Enum.GetValues(typeof(GirlDetailType)))
                {
                    GirlDetailType type = (GirlDetailType)obj;
                    girlData.KnowDetail(type);
                }
            }

            SaveUtils.Save();
        }

        // Token: 0x06000BCD RID: 3021 RVA: 0x0004DD28 File Offset: 0x0004BF28
        private void OpenPuzzle()
        {
            Rect clientRect = new Rect(440f, 20f, 200f, 400f);
            GUI.Window(3, clientRect, new GUI.WindowFunction(this.DoPuzzle), "Puzzle menu");
        }

        // Token: 0x06000BCE RID: 3022 RVA: 0x0004DD6C File Offset: 0x0004BF6C
        private void DoPuzzle(int windowID)
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            this.newMoves = GUILayout.TextField(this.newMoves, 10, new GUILayoutOption[0]);
            if (GUILayout.Button("ChangeMoves", new GUILayoutOption[0]))
            {
                GameManager.System.Puzzle.Game.SetResourceValue(PuzzleGameResourceType.MOVES, int.Parse(this.newMoves),
                    true);
                this.debugLog.AddMessage("Moves changed to: " + this.newMoves);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            this.newAffection = GUILayout.TextField(this.newAffection, 10, new GUILayoutOption[0]);
            if (GUILayout.Button("ChangeAffection", new GUILayoutOption[0]))
            {
                GameManager.System.Puzzle.Game.SetResourceValue(PuzzleGameResourceType.AFFECTION,
                    int.Parse(this.newAffection), true);
                this.debugLog.AddMessage("Affection changed to: " + this.newAffection);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            this.newPassion = GUILayout.TextField(this.newPassion, 10, new GUILayoutOption[0]);
            if (GUILayout.Button("ChangePassion", new GUILayoutOption[0]))
            {
                GameManager.System.Puzzle.Game.SetResourceValue(PuzzleGameResourceType.PASSION, int.Parse(this.newPassion),
                    true);
                this.debugLog.AddMessage("Passion changed to: " + this.newPassion);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            this.newSentiment = GUILayout.TextField(this.newSentiment, 10, new GUILayoutOption[0]);
            if (GUILayout.Button("ChangeSentiment", new GUILayoutOption[0]))
            {
                GameManager.System.Puzzle.Game.SetResourceValue(PuzzleGameResourceType.SENTIMENT,
                    int.Parse(this.newSentiment), true);
                this.debugLog.AddMessage("Sentiment changed to: " + this.newSentiment);
            }

            GUILayout.EndHorizontal();
            if (GUILayout.Button("NoDrain", new GUILayoutOption[0]))
            {
                Paranoia.noDrain = !Paranoia.noDrain;
                this.debugLog.AddMessage("NoDrain now: " + Paranoia.noDrain.ToString());
            }
        }

        private void OpenGirl()
        {
            Rect clientRect = new Rect(240f, 420f, 200f, 400f);
            GUI.Window(4, clientRect, new GUI.WindowFunction(this.DoGirl), "Girl menu");
        }

        private void DoGirl(int windowId)
        {
            if (GUILayout.Button("Naked", new GUILayoutOption[0]))
            {
                var girl = GameManager.Stage.girl;
                var girlDefinition = girl.definition;
                var girlPlayerData = GameManager.System.Player.GetGirlData(girlDefinition);
                
                Naked(girl, girlDefinition);

                this.debugLog.AddMessage("Girl is now naked hihi");
            }

            if (GUILayout.Button("Hairstyle", new GUILayoutOption[0]))
            {
                var girl = GameManager.Stage.girl;
                var girlDefinition = girl.definition;
                var girlPlayerData = GameManager.System.Player.GetGirlData(girlDefinition);

                ChangeHairStyle(i, girl, girlDefinition, girlPlayerData);
                this.debugLog.AddMessage("Changed curr girl hairstyle to: " + i);

                i += 1;
                if (i >= 5) i = 0;
            }
        }

        private int i = 0;
        
        private void ChangePiece(GirlPieceArt pieceArt, DisplayObject container, Girl currGirl)
        {
            container.RemoveAllChildren(true);

            SpriteObject fronthairSpriteObject =
                DisplayUtils.CreateSpriteObject(currGirl.spriteCollection, pieceArt.spriteName, "SpriteObject");
            container.AddChild(fronthairSpriteObject);

            if (currGirl.flip)
            {
                fronthairSpriteObject.sprite.FlipX = true;
                fronthairSpriteObject.SetLocalPosition((float)(1200 - pieceArt.x), (float)(-(float)pieceArt.y));
            }
            else
            {
                fronthairSpriteObject.SetLocalPosition((float)pieceArt.x, (float)(-(float)pieceArt.y));
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

        private void ChangeHairStyle(int id, Girl currGirl, GirlDefinition currGirlDef, GirlPlayerData currGirlPlayerData)
        {
            var currGirlPiece = currGirlDef.pieces[currGirlDef.hairstyles[id].artIndex];
            
            ChangePiece(currGirlPiece.primaryArt, currGirl.fronthair, currGirl);
            ChangePiece(currGirlPiece.secondaryArt, currGirl.backhair, currGirl);
        }
    }
}