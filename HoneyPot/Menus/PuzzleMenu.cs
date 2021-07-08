using HoneyPot.Debug;
using UnityEngine;

namespace HoneyPot.Menus
{
    class PuzzleMenu
    {
        private readonly DebugLog debugLog;

        private string newMoves;
        private string newAffection;
        private string newPassion;
        private string newSentiment;

        private bool noDrain;

        public PuzzleMenu(DebugLog debugLog)
        {
            this.debugLog = debugLog;

            newMoves = "0";
            newAffection = "0";
            newPassion = "0";
            newSentiment = "0";

            noDrain = false;
        }
        
        public void DoPuzzle(int windowID)
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
    }
}
