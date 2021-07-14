using System.Collections;
using TMPro;
using UnityEngine;
using GameConfig;
public class MediumManager : MonoBehaviour
{
    QuestionControllerVThree qc;
    UnitOf whatIsAsk;
    IndicatorManagerV1_1 indicators;
    IndicatorManager jmpDistFromBoulder;
    ConveyorManager conveyor;
    HeartManager life;
    ScoreManager score;
    PlayerCM2 myPlayer;
    StageManager sm = new StageManager();
    [SerializeField] GameObject directorsBubble, rope, AVelocityIndicator, ragdoll, stage1Layout, stage2Layout, UI1, UI2, UI3;
    public GameObject conveyor1, conveyor2;
    Animator playerAnim;
    Animation walk;
    [SerializeField] TMP_Text directorsSpeech;
    [SerializeField] float distance, stuntTime, elapsed, correctAnswer;
    public static int stage;
    float playerAnswer, playerSpeed, conveyorSpeed, animSpeed, correctD, timingD, playerPos, currentPlayerPos, acceleration;
    string question, playerName, playerGender, pronoun, pPronoun, messageTxt;
    bool isAnswered, directorIsCalling, isStartOfStunt, isAnswerCorrect, ropeGrab, isEndOfStunt, ragdollActive;
    // GameObject b2Shadow, b1Shadow, pShadow;
    Vector2 spawnPoint;
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        indicators = FindObjectOfType<IndicatorManagerV1_1>();
        myPlayer = FindObjectOfType<PlayerCM2>();
        life = FindObjectOfType<HeartManager>();
        score = FindObjectOfType<ScoreManager>();

