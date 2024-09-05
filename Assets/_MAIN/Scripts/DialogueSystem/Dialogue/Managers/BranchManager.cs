using System;
using UnityEngine;

namespace DIALOGUE
{
    public class BranchManager 
    {
        private DialogueSystem ds => VisualNovelSL.services.dialogueSystem;
        public BRANCH_DATA branchData;
        public static event Action OnChapterEnd;

        public BranchManager(string path)
        {
            OnChapterEnd = null;
            branchData = new(path.ToLower());
        }

        public void QueueStoryFile(string fileName)
        {
            branchData.QueueFile(fileName.ToLower());
        }

        public void QueueBranch(string directory)
        {
            branchData.QueueSubDirectory(directory.ToLower());
        }

        public void QueueStoryFileFromPreviousBranch(string fileName)
        {
            branchData.ReturnDirectory();
            QueueStoryFile(fileName);
        }

        public void QueueBranchFromPreviousBranch(string directoryName)
        {
            branchData.ReturnDirectory();
            QueueBranch(directoryName);
        }

        public void PlayQueuedBranch()
        {
            branchData.LoadQueuedFile();
            PlayBranch();
        }

        public void PlayBranch()
        {
            if (!isCompleted)
                ds.Say(branchData.currentDialogue);
            else
            {
                Debug.Log("Story has been completed");
                OnChapterEnd?.Invoke();
                OnChapterEnd = null;
            }
        }

        public bool isCompleted => branchData.isCompleted;
    }
}
