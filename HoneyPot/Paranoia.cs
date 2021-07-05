using HoneyPot.Menus;
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
        
        private bool isDebugOpen;
        
        private bool isMenuOpen;
        private bool isPlayerOpen;
        private bool isPuzzleOpen;
        private bool isGirlOpen;
        private bool isSceneOpen;

        public static bool IsBlackScreen;

        public void Start()
        {
            debugLog = new DebugLog();
            
            selectionManager = new SelectionManager();

            playerMenu = new PlayerMenu(debugLog);
            puzzleMenu = new PuzzleMenu(debugLog);
            girlMenu = new GirlMenu(debugLog, selectionManager);
            sceneMenu = new SceneMenu(debugLog, selectionManager);

            IsBlackScreen = false;
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

            if (IsBlackScreen)
            {
                MakeScreenBlack();
            }
        }

        private void OpenSelection()
        {
            var clientRect = new Rect(550f, 420f, 400f, 400f);
            GUI.Window(421, clientRect, selectionManager.DoSelection, "Selection");
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