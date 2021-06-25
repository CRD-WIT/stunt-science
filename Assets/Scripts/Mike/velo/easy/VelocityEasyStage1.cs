using System.Collections;
using UnityEngine;
using TMPro;

public class VelocityEasyStage1 : MonoBehaviour
{
    public GameObject dimensionLine;
    public Player myPlayer;
    public TMP_Text playerNameText, messageText, timer;
    public GameObject AfterStuntMessage, safeZone, rubblesStopper;
    string pronoun, pPronoun, pNoun, playerName, playerGender;
    public float distance, gameTime, Speed, elapsed, currentPos;
    private CeillingGenerator theCeiling;
    StageManager sm = new StageManager();
    float wrongDistance;

    void Start()
    {
        sm.SetGameLevel(1);
        theCeiling = FindObjectOfType<CeillingGenerator>();           
        myPlayer = FindObjectOfType<Player>();
        //playerNameText.text = RegistrationManager.playerName;
        myPlayer.gameObject.SetActive(true);
        //chance = 0;
        VelocityEasyStage1SetUp();
        //talentFee.text = "TF: " + GameMAnager.talentFee.ToString(); 
        string difficulty= sm.GetDifficulty();
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
    }
    void FixedUpdate(){
        float answer = SimulationManager.playerAnswer;
        if(SimulationManager.isSimulating)
        {
            dimensionLine.SetActive(false);
            myPlayer.moveSpeed = answer;
            timer.text = elapsed.ToString("f2")+"s";
            elapsed += Time.fixedDeltaTime;
            if(elapsed>=gameTime)
            {
                StartCoroutine(StuntResult());
                rubblesStopper.SetActive(false);
                myPlayer.moveSpeed = 0;
                SimulationManager.isSimulating = false;
                timer.text = gameTime.ToString("f2")+"s";
                if ((answer == Speed))
                {
                    myPlayer.happy = true;
                    messageText.text = "<b><color=green>Stunt Successful!</color></b>\n\n\n"+PlayerPrefs.GetString("Name")+" ran at exact speed.\n Now, "+pronoun+" is <b><color=green>safe</color></b>.";
                    SimulationManager.isAnswerCorrect= true;
                    myPlayer.transform.position = new Vector2(distance, myPlayer.transform.position.y);
                }
                else{ 
                    myPlayer.standup = true;
                    myPlayer.lost = true;
                    currentPos = SimulationManager.playerAnswer*gameTime;
                    if(answer < Speed){
                        myPlayer.transform.position = new Vector2(currentPos-0.1f, myPlayer.transform.position.y);
                        messageText.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n"+PlayerPrefs.GetString("Name")+" ran too slow and "+pronoun+" stopped before the safe spot.\nThe correct answer is <color=red>"+Speed+"m/s</color>.";
                    }
                    else //if(answer > Speed)
                    {
                        myPlayer.transform.position = new Vector2(currentPos+0.1f, myPlayer.transform.position.y);
                        messageText.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n"+PlayerPrefs.GetString("Name")+" ran too fast and "+pronoun+" stopped after the safe spot.\nThe correct answer is <color=red>"+Speed+"m/s</color>.";
                    }
                }   
            } 
        }      
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
        } 
        dimensionLine.SetActive(true);
        DimensionManager.dimensionLength = distance;
        theCeiling.createQuadtilemap(); 
        safeZone.transform.position = new Vector2(distance, 0.2f);
        timer.text = "0.00s";
        myPlayer.transform.position = new Vector2(0f, myPlayer.transform.position.y);   
        elapsed=0;  
        wrongDistance = SimulationManager.playerAnswer / gameTime;
        rubblesStopper.SetActive(true);
        SimulationManager.isSimulating =false; 
        AfterStuntMessage.SetActive(false);
        SimulationManager.question = "The ceiling is crumbling and the safe area is <color=red>"+distance.ToString()+" meters</color> away from "+playerName+". If "+pronoun+" has exactly <color=#006400>"+gameTime.ToString()+" seconds</color> to go to the safe spot, what should be "+pNoun+" <color=purple>velocity</color>?";         
    } 
    IEnumerator StuntResult(){
        //messageFlag = false;
        yield return new WaitForSeconds(3f); 
        AfterStuntMessage.SetActive(true);        
    }   
}
