using System;
using System.Collections.Generic;
using DIALOGUE;

namespace DIALOGUE
{
    public static class HistoryData
    {
        private static List<DIALOGUE_LINE> finishedLines = new();
        public static event Action<DIALOGUE_LINE> onAddLine;
        
        public static List<DIALOGUE_LINE> GetFinishedLines() => finishedLines;
        public static void AddFinishedLine(DIALOGUE_LINE line) 
        {
            finishedLines.Add(line);
            onAddLine?.Invoke(line);
        }
        public static void Reset() 
        {
            finishedLines = new();
            onAddLine = null;
        } 
    }
}

