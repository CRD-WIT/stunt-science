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
    [SerializeField] GameObject directorsBubble, rope, AVelocityIndicator, ragdoll, stage1Layout, stage2Layout;
    Animator playerAnim;
    [SerializeField] TMP_Text directorsSpeech;
    [SerializeField] float distance, stuntTime, elapsed, correctAnswer;
    public static int stage;
    float playerAnswer, playerSpeed, conveyorSpeed, animSpeed, correctD, timingD, playerPos, currentPlayerPos;
    string question, playerName, playerGender, pronoun, pPronoun, messageTxt;
    bool isAnswered, directorIsCalling, isStartOfStunt, isAnswerCorrect, ropeGrab, isEndOfStunt;
    // GameObject b2Shadow, b1Shadow, pShadow;
    Vector2 spawnPoint;
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        indicators = FindObjectOfType<IndicatorManagerV1_1>();
        conveyor = FindObjectOfType<ConveyorManager>();
        myPlayer = FindObjectOfType<PlayerCM2>();
        life = FindObjectOfType<HeartManager>();
        score = FindObjectOfType<ScoreManager>();

        playerAnim = myPlayer.myAnimator;
        // RagdollV2.myPlayer = myPlayer;
        
        qc.stage = 1;

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
        Debug.Log(conveyorSpeed + "cs");
        Debug.Log(playerSpeed + "ps");
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
                        // myPlayer.moveSpeed = -conveyorSpeed;
                        if (playerAnswer == playerSpeed)
                        {
                            isAnswerCorrect = true;
                            myPlayer.transform.position = new Vector2(distance - 10, myPlayer.transform.position.y);
                            messageTxt = "CORRECT";
                            //correct
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
                            indicators.ShowCorrectDistance(distance, true, new Vector2(stuntTime * playerSpeed, 1.5f));
                            // indicators.ShowCorrectTime(stuntTime, stuntTime * playerVelocity, true);
                        }
                        isAnswered = false;
                        ropeGrab = true;
                        // currentBoulderPos = distance - (boulderVelocity * stuntTime);
                        indicators.AnswerIs(isAnswerCorrect, false);
                    }
                    indicators.IsRunning(playerAnswer, (playerAnswer - conveyorSpeed), elapsed, timingD);
                    break;
                case 2:
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

        indicators.distanceSpawnPnt = new Vector2(myPlayer.transform.position.x, 1);
        spawnPoint = indicators.distanceSpawnPnt;

        // RagdollV2.disableRagdoll = true;
        stage1Layout.SetActive(false);
        stage2Layout.SetActive(false);

        playerAnswer = 0;
        elapsed = 0;
        conveyorSpeed = 0;
        stage = qc.stage;
        qc.timer = "0.00s";
        qc.isSimulating = false;
        myPlayer.climb = false;

        AVelocityIndicator.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = qc.getHexColor(TextColorMode.Given);
        AVelocityIndicator.transform.Find("aVelocityIndicator").gameObject.GetComponent<SpriteRenderer>().color = qc.getHexColor(TextColorMode.Given);
        qc.SetColor(AVelocityIndicator.transform.Find("label").GetComponent<TMP_Text>(), TextColorMode.Given);

        life.losslife = false;
        // float Va = 0, Vb = 0, d = 0, Da = 0, Db = 0, Dj = 0, t = 0;//, tm = 0;
        switch (stage)
        {
            case 1:
                myPlayer.running = true;
                stage1Layout.SetActive(true);
                indicators.heightSpawnPnt = new Vector2(-13.3f, -1.15f);

                qc.limit = 10.4f;
                while (true)
                {
                    float aVelocity = (float)System.Math.Round(Random.Range(150f, 250f), 2);
                    Debug.Log(aVelocity);
                    stuntTime = (float)System.Math.Round(Random.Range(1.9f, 5f), 2);
                    distance = Random.Range(15f, 20f);
                    conveyorSpeed = conveyor.SetConveyorSpeed(aVelocity, stuntTime);
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
                // indicators.distanceSpawnPnt = new Vector2(myPlayer.transform.position.x, 1);
                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(null, distance, null, playerSpeed, stuntTime);
                indicators.UnknownIs('v');

                question = playerName + " is instructed to run on a moving conveyor belt and the rope is<b> " + ConveyorManager.angularVelocity + " degrees per second</b>, how fast should " + playerName + " run if " + pronoun + " is to grab the rope exactly after <b>" + stuntTime.ToString("f2") + " seconds</b>?";
                break;
            case 2:
                myPlayer.walking = true;
                playerAnim.speed = -1;
                stage2Layout.SetActive(true);
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
            yield return new WaitForSeconds(1f);
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
        yield return new WaitForSeconds(1f);
        directorIsCalling = true;
        isStartOfStunt = false;
        yield return new WaitForSeconds(3);
        qc.ActivateResult(messageTxt, isAnswerCorrect);
    }
    IEnumerator GrabRope()
    {
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
            myPlayer.gameObject.SetActive(false);
            ragdoll.transform.position = myPlayer.transform.position;
            ragdoll.SetActive(true);
            ragdoll.GetComponent<Rigidbody2D>().velocity = new Vector2(conveyorSpeed, 0);
            yield return new WaitForSeconds(0.51f);
        }
        yield return new WaitForSeconds(0.5f);
        isAnswered = false;
        isEndOfStunt = true;
    }
    void ragdollSpawn()
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
