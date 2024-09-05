using System;
using UnityEngine;

namespace DIALOGUE
{
    public class ChapterManager 
    {
        private DialogueSystem ds => VisualNovelSL.services.dialogueSystem;
        public CHAPTER_DATA chapterData;
        public static event Action OnChapterEnd;

        public ChapterManager(string path)
        {
            OnChapterEnd = null;
            chapterData = new(path.ToLower());
        }

        public void QueueFile(string fileName)
        {
            chapterData.QueueFile(fileName.ToLower());
        }

        public void QueueDirectory(string directory)
        {
            chapterData.QueueSubDirectory(directory.ToLower());
        }

        public void QueuePreviousFile(string fileName)
        {
            chapterData.ReturnDirectory();
            QueueFile(fileName);
        }

        public void QueuePreviousDirectory(string directoryName)
        {
            chapterData.ReturnDirectory();
            QueueDirectory(directoryName);
        }

        public void PlayQueuedChapter()
        {
            chapterData.LoadQueuedFile();
            PlayChapter();
        }

        public void PlayChapter()
        {
            if (!isCompleted)
                ds.Say(chapterData.currentDialogue);
            else
            {
                Debug.Log("Story has been completed");
                OnChapterEnd?.Invoke();
                OnChapterEnd = null;
            }
        }

        public bool isCompleted => chapterData.isCompleted;
    }
}
