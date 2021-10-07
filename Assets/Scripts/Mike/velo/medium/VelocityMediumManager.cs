using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameConfig;

public class VelocityMediumManager : MonoBehaviour
{
    QuestionControllerVThree qc;
    UnitOf whatIsAsk;
    public AudioSource lightssfx, camerasfx, actionsfx, cutsfx;
    public GameObject actionBtn;
    IndicatorManagerV1_1 indicators;
    IndicatorManager jmpDistFromBoulder;
    public GameObject transitionCanvas;
    StageManager sm = new StageManager();
    [SerializeField]
    GameObject boulder, directorsBubble, boulderA, velocityDirectionArrow1,
                velocityDirectionArrow2, boulderShadow, playerShadow, JDIndicator;
    PlayerV2 myPlayer;
    public TMP_Text debugAnswer;
    HeartManager life;
    ScoreManager score;
    CeillingGenerator createCeilling;
    [SerializeField] TMP_Text directorsSpeech;
    [SerializeField] float playerVelocity, boulderVelocity, boulder2Velocity, stuntTime, distance, jumpDistance, correctAnswer, currentBoulderPos;
    public static int stage;
    Rigidbody2D boulderRB, boulder2RB;
    float playerPos, playerAnswer, elapsed, distanceTraveled, currentPlayerPos, jumpTime, jumpForce, playerDistance;
    string question, playerGender, pronoun, pPronoun, messageTxt;
    string playerName = "Juan";
    bool isStartOfStunt, directorIsCalling, isAnswered, isAnswerCorrect, isEndOfStunt, onShadow;
    [SerializeField] TMP_Text playerSpeed, boulder1Speed, boulder2Speed;
    float correctD, timingD;
    GameObject b2Shadow, b1Shadow, pShadow;
    Vector2 spawnPoint;
    public FirebaseManager firebaseManager;
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        indicators = FindObjectOfType<IndicatorManagerV1_1>();
        jmpDistFromBoulder = FindObjectOfType<IndicatorManager>();

        myPlayer = FindObjectOfType<PlayerV2>();
        createCeilling = FindObjectOfType<CeillingGenerator>();
        boulderRB = boulder.GetComponent<Rigidbody2D>();
        boulder2RB = boulderA.GetComponent<Rigidbody2D>();
        life = FindObjectOfType<HeartManager>();
        score = FindObjectOfType<ScoreManager>();

        RagdollV2.myPlayer = myPlayer;

        //playerName = PlayerPrefs.GetString("Name");
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
        qc.levelDifficulty = Difficulty.Medium;
        VeloMediumSetUp();

