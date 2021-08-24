using UnityEngine;
[System.Serializable]
public class StageManager
{
    string[] levelDifficulties = { "", "easy", "medium", "hard" };
    string[] gameLevel = { "", "Velocity", "Acceleration", "Free Fall", "Projectile Motion", "Circular Motion", "Forces", "Work", "Energy", "Power", "Momemtum" };
    string currentLevelDifficulty, currentGameLevel;

    // public void SetStage(int stageNumber)
    // {
    //     PlayerPrefs.SetInt("stageNumber", stageNumber);
    //     this.stage = stageNumber;
    // }
    public int GetStageFromPlayerPrefs()
    {
        return PlayerPrefs.GetInt("stageNumber");
    }
    public void SetDifficulty(int difficultyLevel)
    {
        //this.currentLevelDifficulty = levelDifficulties[difficultyLevel];
        PlayerPrefs.SetString("Difficulty Level", levelDifficulties[difficultyLevel]);
    }
    public string GetDifficulty()
    {
        //return this.currentLevelDifficulty;
        return PlayerPrefs.GetString("Difficulty Level");
    }
    // public int GetStage()
    // {
    //     return stage;
    // }
    public void SetGameLevel(int level)
    {
        //this.currentGameLevel = gameLevel[level];
        PlayerPrefs.SetString("Level", gameLevel[level]);
    }
    public int GetLevelNum(string n)
    {
        int i = 0;
        while (gameLevel[i] != n)
        {
            i++;
        }
        return i;
    }
    public string GetGameLevel()
    {
        Debug.Log($"Level in DB: {PlayerPrefs.GetString("Level")}");
        return PlayerPrefs.GetString("Level");
    }
}
