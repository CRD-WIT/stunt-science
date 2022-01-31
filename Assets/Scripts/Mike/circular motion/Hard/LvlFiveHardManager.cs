using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;
using TMPro;


public class LvlFiveHardManager : MonoBehaviour
{
    public TMP_Text debugAnswer;
    UnitOf whatIsAsk;

    [SerializeField]
    float distance,
        mechaVelocity,
        aVelocity,
        radius,
        stuntTime,
        playerVelocity,
        correctAnswer,
        elapsed;
    float initialPlayerPos,
        playerAnswer,
        camStartPos,
        mechaPos,
        adjustedAnswer,
        velocityY,
        armVelo;
    public static int stage;
    string playerName,
        playerGender,
        pronoun,
        pPronoun,
        question,
        messageTxt;
    bool directorIsCalling,
        isStartOfStunt,
        isAnswered,
        isAnswerCorrect,
        ragdoll = false,
        playerLanded,
        isEndOfStunt,
        isEnd = false;
    public BoxCollider2D playerStopper;
    MechaManager mm;
    QuestionController2_0_1 qc;
    IndicatorManager2_0_1 indicators;
    AngleAnnotation2 angleIndicators;
    PlayerCM2 myPlayer;
    Rigidbody2D player;
    Grenade grenade;
    GameObject grenadeObj;
    HeartManager2 life;
    StageManager sm = new StageManager();

    [SerializeField]
    GameObject directorsBubble,
        grenadePrefab,
        mechaArm;

    [SerializeField]
    TMP_Text directorsSpeech;

    [SerializeField]
    AudioSource lightssfx,
        camerasfx,
        actionsfx,
        cutsfx;

    [SerializeField]
    HingeJoint2D playerStopper1,
        playerStopper2;

    [SerializeField]
    Camera main;

    [SerializeField]
    Animator playerAnim;

    [SerializeField]
    GameObject[] labels;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Life", 3);
        stage = 1;
        qc = FindObjectOfType<QuestionController2_0_1>();
        mm = FindObjectOfType<MechaManager>();
        myPlayer = FindObjectOfType<PlayerCM2>();
        player = myPlayer.GetComponent<Rigidbody2D>();
        grenade = FindObjectOfType<Grenade>();
        grenadeObj = FindObjectOfType<Grenade>().gameObject;
        indicators = labels[0].GetComponent<IndicatorManager2_0_1>();
        angleIndicators = labels[1].GetComponent<AngleAnnotation2>();

        camStartPos = main.transform.position.x;
        sm.SetGameLevel(4);
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        if (playerGender == "Male")
        {
            pronoun = "he";
            pPronoun = "his";
        }
        else
        {
            pronoun = "she";
            pPronoun = "her";
        }
        qc.levelDifficulty = Difficulty.Hard;

        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        debugAnswer.SetText($"Answer: {correctAnswer}");
        
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        if (isAnswered)
        {
            elapsed += Time.deltaTime;
            switch (stage)
            {
                case 1:
                    mm.SetMechaVelocity(aVelocity, stuntTime, 1.05f);
                    myPlayer.moveSpeed = playerVelocity;
                    float playerPos = myPlayer.transform.position.x + initialPlayerPos;
                    if (playerPos >= playerAnswer)
                    {
                        Debug.Log(playerPos);
                        elapsed = stuntTime;
                        StartCoroutine(Jump());
                    }
                    if (adjustedAnswer == correctAnswer)
                    {
                        isAnswerCorrect = true;
                        messageTxt = "correct";
                    }
                    else
                    {
                        isAnswerCorrect = false;
                        playerStopper.enabled = true;
                        messageTxt = "wrong";
                    }
                    break;
                case 2:
                    myPlayer.walking = false;
                    mm.armRotation = -armVelo;
                    playerStopper1.enabled = false;
                    myPlayer.moveSpeed = playerAnswer + mm.velocity.x;
                    playerAnim.speed = playerAnswer / 2.8f;
                    if (elapsed >= stuntTime) //((myPlayer.transform.position.x - initialPlayerPos) >= 6.55f)
                    {
                        Debug.Log("Stage2");
                        elapsed = stuntTime;
                        if (adjustedAnswer == correctAnswer)
                        {
                            playerStopper2.enabled = true;
                            isAnswerCorrect = true;
                            messageTxt = "correct";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            messageTxt = "wrong";
                        }
                        StartCoroutine(Grab());
                    }
                    break;
                case 3:
                    mm.armRotation = armVelo;
                    if (elapsed >= stuntTime)
                    {
                        grenade.isThrown = true;
                    }
                    else
                    {
                        grenade.isThrown = false;
                    }
                    if (grenade.explode)
                    {
                        Destroy(grenadeObj);
                        if (correctAnswer == adjustedAnswer)
                        {
                            mm.armRotation = 0;
                            mm.velocity = new Vector2(0, 0);
                            mm.off = true;
                            isEnd = true;
                            messageTxt = "correct";
                        }
                        else
                        {
                            life.ReduceLife();
                            messageTxt = "wrong";
                        }
                        isAnswered = false;
                        StartCoroutine(StuntResult());
                    }
                    break;
                    qc.timer = elapsed.ToString("f2") + "s";
            }
        }
        if (playerLanded)
        {
            float camDistanceFromRobot = mechaPos - camStartPos;
            main.transform.position = new Vector3(
                mm.transform.position.x - camDistanceFromRobot,
                main.transform.position.y,
                -10
            );
            if (isAnswerCorrect)
            {
                playerStopper1.enabled = true;
            }
            else
            {
                life.ReduceLife();
                //ragdoll
            }
        }

