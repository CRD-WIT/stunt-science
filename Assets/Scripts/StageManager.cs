using UnityEngine;
[System.Serializable]
public class StageManager
{
    string[] levelDifficulties = { "", "easy", "medium", "hard" };
    string currentLevelDifficulty, currentGameLevel;

    public Settings settingUI = new Settings();
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
        PlayerPrefs.SetString("Level", settingUI.GetLevelNames()[level]);
    }
    public int GetLevelNum(string n)
    {
        int i = 0;
        while (settingUI.GetLevelNames()[i] != n)
        {
            i++;
        }
        return i;
    }
    public string GetGameLevel()
    {
        return PlayerPrefs.GetString("Level");
    }
}