        switch (qc.levelDifficulty)
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
        firebaseManager.GameLogMutation(1, qc.stage, "Medium", Actions.StartedStage, 0);
    }
    // Update is called once per frame
    void Update()
    {
        debugAnswer.SetText($"Answer: {correctAnswer}");
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        if (isAnswered)
        {
            indicators.distanceSpawnPnt = spawnPoint;
            playerAnswer = qc.GetPlayerAnswer();
            qc.timer = elapsed.ToString("f2") + "s";
            elapsed += Time.deltaTime;
            myPlayer.moveSpeed = playerVelocity;
            timingD = currentPlayerPos;
            switch (stage)
            {
                case 1:
                    currentPlayerPos = myPlayer.transform.position.x;
                    currentBoulderPos = boulderA.transform.position.x;
                    FallingBoulders.dropPoint = currentPlayerPos - 0.7f;
                    spawnPoint = new Vector2(currentBoulderPos - jumpDistance, 1);
                    boulder2RB.velocity = new Vector2(-boulderVelocity, 0);
                    distanceTraveled = jumpDistance;
                    if (playerAnswer <= elapsed)
                    {
                        timingD = playerAnswer * playerVelocity;
                        spawnPoint = new Vector2(stuntTime * playerVelocity, 1);
                        elapsed = playerAnswer;

                        qc.timer = playerAnswer.ToString("f2") + "s";
                        if (playerAnswer == stuntTime)
                        {
                            isAnswerCorrect = true;
                            messageTxt = playerName + " has jumped over the boulder <color=green>safely</color>!";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            if (playerAnswer < stuntTime)
                            {
                                messageTxt = playerName + " jumped too soon and hit the boulder.\nThe correct answer is <color=green>" + stuntTime + " seconds</color>.";
                            }
                            else
                                messageTxt = playerName + " jumped too late and hit the boulder.\nThe correct answer is <color=green>" + stuntTime + " seconds</color>.";

                            indicators.ShowCorrectDistance(jumpDistance, true, new Vector2(stuntTime * playerVelocity, 1.5f));
                            indicators.ShowCorrectTime(stuntTime, stuntTime * playerVelocity, true);
                        }
                        StartCoroutine(Jump());
                        currentBoulderPos = distance - (boulderVelocity * stuntTime);
                        if (!onShadow)
                            StartCoroutine(InstantiateShadows(currentPlayerPos, currentBoulderPos, null));
                        indicators.AnswerIs(isAnswerCorrect, false);
                    }

                    indicators.IsRunning(playerAnswer, distanceTraveled, elapsed, timingD);
                    break;
                case 2:
                    JDIndicator.SetActive(true);
                    FallingBoulders.dropPoint = boulder.transform.position.x - 0.7f;
                    currentPlayerPos = myPlayer.transform.position.x;
                    currentBoulderPos = boulder.transform.position.x + jumpDistance;
                    spawnPoint = new Vector2(distance, 1);
                    distanceTraveled = currentPlayerPos - distance;
                    boulderRB.velocity = new Vector2(boulder2Velocity, 0);
                    if (stuntTime <= elapsed)
                    {
                        indicators.UnknownIs('d');
                        timingD = playerAnswer;
                        distanceTraveled = playerAnswer;
                        if (playerAnswer == correctAnswer)
                        {
                            isAnswerCorrect = true;
                            elapsed = stuntTime;
                            messageTxt = playerName + " has jumped over the boulder <color=green>safely</color>!";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            if (playerAnswer < correctAnswer)
                            {
                                messageTxt = playerName + " jumped too near from the boulder, and hits it!\nThe correct answer is <color=red>" + correctAnswer + " meters</color>.";
                            }
                            else
                                messageTxt = playerName + " jumped too far from the boulder, and hits it!\nThe correct answer is <color=red>" + correctAnswer + " meters</color>.";

                            indicators.ShowCorrectDistance(correctAnswer, true, new Vector2(distance, 1.5f));
                        }
                        StartCoroutine(Jump());
                        currentBoulderPos = (boulder2Velocity * stuntTime) + jumpDistance;
                        if (!onShadow)
                            StartCoroutine(InstantiateShadows(distance + playerAnswer, null, boulder2Velocity * stuntTime));
                        indicators.AnswerIs(isAnswerCorrect, false);
                    }
                    jmpDistFromBoulder.spawnPoint = new Vector2(currentBoulderPos, boulder.transform.position.y);
                    jmpDistFromBoulder.distance = -jumpDistance;

                    indicators.IsRunning(playerAnswer, distanceTraveled, elapsed, timingD);
                    break;
                case 3:
                    JDIndicator.SetActive(false);
                    Vector2 timeSpawn = boulder.transform.position;
                    FallingBoulders.dropPoint = (boulder.transform.position.x - 0.7f);
                    FallingBoulders.moreDropPoint = (boulderA.transform.position.x + 0.7f);
                    currentPlayerPos = myPlayer.transform.position.x - playerDistance;
                    spawnPoint = new Vector2(distance + jmpDistFromBoulder.distance, spawnPoint.y);
                    distanceTraveled = currentPlayerPos;
                    myPlayer.moveSpeed = playerAnswer;
                    boulderRB.velocity = new Vector2(boulderVelocity, 0);
                    boulder2RB.velocity = new Vector2(-boulder2Velocity, 0);
                    if (stuntTime <= elapsed)
                    {
                        timeSpawn = new Vector2(stuntTime * boulderVelocity, timeSpawn.y);
                        timingD = playerAnswer * stuntTime;
                        myPlayer.moveSpeed = 0;
                        distanceTraveled = (float)System.Math.Round((playerAnswer * stuntTime), 2);
                        elapsed = stuntTime;
                        boulderRB.velocity = new Vector2(boulderRB.velocity.x, boulderRB.velocity.y);
                        boulder2RB.velocity = new Vector2(boulder2RB.velocity.x, boulder2RB.velocity.y);
                        if (playerAnswer == correctAnswer)
                        {
                            isAnswerCorrect = true;
                            elapsed = stuntTime;
                            messageTxt = playerName + " has jumped over the boulder <color=green>safely</color>!";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            if (playerAnswer < correctAnswer)
                            {
                                messageTxt = playerName + " jumped too far from the boulder, and hits it!\nThe correct answer is <color=red>" + correctAnswer + " seconds</color>.";
                            }

                            else
                                messageTxt = playerName + " jumped too near from the boulder, and hits it!\nThe correct answer is <color=red>" + correctAnswer + " seconds</color>.";

                            indicators.ShowCorrectDistance(correctD, true, new Vector2(spawnPoint.x, 1.5f));
                        }
                        StartCoroutine(Jump());
                        indicators.AnswerIs(isAnswerCorrect, false);
                    }
                    indicators.timeSpawnPnt = timeSpawn;
                    indicators.IsRunning(playerAnswer, distanceTraveled, 0.5f, 0.5f * (boulderVelocity + boulder2Velocity));
                    break;
            }
        }
        indicators.SetPlayerPosition(myPlayer.transform.position);
        if (isEndOfStunt)
        {
            StartCoroutine(StuntResult());
        }
        if (qc.isSimulating)
            Play();
        else
        {
            if (qc.nextStage)
            {
                StartCoroutine(Next());
            }
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
        JDIndicator.SetActive(false);
        currentBoulderPos = 0;
        onShadow = false;
        boulderRB.sharedMaterial.bounciness = 0.8f;
        boulder2RB.sharedMaterial.bounciness = 0.8f;
        boulderRB.sharedMaterial.friction = 0;
        boulder2RB.sharedMaterial.friction = 0;
        boulderRB.rotation = 0;
        boulderRB.freezeRotation = true;
        boulder.SetActive(false);
        boulderA.SetActive(false);
        velocityDirectionArrow1.SetActive(false);
        velocityDirectionArrow2.SetActive(false);

        FallingBoulders.dropPoint = 0;
        FallingBoulders.moreDropPoint = 0;
        RumblingManager.shakeON = true;

        indicators.distanceSpawnPnt = new Vector2(myPlayer.transform.position.x, 1);
        spawnPoint = indicators.distanceSpawnPnt;


        myPlayer.isLanded = false;
        myPlayer.standup = false;
        RagdollV2.disableRagdoll = true;

        playerAnswer = 0;
        elapsed = 0;
        stage = qc.stage;
        qc.timer = "0.00s";
        qc.isSimulating = false;

        life.losslife = false;
        float Va = 0, Vb = 0, d = 0, Da = 0, Db = 0, Dj = 0, t = 0;//, tm = 0;
        switch (stage)
        {
            case 1:
                boulderVelocity = 0;
                boulderA.SetActive(true);
                velocityDirectionArrow1.SetActive(true);

                qc.limit = 30 / 8;
                while (true)
                {
                    Va = Random.Range(8f, 10f);
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

                    if (correctAnswer < qc.limit) break;
                }
                whatIsAsk = UnitOf.time;

                myPlayer.transform.position = new Vector2(0, boulder.transform.position.y);
                myPlayer.gameObject.SetActive(true);
                boulderA.transform.position = new Vector2(distance, boulder.transform.position.y);

                jumpDistance = (float)System.Math.Round(Dj, 2);
                jumpTime = Dj / Va;
                jumpForce = 2f / (jumpTime);

                indicators.timeSpawnPnt = myPlayer.transform.position;
                indicators.distanceSpawnPnt = new Vector2(myPlayer.transform.position.x, 1);
                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(distance, correctD, null, playerVelocity, stuntTime);
                indicators.UnknownIs('t');

                question = playerName + " is instructed to run until the end of the scene while jumping over the rolling boulder. If " + pronoun + " is running at a velocity of <color=purple>" + playerVelocity + " meters per second</color> while an incoming boulder at the front <color=red>" + distance + " meters</color> away is rolling at the velocity of <color=purple>" + boulderVelocity + "meters per second</color>, at after how many <color=#006400>seconds</color> will " + playerName + " jump if " + pronoun + " has to jump at exactly <color=red>" + jumpDistance + " meters</color> away from the boulder in order to jump over it safely?";
                break;
            case 2:
                boulder.SetActive(true);
                velocityDirectionArrow2.SetActive(true);

                qc.limit = 10;
                while (true)
                {
                    Va = Random.Range(8f, 10f);
                    Vb = Random.Range(14f, 16f);
                    d = Random.Range(13f, 15f);
                    playerVelocity = (float)System.Math.Round(Va, 2);
                    boulder2Velocity = (float)System.Math.Round(Vb, 2);
                    distance = (float)System.Math.Round(d, 2);
                    Dj = (Vb - Va) / 2;
                    t = (distance - Dj) / (boulder2Velocity - playerVelocity);
                    stuntTime = (float)System.Math.Round(t, 2);
                    Da = playerVelocity * stuntTime;
                    Db = boulder2Velocity * stuntTime;
                    correctAnswer = (float)System.Math.Round(Da, 2);
                    if (correctAnswer < qc.limit && (stuntTime > 1f || stuntTime < 5f)) break;
                }

                whatIsAsk = UnitOf.distance;

                boulder.transform.position = new Vector2(playerPos, 0);
                myPlayer.transform.position = new Vector2(distance, boulder.transform.position.y);

                jumpDistance = (float)System.Math.Round(Dj, 2);
                jumpTime = Dj / Va;
                jumpForce = 1.8f / (jumpTime);
                correctD = correctAnswer;

                indicators.distanceSpawnPnt = new Vector2(spawnPoint.x, 1);
                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(distance, null, null, playerVelocity, stuntTime);
                indicators.UnknownIs('N');

                question = playerName + " is instructed to run until the end of the scene while jumping over the rolling boulder. If " + pronoun + " is running at a velocity of <color=purple>" + playerVelocity + " meters per second</color> while an incoming fast moving boulder <color=red>" + distance + " meters</color> away is catchind up from behind with a velocity of <color=purple>" + boulder2Velocity + "meters per second</color>, at after how many <color=red>meters</color> should " + playerName + " be jumping if " + pronoun + " has to jump at exactly <color=red>" + jumpDistance + " meters</color> away from the boulder in order to jump over it safely?";
                break;
            case 3:
                whatIsAsk = UnitOf.velocity;
                boulder.SetActive(true);
                boulderA.SetActive(true);
                velocityDirectionArrow1.SetActive(true);
                velocityDirectionArrow2.SetActive(true);

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
                qc.limit = 10;

                boulder.transform.position = new Vector2(0, 0);
                boulderA.transform.position = new Vector2(boulder.transform.position.x + d, 0);
                myPlayer.transform.position = new Vector2(boulderA.transform.position.x - Dac, 0);
                jumpTime = 0.5f;
                jumpForce = 1.5f / jumpTime;

                JDIndicator.SetActive(true);
                jmpDistFromBoulder.spawnPoint = new Vector2(boulderA.transform.position.x, 1.5f);
                jmpDistFromBoulder.distance = -Dac;
                indicators.timeSpawnPnt = new Vector2(myPlayer.transform.position.x, 1);
                indicators.distanceSpawnPnt = new Vector2(spawnPoint.x, 1);
                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(distance, correctD, null, playerVelocity, stuntTime);
                indicators.UnknownIs('v');

                question = playerName + " is instructed to vertically jump over between two incoming boulders at precisely <color=#006400>0.5 seconds</color> before they collide. If the boulder in front is <color=red>" + Dac + " meters</color> away from " + playerName + " is approaching at <color=purple>" + boulder2Velocity + " meters per second</color>, and the boulder behind " + pPronoun + " is <color=red>" + distance + " meters</color> away from the first boulder and is approaching at <color=purple>" + boulderVelocity + "meters per second</color>, at what <color=purple>velocity</color> should " + pronoun + " run forward for <color=#006400>" + stuntTime + " seconds</color> before doing the vertical jump?";
                break;
        }
        qc.SetUnitTo(whatIsAsk);
        playerSpeed.text = playerVelocity.ToString("f2") + "m/s";
        playerSpeed.fontSize = 6;
        qc.SetColor(playerSpeed, TextColorMode.Given);
        boulder1Speed.text = boulderVelocity.ToString("f2") + "m/s";
        boulder1Speed.fontSize = 6;
        qc.SetColor(boulder1Speed, TextColorMode.Given);
        boulder2Speed.text = boulder2Velocity.ToString("f2") + "m/s";
        boulder2Speed.fontSize = 6;
        qc.SetColor(boulder2Speed, TextColorMode.Given);
        qc.SetQuestion(question);
        myPlayer.jumpforce = jumpForce;
        velocityDirectionArrow1.GetComponent<SpriteRenderer>().color = qc.getHexColor(TextColorMode.Given);
        velocityDirectionArrow2.GetComponent<SpriteRenderer>().color = qc.getHexColor(TextColorMode.Given);

        myPlayer.moveSpeed = 0;
        boulderRB.velocity = new Vector2(0, 0);
    }
    public void Play()
    {
        qc.isSimulating = false;
        isStartOfStunt = true;
        directorIsCalling = true;
    }
    IEnumerator Retry()
    {
        transitionCanvas.SetActive(true);
        Destroy(pShadow);
        Destroy(b1Shadow);
        Destroy(b2Shadow);
        FallingBoulders.isRumbling = false;
        qc.retried = false;
        PrefabDestroyer.end = true;
        StartCoroutine(life.endBGgone());
        yield return new WaitForSeconds(1);
        myPlayer.ToggleTrigger();
        myPlayer.transform.position = new Vector2(0, boulder.transform.position.y);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        RumblingManager.isCrumbling = false;
        VeloMediumSetUp();
        transitionCanvas.SetActive(false);
    }
    IEnumerator Next()
    {
        // Do Transition Here
        transitionCanvas.SetActive(true);
        Destroy(pShadow);
        Destroy(b1Shadow);
        Destroy(b2Shadow);
        FallingBoulders.isRumbling = false;
        qc.nextStage = false;
        myPlayer.happy = false;
        PrefabDestroyer.end = true;
        yield return new WaitForSeconds(1f);
        StartCoroutine(life.endBGgone());
        yield return new WaitForSeconds(1.3f);
        myPlayer.transform.position = new Vector2(0, boulder.transform.position.y);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        RumblingManager.isCrumbling = false;
        VeloMediumSetUp();
        transitionCanvas.SetActive(false);

    }
    public IEnumerator DirectorsCall()
    {
        directorIsCalling = false;
        if (isStartOfStunt)
        {
            isStartOfStunt = false;
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Lights!";
            lightssfx.Play();
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Camera!";
            camerasfx.Play();
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Action!";
            actionsfx.Play();
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "";
            directorsBubble.SetActive(false);
            isAnswered = true;
            FallingBoulders.isRumbling = true;
        }
        else
        {
            yield return new WaitForSeconds((35 / playerVelocity) - stuntTime);
            cutsfx.Play();
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";

            RumblingManager.isCrumbling = true;
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
        boulderRB.sharedMaterial.bounciness = 0;
        boulder2RB.sharedMaterial.bounciness = 0;
        boulderRB.sharedMaterial.friction = 0.8f;
        boulder2RB.sharedMaterial.friction = 0.8f;
        RumblingManager.shakeON = false;
        RumblingManager.isCrumbling = true;
        yield return new WaitForSeconds(3);
        if (stage == 3)
        {
            qc.ActivateResult(messageTxt, isAnswerCorrect, true);
        }
        else
        {
            qc.ActivateResult(messageTxt, isAnswerCorrect);
        }

    }
    IEnumerator Jump()
    {
        if (playerAnswer != correctAnswer)
        {
            if (stage == 3)
            {
                myPlayer.jumpforce = jumpForce - 0.1f;
                jumpTime -= 0.04f;
            }
            else if (stage == 1)
            {
                if (playerAnswer > correctAnswer)
                {
                    myPlayer.jumpforce = jumpForce - 0.04f;
                    jumpTime -= 0.08f;
                }
                else
                {
                    myPlayer.jumpforce = jumpForce - 0.03f;
                    jumpTime -= 0.04f;
                }
            }
            else
            {
                if (playerAnswer > correctAnswer)
                {
                    myPlayer.jumpforce = jumpForce - 0.04f;
                    jumpTime -= 0.08f;
                }
                else
                {
                    myPlayer.jumpforce = jumpForce - 0.03f;
                    jumpTime -= 0.16f;
                }

            }
        }
        else
        {
            myPlayer.jumpforce = jumpForce;
        }
        myPlayer.jump();
        yield return new WaitForSeconds(jumpTime / 2);
        isAnswered = false;
        yield return new WaitForEndOfFrame();
        isEndOfStunt = true;
    }
    IEnumerator InstantiateShadows(float playerPos, float? b1Pos, float? b2Pos)
    {
        onShadow = true;
        yield return new WaitForEndOfFrame();
        if (b2Pos != null)
        {
            b2Shadow = Instantiate(boulderShadow);
            b2Shadow.transform.position = new Vector2((float)b2Pos - 0.5f, boulder.transform.position.y);
        }
        if (b1Pos != null)
        {
            b1Shadow = Instantiate(boulderShadow);
            b1Shadow.transform.position = new Vector2((float)b1Pos + 0.5f, boulderA.transform.position.y);
        }
        pShadow = Instantiate(playerShadow);
        pShadow.transform.position = new Vector2(playerPos, myPlayer.transform.position.y);
    }
}
