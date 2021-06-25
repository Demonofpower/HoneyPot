using System.Collections.Generic;
using UnityEngine;

namespace HoneyPot
{
    class SelectionManager
    {
        public delegate void SelectionEventHandler();
        
        public int Columns;

        public List<string> Values;

        private int selectionId;

        public bool IsSelectionOpen;

        public SelectionManager()
        {
            Reset();
        }

        private event SelectionEventHandler SelectionEvent;
        
        public void NewSelection(List<string> values, int columns, SelectionEventHandler toExec)
        {
            Reset();
            
            Values = values;
            Columns = columns;

            IsSelectionOpen = true;
            SelectionEvent += toExec;
        }

        private void Reset()
        {
            Values = new List<string>();
            Columns = -1;
            selectionId = -1;
            IsSelectionOpen = false;
            SelectionEvent = null;
        }
        
        public void DoSelection(int windowID)
        {
            var columns = Columns;
            var rows = (Values.Count - 1) / columns + 1;

            for (var i = 0; i < rows; i++)
            {
                GUILayout.BeginHorizontal("i");
                for (var j = 0; j < columns; j++)
                {
                    var currNumber = j + i * columns;

                    if (currNumber >= Values.Count) continue;

                    var currName = Values[currNumber];

                    if (GUILayout.Button(currName)) SelectionId = currNumber;
                }

                GUILayout.EndHorizontal();
            }
        }
        
        public int SelectionId
        {
            get => selectionId;
            set
            {
                selectionId = value;
                IsSelectionOpen = false;

                if (SelectionEvent != null)
                {
                    SelectionEvent.Invoke();
                    foreach (var d in SelectionEvent.GetInvocationList()) SelectionEvent -= (SelectionEventHandler)d;
                }
            }
        }
    }
}
