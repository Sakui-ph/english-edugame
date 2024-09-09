using System.Collections.Generic;
using System.Linq;
using CARD_GAME;
using DIALOGUE;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Level> levels;
    public MultipleChoiceButton[] lowerOrderLevels = new MultipleChoiceButton[5];
    public MultipleChoiceButton[] higherOrderLevels = new MultipleChoiceButton[5];
    public GameObject[] levelData;
    public GameObject[] scoreData;

    void Awake()
    {
        levelData = GameObject.FindGameObjectsWithTag(LevelTags.LevelName.ToString());
        scoreData = GameObject.FindGameObjectsWithTag(LevelTags.ScoreData.ToString());
    }

    void Start()
    {
        // Initialize Official Levels
        levels = new()
        {
            { new("Trial 1: Cat adoption!", "Level1", LevelType.ClassTrial) },
            { new("Trial 2: Tris and... whose crush?!", "Level2", LevelType.ClassTrial) },
            { new("Trial 3: The star player", "Level3", LevelType.ClassTrial) },
            { new("Trial 4: Internet offense", "Level4", LevelType.ClassTrial) },
            { new("Trial 5: The working student", "Level5", LevelType.ClassTrial) },
            { new("Verdict 1: The school's cat adoption debate", "Level6", LevelType.CardGame, "level6_end") },
            { new("Verdict 2: Tris' punishment for the love letter dilemma", "Level7", LevelType.CardGame, "level7_end") },
            { new("Verdict 3: The toxic teammate's gotta go", "Level8", LevelType.CardGame, "level8_end") },
            { new("Verdict 4: Detention due to TikTok", "Level9", LevelType.CardGame, "level9_end") },
            { new("Verdict 5: Considerations for Anjelo", "Level10", LevelType.CardGame, "level10_end") },
        };


        for (int i = 0; i < levels.Count; i++)
        {
            MultipleChoiceButton button = i > 4 ? higherOrderLevels[i % 5] : lowerOrderLevels[i];
            string levelReference = levels[i].levelReference;
            Level level = levels[i];
            button.OnClickAction += () => {
                GameSystem.instance.currentLevel = level;
                if (level.levelType == LevelType.ClassTrial)
                {
                    GameSystem.instance.LoadVisualNovel(levelReference, EndLevel);
                }
                if (level.levelType == LevelType.CardGame)
                {
                    CardMinigameLevelLoader.postLevelReference = level.postLevelReference;
                    GameSystem.instance.LoadVisualNovel(levelReference);
                }

            };
            GameObject buttonObject = button.gameObject;
            LoadLevelData(buttonObject, levels[i]);
        }
    }

    public void EndLevel()
    {
        GameSystem.instance.currentLevel.EndLevel();
    }

    public void LoadLevelData(GameObject levelObject, Level level)
    {
        Player player = GameSystem.instance.GetLoadedPlayer();
        TextMeshProUGUI[] textmeshArray = levelObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textmesh in textmeshArray)
        {
            if (levelData.Contains(textmesh.gameObject))
            {
                textmesh.text = level.levelName;
            }

            if (scoreData.Contains(textmesh.gameObject))
            {
                if (player.playerScore.ContainsKey(level.levelReference))
                {
                    float score = player.playerScore[level.levelReference] * 100;
                    score = Mathf.Round(score);
                    textmesh.text = $"{score}%";

                }
                else
                    textmesh.text = "-%";
            }
        }
    }

    private enum LevelTags {LevelName, ScoreData}
}
