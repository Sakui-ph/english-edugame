using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

namespace DIALOGUE
{
    // Todo: this is super hard to read
    public class BranchLoader
    {
        // This is literally the only one we need to have here
        public List<string> currentDialogue;
        public int queuedFile;
        public List<string> fileNames;
        public string currentPath;
        public string currentDirectory;
        private string returnDirectory;
        private string subPath;
        private int current_index = 0;
        public bool isCompleted = false;


        public BranchLoader(string levelDirectory)
        {
            QueueMainDirectory(levelDirectory);
        }
        private void QueueMainDirectory(string directory)
        {
            ResetAttributes();
            this.currentDirectory = directory;
            this.currentPath = GetPathToBranch(directory);
            if (GetFileNamesList(directory))
            {
                subPath = $"{directory}";
                LoadFileAtIndex();
            }
        }

        public void LoadFileAtIndex()
        {
            if (current_index >= fileNames.Count)
            {
                isCompleted = true;
                return;
            }

            if (current_index <= -1 || current_index > fileNames.Count)
            {
                Debug.LogError($"Out of bounds! Exited out of nowhere! Current index is {current_index} and filename count is {fileNames.Count}");
                GameSystemSL.services.gameSystem.LoadMainMenu();
                return;
            }

            currentDialogue = FileManager.ReadTextAsset(FilePaths.branch_files + $"{subPath}/{fileNames[current_index]}");
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

        public void QueueSubDirectory(string subdirectory)
        {
            returnDirectory = currentPath;
            ResetAttributes();

            currentPath += $"/{subdirectory}";
            if (GetFileNamesList(subPath + $"/{subdirectory}"))
            {
                subPath += $"/{subdirectory}";
            }
        }

        public void ReturnDirectory()
        {
            ResetAttributes();
            currentPath = returnDirectory;
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
                return GetBranchesFromStreamingAssets(path);
            #else
                return GetBranchesFromDefaultFolder(path);
            #endif
        }

        private bool GetBranchesFromDefaultFolder(string path)
        {
            try {
                    path = GetPathToBranch(path);
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
        }

        private bool GetBranchesFromStreamingAssets(string directory)
        {
            try
            {
                string folderPath = FilePaths.better_stream_assets_levels + directory;
                fileNames = BetterStreamingAssets.GetFiles(folderPath, "*.txt").Select<string, string>(Path.GetFileNameWithoutExtension)
                            .ToList<string>();
                return true;
            }
            catch
            {
                Debug.Log("Could not load main levels!");
            }
            return false;
        }

        private string GetPathToBranch(string branchName)
        {
            string defaultPath = FilePaths.stream_assets_branch;
            if (branchName.StartsWith(FilePaths.HOME_DIRECTORY_SYMBOL))
            {
                return branchName.Substring(FilePaths.HOME_DIRECTORY_SYMBOL.Length);
            }
            return defaultPath + branchName;
        }

        private void ResetAttributes()
        {
            fileNames = new();
            this.queuedFile = 0;
            this.current_index = 0;
            isCompleted = false;
        }
    }
}
