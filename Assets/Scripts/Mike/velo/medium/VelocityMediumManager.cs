using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameConfig;

public class VelocityMediumManager : MonoBehaviour
{
    QuestionControllerVThree qc;
    UnitOf whatIsAsk;
    // IndicatorManager labels;
    StageManager sm = new StageManager();
    [SerializeField] GameObject boulder, directorsBubble, floor, boulderA;
    [SerializeField] LineRenderer EndOfAnnotation;
    Annotation1 distanceMeassure;
    PlayerV2 myPlayer;
    HeartManager life;
    ScoreManager score;
    CeillingGenerator createCeilling;
    [SerializeField] TMP_Text directorsSpeech;
    [SerializeField] float playerVelocity, boulderVelocity, boulder2Velocity, stuntTime, distance, jumpDistance, correctAnswer, currentBoulderPos;
    public static int stage;
    Rigidbody2D boulderRB, boulder2RB;
    GameObject ragdoll;
    RockDestroyer clearBoulders;
    float playerPos, playerAnswer, elapsed, distanceTraveled, currentPlayerPos, jumpTime, jumpForce, playerDistance;
    Vector2 labelStartPoint;
    string question, playerName, playerGender, pronoun, pPronoun, messageTxt;
    bool isStartOfStunt, directorIsCalling, isAnswered, isAnswerCorrect, isEndOfStunt;
    [SerializeField] TMP_Text playerSpeed, boulder1Speed, boulder2Speed;
    float correctD;
    Vector2 spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        // labels = FindObjectOfType<IndicatorManager>();
        myPlayer = FindObjectOfType<PlayerV2>();
        distanceMeassure = FindObjectOfType<Annotation1>();
        createCeilling = FindObjectOfType<CeillingGenerator>();
        boulderRB = boulder.GetComponent<Rigidbody2D>();
        boulder2RB = boulderA.GetComponent<Rigidbody2D>();
        life = FindObjectOfType<HeartManager>();
        score = FindObjectOfType<ScoreManager>();

