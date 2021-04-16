using System.Collections;
using UnityEngine;
using TMPro;


public class StageTwoManager : MonoBehaviour
{
    private Player thePlayer;
    private CeillingGenerator theCeiling;
    private HeartManager theHeart;
    [SerializeField]
    float distance, speed, finalSpeed, answer, answerRO, currentPos, playerAnswer, playerDistance;
    string gender, pronoun;
    Vector2 PlayerStartPoint;
    public float gameTime, elapsed;
    public TMP_Text playerNameText, messageText, timer;
    public GameObject AfterStuntMessage, safePoint, rubbleStopper, rubbleBlocker, ragdollSpawn, dimensionLine, groundPlatform;
    private RumblingManager theRumbling;
    private ScoreManager theScorer;
    bool simulate;
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        gender = PlayerPrefs.GetString("Gender");
        PlayerStartPoint = thePlayer.transform.position;
        theCeiling = FindObjectOfType<CeillingGenerator>();
        theRumbling = FindObjectOfType<RumblingManager>();
        theHeart = FindObjectOfType<HeartManager>();
    }
    void FixedUpdate()
    {
        currentPos = thePlayer.transform.position.x;
        playerAnswer = SimulationManager.playerAnswer;
        if (SimulationManager.isSimulating)
        {
            dimensionLine.SetActive(false);
            thePlayer.moveSpeed = speed;
            elapsed += Time.fixedDeltaTime;
            timer.text = elapsed.ToString("f2") + "s";
            /*if (currentPos > 30)
            {
                SimulationManager.isSimulating = false;
                timer.text = elapsed.ToString("f2") + "s";
                thePlayer.moveSpeed = 0;
                thePlayer.transform.position = new Vector2(currentPos, -10);
            }*/
            if ((elapsed >= SimulationManager.playerAnswer) || (currentPos > 30))
            {
                if (currentPos > 30)
                {
                    rubbleStopper.SetActive(true);
                    timer.text = elapsed.ToString("f2") + "s";
                }
                else
                {
                    timer.text = playerAnswer.ToString("f2") + "s";
                    rubbleStopper.SetActive(false);
                }
                thePlayer.moveSpeed = 0;
                RumblingManager.isCrumbling = true;
                //thePlayer.transform.position = new Vector2(distance, -3);
                SimulationManager.isSimulating = false;
                theRumbling.collapse();
                StartCoroutine(StuntResult());
                if (playerAnswer == answerRO)
                {
                    rubbleBlocker.SetActive(true);
                    SimulationManager.isAnswerCorrect = true;
                    messageText.text = "<b><color=green>Stunt Successful!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " is <color=green>safe</color>!";
                    thePlayer.transform.position = new Vector2(distance, thePlayer.transform.position.y);
                }
                else//if (playerAnswer != answerRO)
                {
                    SimulationManager.isAnswerCorrect = false;
                    theHeart.ReduceLife();
                    if (SimulationManager.isRagdollActive)
                    {
                        thePlayer.lost = false;
                        SimulationManager.isRagdollActive = false;
                    }
                    else
                    {
                        thePlayer.lost = true;
                    }
                    playerDistance = playerAnswer * speed;
                    if (playerAnswer < answerRO)
                    {
                        thePlayer.transform.position = new Vector2(playerDistance - 0.2f, thePlayer.transform.position.y);
                        messageText.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " stopped too early and " + pronoun + " stopped before the safe spot!\nThe correct answer is <color=red>" + answerRO + "s.</color>";
                    }
                    else if (playerAnswer > answerRO)
                    {
                        if (currentPos > 30)
                        {
                            thePlayer.transform.position = new Vector2(currentPos, -10);
                        }
                        else
                            thePlayer.transform.position = new Vector2(playerDistance + 0.2f, thePlayer.transform.position.y);
                        messageText.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " stopped too late and " + pronoun + " stopped after the safe spot!\nThe correct answer is <color=red>" + answerRO + "s.</color>";
                    }

                }
            }
        }
    }
    public void generateProblem()
    {
        answer = 0;
        if (gender == "Male")
        {
            pronoun = ("he");
        }
        if (gender == "Female")
        {
            pronoun = ("she");
        }
        while ((answer < 2.63f) || (answer > 3.1f))
        {
            float d = UnityEngine.Random.Range(21f, 28f);
            distance = (float)System.Math.Round(d, 2);
            speed = UnityEngine.Random.Range(1.5f, 10f);
            finalSpeed = (float)System.Math.Round(speed, 2);
            answer = distance / speed;
        }
        thePlayer.transform.position = new Vector2(0, -3);
        RumblingManager.shakeON = true;
        timer.text = "0.00s";
        SimulationManager.question = "The ceiling is still crumbling and the next safe spot is <color=red>" + distance.ToString() + " meters</color> away from " + PlayerPrefs.GetString("Name") + ". If " + pronoun + " runs at a constant velocity of <color=purple>" + finalSpeed.ToString() + " meters per second</color>, how <color=#006400>long</color> should " + pronoun + " run so " + pronoun + " will stop exactly on the same spot?";
        answerRO = (float)System.Math.Round(answer, 2);
        dimensionLine.SetActive(true);
        DimensionManager.startLength = 0;
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
        yield return new WaitForSeconds(1f);
        SimulationManager.directorIsCalling = true;
        SimulationManager.isStartOfStunt = false;
        yield return new WaitForSeconds(3f);
        rubbleBlocker.SetActive(false);
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
