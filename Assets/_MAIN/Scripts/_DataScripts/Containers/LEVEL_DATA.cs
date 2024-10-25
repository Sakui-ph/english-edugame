
using System;
using System.IO;
using UnityEngine;

public class LEVEL_DATA
{
    public string levelName;
    public int levelId;
    public string levelDescription;
    public bool isOfficialLevel;
    public int levelNumber;
    public string levelPath;
    public int playerScore;
    public LevelType levelType;

    public LEVEL_DATA (LevelData level, int levelId, string path)
    {
        levelName = level.levelName;
        this.levelId = levelId;
        levelDescription = level.levelDescription;
        isOfficialLevel = level.isOfficial;
        levelNumber = level.levelNumber;
        levelPath = path;
        playerScore = level.playerScore;
        levelType = (LevelType)level.levelType;
    }

    public Sprite TryLoadThumbnail()
    {
        string pngFile = Path.Combine(levelPath, "thumbnail.png");
        string jpgFile = Path.Combine(levelPath, "thumbnail.jpg");
        if (File.Exists(jpgFile)) {
            return LoadThumbnail(jpgFile);
        }
        if (File.Exists(pngFile)) {
            return LoadThumbnail(pngFile);
        }
        Debug.LogWarning("No thumbnail found for level " + levelName);
        return null;
    }

    private Sprite LoadThumbnail(string path)
    {
        byte[] imageData;

        if (!File.Exists(path))
        {
            Debug.LogWarning("Level Thumbnail not found, using default thumbnail.");
            return null;
        }
        Texture2D thumbnail = new Texture2D(2,2);
        imageData = File.ReadAllBytes(path);
        if (!thumbnail.LoadImage(imageData))
        {
            Debug.LogWarning("Level Thumbnail could not be loaded!");
            return null;
        }
        Debug.Log("Image successfully loaded");
        Sprite sprite = Sprite.Create(thumbnail, new Rect(0, 0, thumbnail.width, thumbnail.height), new Vector2(thumbnail.width/2, thumbnail.height/2));
        return sprite;
    }

    public override string ToString()
    {
        return $"Name: {levelName} \n Level: {levelId} \n Official? {isOfficialLevel} \n Path: {levelPath}";
    }

    public override bool Equals(object obj)
    {
        if (!(obj is LEVEL_DATA))
            return false;
        LEVEL_DATA otherLevelData = (LEVEL_DATA)obj;
        if (otherLevelData.levelName != this.levelName)
            return false;
        if (otherLevelData.levelId != this.levelId)
            return false;
        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 419;
            hash = hash + 569 + levelName.GetHashCode();
            hash = hash + 569 + levelId.GetHashCode();
            return hash;
        }
    }
}
