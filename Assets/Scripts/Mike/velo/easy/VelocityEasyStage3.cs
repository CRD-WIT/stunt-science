using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameConfig;

public class VelocityEasyStage3 : MonoBehaviour
{
    // Stunt Guide
    public GameObject[] stuntGuideObjectPrefabs;
    public Image stuntGuideImage;
    public Sprite stuntGuideImageSprite;
    // End of Stunt Guide
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
    public QuestionControllerVThree qc;
    public TMP_Text debugAnswer;
    public AudioSource scream;
    public FirebaseManager firebaseManager;
    AnswerGuards answerGuards = new AnswerGuards();

    // Start is cdimensionLineled before the first frame update

    public void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//to prevent screen from sleeping
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

        firebaseManager.GameLogMutation(1, 1, "Easy", Actions.Started, 0);

        Stage3SetUp();

    }
    void FixedUpdate()
    {
        //Stunt Guide
        stuntGuideObjectPrefabs[0].SetActive(false);
        stuntGuideObjectPrefabs[1].SetActive(false);
        stuntGuideObjectPrefabs[2].SetActive(true);
        stuntGuideImage.sprite = stuntGuideImageSprite;

        answer = qc.GetPlayerAnswer();
        debugAnswer.SetText($"Answer: {distance}");
        if (SimulationManager.stage3Flag)
        {
            float totalDistance = 40f;
            initialDistance = totalDistance - answer;
            dimensionLine.GetComponent<IndicatorManagerV1_1>().distanceSpawnPnt.x = initialDistance;
            // dimensionLine.GetComponent<IndicatorManagerV1_1>().timeSpawnPnt.x = initialDistance;
            dimensionLine.GetComponent<IndicatorManagerV1_1>().showLines(answer, null, Speed, gameTime);
        }
        if (SimulationManager.isAnswered)
        {
            dimensionLine.GetComponent<IndicatorManagerV1_1>().ShowVelocityLabel(true);
            dimensionLine.GetComponent<IndicatorManagerV1_1>().SetPlayerPosition(myPlayer.transform.position);
            myPlayer.moveSpeed = Speed;
            qc.timer = elapsed.ToString("f2") + "s";
            elapsed += Time.fixedDeltaTime;
            StartCoroutine(RagdollCollider());
            if (elapsed >= gameTime)
            {
                StartCoroutine(StuntResult());
                SimulationManager.isAnswered = false;
                RumblingManager.isCrumbling = true;
                rubblesStopper.SetActive(false);
                myPlayer.moveSpeed = 0;
                qc.timer = gameTime.ToString("f2") + "s";
                StartCoroutine(ManholeCover());
                if (SimulationManager.adjustedAnswer == distance)//((answer == distance))
                {
                    myPlayer.slide = true;
                    elapsed = gameTime;
                    // answerMessage = PlayerPrefs.GetString("Name") + " is finaly <b><color=green>safe</color></b>.";
                    answerMessage = $"<b>{playerName}</b> successfully performed the stunt and went in the manhole!";
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
                    if (SimulationManager.adjustedAnswer<distance) //(answer < distance)
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
                    // answerMessage = $"<b>{playerName}</b> has unable to stop exactly at the center of the manhole and enter it. Stunt Failed!";
                }
                dimensionLine.GetComponent<IndicatorManagerV1_1>().AnswerIs(answerIs, true);
            }
            dimensionLine.GetComponent<IndicatorManagerV1_1>().IsRunning(answer, myPlayer.transform.position.x - (40 - answer));
        }
        dimensionLine.GetComponent<IndicatorManagerV1_1>().SetPlayerPosition(myPlayer.transform.position);
    }
    public void Stage3SetUp()
    {
        Debug.Log("Setup launched");
        string question;

        qc.SetUnitTo(UnitOf.distance);
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
        qc.limit = 10 * 3.5f;
        HeartManager.losslife = false;
        myPlayer.lost = false;
        myPlayer.standup = false;
        RumblingManager.shakeON = true;
        theCeiling.createQuadtilemap(qc.stage);
        safeZone.transform.position = new Vector2(40, -3);
        safeZone.GetComponent<BoxCollider2D>().enabled = false;
        ragdollSpawn.SetActive(true);
        ragdollSpawn.transform.position = new Vector2(41.2f, -3);
        qc.timer = "0.00s";
        myPlayer.transform.position = new Vector2(0f, -3);
        elapsed = 0;
        SimulationManager.isAnswered = false;
        question = $"<b>{playerName}</b> is instructed to run and stop exactly on the center of the manhole and enter it before the entire ceiling crumbles down. If <b>{pronoun}</b> runs at a constant velocity of <b>{Speed.ToString("f2")} {qc.Unit(UnitOf.velocity)}</b> for exactly <b>{gameTime}{qc.Unit(UnitOf.time)}</b>, how far should <b>{playerName}</b> be from the manhole before she starts running?";
        qc.SetQuestion(question);

        dimensionLine.GetComponent<IndicatorManagerV1_1>().UnknownIs('d');
        dimensionLine.GetComponent<IndicatorManagerV1_1>().showLines(distance, null, Speed, gameTime);
        dimensionLine.GetComponent<IndicatorManagerV1_1>().distanceSpawnPnt = new Vector2(40 - distance, -2);
        // dimensionLine.GetComponent<IndicatorManagerV1_1>().timeSpawnPnt = new Vector2(40-distance,-2.25f);
        dimensionLine.GetComponent<IndicatorManagerV1_1>().ShowVelocityLabel(true);
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(1f);
        SimulationManager.directorIsCalling = true;
        SimulationManager.isStartOfStunt = false;
        yield return new WaitForSeconds(3f);
        // True means the level is complete.
        qc.ActivateResult(answerMessage, answerIs, true);
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

