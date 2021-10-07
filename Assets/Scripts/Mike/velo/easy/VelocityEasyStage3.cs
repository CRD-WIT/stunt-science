using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameConfig;

public class VelocityEasyStage3 : MonoBehaviour
{
    public ScoreManager Scorer;
    public PlayerV1_1 myPlayer;
    public CeillingGenerator theCeiling;
    public HeartManager HeartManager;
    public float distance, gameTime, Speed, elapsed, currentPos;
    string pronoun, playerName, playerGender, answerMessage;
    public GameObject slidePlatform, lowerGround, safeZone, rubblesStopper, ragdollSpawn, manholeCover, templeBeam;
    bool answerIs;
    float answer, initialDistance;
    public GameObject dimensionLine;
    public QuestionControllerVThree questionController;
    public TMP_Text debugAnswer;
    public AudioSource scream;

    public FirebaseManager firebaseManager;

    // Start is cdimensionLineled before the first frame update

    public void Start()
    {
        playerGender = PlayerPrefs.GetString("Gender");
        if (playerGender == "Male")
        {
            pronoun = "he";
        }
        else
        {
            pronoun = "she";
        }
        playerName = PlayerPrefs.GetString("Name");

        Stage3SetUp();

        switch (questionController.levelDifficulty)
        {
            case Difficulty.Medium:
                PlayerPrefs.SetString("DifficultyName", "Medium"); break;
            case Difficulty.Hard:
                PlayerPrefs.SetString("DifficultyName", "Hard"); break;
            default:
                PlayerPrefs.SetString("DifficultyName", "Easy");
                break;
        }

        PlayerPrefs.SetString("LevelNumber", questionController.levelNumber.ToString());
        firebaseManager.GameLogMutation(1, 3, "Easy", Actions.StartedStage, 0);
    }
    void FixedUpdate()
    {
        answer = questionController.GetPlayerAnswer();
        debugAnswer.SetText($"Answer: {distance}");
        if (SimulationManager.stage3Flag)
        {
            float totalDistance = 40f;
            initialDistance = totalDistance - answer;
            dimensionLine.GetComponent<IndicatorManagerV1_1>().distanceSpawnPnt.x = initialDistance;
            dimensionLine.GetComponent<IndicatorManagerV1_1>().timeSpawnPnt.x = initialDistance;
            dimensionLine.GetComponent<IndicatorManagerV1_1>().showLines(answer, answer, null, Speed, gameTime);
        }
        if (SimulationManager.isAnswered)
        {
            dimensionLine.GetComponent<IndicatorManagerV1_1>().ShowVelocityLabel(true);
            dimensionLine.GetComponent<IndicatorManagerV1_1>().SetPlayerPosition(myPlayer.transform.position);
            myPlayer.moveSpeed = Speed;
            questionController.timer = elapsed.ToString("f2") + "s";
            elapsed += Time.fixedDeltaTime;
            StartCoroutine(RagdollCollider());
            if (elapsed >= gameTime)
            {
                StartCoroutine(StuntResult());
                SimulationManager.isAnswered = false;
                RumblingManager.isCrumbling = true;
                rubblesStopper.SetActive(false);
                myPlayer.moveSpeed = 0;
                questionController.timer = gameTime.ToString("f2") + "s";
                StartCoroutine(ManholeCover());
                if ((answer == distance))
                {
                    myPlayer.slide = true;
                    elapsed = gameTime;
                    answerMessage = PlayerPrefs.GetString("Name") + " is finaly <b><color=green>safe</color></b>.";
                    answerIs = true;
                    myPlayer.transform.position = new Vector2(40, myPlayer.transform.position.y);
                }
                else
                {
                    HeartManager.ReduceLife();
                    answerIs = false;
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
                    currentPos = (40 - answer) + (Speed * gameTime);
                    if (answer < distance)
                    {
                        scream.Play();
                        myPlayer.transform.position = new Vector2(currentPos + 0.4f, myPlayer.transform.position.y);
                        answerMessage = PlayerPrefs.GetString("Name") + " ran too near from the manhole and " + pronoun + " stopped after the safe spot.\nThe correct answer is <color=red>" + distance + "m</color>.";
                    }
                    else
                    {
                        scream.Play();
                        myPlayer.transform.position = new Vector2(currentPos - 0.4f, myPlayer.transform.position.y);
                        answerMessage = PlayerPrefs.GetString("Name") + " ran too far from the manhole and " + pronoun + " stopped before the safe spot.\nThe correct answer is <color=red>" + distance + "m</color>.";
                    }
                }
                dimensionLine.GetComponent<IndicatorManagerV1_1>().AnswerIs(answerIs, true);
            }
            dimensionLine.GetComponent<IndicatorManagerV1_1>().IsRunning(answer, myPlayer.transform.position.x - (40 - answer), elapsed, myPlayer.transform.position.x - (40 - answer));
        }
    }
    public void Stage3SetUp()
    {
        Debug.Log("Setup launched");
        string question;
        dimensionLine.GetComponent<IndicatorManagerV1_1>().showLines(null, null, null, 0, 0);
        dimensionLine.GetComponent<IndicatorManagerV1_1>().ShowVelocityLabel(false);

        questionController.SetUnitTo(UnitOf.distance);
        templeBeam.SetActive(false);
        distance = 0;
        rubblesStopper.SetActive(true);
        slidePlatform.SetActive(true);
        lowerGround.SetActive(false);
        manholeCover.SetActive(true);
        float v = Random.Range(9f, 10f);
        Speed = (float)System.Math.Round(v, 2);
        float t = Random.Range(3f, 3.5f);
        gameTime = (float)System.Math.Round(t, 2);
        distance = (float)System.Math.Round((Speed * gameTime), 2);
        questionController.limit = 10 * 3.5f;
        HeartManager.losslife = false;
        myPlayer.lost = false;
        myPlayer.standup = false;
        RumblingManager.shakeON = true;
        theCeiling.createQuadtilemap(questionController.stage);
        safeZone.transform.position = new Vector2(40, -3);
        safeZone.GetComponent<BoxCollider2D>().enabled = false;
        ragdollSpawn.SetActive(true);
        ragdollSpawn.transform.position = new Vector2(41.2f, -3);
        questionController.timer = "0.00s";
        myPlayer.transform.position = new Vector2(0f, -3);
        elapsed = 0;
        SimulationManager.isAnswered = false;
        question = $"The entire ceiling is now crumbling and the only safe way out is for <b>{playerName}</b> to jump and slide down the manhole. If {pronoun} runs at constant velocity of <color=red>{Speed.ToString()} meters</color> per second for exactly <color=#006400>{gameTime.ToString()} seconds</color>, <color=purple>how far</color> from the center of the manhole should <b>{playerName}</b> start running so that {pronoun} will fall down in it when {pronoun} stops?";
        questionController.SetQuestion(question);
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(1f);
        SimulationManager.directorIsCalling = true;
        SimulationManager.isStartOfStunt = false;
        yield return new WaitForSeconds(3f);
        // True means the level is complete.
        questionController.ActivateResult(answerMessage, answerIs, true);
    }
    IEnumerator RagdollCollider()
    {
        yield return new WaitUntil(() => SimulationManager.isRagdollActive);
        ragdollSpawn.SetActive(false);
    }
    IEnumerator ManholeCover()
    {
        manholeCover.SetActive(false);
        yield return new WaitForSeconds(1f);
        manholeCover.SetActive(true);
    }
}

