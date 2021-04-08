using System.Collections;
using UnityEngine;
using TMPro;


public class StageTwoManager : MonoBehaviour
{
    private Player thePlayer;
    private CeillingGenerator theCeiling;
    private HeartManager theHeart;
    float distance;
    float speed;
    float finalSpeed;
    string gender;
    string pronoun;
    float answer;
    float answerRO;
    Vector2 PlayerStartPoint;
    public float gameTime, elapsed;
    public TMP_Text playerNameText, messageText, timer;
    public GameObject AfterStuntMessage;
    public GameObject safePoint;
    
    public GameObject rubbleStopper;
    float currentPos;
    
    private RumblingManager theRumbling;
    public GameObject rubbleBlocker;


    //TimeSpan duration;
    //private float gameTime = 0.0f;


    //public TextMeshProUGUI timer;
    bool simulate;
    float playerAnswer;
    float playerDistance;
    public GameObject ragdollSpawn;
    


    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        gender = PlayerPrefs.GetString("Gender");
        PlayerStartPoint = thePlayer.transform.position;
        theCeiling = FindObjectOfType<CeillingGenerator>();
        theRumbling = FindObjectOfType<RumblingManager>();
        theHeart = FindObjectOfType<HeartManager>();
        
        
        


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        currentPos = thePlayer.transform.position.x;
        playerAnswer = SimulationManager.playerAnswer;
        playerDistance = playerAnswer * speed;
        

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
        if (SimulationManager.isSimulating)
        {
            thePlayer.moveSpeed = speed;
            if (elapsed <= SimulationManager.playerAnswer)
            {
                elapsed += Time.fixedDeltaTime;
                timer.text = elapsed.ToString("f2")+"s";
            }
            
            if (playerAnswer == answerRO)
            {
                SimulationManager.isAnswerCorrect= true;
                if(currentPos >= distance)
                {
                    thePlayer.moveSpeed = 0;
                    rubbleStopper.SetActive(false);
                    thePlayer.happy = true;
                    thePlayer.transform.position = new Vector2(distance, transform.position.y);
                    timer.text = playerAnswer.ToString("f2")+"s";
                    SimulationManager.isSimulating = false;
                    theRumbling.collapse();
                    StartCoroutine(StuntResult());
                    SimulationManager.isAnswerCorrect= true;
                    messageText.text = "<b>Stunt Successful!!!</b>\n\n"+PlayerPrefs.GetString("Name")+" ran at exact speed.\n Now, "+pronoun+" is <b>safe</b> from falling down the ground.";
                }
            }
            if (playerAnswer != answerRO)
            {
                SimulationManager.isAnswerCorrect= false;
                if(currentPos >= playerDistance)
                {
                    theHeart.losinglife();
                    thePlayer.moveSpeed = 0;
                    if(currentPos < 25)
                    {
                        rubbleStopper.SetActive(false);
                        thePlayer.lost = true;
                        thePlayer.standup = true;
                        thePlayer.transform.position = new Vector2(playerDistance, transform.position.y);
                        timer.text = playerAnswer.ToString("f2")+"s";
                        SimulationManager.isSimulating = false;
                        theRumbling.collapse();
                        StartCoroutine(StuntResult());
                        safeSpot();
                    }
                    
                    if (playerAnswer < answerRO)
                    {
                        
                        messageText.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " ran too short!</color>";
                    }
                    else if (playerAnswer > answerRO)
                    {
                        messageText.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " ran too long!</color>";
                        
                    }
                }
                if (currentPos >= 25)
                {
                    theHeart.losinglife();
                    thePlayer.moveSpeed = 0;
                    rubbleStopper.SetActive(false);
                    thePlayer.standup = true;
                    SimulationManager.isSimulating = false;
                    theRumbling.collapse();
                    StartCoroutine(StuntResult());
                    safeSpot();
                    
                    
                    
                }

            }
            /*if ((elapsed <= SimulationManager.playerAnswer))
            {

                elapsed += Time.fixedDeltaTime;
                thePlayer.moveSpeed = speed;
            }
            else
            {
                thePlayer.moveSpeed = 0;
                rubbleStopper.SetActive(false);
                if ((SimulationManager.playerAnswer == answerRO))
                {
                    messageText.text = "<b>Stunt Successful!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " ran at exact speed.\n Now, " + pronoun + " is <b>safe</b> from falling down the ground.";
                    SimulationManager.isAnswerCorrect = true;
                    thePlayer.happy = true;
                }
                else
                {
                    if (SimulationManager.playerAnswer < answerRO)
                    {
                        thePlayer.lost = true;
                        thePlayer.standup = true;
                        messageText.text = "<b>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " ran too short!";
                    }
                    else if (SimulationManager.playerAnswer > answerRO)
                    {
                        messageText.text = "<b>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " ran too long!";
                        thePlayer.lost = true;
                        thePlayer.standup = true;
                    }
                    SimulationManager.isAnswerCorrect = false;
                }
                StartCoroutine(StuntResult());
                SimulationManager.isSimulating = false;
            }*/
        }
        else
        {
            timer.text = ("0.00s");
        }
        /*if(PlayerPrefs.GetInt("stageNumber") != 1)
        {
        AfterStuntMessage.SetActive(false);
        thePlayer.moveSpeed = 3;
        }*/

    }

    public void generateProblem()
    {
        distance = UnityEngine.Random.Range(9, 18);
        speed = UnityEngine.Random.Range(3.0f, 6.0f);
        finalSpeed = (float)System.Math.Round(speed, 2);
        SimulationManager.question = (("The ceiling is still crumbling and the next safe spot is <b>") + distance + ("</b> meter away from  <b>") + PlayerPrefs.GetString("Name") + ("</b>. If <b>") + PlayerPrefs.GetString("Name") + ("</b> will now run at exactly <b>") + finalSpeed.ToString("F1") + ("</b> m/s, how long should ") + pronoun + (" run so that ") + pronoun + (" will not get hit by the crumbling debris of the ceiling?"));
        answer = distance / speed;
        answerRO = (float)System.Math.Round(answer, 2);
        resetTime();
        safePoint.transform.position = new Vector2(distance, 0.23f);
        theCeiling.createQuadtilemap();
        ragdollSpawn.SetActive(true);
        rubbleStopper.SetActive(true);
        theHeart.losslife = false;

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
        theRumbling.collapsing = true;
        rubbleBlocker.SetActive(false);

        
    }
    IEnumerator StuntResult()
    {
        //messageFlag = false;
        yield return new WaitForSeconds(4f);
        AfterStuntMessage.SetActive(true);
    }
    void resetTime()
    {
        elapsed = 0;
    }
    void safeSpot()
    {
        if(currentPos < distance - 0.5f || currentPos > distance + 0.5f)
        {
            rubbleBlocker.SetActive(true);
        }
    }
}
