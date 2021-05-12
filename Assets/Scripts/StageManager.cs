using UnityEngine;

public class StageManager
{
    public int stage;
    static string[] levelDifficulties = { "", "easy", "medium", "hard" };
    static string[] gameLevel = {"", "Velocity", "Acceleration", "Free Fall", "Projectile", "Circular Motion", "Forces", "Work", "Energy", "Power", "Momemtum", "Impulse"};
    string currentLevelDifficulty, currentGameLevel;

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
        //this.currentLevelDifficulty = levelDifficulties[difficultyLevel];
        PlayerPrefs.SetString("Difficulty Level",levelDifficulties[difficultyLevel]);
    }
    public string GetDifficulty()
    {
        //return this.currentLevelDifficulty;
        return PlayerPrefs.GetString("Difficulty Level");
    }
    public int GetStage()
    {
        return stage;
    }
    public void SetGameLevel(int level){
        //this.currentGameLevel = gameLevel[level];
        PlayerPrefs.SetString("Level",gameLevel[level]);
    }
    public string GetGameLevel(){
        return PlayerPrefs.GetString("Level");
    }
}

