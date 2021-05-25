using System.Collections;
using UnityEngine;
using TMPro;
using GameConfig;

public class VelocityEasyStage1 : MonoBehaviour
{
    Player myPlayer;
    HeartManager HeartManager;
    [SerializeField] GameObject AfterStuntMessage, safeZone, rubblesStopper, ragdollSpawn, rubbleBlocker, givenDistance, annotationFollower;
    [SerializeField] LineRenderer endOfAnnotation;
    string pronoun, pNoun, playerName, playerGender, question, errorMessage;
    bool answerIs;
    public float distance, gameTime, Speed, elapsed, currentPos;
    CeillingGenerator theCeiling;
    StageManager sm = new StageManager();
    Annotation dimensionLine;
    QuestionControllerVThree qc;
    IndicatorManager followLine;

    void Start()
    {
        RumblingManager.isCrumbling = false;
        sm.SetGameLevel(1);
        sm.SetDifficulty(1);
        qc = FindObjectOfType<QuestionControllerVThree>();
        dimensionLine = givenDistance.GetComponent<Annotation>();
        followLine = annotationFollower.GetComponent<IndicatorManager>();
        theCeiling = FindObjectOfType<CeillingGenerator>();
        myPlayer = FindObjectOfType<Player>();
        HeartManager = FindObjectOfType<HeartManager>();
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        followLine.whatIsAsk = UnitOf.velocity;
        qc.Unit(followLine.whatIsAsk);
        VelocityEasyStage1SetUp();
    }
    void FixedUpdate()
    {
        followLine.distanceTraveled = currentPos;
        float answer = qc.GetPlayerAnswer();
        if (SimulationManager.isAnswered)
        {
            currentPos = myPlayer.transform.position.x;
            givenDistance.SetActive(false);
            myPlayer.moveSpeed = answer;
            qc.timer = elapsed.ToString("f2") + "s";
            elapsed += Time.fixedDeltaTime;
            annotationFollower.SetActive(true);
            followLine.playerVelocity = answer;
            if (elapsed >= gameTime)
            {
                RumblingManager.isCrumbling = true;
                rubblesStopper.SetActive(false);
                StartCoroutine(StuntResult());
                myPlayer.moveSpeed = 0;
                SimulationManager.isAnswered = false;
                qc.timer = gameTime.ToString("f2") + "s";
                if ((answer == Speed))
                {
                    followLine.valueIs = TextColorMode.Correct;
                    currentPos = distance;
                    rubbleBlocker.SetActive(true);
                    errorMessage = PlayerPrefs.GetString("Name") + " is <color=green>safe</color>!";
                    answerIs = true;
                    myPlayer.transform.position = new Vector2(currentPos, myPlayer.transform.position.y);
                }
                else
                {
                    currentPos = answer * gameTime;
                    followLine.valueIs = TextColorMode.Wrong;
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
                    answerIs = false;
                    
                    if (answer < Speed)
                    {
                        myPlayer.transform.position = new Vector2(currentPos - 0.2f, myPlayer.transform.position.y);
                        errorMessage = PlayerPrefs.GetString("Name") + " ran too slow and " + pronoun + " stopped before the safe spot.\nThe correct answer is <color=red>" + Speed + "m/s</color>.";
                    }
                    else //if(answer > Speed)
                    {
                        myPlayer.transform.position = new Vector2(currentPos + 0.2f, myPlayer.transform.position.y);
                        errorMessage = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " ran too fast and " + pronoun + " stopped after the safe spot.\nThe correct answer is <color=red>" + Speed + "m/s</color>.";
                    }
                }
            }
        }
    }
    public void VelocityEasyStage1SetUp()
    {
        followLine.valueIs = TextColorMode.Given;
        followLine.whatIsAsk = UnitOf.velocity;
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
        qc.timer = "0.00s";
        myPlayer.transform.position = new Vector2(0f, myPlayer.transform.position.y);
        elapsed = 0;
        rubblesStopper.SetActive(true);
        SimulationManager.isAnswered = false;
        AfterStuntMessage.SetActive(false);
        question = "The ceiling is crumbling and the safe area is <color=red>" + distance.ToString() + " meters</color> away from " + playerName + ". If " + pronoun + " has exactly <color=#006400>" + gameTime.ToString() + " seconds</color> to go to the safe spot, what should be " + pNoun + " <color=purple>velocity</color>?";
        qc.SetQuestion(question);
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
}
