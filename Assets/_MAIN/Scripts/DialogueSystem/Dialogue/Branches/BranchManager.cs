using System;
using UnityEngine;

namespace DIALOGUE
{
    public class BranchManager 
    {
        private DialogueSystem ds => VisualNovelSL.services.dialogueSystem;
        public BranchLoader branchLoader;
        public static event Action OnBranchEnd;

        public BranchManager(string path)
        {
            OnBranchEnd = null;
            branchLoader = new(path.ToLower());
        }

        public void QueueStoryFile(string fileName)
        {
            branchLoader.QueueFile(fileName.ToLower());
        }

        public void QueueBranch(string directory)
        {
            branchLoader.QueueSubDirectory(directory.ToLower());
        }

        public void QueueStoryFileFromPreviousBranch(string fileName)
        {
            branchLoader.ReturnDirectory();
            QueueStoryFile(fileName);
        }

        public void QueueBranchFromPreviousBranch(string directoryName)
        {
            branchLoader.ReturnDirectory();
            QueueBranch(directoryName);
        }

        public void PlayQueuedBranch()
        {
            branchLoader.LoadQueuedFile();
            PlayBranch();
        }

        public void PlayBranch()
        {
            if (!isCompleted)
                ds.Say(branchLoader.currentDialogue);
            else
            {
                Debug.Log("Story has been completed");
                OnBranchEnd?.Invoke();
                OnBranchEnd = null;
            }
        }

        public bool isCompleted => branchLoader.isCompleted;
    }
}
