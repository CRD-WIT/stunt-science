using System.Collections;
using UnityEngine;
using TMPro;
using GameConfig;

public class VelocityEasyStage1 : MonoBehaviour
{
    Player myPlayer;
    HeartManager HeartManager;
    [SerializeField] TMP_Text playerNameText, messageText, timer;
    [SerializeField] GameObject AfterStuntMessage, safeZone, rubblesStopper, ragdollSpawn, rubbleBlocker, givenDistance, annotationFollower;
    [SerializeField] LineRenderer endOfAnnotation;
    string pronoun, pNoun, playerName, playerGender;
    public float distance, gameTime, Speed, elapsed, currentPos;
    CeillingGenerator theCeiling;
    StageManager sm = new StageManager();
    Annotation dimensionLine;
    IndicatorManager followLine;
    void Awake()
    {
        if (SimulationManager.isSimulating)
        {
            
        }
    }

    void Start()
    {
        RumblingManager.isCrumbling = false;
        sm.SetGameLevel(1);
        sm.SetDifficulty(1);
        dimensionLine = givenDistance.GetComponent<Annotation>();
        followLine = annotationFollower.GetComponent<IndicatorManager>();
        theCeiling = FindObjectOfType<CeillingGenerator>();
        myPlayer = FindObjectOfType<Player>();
        HeartManager = FindObjectOfType<HeartManager>();
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        VelocityEasyStage1SetUp();
    }
    void FixedUpdate()
    {
        followLine.distanceTraveled = currentPos;
        float answer = SimulationManager.playerAnswer;
        if (SimulationManager.isSimulating)
        {
            currentPos = myPlayer.transform.position.x;
            givenDistance.SetActive(false);
            myPlayer.moveSpeed = answer;
            timer.text = elapsed.ToString("f2") + "s";
            elapsed += Time.fixedDeltaTime;
            annotationFollower.SetActive(true);
            followLine.playerVelocity = answer;
            //followLine.distanceTraveled = myPlayer.transform.position.x;
            if (elapsed >= gameTime)
            {
                RumblingManager.isCrumbling = true;
                rubblesStopper.SetActive(false);
                StartCoroutine(StuntResult());
                myPlayer.moveSpeed = 0;
                SimulationManager.isSimulating = false;
                timer.text = gameTime.ToString("f2") + "s";
                if ((answer == Speed))
                {
                    followLine.valueIs = TextColorMode.Correct;
                    // followLine.AnswerIs("correct");
                    currentPos = distance;
                    rubbleBlocker.SetActive(true);
                    messageText.text = "<b><color=green>Stunt Successful!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " is <color=green>safe</color>!";
                    SimulationManager.isAnswerCorrect = true;
                    myPlayer.transform.position = new Vector2(currentPos, myPlayer.transform.position.y);
                }
                else
                {
                    followLine.valueIs = TextColorMode.Wrong;
                    // followLine.AnswerIs("wrong");
                    HeartManager.ReduceLife();
                    if (SimulationManager.isRagdollActive)
                    {
                        myPlayer.lost = false;
                        SimulationManager.isRagdollActive = false;
                    }
                    else
                    {
                        myPlayer.lost = true;
                    }
                    SimulationManager.isAnswerCorrect = false;
                    currentPos = SimulationManager.playerAnswer * gameTime;
                    if (answer < Speed)
                    {
                        myPlayer.transform.position = new Vector2(currentPos - 0.2f, myPlayer.transform.position.y);
                        messageText.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " ran too slow and " + pronoun + " stopped before the safe spot.\nThe correct answer is <color=red>" + Speed + "m/s</color>.";
                    }
                    else //if(answer > Speed)
                    {
                        myPlayer.transform.position = new Vector2(currentPos + 0.2f, myPlayer.transform.position.y);
                        messageText.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " ran too fast and " + pronoun + " stopped after the safe spot.\nThe correct answer is <color=red>" + Speed + "m/s</color>.";
                    }
                }
            }
        }
    }
    public void VelocityEasyStage1SetUp()
    {
        followLine.valueIs = TextColorMode.Given;
        followLine.whatIsAsk = UnitOf.velocity;
        // followLine.AnswerIs("");
        myPlayer.lost = false;
        myPlayer.standup = false;
        Speed = 0;
        if (playerGender == "Male")
        {
            pronoun = "he";
            pNoun = "his";
        }
        else
        {
            pronoun = "she";
            pNoun = "her";
        }
        while ((Speed < 1.5f) || (Speed > 10f))
        {
            float d = Random.Range(9f, 18f);
            distance = (float)System.Math.Round(d, 2);
            float t = Random.Range(1.5f, 2.5f);
            gameTime = (float)System.Math.Round(t, 2);
            Speed = (float)System.Math.Round((distance / gameTime), 2);
        }
        rubbleBlocker.SetActive(false);
        ragdollSpawn.SetActive(true);
        HeartManager.losslife = false;
        RumblingManager.shakeON = true;

        givenDistance.SetActive(true);
        annotationFollower.SetActive(false);
        dimensionLine.distance = distance;
        followLine.distance = distance;
        endOfAnnotation.SetPosition(0, new Vector2(distance, -3));
        endOfAnnotation.SetPosition(1, new Vector2(distance, -1.5f));

        theCeiling.createQuadtilemap();
        safeZone.transform.position = new Vector2(distance, -2);
        timer.text = "0.00s";
        myPlayer.transform.position = new Vector2(0f, myPlayer.transform.position.y);
        elapsed = 0;
        rubblesStopper.SetActive(true);
        SimulationManager.isSimulating = false;
        AfterStuntMessage.SetActive(false);
        SimulationManager.question = "The ceiling is crumbling and the safe area is <color=red>" + distance.ToString() + " meters</color> away from " + playerName + ". If " + pronoun + " has exactly <color=#006400>" + gameTime.ToString() + " seconds</color> to go to the safe spot, what should be " + pNoun + " <color=purple>velocity</color>?";
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
}
