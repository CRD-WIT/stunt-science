using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class StageManager{
   public int stage;

  static string[] levelDifficulties = {"easy", "medium", "hard"};

  string currentLevelDifficulty = levelDifficulties[0];


   public void SetStage(int stageNumber){
       PlayerPrefs.SetInt("stageNumber", stageNumber);
       this.stage = stageNumber;
   }

    public int GetStageFromPlayerPrefs(){
       return PlayerPrefs.GetInt("stageNumber");
   }

   public void SetDifficulty(int difficultyLevel){
       this.currentLevelDifficulty = levelDifficulties[difficultyLevel];
   }

   public string GetDifficulty(){
       return this.currentLevelDifficulty;
   }

   public int GetStage(){
       return stage;
   }

}
