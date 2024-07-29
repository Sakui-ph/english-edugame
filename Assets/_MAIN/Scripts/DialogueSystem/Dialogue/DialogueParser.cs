using System.Text.RegularExpressions;


namespace DIALOGUE 
{
    public class DialogueParser
    {
        // Any word followed by a space and then an open parenthesis
        private const string playerNameLocator = "{name}";
        private const string playerSubjectPronounLocator = "{sp}"; // he, she
        private const string playerObjectPronounLocator = "{op}"; // him, her
        private const string playerPossessivePronounLocator = "{pp}"; // his, hers
        private const string playerDataLocatorRegexPattern = @"{name}|{sp}|{op}|{pp}";
        private const string firstWordRegexPattern = @"^([\w\{\}\-\p{P}]+)";
        private const string commandRegexPattern = @"[\w\[\]]*[^\s]\(";
        private const string playerNameSpeakerLocator = "PC";
        public static DIALOGUE_LINE Parse(string rawLine) 
        {
            (string speaker, string dialogue, string commands) = RipContent(rawLine);
            return new DIALOGUE_LINE(speaker, dialogue, commands);
        }

        private static (string, string, string) RipContent(string rawLine) 
        {
            string speaker, dialogue, commands;
            speaker = dialogue = commands = string.Empty;

            int dialogueStart = -1;
            int dialogueEnd = -1;
            bool isEscaped = false;
            
            // get the dialogue start
            for (int i = 0; i < rawLine.Length; i++)
            {
                char current = rawLine[i];
                if (current == '\\')
                {
                    isEscaped = !isEscaped;
                }
                else if (current == '"' && !isEscaped) 
                {
                    if (dialogueStart == -1)
                        dialogueStart = i;
                    else if (dialogueEnd == -1)
                        dialogueEnd = i;
                }
                else 
                    isEscaped = false;
                
            }


            // Match command pattern
            Regex commandRegex = new Regex(commandRegexPattern);
            MatchCollection matches = commandRegex.Matches(rawLine);
            int commandStart = -1;

            foreach (Match match in matches)
            {
                if (match.Index < dialogueStart || match.Index > dialogueEnd)
                {
                    commandStart = match.Index;
                    break;
                }
            }

            if (commandStart != -1 && dialogueStart == -1 && dialogueEnd == -1)
                return("", "", rawLine.Trim());
                


            // If we've found dialogue and possibly commands, we need to make sure commands come after the dialogue ending
            if(dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
            {
                speaker = rawLine.Substring(0, dialogueStart).Trim();
                dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1).Replace("\\\"", "\"");

                dialogue = FindAndReplaceNameAndPronouns(dialogue);

                if(commandStart != -1) 
                {
                    commands = rawLine.Substring(commandStart).Trim();
                }
                
            }
            else if(commandStart != -1 && dialogueStart > commandStart)
            {
                commands = rawLine;
            }
            else
                // If there is no formatting at all, we just assume its speaker data
                speaker = rawLine;
            
            if (speaker == playerNameSpeakerLocator)
                speaker = GameSystem.instance.GetLoadedPlayer().playerName;
            return (speaker, dialogue, commands);
        }

        private static string FindAndReplaceNameAndPronouns(string dialogue)
        {
            bool capitalize = false;
            string resultText = "";
            int lastIndex = 0;

            // somehow do all the possible things but don't forget to handle capitalization

            Regex pattern = new Regex(playerDataLocatorRegexPattern);
            MatchCollection matches = pattern.Matches(dialogue);

            if (matches.Count == 0)
                return dialogue;

            foreach (Match match in matches)
            {
                if (lastIndex != match.Index)
                    resultText += dialogue[lastIndex..match.Index];
                string matchSubstring = dialogue.Substring(match.Index);

                capitalize = CapitalizationCheck(dialogue, match.Index);
                
                if (match != matches[matches.Count - 1])
                {
                    
                    var result = Regex.Match(matchSubstring, firstWordRegexPattern);
                    matchSubstring = result.Value;

                    lastIndex = match.Index + matchSubstring.Length;
                    
                    resultText += ReplaceNameAndPronoun(matchSubstring, capitalize);
                }
                else 
                {
                    resultText += ReplaceNameAndPronoun(matchSubstring, capitalize);
                }
            }

            return resultText;
        }

        private static string ReplaceNameAndPronoun(string substring, bool capitalize)
        {
            Player player = GameSystem.instance.GetLoadedPlayer();
            string playerName;
            PlayerGender gender;

            #region Set Player Values
            if (player == null)
            {
                playerName = "John";
                gender = PlayerGender.Male;
            } 
            else {
                playerName = player.playerName;
                gender = player.playerGender;
            }
            #endregion

            if (substring.StartsWith(playerNameLocator))
            {   
                substring = substring.Remove(0, playerNameLocator.Length);
                substring = substring.Insert(0, playerName);
                return substring;
            }
            

            string pronounText = "";
            
            if (substring.StartsWith(playerObjectPronounLocator))
            {
                substring = substring.Remove(0, playerObjectPronounLocator.Length);
                pronounText = gender == PlayerGender.Male ? "him" : "her";
            }

            if (substring.StartsWith(playerPossessivePronounLocator))
            {
                substring = substring.Remove(0, playerPossessivePronounLocator.Length);
                pronounText = gender == PlayerGender.Male ? "his" : "hers";
            }

            if (substring.StartsWith(playerSubjectPronounLocator))
            {
                substring = substring.Remove(0, playerSubjectPronounLocator.Length);
                pronounText = gender == PlayerGender.Male ? "he" : "she";
            }

            if (pronounText == "")
            {
                return substring;
            }

            if (capitalize) pronounText = Capitalize(pronounText);
                
            substring = substring.Insert(0, pronounText);


            return substring;
        }

        private static string Capitalize(string text)
        {
            string result = char.ToUpper(text[0]) + text.Substring(1).ToLower();
            return result; 
        }

        private static bool CapitalizationCheck(string text, int index)
        {
            if (index - 2 < 0 || text[index - 2] == '.')
            {
                return true;
            }

            return false;
        }
    }
}