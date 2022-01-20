using System.Collections;
using TMPro;
using UnityEngine;
using GameConfig;

public class MediumManager : MonoBehaviour
{
    QuestionController2_0_1 qc;
    UnitOf whatIsAsk;
    IndicatorManagerV1_1 indicators;
    IndicatorManager jmpDistFromBoulder;
    ConveyorManager conveyor;
    HeartManager2 life;
    ScoreManager score;
    PlayerCM2 myPlayer;
    StageManager sm = new StageManager();
    
    [SerializeField] NewConveyorManager cm;

    [SerializeField]
    GameObject directorsBubble,
        rope,
        ragdoll,
        stage1Layout,
        stage2Layout,
        stage3Layout,
        UI1,
        UI2,
        UI3,
        conveyor1,
        conveyor2,
        jumperChar;
    Animator playerAnim;
    Animation walk;

    [SerializeField]
    TMP_Text directorsSpeech;

    [SerializeField]
    float distance,
        stuntTime,
        elapsed,
        correctAnswer, av;
    public static int stage;
    float playerAnswer,
        playerSpeed,
        conveyorSpeed,
        animSpeed,
        timingD,
        playerPos,
        currentPlayerPos,
        acceleration,
        speedOffset;
    string question,
        playerName,
        playerGender,
        pronoun,
        pNoun,
        messageTxt,
        errorMessage;
    bool isAnswered,
        directorIsCalling,
        isStartOfStunt,
        isAnswerCorrect,
        ropeGrab,
        isEndOfStunt,
        ragdollActive,
        isJumping;

    Vector2 spawnPoint,
        jmpCharInitPos;

    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionController2_0_1>();
        myPlayer = FindObjectOfType<PlayerCM2>();
        // conveyor = FindObjectOfType<ConveyorManager>();
        indicators = FindObjectOfType<IndicatorManagerV1_1>();
        life = FindObjectOfType<HeartManager2>();
        score = FindObjectOfType<ScoreManager>();

