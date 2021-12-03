using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;

public class LvlFiveHardManager : MonoBehaviour
{
    UnitOf whatIsAsk;
    [SerializeField]float distance, velocity, aVelocity, radius, time;
    int stage;
    string playerName, playerGender, pronoun, pPronoun;
    MechaManager mm;
    QuestionControllerVThree qc;
    IndicatorManagerV1_1 indicators;
    PlayerCM2 myPlayer;
    StageManager sm = new StageManager();
    
    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        mm = FindObjectOfType<MechaManager>();
        indicators = FindObjectOfType<IndicatorManagerV1_1>();
        myPlayer = FindObjectOfType<PlayerCM2>();

        sm.SetGameLevel(5);
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        if (playerGender == "Male")
        {
            pronoun = "he";
            pPronoun = "his";
        }
        else
        {
            pronoun = "she";
            pPronoun = "her";
        }
        qc.levelDifficulty = Difficulty.Medium;

        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        mm.SetMechaVelocity(aVelocity, time);
    }
    void SetUp(){
        radius = 1.05f;
        switch(stage){
            case 1:
                //player velocity to jump exactly to the mecha
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
    //1st gear d =1.18, 2nd gear d= 1.55, 3rd gear d =1.15
}
