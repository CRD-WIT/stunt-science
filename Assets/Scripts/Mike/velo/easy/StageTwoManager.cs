using System.Collections;
using UnityEngine;
using GameConfig;
public class StageTwoManager : MonoBehaviour
{
    private PlayerV1_1 thePlayer;
    UnitOf whatIsAsk;
    private CeillingGenerator theCeiling;
    private HeartManager theHeart;
    [SerializeField] float distance, speed, finalSpeed, answer, answerRO, currentPos, playerAnswer, playerDistance;
    string gender, pronoun, question, errorMessage;
    Vector2 PlayerStartPoint;
    public float elapsed;
    public GameObject safePoint, rubbleStopper, rubbleBlocker, ragdollSpawn, groundPlatform;
    private RumblingManager theRumbling;
    private ScoreManager theScorer;
    bool answerIs;
    IndicatorManagerV1_1 labels;
    QuestionControllerVThree qc;
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        labels = FindObjectOfType<IndicatorManagerV1_1>();
        thePlayer = FindObjectOfType<PlayerV1_1>();
        theScorer = FindObjectOfType<ScoreManager>();
        gender = PlayerPrefs.GetString("Gender");
        PlayerStartPoint = thePlayer.transform.position;
        theCeiling = FindObjectOfType<CeillingGenerator>();
        theRumbling = FindObjectOfType<RumblingManager>();
        theHeart = FindObjectOfType<HeartManager>();
        whatIsAsk = UnitOf.time;
        reset();
    }
    void FixedUpdate()
    {
        playerAnswer = qc.GetPlayerAnswer();
        if (SimulationManager.isAnswered)
        {
            labels.distanceSpawnPnt = new Vector2(0, -2);
            labels.timeSpawnPnt = new Vector2(0, -2.25f);
            thePlayer.moveSpeed = speed;
            elapsed += Time.fixedDeltaTime;
            currentPos = thePlayer.transform.position.x;
            qc.timer = elapsed.ToString("f2") + "s";
            if ((elapsed >= playerAnswer) || (currentPos > 30))
            {
                if (currentPos > 30)
                {
                    rubbleStopper.SetActive(true);
                    qc.timer = elapsed.ToString("f2") + "s";
                }
                else
                {
                    qc.timer = playerAnswer.ToString("f2") + "s";
                    rubbleStopper.SetActive(false);
                }
                thePlayer.moveSpeed = 0;
                RumblingManager.isCrumbling = true;
                SimulationManager.isAnswered = false;
                theRumbling.collapse();
                StartCoroutine(StuntResult());
                if (playerAnswer == answerRO)
                {
                    currentPos = distance;
                    elapsed = answerRO;
                    rubbleBlocker.SetActive(true);
                    answerIs = true;
                    errorMessage = PlayerPrefs.GetString("Name") + " is <color=green>safe</color>!";
                    thePlayer.transform.position = new Vector2(currentPos, thePlayer.transform.position.y);
                }
                else//if (playerAnswer != answerRO)
                {
                    currentPos = playerAnswer * speed;
                    answerIs = false;
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
                        errorMessage = PlayerPrefs.GetString("Name") + " stopped too early and " + pronoun + " stopped before the safe spot!\nThe correct answer is <color=red>" + answerRO + "s.</color>";
                    }
                    else if (playerAnswer > answerRO)
                    {
                        if (currentPos > 30)
                        {
                            thePlayer.transform.position = new Vector2(currentPos, -10);
                        }
                        else
                            thePlayer.transform.position = new Vector2(playerDistance + 0.2f, thePlayer.transform.position.y);
                        errorMessage = PlayerPrefs.GetString("Name") + " stopped too late and " + pronoun + " stopped after the safe spot!\nThe correct answer is <color=red>" + answerRO + "s.</color>";

                        labels.ShowCorrectDistance(distance, true, new Vector2(0, 2));
                        labels.ShowCorrectTime(answer, answer * speed, true);
                    }
                }
                labels.AnswerIs(answerIs, true);
            }
            labels.IsRunning(playerAnswer, currentPos, elapsed, currentPos);
        }
        SimulationManager.isAnswerCorrect = answerIs;
        labels.SetPlayerPosition(thePlayer.transform.position);
    }
    public void generateProblem()
    {
        qc.SetUnitTo(whatIsAsk);
        playerAnswer = 0;
        answer = 0;
        elapsed = 0;
        if (gender == "Male")
        {
            pronoun = "he";
        }
        if (gender == "Female")
        {
            pronoun = "she";
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
        qc.timer = "0.00s";

        qc.limit = 5f;
        question = "The ceiling is still crumbling and the next safe spot is <color=red>" + distance.ToString() + " meters</color> away from " + PlayerPrefs.GetString("Name") + ". If " + pronoun + " runs at a constant velocity of <color=purple>" + finalSpeed.ToString() + " meters per second</color>, how <color=#006400>long</color> should " + pronoun + " run so " + pronoun + " will stop exactly on the same spot?";
        qc.SetQuestion(question);
        answerRO = (float)System.Math.Round(answer, 2);
        safePoint.transform.position = new Vector2(distance, -2);
        theCeiling.createQuadtilemap(qc.stage);
        ragdollSpawn.SetActive(true);
        rubbleStopper.SetActive(true);
        theHeart.losslife = false;
        groundPlatform.transform.localScale = new Vector3(68.05f, groundPlatform.transform.localScale.y, 1);
        ragdollSpawn.transform.position = new Vector3(30.5f, ragdollSpawn.transform.position.y, 0);
        
        labels.timeSpawnPnt = new Vector2(0, -2.25f);
        labels.distanceSpawnPnt = new Vector2(0, -2);
        labels.showLines(distance, distance, null, speed, answer);
        labels.UnknownIs('t');
    }
    public void reset()
    {
        thePlayer.transform.position = new Vector2(0, -3);
        thePlayer.moveSpeed = 0;
        thePlayer.lost = false;
        thePlayer.standup = false;
        generateProblem();
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
        qc.ActivateResult(errorMessage, answerIs);
    }
    void safeSpot()
    {
        if (currentPos < distance - 0.5f || currentPos > distance + 0.5f)
        {
            rubbleBlocker.SetActive(true);
        }
    }
}