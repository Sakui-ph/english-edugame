using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FileManager
{
    // referenced from absolute files
    public static List<string> ReadTextFile(string filePath, bool includeBlankLines = true)
    {
        //check if not absolute
        if (!filePath.StartsWith('/'))
            filePath = FilePaths.root + filePath;

        List<string> lines = new List<string>();
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while(!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                }
            }
        }
        catch (FileNotFoundException ex) 
        {
            Debug.LogError($"File no found: '{ex.FileName}'");
        }

        return lines;
    }

    // referenced from resources
    public static List<string> ReadTextAsset(string filePath, bool includeBlankLines = true)
    {
        Debug.Log("Reading File = "+ filePath);
        TextAsset asset = Resources.Load<TextAsset>(filePath);
        if(asset == null) 
        {
            // If it doesn't exist as a text asset, we try streaming assets
            return ReadChapterFromStreamingAssets(filePath, includeBlankLines);
        }

        return ReadTextAsset(asset, includeBlankLines);
    }

    public static List<string> ReadTextAsset(TextAsset asset, bool includeBlankLines = true)
    {
        List<string> lines = new List<string>();
            // We use StringReader because TextAsset is just one massive string
        using (StringReader sr = new StringReader(asset.text))
        {
            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();
                if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                    lines.Add(line);
            }
        }

        return lines;
    }

    public static List<string> ReadChapterFromStreamingAssets(string filePath, bool includeBlankLines = true)
    {
        string relativePath = GetRelativePath(filePath, "chapters");
        try{
            List<string> data = BetterStreamingAssets.ReadAllLines($"{relativePath}.txt").ToList();
                    if (includeBlankLines)
            return data;
            data = data.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            return data;
        }
        catch
        {
            GameSystem.instance.LoadMainMenu();
            Debug.Log($"Error, file not found in {filePath}");
            return null;
        }
    }

    public static string GetRelativePath(string fullPath, string baseDirectory)
    {
        string[] fullPathSegments = fullPath.ToLower().Split('/', '\\');

        int index = Array.FindIndex(fullPathSegments, segment => segment.EndsWith(baseDirectory));
        if (index == -1)
        {
            Debug.Log("No base directory found");
            return fullPath;
        }

        return string.Join("/", fullPathSegments.Skip(index));
    }
}