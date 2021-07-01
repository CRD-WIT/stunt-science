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
    IndicatorManagerV1_1 dimensionLine;
    QuestionControllerVThree qc;

    // Start is cdimensionLineled before the first frame update

    public void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        theCeiling = FindObjectOfType<CeillingGenerator>();
        myPlayer = FindObjectOfType<PlayerV1_1>();
        HeartManager = FindObjectOfType<HeartManager>();
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        Scorer = FindObjectOfType<ScoreManager>();
        dimensionLine = FindObjectOfType<IndicatorManagerV1_1>();
        Stage3SetUp();
    }
    void FixedUpdate()
    {
        answer = qc.GetPlayerAnswer();
        if (SimulationManager.stage3Flag)
        {
            float totalDistance = 40f;
            initialDistance = totalDistance - answer;
            dimensionLine.distanceSpawnPnt.x = initialDistance;
            dimensionLine.timeSpawnPnt.x = initialDistance;
            dimensionLine.showLines(answer, answer, null, Speed, gameTime);
        }
        if (SimulationManager.isAnswered)
        {
            dimensionLine.ShowVelocityLabel(true);
            dimensionLine.SetPlayerPosition(myPlayer.transform.position);
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
                        myPlayer.transform.position = new Vector2(currentPos + 0.4f, myPlayer.transform.position.y);
                        answerMessage = PlayerPrefs.GetString("Name") + " ran too near from the manhole and " + pronoun + " stopped after the safe spot.\nThe correct answer is <color=red>" + distance + "m</color>.";
                    }
                    else
                    {
                        myPlayer.transform.position = new Vector2(currentPos - 0.4f, myPlayer.transform.position.y);
                        answerMessage = PlayerPrefs.GetString("Name") + " ran too far from the manhole and " + pronoun + " stopped before the safe spot.\nThe correct answer is <color=red>" + distance + "m</color>.";
                    }
                }
                dimensionLine.AnswerIs(answerIs, true);
            }
            dimensionLine.IsRunning(answer, myPlayer.transform.position.x - (40 - answer), elapsed, myPlayer.transform.position.x - (40 - answer));
        }
    }
    public void Stage3SetUp()
    {
        dimensionLine.showLines(null, null, null, 0, 0);
        dimensionLine.ShowVelocityLabel(false);
        string question;
        qc.SetUnitTo(UnitOf.distance);
        templeBeam.SetActive(false);
        distance = 0;
        rubblesStopper.SetActive(true);
        slidePlatform.SetActive(true);
        lowerGround.SetActive(false);
        manholeCover.SetActive(true);
        if (playerGender == "MdimensionLinee")
        {
            pronoun = "he";
        }
        else
        {
            pronoun = "she";
        }
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
        ragdollSpawn.transform.position = new Vector2(43.65f, -3);
        qc.timer = "0.00s";
        myPlayer.transform.position = new Vector2(0f, -3);
        elapsed = 0;
        SimulationManager.isAnswered = false;
        question = "The entire ceiling is now crumbling and the only safe way out is for " + playerName + " to jump and slide down the manhole. If " + pronoun + " runs at constant velocity of <color=purple>" + Speed.ToString() + " meters per second</color> for exactly <color=#006400>" + gameTime.ToString() + " seconds</color>, how  <color=red>far</color> from the center of the manhole should " + playerName + " start running so that " + pronoun + " will fdimensionLinel down in it when " + pronoun + " stops?";
        qc.SetQuestion(question);
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(1f);
        SimulationManager.directorIsCalling = true;
        SimulationManager.isStartOfStunt = false;
        yield return new WaitForSeconds(3f);
        qc.ActivateResult(answerMessage, answerIs);
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

