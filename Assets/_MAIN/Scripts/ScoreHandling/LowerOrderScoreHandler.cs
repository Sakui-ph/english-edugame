using UnityEngine;

public static class LowerOrderScoreHandler
{
    private static int score = 0;
    private static int total = 0;

    public static void CheckAnswer(bool answer, bool expected)
    {
        if (answer != expected) {
            Debug.Log("Incorrect!");
            Incorrect();
            return;
        }
        Debug.Log("Correct!");
        Correct();
    }

    private static void Correct()
    {
        total++;
        score++;
    }

    public static float GetPercentage()
    {
        return (float)score / (float)total;
    }

    private static void Incorrect()
    {
        total++;
    }

    public static void Reset()
    {
        score = 0;
        total = 0;
    }
}