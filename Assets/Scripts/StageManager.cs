using UnityEngine;

public class StageManager
{
    public int stage;

    static string[] levelDifficulties = { "easy", "medium", "hard" };
    static string[] gameLevel = {"velocity", "acceleration", "free fall", "projectile", "circular motion", "forces", "work", "energy", "power", "momemtum", "impulse"};

    string currentLevelDifficulty = levelDifficulties[0];


    public void SetStage(int stageNumber)
    {
        PlayerPrefs.SetInt("stageNumber", stageNumber);
        this.stage = stageNumber;
    }

    public int GetStageFromPlayerPrefs()
    {
        return PlayerPrefs.GetInt("stageNumber");
    }

    public void SetDifficulty(int difficultyLevel)
    {
        this.currentLevelDifficulty = levelDifficulties[difficultyLevel];
    }

    public string GetDifficulty()
    {
        return this.currentLevelDifficulty;
    }

    public int GetStage()
    {
        return stage;
    }

}
