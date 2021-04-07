using System.Collections;
using UnityEngine;
using TMPro;

public class VelocityEasyStage1 : MonoBehaviour
{
    public Player myPlayer;
    public TMP_Text playerNameText, messageText, timer;
    public GameObject AfterStuntMessage;/*, safeZone*/
    string pronoun, pPronoun, pNoun, playerName, playerGender;
    public float distance, gameTime, Speed, elapsed;
    StageManager sm = new StageManager();

    void Start()
    {
        sm.SetGameLevel(1);
        //prodProps = FindObjectOfType<prodProps>();    
        //tileGenerator = FindObjectOfType<generateGround>();                
        myPlayer = FindObjectOfType<Player>();
        //playerNameText.text = RegistrationManager.playerName;
        myPlayer.gameObject.SetActive(true);
        //chance = 0;
        VelocityEasyStage1SetUp();
        //talentFee.text = "TF: " + GameMAnager.talentFee.ToString(); 

       
        //sm.SetStage(1);
        int stage = sm.GetStage();
        string difficulty= sm.GetDifficulty();
        sm.GetStageFromPlayerPrefs();

        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");

        //string p = $"Name: <color color=green>{playerName}</color>";
    }

    // Update is called once per frame
    void Update()
    {
        /*if(sm.GetStage() != 1){
            this.gameObject.SetActive(false);
        }else
            this.gameObject.SetActive(true);*/{
            if(SimulationManager.isSimulating){
                if((elapsed <= gameTime)){
                    timer.text = elapsed.ToString("f2")+"s";
                    elapsed += Time.fixedDeltaTime;                                                    
                    myPlayer.moveSpeed = SimulationManager.playerAnswer;
                        /*if(myPlayer.transform.position.x >= distance){
                            myPlayer.transform.position = new Vector3(distance+33 ,myPlayer.transform.position.y, 0);
                            myPlayer.ragdollActive = false;                            
                        }*/          
                } 
                else{
                        //camManager.shakeDuration=2.5f; 
                    myPlayer.moveSpeed = 0; 
                    timer.text = gameTime.ToString("f2")+"s";
                    SimulationManager.isStartOfStunt = false;
                    SimulationManager.directorIsCalling = true;
                        //fallingCeilings.ceilling = true;
                    if ((SimulationManager.playerAnswer == Speed)){ 
                            /*myPlayer.playerPosition = distance-0.2f;
                            
                            if(nextStage){
                                if(myPlayer.playerPosition <= 22.85){
                                    //StartCoroutine(jumpFlip());
                                    myPlayer.playerSpeed = randomSpeed;                                    
                                }
                                else{                                    
                                    myPlayer.playerSpeed = 0;                                    
                                }
                            }*/
                            //correctAnswer =true;
                        myPlayer.happy = true;
                        messageText.text = "<b><color=green>Stunt Successful!!!</color></b>\n\n"+PlayerPrefs.GetString("Name")+" ran at exact speed.\n Now, "+pronoun+" is <b>safe</b>.";
                        SimulationManager.isAnswerCorrect= true;
                    //AfterStuntMessage.SetActive(true);
                    }
                    else{     
                                          
                        if(SimulationManager.playerAnswer < Speed){
                            messageText.text = "<b><color=red>Stunt Failed!!!</color></b>\n\n"+PlayerPrefs.GetString("Name")+" ran too slow.";
                            myPlayer.lost = true;
                        }
                        else if(SimulationManager.playerAnswer > Speed){
                            messageText.text = "<b><color=red>Stunt Failed!!!</color></b>\n\n"+PlayerPrefs.GetString("Name")+" ran too fast.";
                        }
                        myPlayer.standup = true;
                        SimulationManager.isAnswerCorrect= false;
                    //AfterStuntMessage.SetActive(true);
                            //runTime.text = "Time: "+elapsed.ToString("f2")+"s";                                                      
                            //correctAnswer = false;                                
                    }
                        /*AnswerChecker();
                        if(!minusLife){
                            Takes.Retake();
                            minusLife=true;
                        }*/
                    StartCoroutine(StuntResult());
                    SimulationManager.isSimulating = false;
                }
            }
        }
        
                /*else{
                    if(myPlayer.playerPosition <= 22.85){
                        //StartCoroutine(jumpFlip());
                        myPlayer.playerSpeed = randomSpeed;                                    
                    }
                    else{                                   
                        myPlayer.playerSpeed = 0;
                    }
                    if(!shakeFlag){
                        StartCoroutine(shake());
                    } 
                }    */   
                // Debug.Log("time "+elapsed);         
    }
    public void VelocityEasyStage1SetUp(){
        myPlayer.lost = false;
        myPlayer.standup = false;
        Speed = 0;
        if(playerGender == "Male"){
            pronoun = "he";
            pPronoun = "him";
            pNoun = "his";
        }else{
            pronoun = "she";
            pPronoun = "her";
            pNoun = "her";}
        while((Speed < 1.5f)||(Speed > 10f)){
            float d = Random.Range(9f, 18f);
            distance = (float)System.Math.Round(d,2);
            float t = Random.Range(1.5f, 2.5f);
            gameTime = (float)System.Math.Round(t,2);
            Speed = (float)System.Math.Round((distance/t), 2);
            //startingPoint = myPlayer.transform.position;
        }  
        timer.text = "0.00s";
        //safeZone.transform.position = new Vector2(distance, 0.23f);
        myPlayer.transform.position = new Vector2(0f, myPlayer.transform.position.y);   
        elapsed=0;  
        SimulationManager.isSimulating =false; 
        AfterStuntMessage.SetActive(false);
        SimulationManager.question = "The ceiling is crumbling and the safe area is <color=red>"+distance.ToString()+" meters</color> away from "+playerName+". If "+pronoun+" has exactly <color=#006400>"+gameTime.ToString()+" seconds</color> to go to the safe spot, what should be "+pNoun+" <color=purple>velocity</color>?";

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
    runTime.text = "gameTime: 0.00s";*/

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
    IEnumerator StuntResult(){
        //messageFlag = false;
        yield return new WaitForSeconds(1f); 
        AfterStuntMessage.SetActive(true);        
    }   
}
