using System;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace DIALOGUE
{
    public class HistoryManager : MonoBehaviour
    {
        public static event Action<DIALOGUE_LINE> onAddLine;
        private List<DIALOGUE_LINE> finishedLines = new();
        public List<DIALOGUE_LINE> GetFinishedLines() => finishedLines;
        public void AddFinishedLine(DIALOGUE_LINE line) 
        {
            finishedLines.Add(line);
            onAddLine?.Invoke(line);
        }
        public void ResetLines() 
        {
            finishedLines = new();
            onAddLine = null;
        } 
    }
}

