using UnityEngine;

public class StageManager
{
    public int stage;
    static string[] levelDifficulties = { "easy", "medium", "hard" };
    static string[] gameLevel = { "", "Velocity", "Acceleration", "Free Fall", "Projectile", "Circular Motion", "Forces", "Work", "Energy", "Power", "Momemtum", "Impulse" };
    string currentLevelDifficulty = levelDifficulties[0], currentGameLevel = gameLevel[0];

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
    public void SetGameLevel(int level)
    {
        //this.currentGameLevel = gameLevel[level];
        PlayerPrefs.SetString("Level", gameLevel[level]);
    }
    public string GetGameLevel()
    {
        return PlayerPrefs.GetString("Level");
    }
}
