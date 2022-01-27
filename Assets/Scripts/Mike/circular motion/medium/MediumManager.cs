using System.Collections;
using TMPro;
using UnityEngine;
using GameConfig;

public class MediumManager : MonoBehaviour
{
    QuestionController2_0_1 qc;
    UnitOf whatIsAsk;
    IndicatorManager2_0_1 labels;
    IndicatorManager jmpDistFromBoulder;
    ConveyorManager conveyor;
    HeartManager2 life;
    ScoreManager score;
    PlayerCM2 myPlayer;
    StageManager sm = new StageManager();

    [SerializeField]
    NewConveyorManager cm;

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
        jumperChar,
        hanger,
        labels2;
    Animator playerAnim;
    Animation walk;

    [SerializeField]
    TMP_Text directorsSpeech;

    [SerializeField]
    float distance = 0,
        stuntTime,
        elapsed,
        correctAnswer,
        av;
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
        labels = FindObjectOfType<IndicatorManager2_0_1>();
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
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        // Debug.Log(conveyorSpeed + "cs");
        // Debug.Log(playerSpeed + "ps");
        if (isAnswered)
        {
            labels.distanceSpawnPnt = spawnPoint;
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
                            labels.ShowCorrectDistance(distance, true, new Vector2(-10, 0.5f));
                        }
                        isAnswered = false;
                        labels.AnswerIs(isAnswerCorrect, false);
                    }
                    labels.IsRunning(playerAnswer, (playerAnswer - conveyorSpeed) * elapsed);
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
                    labels.IsRunning(myPlayer.moveSpeed, myPlayer.transform.position.x);
                    break;
                case 3:

                    cm.SetHangerVelocity(-av, stuntTime, 3.5f);
                    if (elapsed >= playerAnswer)
                    {
                        ropeGrab = true;
                        elapsed = stuntTime;
                        myPlayer.running = false;
                        myPlayer.moveSpeed = 0;
                        isAnswered = false;
                        
                        if(playerAnswer == correctAnswer){
                            messageTxt = "Correct";
                        }
                        else{
                            messageTxt = "wrong";
                        }
                    }
                    else
                    {
                        myPlayer.walking = false;
                        myPlayer.moveSpeed = playerSpeed;
                        if (playerSpeed >= 2.5f)
                        {
                            myPlayer.myAnimator.speed = 1;
                            playerAnim.SetBool("walkForward", false);
                            myPlayer.running = true;
                        }
                        else
                        {
                            playerAnim.speed = 0.4f;
                            playerAnim.SetBool("walkForward", true);
                            myPlayer.running = false;
                        }
                    }
                    break;
            }
        }
        if (Hanger.isHanging)
        {
            isAnswered = false;
            jumperChar.GetComponent<Animator>().SetBool("dive", false);
            isEndOfStunt = true;
        }
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
        labels.SetPlayerPosition(myPlayer.transform.position);
    }

    void SetUp()
    {
        cm.gameObject.SetActive(false);
        this.gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        jumperChar.SetActive(false);
        ragdoll.SetActive(false);
        ragdollActive = false;

        labels.distanceSpawnPnt = new Vector2(myPlayer.transform.position.x, 1);

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

        life.losslife = false;
        float aVelocity,
            height = 0;
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

                labels.heightSpawnPnt = new Vector2(-13, -1.15f);
                labels.distanceSpawnPnt = new Vector2(-10, 2);
                labels.UnknownIs('v');
                height = 2.3f;

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

                distance = (float)System.Math.Round(Random.Range(15F, 20F), 2);
                aVelocity = (float)System.Math.Round(Random.Range(54f, 59f), 2);
                stuntTime = (float)System.Math.Round(Random.Range(3.5f, 5f), 2);
                conveyor.SetConveyorSpeed(-aVelocity, stuntTime, 1.925f);
                conveyorSpeed = (float)System.Math.Round((conveyor.GetConveyorVelocity() * -1), 2);
                float Vfp = 15 - conveyorSpeed;
                acceleration = (float)System.Math.Round((Vfp / stuntTime), 2);

                whatIsAsk = UnitOf.acceleration;
                myPlayer.transform.position = new Vector2(6 - distance, 3);
                myPlayer.gameObject.SetActive(true);

                labels.distanceSpawnPnt = new Vector2(6 - distance, 3);
                labels.heightSpawnPnt = new Vector2(-17f, -1.925f);
                labels.UnknownIs('N');
                height = 3.85f;
                question =
                    $"{playerName} is instructed to run on a moving conveyor with the speed of <b>{conveyorSpeed}{qc.Unit(UnitOf.angularVelocity)}</b>. What should be {pNoun} aceleration if {pronoun} starts at stationary ";
                break;
            case 3:
                cm.gameObject.SetActive(true);
                stage3Layout.SetActive(true);
                qc = UI3.GetComponent<QuestionController2_0_1>();

                qc.stage = 2;
                conveyor = FindObjectOfType<ConveyorManager>();
                playerAnim.speed = 1 / 2f;
                myPlayer.climb = false;
                myPlayer.running = false;
                myPlayer.walking = true;
                qc.limit = 5f;

                float hangerDist,
                    dT = 37,
                    dj,
                    cmAv;
                while (true)
                {
                    distance = (float)System.Math.Round((Random.Range(16F, 21F)), 2);
                    aVelocity = (float)System.Math.Round((Random.Range(15f, 25f)), 2);
                    playerSpeed = (float)System.Math.Round(Random.Range(2f, 10.49f), 2);
                    conveyor.SetConveyorSpeed(-aVelocity, stuntTime, 1.15f);
                    conveyorSpeed = (float)System.Math.Round(
                        (conveyor.GetConveyorVelocity() * -1),
                        2
                    );

                    stuntTime = distance / (playerSpeed + conveyorSpeed);
                    av = (dT / stuntTime) - (playerSpeed + conveyorSpeed);
                    if ((stuntTime <= qc.limit) && (stuntTime >= 3))
                        break;
                }

                correctAnswer = (float)System.Math.Round(stuntTime, 2);
                whatIsAsk = UnitOf.time;
                myPlayer.transform.position = new Vector2(-18, 3);
                myPlayer.gameObject.SetActive(true);

                labels.distanceSpawnPnt = new Vector2(myPlayer.transform.position.x, 3);
                labels.heightSpawnPnt = new Vector2(2.9f, -0.64f);
                height = 2.8f;
                labels.UnknownIs('t');
                break;
            default:
                break;
        }
        spawnPoint = labels.distanceSpawnPnt;
        labels.SetPlayerPosition(myPlayer.transform.position);
        labels.showLines(distance, height, playerSpeed, stuntTime);
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
        PrefabDestroyer.end = true;
        // conveyor.isActive = false;
        // FallingBoulders.isRumbling = false;
        qc.retried = false;
        StartCoroutine(life.endBGgone());
        yield return new WaitForSeconds(3);
        // myPlayer.ToggleTrigger();
        // myPlayer.transform.position = new Vector2(0, boulder.transform.position.y);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        // RumblingManager.isCrumbling = false;
        ConveyorManager.isActive = false;
        yield return new WaitForEndOfFrame();
        SetUp();
    }

    IEnumerator Next()
    {
        PrefabDestroyer.end = true;
        qc.nextStage = false;
        myPlayer.happy = false;
        yield return new WaitForSeconds(3f);
        // StartCoroutine(life.endBGgone());
        ConveyorManager.isActive = false;
        yield return new WaitForSeconds(0.5f);
        // myPlayer.transform.position = new Vector2(0, boulder.transform.position.y);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        // RumblingManager.isCrumbling = false;

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
