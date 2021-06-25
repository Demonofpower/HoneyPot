using System;
using System.Collections.Generic;
using HoneyPot.Menus;
using UnityEngine;

namespace HoneyPot
{
    public class Paranoia : MonoBehaviour
    {
        public static bool noDrain;
        
        private DebugLog debugLog;

        private PlayerMenu playerMenu;
        
        private bool isDebugOpen;
        private bool isGirlOpen;

        private bool isMenuOpen;
        private bool isPlayerOpen;
        private bool isPuzzleOpen;
        private bool isSceneOpen;
        private string newAffection;

        private string newMoves;
        private string newPassion;
        private string newSentiment;


        private SelectionManager selectionManager;

        public void Start()
        {
            debugLog = new DebugLog();
            selectionManager = new SelectionManager();

            playerMenu = new PlayerMenu(debugLog);
            
            newMoves = "0";
            newAffection = "0";
            newPassion = "0";
            newSentiment = "0";
            noDrain = false;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                isMenuOpen = !isMenuOpen;
                isPlayerOpen = false;
                isPuzzleOpen = false;
                isGirlOpen = false;
                isSceneOpen = false;
                debugLog.AddMessage("Menu opened/closed");
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                GameManager.Stage.girl.HideBra();
                GameManager.Stage.girl.ChangeExpression(GirlExpressionType.HORNY, true, true, true, 0f);
            }
        }

        public void OnGUI()
        {
            GUI.contentColor = Color.magenta;
            GUI.Label(new Rect(0f, 30f, 150f, 50f), "HoneyPot");
            GUI.contentColor = Color.white;
            GUI.Label(new Rect(0f, 50f, 150f, 50f), "Press F1 for Menu");
            GUI.contentColor = Color.red;
            GUI.Label(new Rect(0f, 70f, 150f, 50f), "By Paranoia with <3");
            if (isMenuOpen)
            {
                OpenMenu();
            }

            if (isDebugOpen)
            {
                OpenDebug();
            }

            if (selectionManager.IsSelectionOpen)
            {
                OpenSelection();
            }

            if (isPlayerOpen)
            {
                OpenPlayer();
            }

            if (isPuzzleOpen)
            {
                OpenPuzzle();
            }

            if (isGirlOpen)
            {
                OpenGirl();
            }

            if (isSceneOpen)
            {
                OpenScene();
            }
        }

        private void OpenMenu()
        {
            var clientRect = new Rect(120f, 20f, 120f, 150f);
            GUI.Window(0, clientRect, DoMenu, "Cool Menu");
        }

        private void DoMenu(int windowID)
        {
            if (GUILayout.Button("debug log"))
            {
                isDebugOpen = !isDebugOpen;
                debugLog.AddMessage("Debug opened/closed");
            }

            if (GUILayout.Button("Player Menu"))
            {
                isPlayerOpen = !isPlayerOpen;
                debugLog.AddMessage("Player opened/closed");
            }

            if (GUILayout.Button("Puzzle Menu"))
            {
                isPuzzleOpen = !isPuzzleOpen;
                debugLog.AddMessage("Puzzle opened/closed");
            }

            if (GUILayout.Button("Girl Menu"))
            {
                isGirlOpen = !isGirlOpen;
                debugLog.AddMessage("Girl opened/closed");
            }

            if (GUILayout.Button("Scene Menu"))
            {
                isSceneOpen = !isSceneOpen;
                debugLog.AddMessage("Scene opened/closed");
            }
        }

        private void OpenDebug()
        {
            var clientRect = new Rect(640f, 20f, 500f, 400f);
            GUI.Window(1, clientRect, DoDebugLog, "Debug log");
        }

        private void DoDebugLog(int windowID)
        {
            foreach (var text in debugLog.PrintLastMessages()) GUILayout.Label(text);
        }

        private void OpenSelection()
        {
            var clientRect = new Rect(550f, 420f, 400f, 400f);
            GUI.Window(421, clientRect, selectionManager.DoSelection, "Selection");
        }
        
        private void OpenPlayer()
        {
            var clientRect = new Rect(240f, 20f, 200f, 400f);
            GUI.Window(2, clientRect, playerMenu.DoPlayer, "Player menu");
        }

        private void OpenPuzzle()
        {
            var clientRect = new Rect(440f, 20f, 200f, 400f);
            GUI.Window(3, clientRect, DoPuzzle, "Puzzle menu");
        }
        
