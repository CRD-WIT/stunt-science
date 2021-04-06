using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class StageTwoManager : MonoBehaviour
{
    private Player thePlayer;
    float distance;
    float speed;
    float finalSpeed;
    string gender;
    string pronoun;
    float answer;
    float answerRO;
    Vector2 PlayerStartPoint;
    public float  gameTime, elapsed;
    public TMP_Text playerNameText, messageText;
    public GameObject AfterStuntMessage;
    public GameObject safePoint;
    
    //TimeSpan duration;
    //private float gameTime = 0.0f;

   
    //public TextMeshProUGUI timer;
    bool simulate;
    
    
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        gender = PlayerPrefs.GetString("Gender");
        PlayerStartPoint = thePlayer.transform.position;
        
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (gender == "Male")
        {
            pronoun = ("he");
        }
        if (gender == "Female")
        {
            pronoun = ("she");
        }
        /*if(simulate)
        {
            duration = TimeSpan.FromMilliseconds(gameTime * 1000);

            int milliseconds = Convert.ToInt32(duration.ToString(@"ff"));
            int seconds = Convert.ToInt32(duration.ToString(@"ss"));

            timer.SetText($"{seconds}:{milliseconds} sec");
        }*/
        if(SimulationManager.isSimulating)
        {
           if((elapsed <= SimulationManager.playerAnswer))
           {         
                          
                elapsed += Time.fixedDeltaTime;                                                    
                thePlayer.moveSpeed = speed;                        
            } 
                else{                        
                    thePlayer.moveSpeed = 0;                        
                    if ((SimulationManager.playerAnswer == answerRO))
                    {                             
                        messageText.text = "<b>Stunt Successful!!!</b>\n\n"+PlayerPrefs.GetString("Name")+" ran at exact speed.\n Now, "+pronoun+" is <b>safe</b> from falling down the ground.";
                        SimulationManager.isAnswerCorrect= true;
                        thePlayer.happy = true;                    
                    }
                    else{
                        if(SimulationManager.playerAnswer < answerRO)
                        {
                            thePlayer.lost = true;
                            thePlayer.standup = true;    
                            messageText.text = "<b>Stunt Failed!!!</b>\n\n"+PlayerPrefs.GetString("Name")+" ran too short!";
                        }
                        else if(SimulationManager.playerAnswer > answerRO)
                        {
                            messageText.text = "<b>Stunt Failed!!!</b>\n\n"+PlayerPrefs.GetString("Name")+" ran too long!";
                            thePlayer.lost = true;
                            thePlayer.standup = true;    
                        }
                            SimulationManager.isAnswerCorrect= false;                                             
                        }                       
                    StartCoroutine(StuntResult());
                    SimulationManager.isSimulating = false;
                }
            }
            /*if(PlayerPrefs.GetInt("stageNumber") != 1)
            {
                AfterStuntMessage.SetActive(false);
                thePlayer.moveSpeed = 3;
            }*/
    
        }
        
    public void generateProblem()
    {
        distance = UnityEngine.Random.Range(5, 10);
        speed = UnityEngine.Random.Range(2.0f, 5.0f);
        finalSpeed = (float)System.Math.Round(speed, 2);
        SimulationManager.question = (("The ceiling is still crumbling and the next safe spot is <b>")+ distance + ("</b> meter away from  <b>") + PlayerPrefs.GetString("Name") + ("</b>. If <b>") + PlayerPrefs.GetString("Name") + ("</b> will now run at exactly <b>")+ finalSpeed.ToString("F1")+ ("</b> m/s, how long should ")+ pronoun + (" run so that ")+ pronoun + (" will not get hit by the crumbling debris of the ceiling?"));
        answer = distance / speed;
        answerRO = (float)System.Math.Round(answer, 2);
        resetTime();
        safePoint.transform.position = new Vector2(distance, 0.23f);
    }
    public void play()
    {
        //simulate = true;
        //answer = SimulationManager.playerAnswer;
        //thePlayer.moveSpeed = speed;

    }
    public void reset()
    {
        thePlayer.transform.position = new Vector2(0, transform.position.y);
        thePlayer.moveSpeed = 0;
        thePlayer.lost = false;
        thePlayer.standup = false;
        AfterStuntMessage.SetActive(false);
        generateProblem();
        resetTime();
    }
     IEnumerator StuntResult()
    {
        //messageFlag = false;
        yield return new WaitForSeconds(1f); 
        AfterStuntMessage.SetActive(true);        
    } 
    void resetTime()
    {
        elapsed = 0;
    }  
}