using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GameConfig;
using TMPro;

public class VelocityEasyStage1 : MonoBehaviour
{
    // Stunt Guide
    public GameObject[] stuntGuideObjectPrefabs;
    public Image stuntGuideImage;
    public Sprite stuntGuideImageSprite;
    // End of Stunt Guide

    AnswerGuards answerGuards = new AnswerGuards();
    public FirebaseManager firebaseManager;
    PlayerV1_1 myPlayer;
    UnitOf whatIsAsk;
    HeartManager HeartManager;
    [SerializeField] GameObject safeZone, rubblesStopper, ragdollSpawn, rubbleBlocker;
    string pronoun, pNoun, playerName, playerGender, question, errorMessage;
    bool answerIs;
    public float distance, gameTime, Speed, elapsed, currentPos;
    CeillingGenerator theCeiling;
    StageManager sm = new StageManager();
    public AudioSource scream;
    IndicatorManagerV1_1 labels;
    public QuestionControllerVThree questionController;

    public Settings settings;

    public TMP_Text debugAnswer;
    public AudioSource lightssfx, camerasfx, actionsfx, cutsfx;

    bool answerAdjusted;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//to prevent screen from sleeping
        firebaseManager.GameLogMutation(1, 1, "Easy", Actions.Started, 0);
        settings.ResetLife();
        RumblingManager.isCrumbling = false;
        sm.SetGameLevel(1);
        sm.SetDifficulty(1);
        labels = FindObjectOfType<IndicatorManagerV1_1>();
        theCeiling = FindObjectOfType<CeillingGenerator>();
        myPlayer = FindObjectOfType<PlayerV1_1>();
        HeartManager = FindObjectOfType<HeartManager>();
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        whatIsAsk = UnitOf.velocity;
        VelocityEasyStage1SetUp();



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

        PlayerPrefs.SetString("LevelNumber", "1");

    }
    void FixedUpdate()
    {
        //Stunt Guide
        stuntGuideObjectPrefabs[0].SetActive(true);
        stuntGuideObjectPrefabs[1].SetActive(false);
        stuntGuideObjectPrefabs[2].SetActive(false);
        stuntGuideImage.sprite = stuntGuideImageSprite;

        float answer = questionController.GetPlayerAnswer();
        debugAnswer.SetText($"Answer: {Speed}");
        float adjustedAnswer = questionController.AnswerTolerance(Speed);
        if (SimulationManager.isAnswered)
        {
            labels.distanceSpawnPnt = new Vector2(0, -2);
            currentPos = answer * elapsed;
            myPlayer.moveSpeed = answer;
            questionController.timer = elapsed.ToString("f2") + "s";
            elapsed += Time.fixedDeltaTime;
            if (elapsed >= gameTime)
            {
                RumblingManager.isCrumbling = true;
                rubblesStopper.SetActive(false);
                StartCoroutine(StuntResult());
                myPlayer.moveSpeed = 0;
                SimulationManager.isAnswered = false;
                elapsed = gameTime;
                questionController.timer = gameTime.ToString("f2") + "s";
                if (adjustedAnswer == Speed)//((answer == Speed))
                {
                    currentPos = distance;
                    rubbleBlocker.SetActive(true);
                    errorMessage = $"<b>{playerName}</b> ran at precise speed to stop exactly at the safo spot.\n\nStunt succesesfully executed";             answerIs = true;
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
                    if (adjustedAnswer < Speed)//(answer < Speed)
                    {
                        scream.Play();
                        myPlayer.transform.position = new Vector2(currentPos - 0.2f, myPlayer.transform.position.y);
                        errorMessage = PlayerPrefs.GetString("Name") + " ran too slow and was unable to stop at the exact safe spot.\n\nStunt failed! The correct answer is <color=red>" + Speed + "m/s</color>.";
                    }
                    else //if(answer > Speed)
                    {
                        scream.Play();
                        myPlayer.transform.position = new Vector2(currentPos + 0.2f, myPlayer.transform.position.y);
                        errorMessage = PlayerPrefs.GetString("Name") + " ran too fast and was unable to stop at the exact safe spot.\n\nStunt failed! The correct answer is <color=red>" + Speed + "m/s</color>.";
                    }
                    errorMessage = $"<b>{playerName}</b> has unable to stop exactly at the safe spot. Stunt Failed!";
                    labels.ShowCorrectDistance(distance, true, new Vector2(0, 1.25f));
                }
                labels.AnswerIs(answerIs, true);
            }
            labels.IsRunning(answer, currentPos);
        }
        SimulationManager.isAnswerCorrect = answerIs;
        labels.SetPlayerPosition(myPlayer.transform.position);
    }
    public void VelocityEasyStage1SetUp()
    {
        questionController.SetUnitTo(whatIsAsk);
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
        questionController.limit = 10.4f;
        rubbleBlocker.SetActive(false);
        ragdollSpawn.SetActive(true);
        HeartManager.losslife = false;
        RumblingManager.shakeON = true;

        // labels.timeSpawnPnt = new Vector2(0, -2.25f);
        labels.distanceSpawnPnt = new Vector2(0, -2);
        labels.showLines(distance, null, Speed, gameTime);
        labels.UnknownIs('v');

        theCeiling.createQuadtilemap(questionController.stage);
        safeZone.transform.position = new Vector2(distance, -2);
        questionController.timer = "0.00s";
        myPlayer.transform.position = new Vector2(0f, myPlayer.transform.position.y);
        elapsed = 0;
        rubblesStopper.SetActive(true);
        SimulationManager.isAnswered = false;

        question = $"<b>{playerName}</b> is instructed to run and stop exactly at the safe spot <b>{distance.ToString("f2")}{questionController.Unit(UnitOf.distance)}</b> away before the ceiling crumbles down. If <b>{playerName}</b> runs for exactly <b>{gameTime.ToString("f2")}{questionController.Unit(UnitOf.time)}</b> before stopping, what should be {pNoun} velocity so {pronoun} will stop exactly at the safe spot?";
        questionController.SetQuestion(question);
        labels.SetPlayerPosition(myPlayer.transform.position);
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(1f);
        SimulationManager.directorIsCalling = true;
        SimulationManager.isStartOfStunt = false;
        yield return new WaitForSeconds(3f);
        rubbleBlocker.SetActive(false);
        questionController.ActivateResult(errorMessage, answerIs);
    }
}