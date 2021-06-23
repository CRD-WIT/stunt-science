using System.Collections;
using UnityEngine;
using GameConfig;

public class VelocityEasyStage1 : MonoBehaviour
{
    PlayerV1_1 myPlayer;
    UnitOf whatIsAsk;
    HeartManager HeartManager;
    [SerializeField] GameObject safeZone, rubblesStopper, ragdollSpawn, rubbleBlocker, givenDistance;
    string pronoun, pNoun, playerName, playerGender, question, errorMessage;
    bool answerIs;
    public float distance, gameTime, Speed, elapsed, currentPos;
    CeillingGenerator theCeiling;
    StageManager sm = new StageManager();
    Annotation1 dimensionLine;
    QuestionControllerVThree qc;

    void Start()
    {
        RumblingManager.isCrumbling = false;
        sm.SetGameLevel(1);
        sm.SetDifficulty(1);
        qc = FindObjectOfType<QuestionControllerVThree>();
        dimensionLine = givenDistance.GetComponent<Annotation1>();
        theCeiling = FindObjectOfType<CeillingGenerator>();
        myPlayer = FindObjectOfType<PlayerV1_1>();
        HeartManager = FindObjectOfType<HeartManager>();
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        whatIsAsk = UnitOf.velocity;
        dimensionLine.Show(false);
        VelocityEasyStage1SetUp();
    }
    void FixedUpdate()
    {
        float answer = qc.GetPlayerAnswer();
        if (SimulationManager.isAnswered)
        {
            dimensionLine.PlayerAnswerIs(answer);
            currentPos = answer * elapsed;
            myPlayer.moveSpeed = answer;
            qc.timer = elapsed.ToString("f2") + "s";
            elapsed += Time.fixedDeltaTime;
            if (elapsed >= gameTime)
            {
                RumblingManager.isCrumbling = true;
                rubblesStopper.SetActive(false);
                StartCoroutine(StuntResult());
                myPlayer.moveSpeed = 0;
                SimulationManager.isAnswered = false;
                elapsed =gameTime;
                qc.timer = gameTime.ToString("f2") + "s";
                if ((answer == Speed))
                {
                    currentPos = distance;
                    rubbleBlocker.SetActive(true);
                    errorMessage = PlayerPrefs.GetString("Name") + " is <color=green>safe</color>!";
                    answerIs = true;
                    myPlayer.transform.position = new Vector2(currentPos, myPlayer.transform.position.y);
                }
                else
                {
                    currentPos = answer * gameTime;
                    HeartManager.ReduceLife();
                    if (SimulationManager.isRagdollActive)
                    {
                        myPlayer.lost = false;
                        myPlayer.happy = false;
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
                        errorMessage = PlayerPrefs.GetString("Name") + " ran too fast and " + pronoun + " stopped after the safe spot.\nThe correct answer is <color=red>" + Speed + "m/s</color>.";
                    }
                }
                dimensionLine.AnswerIs(answerIs);
            }
        dimensionLine.IsRunning(currentPos, elapsed, null);
        }
        SimulationManager.isAnswerCorrect = answerIs;
        dimensionLine.SetPlayerPosition(myPlayer.transform.position);
    }
    public void VelocityEasyStage1SetUp()
    {
        qc.SetUnitTo(whatIsAsk);
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
        while ((Speed < 1.5f) || (Speed > 10.4f))
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

        // givenDistance.SetActive(true);
        dimensionLine.Variables(distance, gameTime, Speed, 'v', null);

        theCeiling.createQuadtilemap(qc.stage);
        safeZone.transform.position = new Vector2(distance, -2);
        qc.timer = "0.00s";
        myPlayer.transform.position = new Vector2(0f, myPlayer.transform.position.y);
        elapsed = 0;
        rubblesStopper.SetActive(true);
        SimulationManager.isAnswered = false;
        
        qc.limit = 14f;
        question = "The ceiling is crumbling and the safe area is <color=red>" + distance.ToString() + " meters</color> away from " + playerName + ". If " + pronoun + " has exactly <color=#006400>" + gameTime.ToString() + " seconds</color> to go to the safe spot, what should be " + pNoun + " <color=purple>velocity</color>?";
        qc.SetQuestion(question);
        dimensionLine.Show(true);
        dimensionLine.SetPlayerPosition(myPlayer.transform.position);
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
