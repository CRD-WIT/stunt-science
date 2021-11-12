using System.Collections;
using UnityEngine;
using TMPro;
using GameConfig;
// Test

public class HardManager : MonoBehaviour
{
    AngleAnnotaion labels;
    AngleAnnotaion1 triangleAnnotaion;
    IndicatorManagerV1_1 indicators;
    BossScript boss;
    PlayerV2 myPlayer;
    QuestionControllerVThree qc;
    public GameObject directorsBubble, bossHead, stonePrefab, triangle, rumbling, ragdollSpawn, throwingPath, throwingPathTxt, gem, rock, veloTime, stoneVeloLabel, bossVeloLabel;
    public GameObject[] bossParts;
    public HingeJoint2D[] joints;
    public TMP_Text directorsSpeech;
    float x, y, bossV, playerAnswer, stuntTime, elapsed, bossDistance, stoneV, correctAnswer, angle, distance, throwTime, stonePosX, initialDistance,
        sX, sY, dT, xS, shakeDuration, decreaseFactor = 1.0f, shakeAmount = 0.08f, distanceTraveled, angleB = 0;
    bool isAnswered, isEndOfStunt, isStartOfStunt, directorIsCalling, isAnswerCorrect, isThrown, stage1Flag, stoneIsPresent, reset, ragdoll = false, isEnd =false;
    public bool readyToCheck;
    string messageTxt, question, playerName, playerGender, pronoun, pPronoun;
    public int stage;
    Vector2 bossStartPos, currentBossPos, initialPlayerPos;
    Vector3 originaCamlPos;
    public Rigidbody2D bossRB;
    GameObject stone;
    Rock stoneScript;
    public Transform camTransform;
    public Animator bossAnim;
    StageManager sm = new StageManager();
    public HeartManager life;
    public TMP_Text debugAnswer;
    float? timeL;

    public FirebaseManager firebaseManager;
    public AudioSource lightssfx, camerasfx, actionsfx, cutsfx;
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//to prevent screen from sleeping
        indicators = FindObjectOfType<IndicatorManagerV1_1>();
        labels = FindObjectOfType<AngleAnnotaion>();
        triangleAnnotaion = triangle.GetComponent<AngleAnnotaion1>();
        boss = FindObjectOfType<BossScript>();
        myPlayer = FindObjectOfType<PlayerV2>();
        qc = FindObjectOfType<QuestionControllerVThree>();
        bossRB = bossHead.GetComponent<Rigidbody2D>();
        RagdollV2.myPlayer = myPlayer;
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
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
        sm.SetGameLevel(1);
        qc.levelDifficulty = Difficulty.Hard;
        initialPlayerPos = myPlayer.transform.position;
        bossStartPos = bossHead.transform.position;
        qc.stage = stage;
        SetUp();

