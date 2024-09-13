
public class LEVEL_DATA
{
    string levelName;
    string levelId;
    string levelDescription;
    bool isOfficial;
    int levelNumber;
    string levelPath;

    public LEVEL_DATA (LevelData level, string path)
    {
        levelName = level.levelName;
        levelId = level.levelId;
        levelDescription = level.levelDescription;
        isOfficial = level.isOfficial;
        levelNumber = level.levelNumber;
        levelPath = path;
    }

    public override string ToString()
    {
        return $"Name: {levelName} \n Level: {levelId} \n Official? {isOfficial} \n Path: {levelPath}";
    }
}
