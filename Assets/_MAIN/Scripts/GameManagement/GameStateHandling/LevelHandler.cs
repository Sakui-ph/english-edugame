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
    private static Level currentlyRunningLevel;

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
            string levelReference = levels[i].levelChapterReference;
            Level level = levels[i];
            button.OnClickAction += () => {
                currentlyRunningLevel = level;
                if (level.levelType == LevelType.ClassTrial)
                {
                    GameSystem.instance.LoadVisualNovel(levelReference, EndLevel);
                }
                if (level.levelType == LevelType.CardGame)
                {
                    CardMinigameLevelLoader.OnCardGameEnd += EndLevel;
                    GameSystem.instance.LoadVisualNovel(levelReference);
                }

            };
            GameObject buttonObject = button.gameObject;
            LoadLevelData(buttonObject, levels[i]);
        }
    }

    public void EndLevel()
    {
        currentlyRunningLevel.EndLevel();
    }

    public void LoadLevelData(GameObject levelObject, Level level)
    {
        Player player = GameSystem.instance.GetLoadedPlayer();
        TextMeshProUGUI[] tmps = levelObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var tmp in tmps)
        {
            if (levelData.Contains(tmp.gameObject))
            {
                tmp.text = level.levelName;
            }

            if (scoreData.Contains(tmp.gameObject))
            {
                if (player.playerScore.ContainsKey(level.levelChapterReference))
                {
                    float score = player.playerScore[level.levelChapterReference] * 100;
                    score = Mathf.Round(score);
                    tmp.text = $"{score}%";

                }
                else
                    tmp.text = "-%";
            }
        }
    }

    private enum LevelTags {LevelName, ScoreData}
}