        playerAnim = myPlayer.myAnimator;
        // RagdollV2.myPlayer = myPlayer;
        // qc.stage = 1;
        playerName = PlayerPrefs.GetString("Name");
        string playerGender = PlayerPrefs.GetString("Gender");
        sm.SetGameLevel(5);
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
        SetUp();
    }
    // Update is called once per frame
    void Update()
    {
        myPlayer.walking = true;
        Debug.Log(conveyorSpeed + "cs");
        Debug.Log(playerSpeed + "ps");
        Debug.Log(acceleration + "ac");
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        if (isAnswered)
        {
            indicators.distanceSpawnPnt = spawnPoint;
            playerAnswer = qc.GetPlayerAnswer();
            qc.timer = elapsed.ToString("f2") + "s";
            elapsed += Time.deltaTime;
            timingD = currentPlayerPos;
            switch (stage)
            {
                case 1:
                    playerAnim.speed = playerAnswer / 5.6f; // set to 1 before grabbing.
                    myPlayer.moveSpeed = playerAnswer - conveyorSpeed;
                    timingD = myPlayer.transform.position.x + 10;
                    if (elapsed >= stuntTime)
                    {
                        // timingD = playerAnswer * playerVelocity;
                        // spawnPoint = new Vector2(stuntTime * playerVelocity, 1);
                        // playerAnim.SetBool("running", false);
                        myPlayer.running = false;
                        myPlayer.moveSpeed = 0;
                        timingD = (playerAnswer - conveyorSpeed) * stuntTime;
                        playerAnim.speed = 1;
                        elapsed = stuntTime;
                        if (playerAnswer == playerSpeed)
                        {
                            isAnswerCorrect = true;
                            myPlayer.transform.position = new Vector2(distance - 10, myPlayer.transform.position.y);
                            messageTxt = "CORRECT";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            if (playerAnswer > playerSpeed)
                            {
                                // myPlayer.transform.position = new Vector2(myPlayer.transform.position.x + 0.2f, myPlayer.transform.position.y);
                                messageTxt = "Above";
                            }
                            else
                            {
                                // myPlayer.transform.position = new Vector2(myPlayer.transform.position.x - 0.2f, myPlayer.transform.position.y);
                                messageTxt = "Below";
                            }
                            indicators.ShowCorrectDistance(distance, true, new Vector2(-10, 0.5f));
                            // indicators.ShowCorrectTime(stuntTime, stuntTime * playerVelocity, true);
                        }
                        isAnswered = false;
                        ropeGrab = true;
                        indicators.AnswerIs(isAnswerCorrect, false);
                    }
                    indicators.IsRunning(playerAnswer, (playerAnswer - conveyorSpeed), elapsed, timingD);
                    break;
                case 2:
                    if (myPlayer.transform.position.x >= distance)
                    {
                        elapsed = stuntTime;
                        myPlayer.running = false;
                        if (playerAnswer == acceleration)
                        {
                            myPlayer.moveSpeed = 15;
                            isAnswerCorrect = true;
                            messageTxt = "Answer is correct";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            if (playerAnswer > acceleration)
                            {
                                messageTxt = "Answer is more than correct";
                            }
                            else
                            {
                                messageTxt = "Answer is less than correct";
                            }
                        }
                        ropeGrab = true;
                    }
                    else
                    {
                        myPlayer.walking = false;
                        playerSpeed = playerAnswer * elapsed;
                        myPlayer.moveSpeed = playerSpeed + conveyorSpeed;
                        if (playerSpeed >= 2.5f)
                        {
                            myPlayer.myAnimator.speed = elapsed / 3;
                            playerAnim.SetBool("walkForward", false);
                            myPlayer.running = true;
                        }
                        else
                        {
                            playerAnim.speed = elapsed + 0.4f;
                            playerAnim.SetBool("walkForward", true);
                            myPlayer.running = false;
                        }
                    }
                    break;
                case 3:
                    break;
            }
        }
        indicators.SetPlayerPosition(myPlayer.transform.position);
        if (isEndOfStunt)
            StartCoroutine(StuntResult());
        if (ropeGrab)
            StartCoroutine(GrabRope());
        if (ragdollActive)
            RagdollSpawn();
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
    void SetUp()
    {
        // onShadow = false;
        ragdollActive = false;
        indicators.distanceSpawnPnt = new Vector2(myPlayer.transform.position.x, 1);
        spawnPoint = indicators.distanceSpawnPnt;
        stage1Layout.SetActive(false);
        stage2Layout.SetActive(false);

        playerAnim.SetBool("ropeGrab", false);
        playerAnim.SetBool("successGrab", false);

        playerAnswer = 0;
        elapsed = 0;
        conveyorSpeed = 0;
        playerSpeed = 0;
        stage = qc.stage;
        qc.timer = "0.00s";
        qc.isSimulating = false;
        myPlayer.climb = false;

        AVelocityIndicator.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = qc.getHexColor(TextColorMode.Given);
        AVelocityIndicator.transform.Find("aVelocityIndicator").gameObject.GetComponent<SpriteRenderer>().color = qc.getHexColor(TextColorMode.Given);
        qc.SetColor(AVelocityIndicator.transform.Find("label").GetComponent<TMP_Text>(), TextColorMode.Given);

        life.losslife = false;
        float aVelocity;
        switch (stage)
        {
            case 1:
                stage1Layout.SetActive(true);
                qc = UI1.GetComponent<QuestionControllerVThree>();
                qc.stage = 1;
                Instantiate(conveyor1).transform.position = new Vector2(0, 0);
                Instantiate(conveyor1).transform.position = new Vector2(23.5f, 3.3f);
                conveyor = FindObjectOfType<ConveyorManager>();

                myPlayer.running = true;
                indicators.heightSpawnPnt = new Vector2(-13.3f, -1.15f);

                qc.limit = 10.4f;
                while (true)
                {
                    aVelocity = (float)System.Math.Round(Random.Range(150f, 250f), 2);
                    stuntTime = (float)System.Math.Round(Random.Range(1.9f, 5f), 2);
                    distance = Random.Range(15f, 20f);
                    conveyor.SetConveyorSpeed(aVelocity, stuntTime, 1.15f);
                    conveyorSpeed = conveyor.GetConveyorVelocity();
                    playerSpeed = (float)System.Math.Round(((distance / stuntTime) + conveyorSpeed), 2);
                    AVelocityIndicator.transform.Find("label").GetComponent<TMP_Text>().text = aVelocity.ToString("f2") + qc.Unit(UnitOf.angularVelocity);
                    rope.transform.position = new Vector2(distance - 10, rope.transform.position.y);
                    playerAnim.speed = conveyorSpeed / 5.6f; // set to 1 before grabbing.
                    indicators.showLines(null, distance, 2.3f, playerSpeed, stuntTime);
                    if (playerSpeed > 3 && playerSpeed < 10.4f) break;
                }
                whatIsAsk = UnitOf.velocity;

                myPlayer.transform.position = new Vector2(-10, myPlayer.transform.position.y);
                myPlayer.gameObject.SetActive(true);

                indicators.timeSpawnPnt = new Vector2(-10, .5f);
                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(null, distance, null, playerSpeed, stuntTime);
                indicators.UnknownIs('v');

                question = playerName + " is instructed to run on a moving conveyor belt and the rope is<b> " + ConveyorManager.angularVelocity + " degrees per second</b>, how fast should " + playerName + " run if " + pronoun + " is to grab the rope exactly after <b>" + stuntTime.ToString("f2") + " seconds</b>?";
                break;
            case 2:
                stage2Layout.SetActive(true);
                qc = UI2.GetComponent<QuestionControllerVThree>();
                qc.stage = 2;
                Instantiate(conveyor2).transform.position = new Vector2(-5, 0);
                conveyor = FindObjectOfType<ConveyorManager>();

                playerAnim.speed = 1;
                myPlayer.climb = false;
                myPlayer.running = false;
                myPlayer.walking = true;
                qc.limit = 10.4f;

                distance = Random.Range(15F, 20F);
                aVelocity = Random.Range(54f, 59f);
                stuntTime = Random.Range(3.5f, 5f);
                conveyor.SetConveyorSpeed(-aVelocity, stuntTime, 1.925f);
                conveyorSpeed = conveyor.GetConveyorVelocity() * -1;
                float Vfp = 15 - conveyorSpeed;
                acceleration = (float)System.Math.Round((Vfp / stuntTime), 2);

                whatIsAsk = UnitOf.acceleration;
                rope.transform.position = new Vector2(6 - distance, 4);
                myPlayer.transform.position = rope.transform.position;

                // float Vfp = playerSpeed;

                indicators.distanceSpawnPnt = new Vector2(6 - distance, 3);
                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(distance, null, null, playerSpeed, stuntTime);
                indicators.UnknownIs('N');

                break;
            case 3:
                break;
        }
        qc.SetUnitTo(whatIsAsk);
        qc.SetQuestion(question);

        myPlayer.moveSpeed = 0;
    }
    public void Play()
    {
        qc.isSimulating = false;
        isStartOfStunt = true;
        directorIsCalling = true;
    }
    IEnumerator Retry()
    {
        ConveyorManager.isActive = false;
        yield return new WaitForEndOfFrame();
        // conveyor.isActive = false;
        FallingBoulders.isRumbling = false;
        qc.retried = false;
        PrefabDestroyer.end = true;
        StartCoroutine(life.endBGgone());
        yield return new WaitForSeconds(3);
        // myPlayer.ToggleTrigger();
        // myPlayer.transform.position = new Vector2(0, boulder.transform.position.y);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        RumblingManager.isCrumbling = false;
        SetUp();
    }
    IEnumerator Next()
    {
        ConveyorManager.isActive = false;
        FallingBoulders.isRumbling = false;
        qc.nextStage = false;
        myPlayer.happy = false;
        PrefabDestroyer.end = true;
        yield return new WaitForSeconds(3f);
        StartCoroutine(life.endBGgone());
        yield return new WaitForSeconds(2.8f);
        // myPlayer.transform.position = new Vector2(0, boulder.transform.position.y);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        RumblingManager.isCrumbling = false;

        SetUp();
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
            FallingBoulders.isRumbling = true;
        }
        else
        {
            yield return new WaitForSeconds(0.25f);
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";
        }
    }
    IEnumerator StuntResult()
    {
        isEndOfStunt = false;
        // yield return new WaitForSeconds(0.25f);
        directorIsCalling = true;
        isStartOfStunt = false;
        yield return new WaitForSeconds(1f);
        qc.ActivateResult(messageTxt, isAnswerCorrect);
    }
    IEnumerator GrabRope()
    {
        ropeGrab = false;
        playerAnim.SetBool("running", false);
        playerAnim.SetBool("ropeGrab", true);
        yield return new WaitForSeconds(0.15f);
        if (isAnswerCorrect)
        {
            playerAnim.SetBool("successGrab", true);
            yield return new WaitForSeconds(1.01f);
            myPlayer.climb = true;
        }
        else
        {
            ragdollActive = true;
            yield return new WaitForSeconds(0.51f);
        }
    }
    void RagdollSpawn()
    {
        myPlayer.gameObject.SetActive(false);
        ragdoll.transform.position = myPlayer.transform.position;
        ragdoll.SetActive(true);
        ragdoll.GetComponent<Rigidbody2D>().velocity = new Vector2(conveyorSpeed, 0);
    }
    // IEnumerator InstantiateShadows(float playerPos, float? b1Pos, float? b2Pos)
    // {
    //     onShadow = true;
    //     yield return new WaitForEndOfFrame();
    //     if (b2Pos != null)
    //     {
    //         b2Shadow = Instantiate(boulderShadow);
    //         b2Shadow.transform.position = new Vector2((float)b2Pos - 0.5f, boulder.transform.position.y);
    //     }
    //     if (b1Pos != null)
    //     {
    //         b1Shadow = Instantiate(boulderShadow);
    //         b1Shadow.transform.position = new Vector2((float)b1Pos + 0.5f, boulderA.transform.position.y);
    //     }
    //     pShadow = Instantiate(playerShadow);
    //     pShadow.transform.position = new Vector2(playerPos, myPlayer.transform.position.y);
    // }
}
