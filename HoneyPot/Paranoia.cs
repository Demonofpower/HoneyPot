using System.Collections.Generic;
using System.Threading;
using HoneyPot.Debug;
using HoneyPot.DebugUtil;
using HoneyPot.Load;
using HoneyPot.Menus;
using HoneyPot.Scene;
using HoneyPot.Scene.Helper;
using HoneyPot.Scene.Steps;
using UnityEngine;

namespace HoneyPot
{
    public class Paranoia : MonoBehaviour
    {
        private DebugLog debugLog;
        
        private SelectionManager selectionManager;
        
        private PlayerMenu playerMenu;
        private PuzzleMenu puzzleMenu;
        private GirlMenu girlMenu;
        private SceneMenu sceneMenu;
        private MiscMenu miscMenu;
        
        private bool isDebugOpen;
        
        private bool isMenuOpen;
        private bool isPlayerOpen;
        private bool isPuzzleOpen;
        private bool isGirlOpen;
        private bool isSceneOpen;
        private bool isMiscOpen;

        private static StepFinishedEventHandler stepFinishedEvent;
        public static bool isBlackScreen;
        private static int blackScreenCounter = 300;
        public static bool isSpeaking;
        private static int speakingCounter = 150;

        public static void ActivateBlackScreen(int duration, StepFinishedEventHandler stepFinishedEvent = null)
        {
            blackScreenCounter = duration;
            isBlackScreen = true;
            Paranoia.stepFinishedEvent = stepFinishedEvent;
        }
        public static void ActivateIsSpeaking(int duration, StepFinishedEventHandler stepFinishedEvent = null)
        {
            speakingCounter = duration;
            isSpeaking = true;
            Paranoia.stepFinishedEvent = stepFinishedEvent;
        }

        public void Start()
        {
            debugLog = new DebugLog();
            
            selectionManager = new SelectionManager();

            playerMenu = new PlayerMenu(debugLog);
            puzzleMenu = new PuzzleMenu(debugLog);
            girlMenu = new GirlMenu(debugLog, selectionManager);
            sceneMenu = new SceneMenu(debugLog, selectionManager);
            miscMenu = new MiscMenu(debugLog);

            isBlackScreen = false;
            isSpeaking = false;

            new LoadManager(debugLog).LoadAll();
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
                isMiscOpen = false;
                debugLog.AddMessage("Menu opened/closed");
            }

            if (ScenePlayer.Instance != null)
            {
                ScenePlayer.Instance.Update();
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

            if (isMiscOpen)
            {
                OpenMisc();
            }

            if (isBlackScreen)
            {
                blackScreenCounter -= 1;
                if (blackScreenCounter <= 0)
                {
                    blackScreenCounter = 300;
                    isBlackScreen = false;
                    stepFinishedEvent?.Invoke();
                }
                
                MakeScreenBlack();
            }

            if (isSpeaking)
            {
                speakingCounter -= 1;
                if (speakingCounter <= 0)
                {
                    speakingCounter = 150;
                    isSpeaking = false;
                    stepFinishedEvent?.Invoke();
                }
            }
        }
        
        
        private void OpenSelection()
        {
            var clientRect = new Rect(640f, 420f, 400f, 400f);
            GUI.Window(421, clientRect, selectionManager.DoSelection, "Selection");
        }

        private void OpenMenu()
        {
            var clientRect = new Rect(120f, 20f, 120f, 170f);
            GUI.Window(0, clientRect, DoMenu, "Cool Menu");
        }

        private void DoMenu(int windowID)
        {
            if (GUILayout.Button("Debug log"))
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
            
            if (GUILayout.Button("Other"))
            {
                isMiscOpen = !isMiscOpen;
                debugLog.AddMessage("Other opened/closed");
            }
        }

        private void OpenDebug()
        {
            var clientRect = new Rect(640f, 20f, 500f, 400f);
            GUI.Window(1, clientRect, debugLog.DoDebugLog, "Debug log");
        }
        
        private void OpenPlayer()
        {
            var clientRect = new Rect(240f, 20f, 200f, 400f);
            GUI.Window(2, clientRect, playerMenu.DoPlayer, "Player menu");
        }

        private void OpenPuzzle()
        {
            var clientRect = new Rect(440f, 20f, 200f, 400f);
            GUI.Window(3, clientRect, puzzleMenu.DoPuzzle, "Puzzle menu");
        }
        
        private void OpenGirl()
        {
            var clientRect = new Rect(240f, 420f, 200f, 400f);
            GUI.Window(4, clientRect, girlMenu.DoGirl, "Girl menu");
        }

        private void OpenScene()
        {
            var clientRect = new Rect(440f, 420f, 200f, 400f);
            GUI.Window(125, clientRect, sceneMenu.DoScene, "Scene menu");
        }

        private void OpenMisc()
        {
            var clientRect = new Rect(0f, 420f, 200f, 400f);
            GUI.Window(162, clientRect, miscMenu.DoMisc, "Misc menu");
        }

        private void MakeScreenBlack()
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.black);
            texture.Apply();
            GUI.skin.box.normal.background = texture;
            GUI.Box(new Rect(0, 0, 3000, 3000), GUIContent.none);
        }
    }
}