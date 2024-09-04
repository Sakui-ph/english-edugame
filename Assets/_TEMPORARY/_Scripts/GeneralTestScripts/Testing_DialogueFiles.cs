using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace TESTING 
{
    public class Testing_DialogueFiles : MonoBehaviour
    {
        public List<TextAsset> dialogueFiles = new List<TextAsset>();
        void Start()
        {
            StartCoroutine(RunConversations());
        }

        IEnumerator StartConversation(TextAsset file)
        {
            List<string> lines = FileManager.ReadTextAsset(file);
            
            VisualNovelSL.services.dialogueSystem.Say(lines);
            while (VisualNovelSL.services.dialogueSystem.isRunningConversation)
                yield return null;
        }

        IEnumerator RunConversations()
        {
            foreach (TextAsset file in dialogueFiles)
            {
                List<string> lines = FileManager.ReadTextAsset(file);
                yield return StartConversation(file);
            }
        }
    }
}