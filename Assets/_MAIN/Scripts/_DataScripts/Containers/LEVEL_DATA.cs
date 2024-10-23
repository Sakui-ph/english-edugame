
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
        isOfficialLevel = level.isOfficialLevel;
        levelNumber = level.levelNumber;
        levelPath = path;
        playerScore = level.playerScore;
        levelType = (LevelType)level.levelType;
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
