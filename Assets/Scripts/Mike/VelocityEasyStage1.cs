using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VelocityEasyStage1 : MonoBehaviour
{
    public Text playerNameText, question;
    string pronoun, pPronoun, pNoun, playerName, playerGender;
    public float distance, Time, Speed, elapsed;
    // Start is called before the first frame update

    StageManager sm = new StageManager();

    void Start()
    {
        //prodProps = FindObjectOfType<prodProps>();    
        //tileGenerator = FindObjectOfType<generateGround>();                
        //myPlayer = FindObjectOfType<Player>();
        //playerNameText.text = RegistrationManager.playerName;
        //myPlayer.gameObject.SetActive(true);
        //chance = 0;
        VelocityEasyStage1SetUp();
        //talentFee.text = "TF: " + GameMAnager.talentFee.ToString(); 

       
        sm.SetStage(1);
        int stage = sm.GetStage();
        string difficulty= sm.GetDifficulty();
        sm.GetStageFromPlayerPrefs();

        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");

        string p = $"Name: <color color=green>{playerName}</color>";
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(SimulationManager.playerAnswer);
    }
    public void VelocityEasyStage1SetUp(){
        if(playerGender == "Male"){
            pronoun = "he";
            pPronoun = "him";
            pNoun = "his";
        }else{
            pronoun = "she";
            pPronoun = "her";
            pNoun = "her";}

        while((Speed < 1.5)||(Speed > 10)){
            distance = Random.Range(5, 10);
            float rTime = Random.Range(4f, 9f);
            Time = (float)System.Math.Round(rTime,2);
            Speed = (float)System.Math.Round((distance/rTime), 2);            
            elapsed=0;
            SimulationManager.isSimulating =false;
            //myPlayer.transform.position = new Vector2(20, -0.7f);
            //startingPoint = myPlayer.transform.position;
        }      
    SimulationManager.question = $"The ceiling is crumbling and the safe area is "+distance.ToString()+"m away from "+playerName+". If "+pronoun+" has exactly "+Time.ToString()+"s to go to the safe spot, what should be "+pNoun+" velocity?";

    //initial values  
    /*shakeFlag = false;
    prodProps.dist = distance;
    prodProps.ceilingHolder.SetActive(true);*/

    //Generate Ground and cielling        
    /*tileGenerator.limit = true;
    tileGenerator.gameObject.SetActive(true);           
    tileGenerator.width = distance;
    tileGenerator.height = height;
    fallingCeilings.ceilling = false;
    StartCoroutine(ceilling());*/
    //Initialize player

    /*myPlayer.ragdollActive = true;  
    myPlayer.playerSpeed=0;       
    runTime.text = "Time: 0.00s";*/

    //Initialize GUI
    /*submitButton.gameObject.SetActive(true);
    correct.gameObject.SetActive(false);
    wrong.gameObject.SetActive(false);
    inputField.GetComponent<Text>().text = playerInput;*/
    //Game penalty

    /*takeReduced = true;
    TFFlag =true;
    minusLife =false;
    gameOver.SetActive(false);  
    GameMAnager.gameOverFlag =false;*/

    //game end stage 
    /*Answered = false;    
    nextStage = false;
    message.SetActive(false);*/

    //PlayerPrefs.SetString("pageOut","1,"+GameMAnager.playerName+","+GameMAnager.playerGender+","+GameMAnager.level+","+GameMAnager.stage+","+GameMAnager.talentFee);           
    }    
}
