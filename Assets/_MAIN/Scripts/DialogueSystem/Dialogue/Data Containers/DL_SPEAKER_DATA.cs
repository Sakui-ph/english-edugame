using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace DIALOGUE 
{
    public class DL_SPEAKER_DATA
    {
        public string name, castName;
        public string displayName => string.IsNullOrWhiteSpace(castName) ? name : castName;
        public Vector2 castPosition;
        public List<(int layer, string expression)> CastExpressions {get; set;}
        public bool hasPositionCast = false;
        public bool hasExpressionCast = false;
        private const string NAMECAST_PATTERN = " as ";
        private const string POSITIONCAST_PATTERN = " at ";
        private const string EXPRESSIONCAST_PATTERN = " [";
        private const char AXIS_SEPARATOR = ':';
        private const char EXPRESSIONLAYER_JOINER = ',';
        private const char EXPRESSIONLAYER_SEPARATOR = ':';



        public DL_SPEAKER_DATA(string rawSpeaker) 
        {
            string pattern = @$"{NAMECAST_PATTERN}|{POSITIONCAST_PATTERN}|{EXPRESSIONCAST_PATTERN.Insert(EXPRESSIONCAST_PATTERN.Length - 1, @"\")}";
            MatchCollection matches = Regex.Matches(rawSpeaker, pattern);

            // populate
            castName = "";
            castPosition = Vector2.Zero;
            CastExpressions = new List<(int Layer, string expression)>();


            if (matches.Count == 0) 
            {
                name = rawSpeaker;
                return;
            }
            int index = matches[0].Index;
            name = rawSpeaker.Substring(0, index);
            
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                string matchValue = match.Value;
                int startIndex = 0, endIndex = 0;

                // Get start and end indices relative to the current match index
                startIndex = match.Index + match.Length;
                endIndex = i + 1 < matches.Count ? matches[i + 1].Index : rawSpeaker.Length;

                if (matchValue == NAMECAST_PATTERN) 
                {  
                    castName = rawSpeaker.Substring(startIndex, endIndex - startIndex);
                }

                if (matchValue == POSITIONCAST_PATTERN)
                {
                    string castPos = rawSpeaker.Substring(startIndex, endIndex - startIndex);
                    hasPositionCast = true;
                    RipPosition(castPos);
                }
                if (matchValue == EXPRESSIONCAST_PATTERN)
                {
                    string castExpression = rawSpeaker.Substring(startIndex, endIndex - startIndex);
                    hasExpressionCast = true;
                    CastExpressions = castExpression.Split(EXPRESSIONLAYER_JOINER)
                    .Select(x => 
                    {
                        char lastChar = x[x.Length - 1];
                        if (lastChar == ']')
                            x = x.Remove(x.Length - 1);
                        string[] split = x.Trim().Split(EXPRESSIONLAYER_SEPARATOR);
                        return (int.Parse(split[0]), split[1]);
                    }).ToList();

                }
            }
        }

        public void RipPosition(string position)
        {
            // Remove empty entries to handle cases where there is an x but no y
            string[] axis = position.Split(AXIS_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
            float.TryParse(axis[0], out castPosition.X);
            if (axis.Length > 1)
                float.TryParse(axis[1], out castPosition.Y);
        }

    }
}