        private void DoPuzzle(int windowID)
        {
            GUILayout.BeginHorizontal();
            newMoves = GUILayout.TextField(newMoves, 10);
            if (GUILayout.Button("ChangeMoves"))
            {
                GameManager.System.Puzzle.Game.SetResourceValue(PuzzleGameResourceType.MOVES, int.Parse(newMoves));
                debugLog.AddMessage("Moves changed to: " + newMoves);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            newAffection = GUILayout.TextField(newAffection, 10);
            if (GUILayout.Button("ChangeAffection"))
            {
                GameManager.System.Puzzle.Game.SetResourceValue(PuzzleGameResourceType.AFFECTION,
                    int.Parse(newAffection));
                debugLog.AddMessage("Affection changed to: " + newAffection);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            newPassion = GUILayout.TextField(newPassion, 10);
            if (GUILayout.Button("ChangePassion"))
            {
                GameManager.System.Puzzle.Game.SetResourceValue(PuzzleGameResourceType.PASSION,
                    int.Parse(newPassion));
                debugLog.AddMessage("Passion changed to: " + newPassion);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            newSentiment = GUILayout.TextField(newSentiment, 10);
            if (GUILayout.Button("ChangeSentiment"))
            {
                GameManager.System.Puzzle.Game.SetResourceValue(PuzzleGameResourceType.SENTIMENT,
                    int.Parse(newSentiment));
                debugLog.AddMessage("Sentiment changed to: " + newSentiment);
            }

            GUILayout.EndHorizontal();
            if (GUILayout.Button("NoDrain"))
            {
                noDrain = !noDrain;
                debugLog.AddMessage("NoDrain now: " + noDrain);
            }
        }

        private void OpenGirl()
        {
            var clientRect = new Rect(240f, 420f, 200f, 400f);
            GUI.Window(4, clientRect, DoGirl, "Girl menu");
        }

        private void DoGirl(int windowId)
        {
            var allGirls = GameManager.Data.Girls.GetAll();

            var girl = GameManager.Stage.girl;
            var girlDefinition = girl.definition;
            var girlPlayerData = GameManager.System.Player.GetGirlData(girlDefinition);

            if (GUILayout.Button("Naked"))
            {
                Naked(girl, girlDefinition);

                debugLog.AddMessage("Girl is now naked hihi");
            }

            if (GUILayout.Button("Hairstyle"))
            {
                var hairstyleNames = new List<string>();
                foreach (var girlDefinitionHairstyle in girlDefinition.hairstyles)
                    hairstyleNames.Add(girlDefinitionHairstyle.styleName);

                selectionManager.NewSelection(hairstyleNames, 3, () =>
                {
                    ChangeHairstyle(selectionManager.SelectionId, girl, girlDefinition);
                    debugLog.AddMessage("Changed curr girl hairstyle to: " + hairstyleNames[selectionManager.SelectionId]);
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

            if (GUILayout.Button("Test"))
            {
                var scenes = GameManager.Data.DialogScenes;

                var dialogManager = GameManager.System.Dialog;


                var _definitions = new Dictionary<int, DialogSceneDefinition>();

                var array =
                    Resources.FindObjectsOfTypeAll(typeof(DialogSceneDefinition)) as DialogSceneDefinition[];
                for (var i = 0; i < array.Length; i++) _definitions.Add(array[i].id, array[i]);

                DialogSceneDefinition sceneX = null;

                foreach (var scene in _definitions.Values)
                {
                    debugLog.AddMessage("-SCENE-");
                    debugLog.AddMessage(scene.id.ToString());
                    debugLog.AddMessage(scene.name);
                    debugLog.AddMessage(scene.editorFromJsonString);

                    if (scene.id == 4) sceneX = scene;
                }

                dialogManager.PlayDialogScene(sceneX);
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

        private void OpenScene()
        {
            var clientRect = new Rect(440f, 420f, 200f, 400f);
            GUI.Window(125, clientRect, DoScene, "Scene menu");
        }

        private void DoScene(int windowId)
        {
            var locationManager = GameManager.System.Location;
            var LocationDefinitions = new List<LocationDefinition>();

            LocationDefinition[] locations =
                Resources.FindObjectsOfTypeAll(typeof(LocationDefinition)) as LocationDefinition[];
            for (int i = 0; i < locations.Length; i++)
            {
                LocationDefinitions.Add(locations[i]);
            }

            var allGirls = GameManager.Data.Girls.GetAll();
            var currGirl = GameManager.Stage.girl.definition;

            if (GUILayout.Button("Travel"))
            {
                var sceneNames = new List<string>();
                foreach (var locationDefinition in LocationDefinitions)
                {
                    if (locationDefinition.type == LocationType.NORMAL)
                    {
                        sceneNames.Add(locationDefinition.fullName);
                    }
                }

                selectionManager.NewSelection(sceneNames, 3, () =>
                {
                    var name = sceneNames[selectionManager.SelectionId];
                    foreach (var locationDefinition in LocationDefinitions)
                    {
                        if (locationDefinition.fullName == name)
                        {
                            locationManager.TravelTo(locationDefinition, currGirl);
                            break;
                        }
                    }


                    debugLog.AddMessage("Traveled to " + name);
                });
            }

            if (GUILayout.Button("Background"))
            {
                var sceneNames = new List<string>();
                foreach (var locationDefinition in LocationDefinitions)
                {
                    sceneNames.Add(locationDefinition.fullName);
                }

                selectionManager.NewSelection(sceneNames, 3, () =>
                {
                    var name = sceneNames[selectionManager.SelectionId];
                    foreach (var locationDefinition in LocationDefinitions)
                    {
                        if (locationDefinition.fullName == name)
                        {
                            GameManager.System.Location.currentLocation = locationDefinition;
                            GameManager.Stage.background.UpdateLocation();
                            break;
                        }
                    }


                    debugLog.AddMessage("Scene changed to " + name);
                });
            }
        }
    }
}