        firebaseManager.GameLogMutation(1, 1, "Hard", Actions.Started, 0); 

    }
    void Update()
    {
        debugAnswer.SetText($"Answer: {correctAnswer}");
        if (reset)
        {
            if (stage == 3)
                boss.SetVelocityOfTheHead(1.5f, 11, -1);
            else if (stage == 2)
                boss.SetVelocityOfTheHead(1.5f, 10, 7);
            else
                boss.SetVelocityOfTheHead(1.5f, -x, -y);
            bossRB.constraints = RigidbodyConstraints2D.None;
            StartCoroutine(Reset());
        }
        if (stage1Flag)
        {
            if (!RagdollV2.disableRagdoll)
            {
                RagdollV2.disableRagdoll = true;
                ragdoll = true;
                y = 0;
                stage1Flag = false;
                Debug.Log("Disabled ragdoll called");
                messageTxt = $"<b>{playerName}</b> has unable to throw the stone. Stunt Failed!";
                isEndOfStunt = true;
            }
            if (myPlayer.transform.position.x < (-playerAnswer + 0.5f))
            {
                initialDistance = 1 - playerAnswer;
                myPlayer.moveSpeed = 2.99f;
                indicators.gameObject.SetActive(true);
                indicators.distanceSpawnPnt = new Vector2(initialDistance, 3);
                // indicators.timeSpawnPnt = new Vector2(myPlayer.transform.position.x + 0.5f, 2);
                indicators.showLines(playerAnswer, bossDistance, stoneV, stuntTime);

                throwingPath.transform.localScale = new Vector2(40 / 5f, throwingPath.transform.localScale.y);
                throwingPath.transform.position = new Vector2(myPlayer.transform.position.x + 0.5f, 3);
            }
            else
            {
                stage1Flag = false;
                myPlayer.moveSpeed = 0;
                myPlayer.transform.position = new Vector2(-playerAnswer + 0.5f, myPlayer.transform.position.y);
                isStartOfStunt = true;
                directorIsCalling = true;
            }
        }
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        if (isThrown)
            StartCoroutine(Throw());
        if (isAnswered)
        {
            throwingPath.SetActive(false);
            throwingPathTxt.SetActive(false);

            elapsed += Time.deltaTime;
            bossRB.constraints = RigidbodyConstraints2D.None;
            switch (stage)
            {
                case 1:
                    if ((boss.transform.position.y) <= (1))//stuntTime)
                    {
                        isAnswered = false;
                        bossRB.constraints = RigidbodyConstraints2D.FreezeAll;
                        isEndOfStunt = true;
                    }
                    if (playerAnswer == correctAnswer)
                    {
                        stonePosX = playerAnswer;
                        isAnswerCorrect = true;
                        messageTxt = $"<b>{playerName}</b> successfully performed the stunt and the rock hit the monster's mouth!";
                    }
                    else
                    {
                        isAnswerCorrect = false;
                        messageTxt = $"<b>{playerName}</b> has unable to hit exactly inside the mouth of the monster. Stunt Failed!";
                    }
                    break;
                case 2:
                    if (elapsed >= (playerAnswer - 1))
                    {
                        elapsed = playerAnswer;
                        isThrown = true;
                        isAnswered = false;
                        if (playerAnswer == correctAnswer)
                        {
                            isAnswerCorrect = true;
                            messageTxt = $"<b>{playerName}</b> successfully performed the stunt and the rock hit the monster's mouth!";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            messageTxt = $"<b>{playerName}</b> has unable to hit exactly inside the mouth of the monster. Stunt Failed!";
                        }
                    }
                    break;
                case 3:
                    boss.SetRotation(triangleAnnotaion.angleB, elapsed, stuntTime);
                    if (elapsed >= (stuntTime - throwTime))
                    {
                        isAnswered = false;
                        elapsed = stuntTime;
                        if (playerAnswer == correctAnswer)
                        {
                            isAnswerCorrect = true;
                            isEnd = true;
                            messageTxt = $"<b>{playerName}</b> successfully performed the stunt and the rock hit the monster's mouth!";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            messageTxt = $"<b>{playerName}</b> has unable to hit exactly inside the mouth of the monster. Stunt Failed!";
                        }
                    }
                    break;
            }
            qc.timer = elapsed.ToString("f2") + "s";
            currentBossPos = bossHead.transform.position;
        }
        if(stage == 1)
            bossVeloLabel.GetComponent<RectTransform>().localPosition = new Vector2(bossHead.transform.position.x + 0.5f, bossHead.transform.position.y + 2);
        else if (stage == 2)
            bossVeloLabel.GetComponent<RectTransform>().localPosition = new Vector2(bossHead.transform.position.x -1.75f, bossHead.transform.position.y + 1);
        else
            bossVeloLabel.GetComponent<RectTransform>().localPosition = new Vector2(bossHead.transform.position.x, bossHead.transform.position.y + 1);
        if (stoneIsPresent)
        {
            indicators.SetPlayerPosition(new Vector2(stone.transform.position.x, stone.transform.position.y - 1.5f));
            if (stage == 1){
                distanceTraveled = stone.transform.position.x + playerAnswer;
                }
            else
                distanceTraveled = stone.transform.position.x - myPlayer.transform.position.x - 0.5f;
            if (stoneScript.hit != null)
            {
                shakeDuration = 2.5f;
                shakeAmount = 0.02f;
                isAnswered = false;
                bool hit = (bool)stoneScript.hit;
                if (isAnswerCorrect)
                    hit = true;
                bossAnim.SetBool("hit", hit);
                bossRB.constraints = RigidbodyConstraints2D.FreezeAll;
                isEndOfStunt = true;
                stoneIsPresent = false;
                if (hit)
                {
                    timeL = null;
                    switch (stage)
                    {
                        case 1:
                            distanceTraveled = correctAnswer;
                            break;
                        case 2:
                            distanceTraveled = distance + x;
                            break;
                        case 3:
                            readyToCheck = true;
                            timeL = xS + x;
                            shakeAmount = 0.05f;
                            gem.SetActive(false);
                            StartCoroutine(BossCrumble());
                            break;
                    }
                }
                else
                {
                    hit = false;
                    shakeAmount = 0;
                    //ToDo: deduct 1 life
                    life.ReduceLife();
                }
            }
            indicators.IsRunning(playerAnswer, distanceTraveled);
        }
        if (shakeDuration > 0)
        {
            rumbling.SetActive(true);
            camTransform.localPosition = originaCamlPos + Random.insideUnitSphere * (shakeAmount * 3);
            shakeDuration -= Time.deltaTime * (decreaseFactor * 2);
        }
        else
        {
            rumbling.SetActive(false);
            camTransform.localPosition = originaCamlPos;
        }
        if (isEndOfStunt)
        {
            if (stage == 3){                
                StartCoroutine(EndOfHard());                
            }
            StartCoroutine(StuntResult());                         
        }
        if (qc.isSimulating)
            Play();
        else
        {
            if (qc.nextStage)
            {
                stage = qc.stage;
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
    IEnumerator EndOfHard()
    {
        if(isAnswerCorrect){
            myPlayer.happy = true;
            yield return new WaitForSeconds(2.5f);
            myPlayer.happy = false;
            myPlayer.moveSpeed = 5;
        }else{
            myPlayer.lost = true;
            yield return new WaitForSeconds(2.5f);
            myPlayer.lost =false;
            myPlayer.standup = true;
            myPlayer.moveSpeed =0;
        }
    }
    void OnEnable()
    {
        originaCamlPos = camTransform.localPosition;
    }
    public void SetUp()
    {
        firebaseManager.GameLogMutation(1, 1, "Easy", Actions.Started, 0); 


        directorIsCalling = false;
        isStartOfStunt = true;
        isEndOfStunt = false;
        bossVeloLabel.SetActive(true);
        bossVeloLabel.GetComponent<RectTransform>().localPosition = new Vector2(bossHead.transform.position.x, bossHead.transform.position.y + 1);
        throwingPath.SetActive(true);
        throwingPathTxt.SetActive(true);
        rock.SetActive(true);

        readyToCheck = false;
        bossAnim.SetBool("hit", false);
        ragdollSpawn.SetActive(true);
        labels.gameObject.SetActive(false);
        triangle.SetActive(false);
        stage = qc.stage;
        isThrown = false;
        stoneV = 0;
        elapsed = 0;
        correctAnswer = 0;
        sY = 0;
        myPlayer.happy = false;
        myPlayer.lost = false;
        myPlayer.standup = false;
        life.losslife = false;
        gem.SetActive(true);
        stoneVeloLabel.SetActive(false);
        RagdollV2.disableRagdoll = true;
        
        switch (stage)
        {
            case 1:
                indicators.SetPlayerPosition(new Vector2(100, 100));
                bossHead.transform.position = bossStartPos - new Vector2(5.8f, -2);
                y = -6;
                x = 0;
                qc.limit = 20.5f;
                bossDistance = 6;
                while (true)
                {
                    distance = (float)System.Math.Round(Random.Range(16.5f, 20.5f), 2);
                    bossV = (float)System.Math.Round(Random.Range(4f, 5f), 2);
                    stuntTime = (bossDistance / bossV);
                    stoneV = distance / stuntTime;
                    if ((stoneV < 40f))
                        break;
                }
                qc.SetUnitTo(UnitOf.distance);
                indicators.showLines(null, bossDistance, 0, 0);
                indicators.UnknownIs('d');
                indicators.heightSpawnPnt = new Vector2(1, 3);
                indicators.ShowVelocityLabel(false);
                indicators.ResizeEndLines(null, 6.5f, 0.25f, 0.25f);
                indicators.distanceSpawnPnt = new Vector2(1 - distance, 3);
                indicators.showLines(distance, bossDistance, stoneV, stuntTime);

                myPlayer.transform.position = initialPlayerPos;

                correctAnswer = distance;
                angle = (float)System.Math.Round(((System.Math.Atan2(x, y) * 180) / System.Math.PI), 2);

                throwingPath.transform.localScale = new Vector2(40f / 5f, throwingPath.transform.localScale.y);
                throwingPath.transform.position = new Vector2(myPlayer.transform.position.x + 0.5f, 3);
                throwingPathTxt.transform.position = throwingPath.transform.position + new Vector3(distance / 2f, 0);
                
                bossVeloLabel.transform.Find("Square").rotation = Quaternion.EulerAngles(0, 0,(-90 - angle) * Mathf.Deg2Rad);
                bossVeloLabel.GetComponent<RectTransform>().localPosition = new Vector2(bossHead.transform.position.x + 0.5f, bossHead.transform.position.y + 1);

                question = $"<b>{playerName}</b> is instucted to throw a rock inside the monster's mouth that is guarding the exit. If the monster is <b>{bossDistance.ToString("f2")} {qc.Unit(UnitOf.distance)}</b> above {pPronoun} horizontal throwing line and the monster is moving <b>{bossV} {qc.Unit(UnitOf.velocity)}</b> downward, how far should <b>{playerName}</b> be away horizontally from the monster if {pronoun} throws the stone at exact and constant velocity of <b>{stoneV.ToString("f2")} {qc.Unit(UnitOf.velocity)}</b>?";
                break;
            case 2:
                bossHead.transform.position = new Vector2(11, bossStartPos.y + 3);
                qc.SetUnitTo(UnitOf.time);
                labels.gameObject.SetActive(true);
                float allowanceTime;
                y = -7;
                qc.limit = 5;
                while (true)
                {
                    bossV = (float)System.Math.Round(Random.Range(4f, 5f), 2);
                    x = Random.Range(-20f, 0);
                    distance = Random.Range(28f, 31f);
                    bossDistance = Mathf.Sqrt((x * x) + (y * y));
                    stuntTime = bossDistance / bossV;
                    angle = Mathf.Atan(x / y) * Mathf.Rad2Deg;
                    allowanceTime = Random.Range(0.75f, 1.5f);
                    stoneV = (distance + x) / (allowanceTime);
                    if ((stuntTime < qc.limit) && (Mathf.Abs(angle) > 45) && (stuntTime > (allowanceTime + 2)&&(stoneV >= 14)))
                        break;
                }
                correctAnswer = (float)System.Math.Round(stuntTime - allowanceTime, 2);

                myPlayer.transform.position = new Vector2(-distance + 0.5f + 10, myPlayer.transform.position.y);
                indicators.distanceSpawnPnt = new Vector2(-distance + 11, 2.5f);
                // indicators.timeSpawnPnt = new Vector2(myPlayer.transform.position.x + 0.5f, 1);
                indicators.showLines(distance, null, stoneV, stuntTime);
                indicators.UnknownIs('t');
                indicators.ResizeEndLines(0.5f, 0.25f, null, null);
                indicators.SetPlayerPosition(new Vector2(myPlayer.transform.position.x, myPlayer.transform.position.y - 0.5f));

                labels.startingAngle = 180;
                labels.SetSpawnPnt(new Vector2(11, bossHead.transform.position.y));
                labels.angleA = angle;
                labels.legA = 7;
                labels.legB = bossDistance;
                labels.HideValuesOf(false, true, true, false, true, true);

                bossVeloLabel.transform.Find("Square").rotation = Quaternion.EulerAngles(0, 0, 0);
                bossVeloLabel.transform.GetComponent<RectTransform>().localRotation = Quaternion.EulerAngles(0, 0,(-270-angle) * Mathf.Deg2Rad);
                bossVeloLabel.GetComponent<RectTransform>().localPosition = new Vector2(bossHead.transform.position.x -1.75f, bossHead.transform.position.y + 1);
                Debug.Log(angle);

                throwingPath.transform.localScale = new Vector2(40 / 5, throwingPath.transform.localScale.y);
                throwingPath.transform.position = new Vector2(myPlayer.transform.position.x + 0.5f, 3);
                throwingPathTxt.transform.position = throwingPath.transform.position + new Vector3((distance + x) / 2f, 0);

                question = $"<b>{playerName}</b> is again instucted to throw another rock into the mouth of the monster. The mouth of the monster this time is <b>{(-y).ToString("f2")} {qc.Unit(UnitOf.distance)}</b> above the horizontal throwing path and horizontally <b>{distance.ToString("f2")} {qc.Unit(UnitOf.distance)}</b> away. If the monster is moving diagonally forward and downward at <b>{angle.ToString("f2")}{qc.Unit(UnitOf.angle)}</b> below horizon with a velocity of <b>{bossV.ToString("f2")} {qc.Unit(UnitOf.velocity)}</b>, how many <b>seconds</b> should <b>{playerName}</b> wait after the monster has moved to hit its mouth with a stone thrown horizontally at a constant velocity of <b>{stoneV.ToString("f2")} {qc.Unit(UnitOf.velocity)}</b>?";
                break;
            case 3:
                bossHead.transform.localEulerAngles = new Vector3(0,0,0);
                float sideA = 0;
                qc.SetUnitTo(UnitOf.velocity);
                ragdollSpawn.SetActive(false);
                labels.gameObject.SetActive(true);
                triangle.SetActive(true);
                qc.limit = 40;
                while (true)
                {
                    bossV = (float)System.Math.Round(Random.Range(4f, 5f), 2);
                    x = Random.Range(-15f, 0f);
                    y = Random.Range(5f, 7f);
                    dT = Random.Range(15f, 18f);
                    // sideA = y - 1;
                    xS = dT - x;
                    distance = Mathf.Sqrt((dT * dT) + (y * y));
                    bossDistance = Mathf.Sqrt((x * x) + (y * y));
                    stuntTime = bossDistance / bossV;
                    stoneV = distance / stuntTime;
                    angle = Mathf.Atan(x / y) * Mathf.Rad2Deg;
                    if ((stoneV < qc.limit) && (Mathf.Abs(x) > 5))//&&(stoneV>=14))
                        break;
                }
                bossHead.transform.position = new Vector2(-4 - x, bossStartPos.y - 4);
                myPlayer.transform.position = new Vector2(-xS - x - 4.5f, myPlayer.transform.position.y);
                indicators.distanceSpawnPnt = new Vector2(-xS - 4 - x, 2.25f);

                indicators.SetPlayerPosition(new Vector2(myPlayer.transform.position.x, myPlayer.transform.position.y - 1.5f));
                indicators.showLines(xS, null, stoneV, stuntTime);
                indicators.UnknownIs('v');
                indicators.ResizeEndLines(0.5f, 0.25f, null, null);
                indicators.ShowVelocityLabel(false);

                correctAnswer = (float)System.Math.Round(stoneV, 2);

                labels.startingAngle = 180;
                labels.SetSpawnPnt(new Vector2(bossHead.transform.position.x + x, bossHead.transform.position.y + y));
                labels.legA = y;
                labels.angleA = angle;
                labels.legB = bossDistance;
                labels.HideValuesOf(true, false, true, true, true, true);

                triangleAnnotaion.startingAngle = 180;
                triangleAnnotaion.SetSpawnPnt(new Vector2(bossHead.transform.position.x + x, bossHead.transform.position.y + y));
                triangleAnnotaion.legA = y;
                triangleAnnotaion.legB = Mathf.Sqrt((dT * dT) + (y * y));
                triangleAnnotaion.angleA = Mathf.Atan(dT / y) * Mathf.Rad2Deg;
                triangleAnnotaion.HideValuesOf(true, false, true, true, true, false);

                throwingPath.transform.localScale = new Vector2(40 / 5, throwingPath.transform.localScale.y);
                throwingPath.transform.Rotate(0, 0, 90 - (Mathf.Atan(dT / y) * Mathf.Rad2Deg) - angleB);
                throwingPath.transform.position = new Vector2(myPlayer.transform.position.x + 0.5f, 3);
                
                veloTime.transform.Rotate(0, 0, 90 - (Mathf.Atan(dT / y) * Mathf.Rad2Deg) - angleB);
                veloTime.SetActive(false);
                veloTime.transform.Find("line1").GetComponent<LineRenderer>().enabled =false;

                bossVeloLabel.transform.GetComponent<RectTransform>().localRotation = Quaternion.EulerAngles(0, 0,(-90 - angle ) * Mathf.Deg2Rad);

                angleB = throwingPath.transform.localEulerAngles.z;
                stoneVeloLabel.SetActive(true);
                stoneVeloLabel.GetComponent<TMP_Text>().text = $"v = ?{qc.Unit(UnitOf.velocity)}";
                stoneVeloLabel.transform.position = new Vector2(myPlayer.transform.position.x, myPlayer.transform.position.y + 1.5f);
                stoneVeloLabel.transform.GetComponent<RectTransform>().localRotation= Quaternion.EulerAngles(0, 0, 90 - (Mathf.Atan(dT / y)) - (angleB*Mathf.Deg2Rad));

                // throwingPathTxt[2].SetActive(true);
                throwingPathTxt.transform.position = throwingPath.transform.position + new Vector3(dT / 2, (y) / 2);
                throwingPathTxt.transform.GetComponent<RectTransform>().localRotation= Quaternion.EulerAngles(0, 0, 90 - (Mathf.Atan(dT / y)) - (angleB*Mathf.Deg2Rad));
                
                bossVeloLabel.GetComponent<RectTransform>().localPosition = new Vector2(bossHead.transform.position.x, bossHead.transform.position.y + 1);
                
                question = $"<b>{playerName}</b> is finally instucted to throw one more rock into the monster's mouth for the last time. If the exact location of the monster's mouth is exactly <b>{xS.ToString("f2")} {qc.Unit(UnitOf.distance)}</b> away horizontally for the starting point of the throwing path and the monster is moving diagonally up and forward at <b>{bossV.ToString("f2")} {qc.Unit(UnitOf.velocity)}</b> at <b>{(90+angle).ToString("f2")}{qc.Unit(UnitOf.angle)}</b>, at what velocity should <b>{playerName}</b> throw the stone at <b>{angleB.ToString("f2")}{qc.Unit(UnitOf.angle)}</b> up to hit the monster's mouth? ";
                break;
        }

        bossVeloLabel.GetComponent<TMP_Text>().text = $"v = {bossV.ToString("f2")}{qc.Unit(UnitOf.velocity)}";
        boss.SetVelocityOfTheHead(stuntTime, x, y);
        sX = x;
        qc.question = question;
        Debug.Log(correctAnswer + " ans");
    }
    void Play()
    {
        playerAnswer = qc.GetPlayerAnswer();
        qc.isSimulating = false;
        switch (stage)
        {
            case 1:
                stage1Flag = true;
                if (playerAnswer > correctAnswer)
                    throwTime = stuntTime + 0.1f;//(playerAnswer / stoneV) + 0.1f;
                else if (playerAnswer < correctAnswer)
                    throwTime = stuntTime - 0.1f;//(playerAnswer / stoneV) - 0.1f;
                else
                    throwTime = stuntTime;//correctAnswer / stoneV;
                break;
            case 2:
                isStartOfStunt = true;
                directorIsCalling = true;

                if (playerAnswer > correctAnswer)
                    throwTime = (stuntTime - playerAnswer) + 0.1f;
                else if (playerAnswer < correctAnswer)
                    throwTime = (stuntTime - playerAnswer) - 0.1f;
                else
                    throwTime = (stuntTime - correctAnswer);
                break;
            case 3:
                // indicators.timeSpawnPnt = new Vector2(myPlayer.transform.position.x + 0.5f, myPlayer.transform.position.y - 1);
                // indicators.SetPlayerPosition(myPlayer.transform.position.x - );
                indicators.showLines(null, null, stoneV, stuntTime);
                isStartOfStunt = true;
                directorIsCalling = true;
                if (playerAnswer > correctAnswer)
                    throwTime = (distance / playerAnswer) - 0.5f;
                else if (playerAnswer < correctAnswer)
                    throwTime = (distance / playerAnswer) + 0.5f;
                else
                    throwTime = (distance / correctAnswer);
                break;
        }
    }
    IEnumerator Reset()
    {
        reset = false;
        yield return new WaitForSeconds(1.5f);
        bossRB.constraints = RigidbodyConstraints2D.FreezeAll;
        SetUp();
    }
    IEnumerator Retry()
    {
        qc.retried = false;
        bossVeloLabel.SetActive(false);
        yield return new WaitForSeconds(1);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        if (!ragdoll)
            stoneScript.destroyer = true;
        ragdoll = false;
        reset = true;
        boss.SetRotation(0, 0, 0);
    }
    IEnumerator Next()
    {
        qc.nextStage = false;
        bossVeloLabel.SetActive(false);
        myPlayer.happy = false;
        yield return new WaitForSeconds(1f);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        stoneScript.destroyer = true;
        reset = true;
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
            yield return new WaitForSeconds(0.50f);
            if (stage == 2)
                isThrown = false;
            else isThrown = true;
            yield return new WaitForSeconds(0.25f);
            directorsSpeech.text = "Action!";
            actionsfx.Play();
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "";
            directorsBubble.SetActive(false);
            isAnswered = true;
        }
        else
        {
            if (stage != 3)
            {
                if (isAnswerCorrect)
                    myPlayer.happy = true;
                else
                {
                    if (ragdoll)
                    {
                        myPlayer.lost = false;
                        myPlayer.standup = false;
                        myPlayer.ToggleTrigger();
                    }
                    else
                    {
                        myPlayer.lost = true;
                        myPlayer.standup = true;
                    }
                }
            }
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            cutsfx.Play();
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";
        }
    }
    IEnumerator BossCrumble()
    {
        bossVeloLabel.SetActive(false);
        yield return new WaitForSeconds(0.15f);
        foreach (var item in bossParts)
            item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        foreach (var item in joints)
            item.enabled = false;

        boss.SetVelocityOfTheHead(1, 10, -8);
        bossRB.constraints = RigidbodyConstraints2D.None;
    }
    IEnumerator StuntResult()
    {
        isEndOfStunt = false;
        RagdollV2.disableRagdoll =false;
        yield return new WaitForSeconds(1f);
        directorIsCalling = true;
        isStartOfStunt = false;
        yield return new WaitForSeconds(3);
        qc.ActivateResult(messageTxt, isAnswerCorrect, isEnd);
    }
    IEnumerator Throw()
    {
        isThrown = false;
        myPlayer.thrown = true;
        yield return new WaitForSeconds(1);
        myPlayer.thrown = false;
        stone = Instantiate(stonePrefab);
        stoneScript = FindObjectOfType<Rock>();
        stone.transform.position = new Vector2(myPlayer.transform.position.x + 0.5f, 3);

        stoneScript.hit = null;
        stone.GetComponent<Rigidbody2D>().gravityScale = 0;
        if (stage == 2)
            stoneScript.SetVelocity(throwTime, distance + x, sY);
        else if (stage == 1){
            stoneScript.SetVelocity(throwTime, distance, sY);
            indicators.ShowVelocityLabel(true);
        }
        else{
            indicators.ShowVelocityLabel(false);
            stoneScript.SetVelocity(throwTime, dT, y);
            stone.transform.Rotate(0,0,angleB);
            veloTime.SetActive(true);
            stoneVeloLabel.SetActive(false);
        }
        rock.SetActive(false);
        stoneIsPresent = true;
    }
}
