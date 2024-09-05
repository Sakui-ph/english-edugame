// NOTE: MOST OF THE CODE FROM THE DIALOGUE SYSTEM IS FROM STELLAR STUDIO'S YOUTUBE CHANNEL TUTORIAL
// CHECK THEM OUT HERE: https://www.youtube.com/@stellarstudio5495

using System;
using System.Collections;
using System.Collections.Generic;
using AUDIO_SYSTEM;
using UnityEngine;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private const string INCONSISTENCY_FLAG = "[I]";
        public DialogueSystem dialogueSystem => VisualNovelSL.services.dialogueSystem;
        private Coroutine process = null;
        public bool isRunning => process != null;
        public TextArchitect architect = null;
        private bool userPrompt = false;
        protected DL_DIALOGUE_DATA currentDialogue;
        public string currentDialogueSentence => JoinSegments(currentDialogue.segments);
        protected List<string> cachedConversation = new List<string>();
        public event Action finishConversation;


        public ConversationManager(TextArchitect architect)
        {
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public void NullifyObservers()
        {
            finishConversation = null;
        }

        public Coroutine StartConversation(List<string> conversation)
        {
            StopConversation();
            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
            return process;
        }

        public void StopConversation()
        {
            if (!isRunning)
                return;

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        public void ResetConversation()
        {
            architect.EmptyBox();
            dialogueSystem.HideSpeakerName();
            CHARACTERS.CharacterManager.instance.ClearCharacters();
            StartConversation(cachedConversation);
        }

        public IEnumerator RunningConversation(List<string> conversation)
        {
            cachedConversation = conversation;
            for (int i = 0; i < conversation.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(conversation[i]) || conversation[i].StartsWith("//"))
                    continue;

                if (conversation[i].Contains("</color>"))
                {
                    AudioManager.instance.PlaySoundEffect("ding", volume: 0.3f);
                }

                if (conversation[i].ToLower().StartsWith("playerwait")){
                    yield return WaitForUserInput();
                    continue;
                }

                // check for inconsistency flag before parsing
                string flaggedConversation = CheckForInconsistencyFlag(conversation[i]);
                DIALOGUE_LINE line = DialogueParser.Parse(flaggedConversation);

                if (line.hasSpeaker)
                    yield return Line_RunSpeaker(line);

                if (line.hasDialogue) {
                    yield return Line_RunDialogue(line);
                        VisualNovelSL.services.historyManager.AddFinishedLine(line);
                }
                    

                if (line.hasCommands)
                    yield return Line_RunCommands(line);

                // if its just a command, we don't need to wait for user input
                if (line.hasDialogue)
                    yield return WaitForUserInput();
            }

            StopConversation();
            finishConversation?.Invoke();
        }

        protected IEnumerator Line_RunSpeaker(DIALOGUE_LINE line)
        {
            DL_SPEAKER_DATA speakerData = line.speakerData;
            dialogueSystem.SetSpeakerData(speakerData);  
            yield return null;
        }

        protected IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            if (line.hasSpeaker)
                dialogueSystem.ShowSpeakerName(line.speakerData.displayName);
            dialogueSystem.ShowDialogueBox();
            currentDialogue = line.dialogueData;
            yield return BuildLineSegments(line.dialogueData);
        }

        protected IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            List<DL_COMMAND_DATA.Command> commands = line.commandData.commands;
            foreach (DL_COMMAND_DATA.Command command in commands)
            {
                string commandName = command.name.ToLower();
                if (command.waitForCompletion || commandName == "wait") 
                    yield return COMMANDS.CommandManager.instance.Execute(commandName, command.args);
                else
                    COMMANDS.CommandManager.instance.Execute(commandName, command.args);
            }
            yield return null;
        }

        protected IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line, TextArchitect architect = null)
        {
            foreach (DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment in line.segments)
            {
                yield return WaitForDialogueSegmentSignalTrigger(segment);
                yield return BuildDialogue(segment.dialogue, segment.appendText, architect);
            }
            
        }

        IEnumerator WaitForDialogueSegmentSignalTrigger(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch (segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                    yield return WaitForUserInput();
                    break;
                default:
                    break;
            }
        }

        public IEnumerator BuildDialogue(string dialogue, bool append = false, TextArchitect otherArchitect = null)
        {
            TextArchitect architect = this.architect;
            if (otherArchitect == null)
                architect = this.architect;
            else
                architect = otherArchitect;

                
            if (append)
                architect.Append(dialogue);
            else
                architect.Build(dialogue);
            while (architect.isBuilding)
            {
                if (userPrompt)
                {
                    if (!architect.hurryUp)
                        architect.hurryUp = true;
                    else
                        architect.ForceComplete();
                }
                userPrompt = false;
                yield return null;
            }
            yield return null;
        }

        protected virtual IEnumerator WaitForUserInput()
        {
            while (!userPrompt)
                yield return null;
            userPrompt = false;
        }

        private string JoinSegments(List<DL_DIALOGUE_DATA.DIALOGUE_SEGMENT> segments)
        {
            string dialogue = "";
            foreach (DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment in segments)
            {
                dialogue += segment.dialogue;
            }
            return dialogue;
        } 

        public string CheckForInconsistencyFlag(string rawText)
        {
            if (rawText[0..4].Contains(INCONSISTENCY_FLAG))
            {
                LowerOrderScoreHandler.isInconsistent = true;
                return rawText.Remove(0, 3);
            }   
            
            return rawText;
        }
    }
}