        if (qc.isSimulating)
            Play();
        else
        {
            if (qc.nextStage)
                Next();
            else if (qc.retried)
                Reset();
            else
            {
                qc.nextStage = false;
                qc.retried = false;
            }
        }
    }

    void SetUp()
    {
        playerStopper.enabled = false;
        foreach (var item in labels)
        {
            item.SetActive(false);
        }
        elapsed = 0;
        playerLanded = false;
        stage = qc.stage;
        float pV,
            mV,
            cT,
            jT,
            d,
            t,
            av,
            jd;
        switch (stage)
        {
            case 1:
                labels[0].gameObject.SetActive(true);
                radius = 1.05f;
                qc.stage = 1;
                qc.limit = 20;
                while (true)
                {
                    pV = (float)System.Math.Round(Random.Range(8f, 10.49f), 2);
                    d = (float)System.Math.Round(Random.Range(30f, 33f), 2);
                    t = (float)System.Math.Round(Random.Range(2.5f, 4f), 2);
                    av = (float)System.Math.Round(Random.Range(-100f, -150f), 2);

                    mV = (float)System.Math.Round(mm.MechaVelocity(av * (-1), t, radius), 2);
                    cT = d / (pV + mV);
                    jT = cT - 0.5f;
                    jd = pV * jT;
                    if (jd < 20)
                        break;
                }
                playerVelocity = pV;
                distance = d;
                stuntTime = t;
                aVelocity = av;
                mechaVelocity = mV;
                correctAnswer = (float)System.Math.Round(jd, 2);

                myPlayer.transform.position = new Vector2(-distance + 12, 0);
                initialPlayerPos = distance - 12;
                Debug.Log(initialPlayerPos);

                indicators.distanceSpawnPnt = new Vector2(myPlayer.transform.position.x, 1);

                // indicators.distanceSpawnPnt = new Vector2(-10, 2);
                indicators.heightSpawnPnt = new Vector2(
                    mm.transform.Find("gear2").position.x - 1,
                    mm.transform.Find("gear2").position.y - 1.05f
                );
                indicators.UnknownIs('v');
                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(distance, 2.1f, playerVelocity, stuntTime);

                whatIsAsk = UnitOf.distance;
                question =
                    $"{playerName} is intructed to run and jump on top of the robot's wheel. The robot has an engine that has <b>{Mathf.Abs(aVelocity)}{qc.Unit(UnitOf.angularVelocity)}</b> revolution with gear that has a radius of <b>1.05 m</b> is moving towards {pPronoun} and {pronoun} is moving <b>{playerVelocity}{qc.Unit(UnitOf.velocity)}</b> towards the robot. If {playerName} needs to jump 0.5s before the collision, How far does {pronoun} have to run before jumping?";
                //player playerVelocity to jump exactly to the mecha
                break;
            case 2:
                labels[1].SetActive(true);
                mm.armRotation = 0;
                radius = 0.775f;
                float pVx,
                    pVy,
                    dx = 6.55f,
                    dy = 1.75f;
                main.transform.position = new Vector3(
                    mm.transform.position.x + 5.5f,
                    main.transform.position.y,
                    -10
                );
                d = 6.78f;
                qc.limit = 10.49f;
                while (true)
                {
                    t = (float)System.Math.Round(Random.Range(2.5f, 4f), 2);
                    pVx = dx / t;
                    pVy = dy / t;
                    av = (float)System.Math.Round(Random.Range(-150f, -200f), 2);

                    armVelo = (float)System.Math.Round(312 / t, 2);
                    mV = (float)System.Math.Round(mm.MechaVelocity(av * (-1), t, radius), 2);
                    pV = d / t + mV;
                    if (pV < 10.5f)
                        break;
                }
                stuntTime = t;
                aVelocity = av;
                mechaVelocity = mV;
                correctAnswer = (float)System.Math.Round(pV, 2);
                initialPlayerPos = myPlayer.transform.position.x;
                whatIsAsk = UnitOf.velocity;
                angleIndicators.startingAngle = 180;
                angleIndicators.SetSpawnPnt(
                    new Vector2(mm.transform.position.x + 8.801765f, 2.275f)
                );
                angleIndicators.angleA = 75.5f;
                angleIndicators.legA = 2.1F;
                angleIndicators.legB = 8.38725F;
                angleIndicators.HideValuesOf(true, false, true, true, true, false);
                question =
                    $"{playerName} is intructed to grab on the lower clamp of the robot's hand. The robot has an engine that has <b>{Mathf.Abs(aVelocity)}{qc.Unit(UnitOf.angularVelocity)}</b> revolution with gear that has a radius of <b>0.775 m</b> is moving forward. If {playerName} needs to grab the clamp at exactly 300{qc.Unit(UnitOf.angle)} from the starting position of the clamp, what should be {playerName}'s velocity?";

                while (mechaArm.transform.localEulerAngles.z != 0)
                {
                    mechaArm.transform.localEulerAngles = new Vector3(0, 0, 0);
                    Debug.Log(mechaArm.transform.localEulerAngles.z);
                }
                mechaArm.transform.localEulerAngles = new Vector3(
                    0,
                    0,
                    mechaArm.transform.localEulerAngles.z + 1.199f
                );
                break;
            case 3:
                // t =
                if (mechaArm.transform.localEulerAngles.z != 0)
                {
                    mm.armRotation = 0;
                    mechaArm.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                qc.limit = 3.5f;
                float armAngularVelo,
                    arc;
                while (true)
                {
                    arc = (float)System.Math.Round(Random.Range(200f, 250f), 2);
                    armAngularVelo = (float)System.Math.Round(Random.Range(50f, 70f), 2);
                    t = (float)System.Math.Round((arc / armAngularVelo), 2);

                    if (t < 3.5f)
                        break;
                }
                armVelo = -armAngularVelo;
                Debug.Log(arc);
                mechaArm.transform.localEulerAngles = new Vector3(0, 0, -250 + arc);
                stuntTime = t;
                correctAnswer = t;
                whatIsAsk = UnitOf.time;
                Debug.Log(stuntTime);
                break;
        }
        qc.SetQuestion(question);
    }

    void Play()
    {
        playerAnswer = qc.GetPlayerAnswer();
        adjustedAnswer = qc.AnswerTolerance(correctAnswer);
        if (stage == 2)
        {
            myPlayer.moveSpeed = playerAnswer * 1.0351f;
            player.velocity = new Vector2(myPlayer.moveSpeed, (playerAnswer * (float)3.8743));
        }
        qc.isSimulating = false;
        isStartOfStunt = true;
        directorIsCalling = true;
    }

    void Reset()
    {
        if (stage == 3)
        {
            Instantiate(grenadeObj);
        }
        SetUp();
    }

    void Next()
    {
        mm.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        qc.nextStage = false;
        SetUp();
    }

    public IEnumerator DirectorsCall()
    {
        directorIsCalling = false;
        if (isStartOfStunt)
        {
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Lights!";
            // lightssfx.Play();
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Camera!";
            // camerasfx.Play();
            yield return new WaitForSeconds(0.50f);
            yield return new WaitForSeconds(0.25f);
            directorsSpeech.text = "Action!";
            // actionsfx.Play();
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "";
            directorsBubble.SetActive(false);
            isAnswered = true;
            isStartOfStunt = false;
        }
        else
        {
            // if (stage != 3)
            // {
            //     if (isAnswerCorrect)
            //         myPlayer.happy = true;
            //     else
            //     {
            //         if (ragdoll)
            //         {
            //             myPlayer.lost = false;
            //             myPlayer.standup = false;
            //             myPlayer.ToggleTrigger();
            //         }
            //         else
            //         {
            //             myPlayer.lost = true;
            //             myPlayer.standup = true;
            //         }
            //     }
            // }
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            // cutsfx.Play();
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";
        }
    }

    IEnumerator Jump()
    {
        myPlayer.jumpforce = 2;
        myPlayer.jump();
        yield return new WaitForSeconds(0.4f);
        myPlayer.moveSpeed = 0;
        myPlayer.jumpforce = 0;
        playerLanded = true;
        isAnswered = false;
        myPlayer.walking = true;
        mechaPos = mm.transform.position.x;
        StartCoroutine(StuntResult());
    }

    IEnumerator Grab()
    {
        if (isAnswerCorrect)
        {
            myPlayer.moveSpeed = 0;
            playerAnim.speed = 1;
            myPlayer.walking = false;
            myPlayer.jumpHang = true;
            yield return new WaitForEndOfFrame();
            myPlayer.isHanging = true;
            // playerStopper2.enabled =true;
            mm.armRotation = -20;
        }
        else
        {
            life.ReduceLife();
            //ragdoll
        }
        isAnswered = false;
        yield return new WaitForSeconds(1);
        StartCoroutine(StuntResult());
    }

    IEnumerator StuntResult()
    {
        isEndOfStunt = false;
        // yield return new WaitForSeconds(0.25f);
        directorIsCalling = true;
        isStartOfStunt = false;
        yield return new WaitForSeconds(1f);
        qc.ActivateResult(messageTxt, isAnswerCorrect, isEnd);
    }
}

/*
Required    |   stage   |   radius
----------------------------------
distance    |   1       |   1.05
velocity    |   2       |   0.775
time        |   3       |   0.575
*/