        playerAnim = myPlayer.myAnimator;
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        sm.SetGameLevel(4);
        playerPos = myPlayer.gameObject.transform.position.x;
        jmpCharInitPos = jumperChar.gameObject.transform.position;
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
        qc.levelDifficulty = Difficulty.Medium;
        qc.stage = 1;
        // playerAnim.SetBool("running", true);
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        indicators.SetPlayerPosition(myPlayer.transform.position);
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        // Debug.Log(conveyorSpeed + "cs");
        // Debug.Log(playerSpeed + "ps");
        if (isAnswered)
        {
            indicators.distanceSpawnPnt = spawnPoint;
            // playerAnswer = qc.GetPlayerAnswer();
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
                        // isAnswered =false;
                        myPlayer.moveSpeed = 0;
                        timingD = (playerAnswer - conveyorSpeed) * stuntTime;
                        playerAnim.speed = 1;
                        elapsed = stuntTime;
                        ropeGrab = true;
                        // myPlayer.moveSpeed = -conveyorSpeed;
                        if (playerAnswer == playerSpeed)
                        {
                            isAnswerCorrect = true;
                            myPlayer.transform.position = new Vector2(
                                distance - 10,
                                myPlayer.transform.position.y
                            );
                            // myPlayer.moveSpeed = 0;
                            // playerAnim.SetBool("running", false);
                            // playerAnim.speed = 1;
                            messageTxt = "CORRECT";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            // myPlayer.gameObject.SetActive(false);
                            // ragdoll.transform.position = myPlayer.transform.position;
                            // ragdoll.SetActive(true);
                            // ragdoll.GetComponent<Rigidbody2D>().velocity = new Vector2(
                            //     conveyorSpeed,
                            //     0
                            // );
                            if (playerAnswer > playerSpeed)
                            {
                                // myPlayer.transform.position = new Vector2(
                                //     myPlayer.transform.position.x + 0.2f,
                                //     myPlayer.transform.position.y
                                // );
                                messageTxt = "Above";
                            }
                            else
                            {
                                // myPlayer.transform.position = new Vector2(
                                //     myPlayer.transform.position.x - 0.2f,
                                //     myPlayer.transform.position.y
                                // );
                                messageTxt = "Below";
                            }
                            indicators.ShowCorrectDistance(distance, true, new Vector2(-10, 0.5f));
                        }
                        isAnswered = false;
                        indicators.AnswerIs(isAnswerCorrect, false);
                    }
                    indicators.IsRunning(playerAnswer, (playerAnswer - conveyorSpeed) * elapsed);
                    break;
                case 2:
                    if (myPlayer.transform.position.x >= distance)
                    {
                        //Put new character. to jump to the pipe
                        // StartCoroutine(ActivateJump());
                        elapsed = stuntTime;
                        myPlayer.running = false;
                        myPlayer.moveSpeed = 0;
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
                    if (playerAnswer == acceleration)
                    {
                        // myPlayer.moveSpeed = 15;
                        isAnswerCorrect = true;
                        messageTxt = "Answer is correct";
                    }
                    else
                    {
                        isAnswerCorrect = false;
                        if (playerAnswer > acceleration)
                        {
                            speedOffset = 1.15f;
                            messageTxt = "Answer is more than correct";
                        }
                        else
                        {
                            speedOffset = 0.85f;
                            messageTxt = "Answer is less than correct";
                        }
                    }
                    break;
                case 3:
                cm.SetConveyorSpeed(av, stuntTime, 5);
                
                    break;
            }
        }
        if (Hanger.isHanging)
        {
            isAnswered = false;
            jumperChar.GetComponent<Animator>().SetBool("dive", false);
            isEndOfStunt = true;
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
        this.gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        jumperChar.SetActive(false);
        ragdoll.SetActive(false);
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

        // life.losslife = false;
        float aVelocity;
        switch (stage)
        {
            case 1:
                stage1Layout.SetActive(true);
                qc = UI1.GetComponent<QuestionController2_0_1>();
                qc.stage = 1;
                Instantiate(conveyor1).transform.position = new Vector2(0, 0);
                Instantiate(conveyor1).transform.position = new Vector2(23.5f, 3.3f);
                conveyor = FindObjectOfType<ConveyorManager>();

                myPlayer.running = true;

                qc.limit = 10.4f;
                while (true)
                {
                    aVelocity = (float)System.Math.Round(Random.Range(150f, 250f), 2);
                    stuntTime = (float)System.Math.Round(Random.Range(1.9f, 5f), 2);
                    distance = Random.Range(15f, 20f);
                    conveyor.SetConveyorSpeed(aVelocity, stuntTime, 1.15f);
                    conveyorSpeed = conveyor.GetConveyorVelocity();
                    playerSpeed = (float)System.Math.Round(
                        ((distance / stuntTime) + conveyorSpeed),
                        2
                    );
                    rope.transform.position = new Vector2(distance - 10, rope.transform.position.y);
                    playerAnim.speed = conveyorSpeed / 5.6f; // set to 1 before grabbing.
                    if (playerSpeed > 3 && playerSpeed < 10.4f)
                        break;
                }
                whatIsAsk = UnitOf.velocity;

                myPlayer.transform.position = new Vector2(-10, myPlayer.transform.position.y);
                myPlayer.gameObject.SetActive(true);

                indicators.heightSpawnPnt = new Vector2(-13, -1.15f);
                indicators.distanceSpawnPnt = new Vector2(-10, 2);
                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(distance, 2.3f, playerSpeed, stuntTime);
                indicators.UnknownIs('v');

                question =
                    playerName
                    + " is instructed to run on a moving conveyor with the speed of <b> "
                    + ConveyorManager.angularVelocity
                    + qc.Unit(UnitOf.angularVelocity)
                    + "</b>, how fast should "
                    + playerName
                    + " run if "
                    + pronoun
                    + " is to grab the rope that is "
                    + distance.ToString("f2")
                    + qc.Unit(UnitOf.distance)
                    + " away from"
                    + pNoun
                    + " at exactly after <b>"
                    + stuntTime.ToString("f2")
                    + qc.Unit(UnitOf.time)
                    + "</b> ?";
                break;
            case 2:
                jumperChar.transform.position = jmpCharInitPos;
                this.gameObject.GetComponent<Collider2D>().enabled = true;
                stage2Layout.SetActive(true);
                qc = UI2.GetComponent<QuestionController2_0_1>();
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
                myPlayer.transform.position = new Vector2(6 - distance, 3);
                myPlayer.gameObject.SetActive(true);

                // float Vfp = playerSpeed;

                indicators.distanceSpawnPnt = new Vector2(6 - distance, 3);
                indicators.heightSpawnPnt = new Vector2(-17f, -1.925f);
                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(distance, 3.85f, playerSpeed, stuntTime);
                indicators.UnknownIs('N');
                break;
            case 3:
                stage3Layout.SetActive(true);
                qc = UI3.GetComponent<QuestionController2_0_1>();
                qc.stage = 2;
                conveyor = FindObjectOfType<ConveyorManager>();
                playerAnim.speed = 1;
                myPlayer.climb = false;
                myPlayer.running = false;
                myPlayer.walking = true;
                qc.limit = 5f;

                float hangerDist, hangerVelo;
                distance = Random.Range(16F, 21F);
                aVelocity = Random.Range(54f, 59f);
                av = Random.Range(5f,10f);
                // stuntTime = Random.Range(3.5f, 5f);
                conveyor.SetConveyorSpeed(-aVelocity, stuntTime, 1.15f);
                conveyorSpeed = conveyor.GetConveyorVelocity() * -1;
                playerSpeed = conveyorSpeed + Random.Range(3f, 10.4f);
                stuntTime = distance/playerSpeed;

                hangerDist = 36 - distance;

                whatIsAsk = UnitOf.time;
                myPlayer.transform.position = new Vector2(-18, 3);
                myPlayer.gameObject.SetActive(true);

                // float Vfp = playerSpeed;

                indicators.distanceSpawnPnt = new Vector2(myPlayer.transform.position.x, 3);
                indicators.heightSpawnPnt = new Vector2(-17f, -1.925f);
                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(distance, 3.85f, playerSpeed, stuntTime);
                indicators.UnknownIs('t');
                break;
            default:
                break;
        }
        qc.SetUnitTo(whatIsAsk);
        qc.SetQuestion(question);

        myPlayer.moveSpeed = 0;

        Debug.Log(conveyorSpeed + "cs");
        Debug.Log(playerSpeed + "ps");
        Debug.Log(acceleration + "ac");
    }

    void Play()
    {
        qc.isSimulating = false;
        playerAnswer = qc.GetPlayerAnswer();
        directorIsCalling = true;
        isStartOfStunt = true;
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
        // StartCoroutine(life.endBGgone());
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
        myPlayer.running = false;
        ropeGrab = false;
        myPlayer.ropeGrab = true;
        // playerAnim.SetBool("running", false);
        // playerAnim.SetBool("ropeGrab", true);
        // myPlayer.successGrab = true;
        yield return new WaitForSeconds(0.15f);
        if (isAnswerCorrect)
        {
            myPlayer.successGrab = true;
            // playerAnim.SetBool("successGrab", true);
            yield return new WaitForSeconds(1.01f);
            myPlayer.ropeGrab = false;
            myPlayer.successGrab = false;
            myPlayer.climb = true;
            // myPlayer.ropeGrab = false;
        }
        else
        {
            ragdollActive = true;
            yield return new WaitForSeconds(0.51f);
            myPlayer.ropeGrab = false;
        }
        // myPlayer.ropeGrab = false;
        yield return new WaitForSeconds(0.5f);
        isEndOfStunt = true;
    }

    void RagdollSpawn()
    {
        myPlayer.gameObject.SetActive(false);
        ragdoll.transform.position = myPlayer.transform.position;
        ragdoll.SetActive(true);
        ragdoll.GetComponent<Rigidbody2D>().velocity = new Vector2(conveyorSpeed, 0);
    }

    IEnumerator ActivateJump()
    {
        jumperChar.SetActive(true);
        myPlayer.gameObject.SetActive(false);
        if (isAnswerCorrect)
            jumperChar.GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
        else
            jumperChar.GetComponent<Rigidbody2D>().velocity = new Vector2(
                (myPlayer.moveSpeed + 10) * speedOffset / 2,
                0
            );
        jumperChar.GetComponent<Animator>().SetBool("dive", true);
        yield return new WaitForSeconds(1.25f);
        isAnswered = false;
        isEndOfStunt = true;
        jumperChar.GetComponent<Animator>().SetBool("dive", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(ActivateJump());
        }
    }
}