        RagdollV2.myPlayer = myPlayer;

        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        sm.SetGameLevel(1);
        playerPos = myPlayer.gameObject.transform.position.x;
        if (playerGender == "Male")
        {
            pronoun = "he";
            pPronoun = "him";
        }
        else
        {
            pronoun = "she";
            pPronoun = "her";
        }
        qc.levelName = sm.GetGameLevel();
        qc.levelDifficulty = Difficulty.Medium;
        VeloMediumSetUp();
    }
    // Update is called once per frame
    void Update()
    {
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        if (isAnswered)
        {
            // labels.gameObject.SetActive(true);
            playerAnswer = qc.GetPlayerAnswer();
            distanceMeassure.PlayerAnswerIs(playerAnswer);
            qc.timer = elapsed.ToString("f2") + "s";
            // labels.stuntTime = elapsed;
            elapsed += Time.deltaTime;
            myPlayer.moveSpeed = playerVelocity;
            distanceMeassure.spawnPoint = spawnPoint;
            switch (stage)
            {
                case 1:
                    currentPlayerPos = myPlayer.transform.position.x;
                    currentBoulderPos = boulderA.transform.position.x;
                    FallingBoulders.dropPoint = currentPlayerPos - 0.7f;
                    spawnPoint = new Vector2(currentPlayerPos, 1);
                    boulder2RB.velocity = new Vector2(-boulderVelocity, 0);
                    distanceTraveled = currentBoulderPos - currentPlayerPos;
                    if (playerAnswer <= elapsed)
                    {
                        distanceTraveled = distance - (boulderVelocity * stuntTime) - (playerAnswer * playerVelocity);
                        spawnPoint = new Vector2(playerAnswer * playerVelocity, 1);
                        StartCoroutine(Jump());
                        elapsed = playerAnswer;
                        // qc.timer = playerAnswer.ToString("f2") + "s";
                        if (playerAnswer == stuntTime)
                        {
                            spawnPoint = new Vector2(stuntTime * playerVelocity, 1);
                            distanceTraveled = jumpDistance;
                            // labels.valueIs = TextColorMode.Correct;
                            isAnswerCorrect = true;
                            messageTxt = playerName + " has jumped over the boulder <color=green>safely</color>!";
                        }
                        else
                        {
                            // TODO: label to be added
                            // labels.valueIs = TextColorMode.Wrong;
                            isAnswerCorrect = false;
                            if (playerAnswer < stuntTime)
                            {
                                messageTxt = playerName + " jumped too soon and hit the boulder.\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                            }
                            else
                                messageTxt = playerName + " jumped too late and hit the boulder.\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                        }
                        distanceMeassure.AnswerIs(isAnswerCorrect);
                    }
                    break;
                case 2:
                    FallingBoulders.dropPoint = boulder.transform.position.x - 0.7f;
                    currentPlayerPos = myPlayer.transform.position.x - distance;
                    spawnPoint = new Vector2(distance, 0.25F);
                    distanceTraveled = currentPlayerPos;
                    boulderRB.velocity = new Vector2(boulder2Velocity, 0);
                    if (stuntTime <= elapsed)
                    {
                        distanceTraveled = playerAnswer;
                        if (playerAnswer == correctAnswer)
                        {
                            // labels.valueIs = TextColorMode.Correct;
                            isAnswerCorrect = true;
                            elapsed = stuntTime;
                            messageTxt = playerName + " has jumped over the boulder <color=green>safely</color>!";
                        }
                        else
                        {
                            // labels.valueIs = TextColorMode.Wrong;
                            isAnswerCorrect = false;
                            elapsed = playerAnswer;
                            if (playerAnswer < stuntTime)
                            {
                                messageTxt = playerName + " jumped too far from the boulder, and hits it!\nThe correct answer is <color=red>" + correctAnswer + " meters</color>.";
                            }
                            else
                                messageTxt = playerName + " jumped too near from the boulder, and hits it!\nThe correct answer is <color=red>" + correctAnswer + " meters</color>.";
                        }
                        StartCoroutine(Jump());
                        distanceMeassure.AnswerIs(isAnswerCorrect);
                    }
                    break;
                case 3:
                    FallingBoulders.dropPoint = (boulder.transform.position.x - 0.7f);
                    FallingBoulders.moreDropPoint = (boulderA.transform.position.x + 0.7f);
                    currentPlayerPos = myPlayer.transform.position.x - playerDistance;
                    spawnPoint = labelStartPoint;
                    distanceTraveled = currentPlayerPos;
                    myPlayer.moveSpeed = playerAnswer;
                    boulderRB.velocity = new Vector2(boulderVelocity, 0);
                    boulder2RB.velocity = new Vector2(-boulder2Velocity, 0);
                    if (stuntTime <= elapsed)
                    {
                        myPlayer.moveSpeed = 0;
                        distanceTraveled = (float)System.Math.Round((playerAnswer * stuntTime), 2);
                        elapsed = stuntTime;
                        boulderRB.velocity = new Vector2(boulderRB.velocity.x, boulderRB.velocity.y);
                        boulder2RB.velocity = new Vector2(boulder2RB.velocity.x, boulder2RB.velocity.y);
                        //currentPlayerPos = myPlayer.transform.position.x;
                        if (playerAnswer == correctAnswer)
                        {
                            // labels.valueIs = TextColorMode.Correct;
                            isAnswerCorrect = true;
                            elapsed = stuntTime;
                            messageTxt = playerName + " has jumped over the boulder <color=green>safely</color>!";
                        }
                        else
                        {
                            // labels.valueIs = TextColorMode.Wrong;
                            isAnswerCorrect = false;
                            elapsed = playerAnswer;
                            if (playerAnswer < stuntTime)
                            {
                                messageTxt = playerName + " jumped too far from the boulder, and hits it!\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                            }

                            else
                                messageTxt = playerName + " jumped too n from the boulder, and hits it!\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                        }
                        StartCoroutine(Jump());
                        distanceMeassure.AnswerIs(isAnswerCorrect);
                    }
                    break;
            }
            distanceMeassure.IsRunning(distanceTraveled, elapsed);
        }
        distanceMeassure.SetPlayerPosition(myPlayer.transform.position);
        if (isEndOfStunt)
        {
            StartCoroutine(StuntResult());
        }
        if (qc.isSimulating)
            Play();
        else
        {
            if (qc.nextStage)
                StartCoroutine(Next());
            else if (qc.retried)
                StartCoroutine(Retry());
            else
            {
                qc.nextStage = false;
                qc.retried = false;
            }
        }

    }
    void VeloMediumSetUp()
    {
        FallingBoulders.dropPoint = 0;
        FallingBoulders.moreDropPoint = 0;
        RagdollV2.disableRagdoll = true;
        // distanceMeassure.gameObject.SetActive(true);
        distanceMeassure.Show(true);
        distanceMeassure.spawnPoint = new Vector2(myPlayer.transform.position.x, myPlayer.transform.position.y + 1);
        // labels.valueIs = TextColorMode.Given;
        // labels.gameObject.SetActive(false);
        stage = qc.stage;
        myPlayer.isLanded = false;
        qc.isSimulating = false;
        float Va = 0, Vb = 0, d = 0, Da = 0, Db = 0, Dj = 0, t = 0;//, tm = 0;
        playerAnswer = 0;
        elapsed = 0;
        qc.timer = "0.00s";
        RumblingManager.shakeON = true;
        // distanceMeassure.gameObject.SetActive(true);

        boulder.SetActive(false);
        boulderA.SetActive(false);


        myPlayer.standup = false;
        boulderRB.rotation = 0;
        boulderRB.freezeRotation = true;
        life.losslife = false;
        // boulder.GetComponent<>();

        switch (stage)
        {
            case 1:
                boulderVelocity = 0;
                whatIsAsk = UnitOf.time;
                // labels.whatIsAsk = UnitOf.time;
                boulderA.SetActive(true);
                Va = Random.Range(8f, 9f);
                Vb = Random.Range(4f, 6f);
                d = Random.Range(27f, 30f);

                playerVelocity = (float)System.Math.Round(Va, 2);
                boulderVelocity = (float)System.Math.Round(Vb, 2);
                distance = (float)System.Math.Round(d, 2);
                Dj = ((Va + Vb) / 2) + 0.53f;
                t = (d - Dj) / (Va + Vb);
                stuntTime = (float)System.Math.Round(t, 2);
                Da = playerVelocity * stuntTime;
                Db = boulderVelocity * stuntTime;
                correctAnswer = stuntTime;
                correctD = Da;

                myPlayer.transform.position = new Vector2(0, boulder.transform.position.y);
                myPlayer.gameObject.SetActive(true);

                boulderA.transform.position = new Vector2(distance, boulder.transform.position.y);
                boulder2RB.freezeRotation = false;

                jumpDistance = (float)System.Math.Round(Dj, 2);
                jumpTime = Dj / Va;
                jumpForce = 1.2f / (jumpTime);
                distanceMeassure.Variables(distance, stuntTime, playerVelocity, 't');
                question = playerName + " is instructed to run until the end of the scene while jumping over the rolling boulder. If " + pronoun + " is running at a velocity of <color=purple>" + playerVelocity + " meters per second</color> while an incoming boulder at the front <color=red>" + distance + " meters</color> away is rolling at the velocity of <color=purple>" + boulderVelocity + "meters per second</color>, at after how many <color=#006400>seconds</color> will " + playerName + " jump if " + pronoun + " has to jump at exactly <color=red>" + jumpDistance + " meters</color> away from the boulder in order to jump over it safely?";
                break;
            case 2:
                whatIsAsk = UnitOf.distance;
                boulder.SetActive(true);
                Va = Random.Range(8f, 9f);
                Vb = Random.Range(20f, 22f);
                d = Random.Range(14f, 16f);

                playerVelocity = (float)System.Math.Round(Va, 2);
                boulder2Velocity = (float)System.Math.Round(Vb, 2);
                distance = (float)System.Math.Round(d, 2);
                Dj = (Vb - Va) / 2;
                t = (distance - Dj) / (boulder2Velocity - playerVelocity);
                stuntTime = (float)System.Math.Round(t, 2);
                Da = playerVelocity * stuntTime;
                Db = boulder2Velocity * stuntTime;


                correctAnswer = (float)System.Math.Round(Da, 2);

                boulder.transform.position = new Vector2(playerPos, 0);

                myPlayer.transform.position = new Vector2(distance, boulder.transform.position.y);
                //boulderRB.rotation = 180;
                boulderRB.freezeRotation = false;

                jumpDistance = (float)System.Math.Round(Dj, 2);
                jumpTime = Dj / Va;
                jumpForce = 1.276f / (jumpTime);
                correctD = correctAnswer;
                distanceMeassure.Variables(distance, stuntTime, playerVelocity, 'd');

                question = playerName + " is instructed to run until the end of the scene while jumping over the rolling boulder. If " + pronoun + " is running at a velocity of <color=purple>" + playerVelocity + " meters per second</color> while an incoming fast moving boulder <color=red>" + distance + " meters</color> away is catchind up from behind with a velocity of <color=purple>" + boulder2Velocity + "meters per second</color>, at after how many <color=red>meters</color> should " + playerName + " be jumping if " + pronoun + " has to jump at exactly <color=red>" + jumpDistance + " meters</color> away from the boulder in order to jump over it safely?";
                break;
            case 3:
                whatIsAsk = UnitOf.velocity;
                boulder.SetActive(true);
                boulderA.SetActive(true);
                float Vp, Dp, Tp, Dac = (float)System.Math.Round(Random.Range(19f, 22f), 2);
                Va = Random.Range(7f, 8f);
                Vb = Random.Range(3f, 4f);
                d = Random.Range(29f, 30f);

                boulderVelocity = (float)System.Math.Round(Va, 2);
                boulder2Velocity = (float)System.Math.Round(Vb, 2);
                distance = (float)System.Math.Round(d, 2);

                t = distance / (Va + Vb);
                Tp = t - 0.5f;
                Db = boulder2Velocity * t;
                Da = boulderVelocity * t;
                Dp = Dac - Db;
                playerDistance = d - Dac;

                stuntTime = (float)System.Math.Round(Tp, 2);
                Vp = Dp / stuntTime;

                correctAnswer = (float)System.Math.Round(Vp, 2);
                correctD = Dp;

                boulder.transform.position = new Vector2(0, 0);
                boulderA.transform.position = new Vector2(boulder.transform.position.x + d, 0);
                myPlayer.transform.position = new Vector2(boulderA.transform.position.x - Dac, 0);
                labelStartPoint = new Vector2((boulderA.transform.position.x - Dac), 0.25f);
                // boulderRB.rotation = 180;
                boulderRB.freezeRotation = false;
                jumpTime = 0.5f;
                jumpForce = 1.276f / jumpTime;
                distanceMeassure.Variables(distance, stuntTime, playerVelocity, 'v');
                question = playerName + " is instructed to vertically jump over between two incoming boulders at precisely <color=#006400>0.5 seconds</color> before they collide. If the boulder in front is <color=red>" + Dac + " meters</color> away from " + playerName + " is approaching at <color=purple>" + boulder2Velocity + " meters per second</color>, and the boulder behind " + pPronoun + " is <color=red>" + distance + " meters</color> away from the first boulder and is approaching at <color=purple>" + boulderVelocity + "meters per second</color>, at what <color=purple>velocity</color> should " + pronoun + " run forward for <color=#006400>" + stuntTime + " seconds</color> before doing the vertical jump?";
                break;
        }
        qc.Unit(whatIsAsk);
        playerSpeed.text = playerVelocity.ToString("f2") + "m/s";
        qc.SetColor(playerSpeed, TextColorMode.Given);
        boulder1Speed.text = boulderVelocity.ToString("f2") + "m/s";
        qc.SetColor(boulder1Speed, TextColorMode.Given);
        boulder2Speed.text = boulder2Velocity.ToString("f2") + "m/s";
        qc.SetColor(boulder2Speed, TextColorMode.Given);
        qc.SetQuestion(question);
        myPlayer.jumpforce = jumpForce;
        // distanceMeassure.distance = distance;

        EndOfAnnotation.SetPosition(0, new Vector2(distance, 0));
        EndOfAnnotation.SetPosition(1, new Vector2(distance, 1.5f));

        myPlayer.moveSpeed = 0;
        boulderRB.velocity = new Vector2(0, 0);
        // createCeilling.mapWitdh = distance;
        // createCeilling.createQuadtilemap2();
    }
    public void Play()
    {
        qc.isSimulating = false;
        isStartOfStunt = true;
        directorIsCalling = true;
    }
    IEnumerator Retry()
    {
        FallingBoulders.isRumbling = false;
        qc.retried = false;
        PrefabDestroyer.end = true;
        StartCoroutine(life.endBGgone());
        yield return new WaitForSeconds(3);
        myPlayer.ToggleTrigger();
        myPlayer.transform.position = new Vector2(0, boulder.transform.position.y);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        RumblingManager.isCrumbling = false;
        VeloMediumSetUp();
    }
    IEnumerator Next()
    {
        FallingBoulders.isRumbling = false;
        qc.nextStage = false;
        myPlayer.happy = false;
        PrefabDestroyer.end = true;
        yield return new WaitForSeconds(3f);
        // myPlayer.ToggleTrigger();
        StartCoroutine(life.endBGgone());
        yield return new WaitForSeconds(2.8f);
        myPlayer.transform.position = new Vector2(0, boulder.transform.position.y);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        RumblingManager.isCrumbling = false;
        VeloMediumSetUp();
    }
    public IEnumerator DirectorsCall()
    {
        directorIsCalling = false;
        if (isStartOfStunt)
        {
            isStartOfStunt = false;
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Lights!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Camera!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Action!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "";
            directorsBubble.SetActive(false);
            isAnswered = true;
            // distanceMeassure.gameObject.SetActive(false);
            FallingBoulders.isRumbling = true;
            //boulder.transform.position = new Vector2(boulder.transform.position.x - 0.5f, boulder.transform.position.y);
        }
        else
        {
            RumblingManager.shakeON = false;
            yield return new WaitForSeconds((35 / playerVelocity) - stuntTime);
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            FallingBoulders.isRumbling = false;
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";

            boulderVelocity = 0;
        }
    }
    IEnumerator StuntResult()
    {
        isEndOfStunt = false;
        yield return new WaitForSeconds(1f);
        directorIsCalling = true;
        isStartOfStunt = false;
        yield return new WaitForSeconds(3f);
        qc.ActivateResult(messageTxt, isAnswerCorrect);
    }
    IEnumerator Jump()
    {
        // myPlayer.myAnimator.speed = (jumpForce/2);
        if (playerAnswer != correctAnswer)
        {
            if (stage == 3)
                myPlayer.jumpforce = jumpForce - 0.08f;
            else
                myPlayer.jumpforce = jumpForce - 0.7f;
        }
        myPlayer.jump();
        yield return new WaitForSeconds(jumpTime / 2);
        isAnswered = false;
        yield return new WaitForEndOfFrame();
        int i = 1;
        Debug.Log(i++);
        isEndOfStunt = true;
    }
}


//TODO: add hide and reveal option on the new labelManager....
