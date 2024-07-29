using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using UnityEngine.Networking;
using Unity.Profiling;
using System.Collections;

namespace DIALOGUE
{
    public class CHAPTER_DATA
    {
        public List<string> currentDialogue;
        public int queuedFile;
        public List<string> fileNames;
        public string path;
        public string directory;
        private string returnDirectory;
        private string subPath;
        private int current_index = 0;
        public bool isCompleted = false;


        public CHAPTER_DATA(string directory)
        {
            QueueMainDirectory(directory);
        }

        public void EndChapter()
        {
            isCompleted = true;
        }

        public void LoadFileAtIndex()
        {
            if (current_index >= fileNames.Count)
            {
                EndChapter();
                return;
            }

            if (current_index <= -1 || current_index > fileNames.Count)
            {
                Debug.LogError($"Out of bounds! Exited out of nowhere! Current index is {current_index} and filename count is {fileNames.Count}");
                GameSystem.instance.LoadMainMenu();
                return;
            }

            currentDialogue = FileManager.ReadTextAsset(FilePaths.chapter_files + $"{subPath}/{fileNames[current_index]}");
            queuedFile = current_index + 1;
        }

        public void LoadQueuedFile()
        {
            current_index = queuedFile;
            LoadFileAtIndex();
        }

        public void QueueFile(string nextFile)
        {
            queuedFile = fileNames.FindIndex(a => nextFile == a);
            if (queuedFile == -1)
            {
                Debug.LogError($"File {nextFile} not found!");
            }
        }

        public void QueueMainDirectory(string directory)
        {
            fileNames = new();
            current_index = 0;
            queuedFile = 0;
            isCompleted = false;


            this.directory = directory;
            this.path = GetPathToChapter(directory);
            if (GetFileNamesList(directory))
            {
                subPath = $"{directory}";
                LoadFileAtIndex();
            }
        }

        public void QueueSubDirectory(string subdirectory)
        {
            returnDirectory = path;
            fileNames = new();
            this.queuedFile = 0;
            this.current_index = 0;
            isCompleted = false;

            path += $"/{subdirectory}";
            if (GetFileNamesList(subPath + $"/{subdirectory}"))
            {
                subPath += $"/{subdirectory}";
            }
        }

        public void ReturnDirectory()
        {
            fileNames = new();
            this.queuedFile = 0;
            this.current_index = 0;
            isCompleted = false;

            path = returnDirectory;
            subPath = ReturnOneDirectory(subPath);
            GetFileNamesList(subPath);
        }
        private string ReturnOneDirectory(string directory)
        {
            char[] reversePath = directory.ToCharArray();
            Array.Reverse(reversePath);
            bool breakInNext = false;
            int removeLength = 0;
            foreach (char c in reversePath)
            {
                if (breakInNext)
                    break;
                if (c == '/')
                    breakInNext = true;
                removeLength++;
                
            }
            
            Array.Reverse(reversePath);
            return new string(reversePath).Substring(0, reversePath.Length-removeLength);
        }

        public bool GetFileNamesList(string path)
        {
            #if UNITY_ANDROID
                GetStreamingChapters(path);
                return true;
            #else
                try {
                    fileNames = Directory.GetFiles(path, "*.txt")
                        .Select<string, string>(Path.GetFileNameWithoutExtension)
                        .ToList<string>();
                    return true;
                }
                catch {
                    Debug.Log("Could not load main levels!");
                    // TODO maybe crash the game and handle this error;
                }
                return false;
            #endif
        }



        private string GetPathToChapter(string chapterName)
        {
            string defaultPath = FilePaths.resource_chapter_files;
            if (chapterName.StartsWith(FilePaths.HOME_DIRECTORY_SYMBOL))
            {
                return chapterName.Substring(FilePaths.HOME_DIRECTORY_SYMBOL.Length);
            }
            return defaultPath + chapterName;
        }

        public void GetStreamingChapters(string directory)
        {
            string folderPath = "chapters/" + directory;
            fileNames = BetterStreamingAssets.GetFiles(folderPath, "*.txt").Select<string, string>(Path.GetFileNameWithoutExtension)
                        .ToList<string>();
        }

    }
}
