using System.Collections;
using UnityEngine;
using TMPro;


public class StageTwoManager : MonoBehaviour
{
    private Player thePlayer;
    private CeillingGenerator theCeiling;
    private HeartManager theHeart;
    float distance, speed, finalSpeed, answer, answerRO, currentPos;
    string gender, pronoun;
    Vector2 PlayerStartPoint;
    public float gameTime, elapsed;
    public TMP_Text playerNameText, messageText, timer;
    public GameObject AfterStuntMessage, safePoint, rubbleStopper, rubbleBlocker, ragdollSpawn, dimensionLine, groundPlatform;
    private RumblingManager theRumbling;
    private ScoreManager theScorer;


    //TimeSpan duration;
    //private float gameTime = 0.0f;
    bool simulate;
    float playerAnswer, playerDistance;
    void Start()
    {

        thePlayer = FindObjectOfType<Player>();
        gender = PlayerPrefs.GetString("Gender");
        PlayerStartPoint = thePlayer.transform.position;
        theCeiling = FindObjectOfType<CeillingGenerator>();
        theRumbling = FindObjectOfType<RumblingManager>();
        theHeart = FindObjectOfType<HeartManager>();
        //theScorer = FindObjectOfType<ScoreManager>();
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
            dimensionLine.SetActive(false);
            thePlayer.moveSpeed = speed;
            if (elapsed <= SimulationManager.playerAnswer)
            {
                elapsed += Time.fixedDeltaTime;
                timer.text = elapsed.ToString("f2") + "s";
            }

            if (playerAnswer == answerRO)
            {
                SimulationManager.isAnswerCorrect = true;
                if (currentPos >= distance)
                {
                    //theScorer.finalstar();
                    thePlayer.moveSpeed = 0;
                    RumblingManager.isCrumbling = true;
                    rubbleStopper.SetActive(false);
                    thePlayer.happy = true;
                    thePlayer.transform.position = new Vector2(distance, -3);
                    timer.text = playerAnswer.ToString("f2") + "s";
                    SimulationManager.isSimulating = false;
                    theRumbling.collapse();
                    StartCoroutine(StuntResult());
                    SimulationManager.isAnswerCorrect = true;
                    messageText.text = "<b>Stunt Successful!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " ran at exact speed.\n Now, " + pronoun + " is <b>safe</b> from falling down the ground.";
                }
            }
            if (playerAnswer != answerRO)
            {
                SimulationManager.isAnswerCorrect = false;
                if (currentPos >= playerDistance)
                {
                    theHeart.ReduceLife();
                    thePlayer.moveSpeed = 0;
                    if (currentPos < 25)
                    {
                        RumblingManager.isCrumbling = true;
                        rubbleStopper.SetActive(false);
                        thePlayer.lost = true;
                        thePlayer.standup = true;
                        thePlayer.transform.position = new Vector2(playerDistance, -3);
                        timer.text = playerAnswer.ToString("f2") + "s";
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
                    theHeart.ReduceLife();
                    thePlayer.moveSpeed = 0;
                    RumblingManager.isCrumbling = true;
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
        while ((answer<2.63f)||(answer>3.1f))
        {
            float d = UnityEngine.Random.Range(21f, 28f);
            distance = (float)System.Math.Round(d, 2);
            speed = UnityEngine.Random.Range(1.5f, 10f);
            finalSpeed = (float)System.Math.Round(speed, 2);
            answer = distance / speed;
        }
        thePlayer.transform.position = new Vector2(0, -3);
        RumblingManager.shakeON = true;
        SimulationManager.question = "The ceiling is still crumbling and the next safe spot is <color=red>" + distance.ToString() + " meters</color> away from " + PlayerPrefs.GetString("Name") + ". If " + pronoun + " runs at a constant velocity of <color=purple>" + finalSpeed.ToString() + " meters per second</color>, how <color=#006400>long</color> should " + pronoun + " run so " + pronoun + " will stop exactly on the same spot?";
        answerRO = (float)System.Math.Round(answer, 2);
        dimensionLine.SetActive(true);
        DimensionManager.dimensionLength = distance;
        resetTime();
        safePoint.transform.position = new Vector2(distance, -2);
        theCeiling.createQuadtilemap();
        ragdollSpawn.SetActive(true);
        rubbleStopper.SetActive(true);
        theHeart.losslife = false;
        groundPlatform.transform.localScale = new Vector3(68.05f, groundPlatform.transform.localScale.y, 1);
        ragdollSpawn.transform.position = new Vector3(30.5f, ragdollSpawn.transform.position.y, 0);
    }
    public void play()
    {
        //simulate = true;
        //answer = SimulationManager.playerAnswer;
        //thePlayer.moveSpeed = speed;

    }
    public void reset()
    {
        thePlayer.transform.position = new Vector2(0, -3);
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
        if (currentPos < distance - 0.5f || currentPos > distance + 0.5f)
        {
            rubbleBlocker.SetActive(true);
        }
    }
}
