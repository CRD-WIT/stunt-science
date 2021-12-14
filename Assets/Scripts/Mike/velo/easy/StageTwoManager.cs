using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GameConfig;
using TMPro;
public class StageTwoManager : MonoBehaviour
{
    // Stunt Guide
    public Text stuntGuideTextObject;
    public string stuntGuideText;
    public Image stuntGuideImage;
    public Sprite stuntGuideImageSprite;
    // End of Stunt Guide
    AnswerGuards answerGuards = new AnswerGuards();
    public PlayerV1_1 thePlayer;
    UnitOf whatIsAsk;
    public CeillingGenerator theCeiling;
    public HeartManager theHeart;
    [SerializeField] float distance, speed, finalSpeed, answer, answerRO, currentPos, playerAnswer, playerDistance;
    string gender, pronoun, question, errorMessage, playerName;
    Vector2 PlayerStartPoint;
    public float elapsed;
    public GameObject safePoint, rubbleStopper, rubbleBlocker, ragdollSpawn, groundPlatform;
    public RumblingManager theRumbling;
    private ScoreManager theScorer;
    bool answerIs;
    public IndicatorManagerV1_1 labels;
    public QuestionControllerVThree qc;
    public AudioSource scream;
    public TMP_Text debugAnswer;
    public FirebaseManager firebaseManager;    
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//to prevent screen from sleeping
        theScorer = FindObjectOfType<ScoreManager>();
        gender = PlayerPrefs.GetString("Gender");
        playerName = PlayerPrefs.GetString("Name");
        PlayerStartPoint = thePlayer.transform.position;
        whatIsAsk = UnitOf.time;
        firebaseManager.GameLogMutation(1, 2, "Easy", Actions.Started, 0);
        reset();
    }
    void FixedUpdate()
    {
        //Stunt Guide
        stuntGuideImage.sprite = stuntGuideImageSprite;
        stuntGuideTextObject.text = stuntGuideText;

        debugAnswer.SetText($"Answer: {answerRO}");
        playerAnswer = qc.GetPlayerAnswer();
        float adjustedAnswer = qc.AnswerTolerance(answerRO);
        if (SimulationManager.isAnswered)
        {
            labels.distanceSpawnPnt = new Vector2(0, -2);
            // labels.timeSpawnPnt = new Vector2(0, -2.25f);
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
                if (adjustedAnswer == answerRO)//(playerAnswer == answerRO)
                {
                    currentPos = distance;
                    elapsed = answerRO;
                    rubbleBlocker.SetActive(true);
                    answerIs = true;
                    errorMessage = $"<b>{playerName}</b> successfully performed the stunt and went to the safe spot!";//PlayerPrefs.GetString("Name") + " is <color=green>safe</color>!";
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
                    if (adjustedAnswer < answerRO)//(playerAnswer < answerRO)
                    {
                        scream.Play();
                        thePlayer.transform.position = new Vector2(playerDistance - 0.2f, thePlayer.transform.position.y);
                        // errorMessage = PlayerPrefs.GetString("Name") + " stopped too early and " + pronoun + " stopped before the safe spot!\nThe correct answer is <color=red>" + answerRO + "s.</color>";
                    }
                    else //if (playerAnswer > answerRO)
                    {
                        if (currentPos > 30)
                        {
                            thePlayer.transform.position = new Vector2(currentPos, -10);
                        }
                        else
                        {
                            scream.Play();
                            thePlayer.transform.position = new Vector2(playerDistance + 0.2f, thePlayer.transform.position.y);
                            // errorMessage = PlayerPrefs.GetString("Name") + " stopped too late and " + pronoun + " stopped after the safe spot!\nThe correct answer is <color=red>" + answerRO + "s.</color>";
                        }

                        labels.ShowCorrectDistance(distance, true, new Vector2(0, 2));
                        // labels.ShowCorrectTime(answer, answer * speed, true);
                    }
                    errorMessage = $"<b>{playerName}</b> has unable to stop exactly at safe spot. Stunt Failed!";
                }
                labels.AnswerIs(answerIs, true);
            }
            labels.IsRunning(playerAnswer, currentPos);
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
        question = $"<b>{playerName}</b> is instucted to run and stop exactly at the safe spot <b>{distance.ToString("f2")} {qc.Unit(UnitOf.distance)}</b> away before the ceiling crumbles down. If <b>{playerName}</b> runs at a constant velocity of <b>{speed.ToString("f2")} {qc.Unit(UnitOf.velocity)}</b>, how long should {pronoun} run before stopping so {pronoun} will stop exactly at the safe spot?";
        qc.SetQuestion(question);
        answerRO = (float)System.Math.Round(answer, 2);
        safePoint.transform.position = new Vector2(distance, -2);
        theCeiling.createQuadtilemap(qc.stage);
        ragdollSpawn.SetActive(true);
        rubbleStopper.SetActive(true);
        theHeart.losslife = false;
        groundPlatform.transform.localScale = new Vector3(68.05f, groundPlatform.transform.localScale.y, 1);
        ragdollSpawn.transform.position = new Vector3(30.5f, ragdollSpawn.transform.position.y, 0);

        // labels.timeSpawnPnt = new Vector2(0, -2.25f);
        labels.distanceSpawnPnt = new Vector2(0, -2);
        labels.showLines(distance, null, speed, answer);
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
