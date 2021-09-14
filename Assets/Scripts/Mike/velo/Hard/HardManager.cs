using System.Collections;
using UnityEngine;
using TMPro;
using GameConfig;

public class HardManager : MonoBehaviour
{
    AngleAnnotaion labels;
    AngleAnnotaion1 triangleAnnotaion;
    IndicatorManagerV1_1 indicators;
    BossScript boss;
    PlayerV2 myPlayer;
    QuestionControllerVThree qc;
    public GameObject directorsBubble, bossHead, stonePrefab, triangle, rumbling, ragdollSpawn;
    public GameObject[] gem, bossParts;
    public HingeJoint2D[] joints;
    public TMP_Text directorsSpeech;
    float x, y, bossV, playerAnswer, stuntTime, elapsed, bossDistance, stoneV, correctAnswer, angle, distance, throwTime, stonePosX, initialDistance,
        sX, sY, dT, xS, shakeDuration, decreaseFactor = 1.0f, shakeAmount = 0.08f, distanceTraveled;
    bool isAnswered, isEndOfStunt, isStartOfStunt, directorIsCalling, isAnswerCorrect, isThrown, stage1Flag, stoneIsPresent, reset, ragdoll = false;
    string messageTxt, question, playerName, playerGender, pronoun, pPronoun;
    public int stage;
    Vector2 bossStartPos, currentBossPos, initialPlayerPos;
    Vector3 originaCamlPos;
    public Rigidbody2D bossRB;
    GameObject stone;
    Rock stoneScript;
    public Transform camTransform;
    void Start()
    {
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
        qc.levelDifficulty = Difficulty.Hard;
        initialPlayerPos = myPlayer.transform.position;
        bossStartPos = bossHead.transform.position;
        qc.stage = stage;
        SetUp();
    }
    void Update()
    {
        if (reset)
        {
            boss.SetVelocityOfTheHead(0.5f, -x, -y);
            bossRB.constraints = RigidbodyConstraints2D.None;
            StartCoroutine(Reset());
        }
        if (stage1Flag)
        {
            if (myPlayer.transform.position.x < (-playerAnswer + 0.5f))
            {
                initialDistance = 1 - playerAnswer;
                myPlayer.moveSpeed = 1.99f;
                indicators.gameObject.SetActive(true);
                indicators.distanceSpawnPnt = new Vector2(initialDistance, 3);
                indicators.timeSpawnPnt = new Vector2(myPlayer.transform.position.x + 0.5f, 2);
                indicators.showLines(playerAnswer, null, bossDistance, stoneV, stuntTime);
            }
            else
            {
                myPlayer.moveSpeed = 0;
                myPlayer.transform.position = new Vector2(-playerAnswer + 0.5f, myPlayer.transform.position.y);
                stage1Flag = false;
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
                        messageTxt = "Correct";
                    }
                    else
                    {
                        isAnswerCorrect = false;
                        messageTxt = "Wrong";
                    }
                    break;
                case 2:
                    if (elapsed >= playerAnswer)
                    {
                        elapsed = playerAnswer;
                        isThrown = true;
                        isAnswered = false;
                        if (playerAnswer == correctAnswer)
                        {
                            isAnswerCorrect = true;
                            messageTxt = "Correct";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            messageTxt = "Wrong";
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
                            messageTxt = "Correct";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            messageTxt = "Wrong";
                        }
                    }
                    break;
            }
            qc.timer = elapsed.ToString("f2") + "s";
            currentBossPos = bossHead.transform.position;
        }
        if (stoneIsPresent)
        {
            if (stage == 1)
                distanceTraveled = stone.transform.position.x + playerAnswer;
            else //(stage == 2)
                distanceTraveled = stone.transform.position.x + distance;
            //     else
            // indicators.SetPlayerPosition(stone.transform.position);
            if (stoneScript.hit != null)
            {
                shakeDuration = 2.5f;
                shakeAmount = 0.02f;
                isAnswered = false;
                // elapsed = stuntTime;
                bossRB.constraints = RigidbodyConstraints2D.FreezeAll;
                bool hit = (bool)stoneScript.hit;
                if (isAnswerCorrect)
                    hit = true;
                isEndOfStunt = true;
                stoneIsPresent = false;
                if (hit)
                {
                    switch (stage)
                    {
                        case 1:
                            distanceTraveled = correctAnswer;
                            gem[0].SetActive(false);
                            gem[1].SetActive(true);
                            break;
                        case 2:
                            distanceTraveled = distance + x;
                            gem[1].SetActive(false);
                            gem[2].SetActive(true);
                            break;
                        case 3:
                            distanceTraveled = sX+x;
                            shakeAmount = 0.05f;
                            gem[2].SetActive(false);
                            StartCoroutine(BossCrumble());
                            break;
                    }
                }
                else
                {
                    hit = false;
                    shakeAmount = 0;
                    //ToDo: deduct 1 life
                }
            }
            indicators.IsRunning(playerAnswer, distanceTraveled, elapsed, stage == 3 ? (stone.transform.position.x + myPlayer.transform.position.x) : (float?)null);
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
            if (stage == 3)
                myPlayer.moveSpeed = 5;
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
        if (RagdollV2.disableRagdoll)
        {
            ragdoll = true;
            y = 0;
            stage1Flag = false;
            isEndOfStunt = true;
        }
    }
    void OnEnable()
    {
        originaCamlPos = camTransform.localPosition;
    }
    public void SetUp()
    {
        ragdollSpawn.SetActive(true);
        foreach (var item in gem)
            item.SetActive(false);
        labels.gameObject.SetActive(false);
        triangle.SetActive(false);
        bossHead.transform.position = bossStartPos;
        stage = qc.stage;
        isThrown = false;
        stoneV = 0;
        elapsed = 0;
        correctAnswer = 0;
        sY = 0;
        myPlayer.happy = false;
        myPlayer.lost = false;
        myPlayer.standup = false;
        switch (stage)
        {
            case 1:
                gem[0].SetActive(true);
                y = -6;
                x = 0;
                qc.limit = 20.5f;
                bossDistance = 6;
                while (true)
                {
                    distance = (float)System.Math.Round(Random.Range(16.5f, 20.5f), 2);
                    bossV = (float)System.Math.Round(Random.Range(4f, 5f), 2);
                    stuntTime = (bossDistance / bossV);
                    stoneV = distance / (stuntTime - 1);
                    if ((stoneV < 40f))
                        break;
                }
                qc.SetUnitTo(UnitOf.distance);
                indicators.showLines(null, null, bossDistance, 0, 0);
                indicators.UnknownIs('n');
                indicators.heightSpawnPnt = new Vector2(1, 3);
                indicators.ShowVelocityLabel(false);
                indicators.ResizeEndLines(null, 6.5f, 0.25f, 0.25f, null, null);

                myPlayer.transform.position = initialPlayerPos;

                correctAnswer = distance;
                angle = (float)System.Math.Round(((System.Math.Atan2(x, y) * 180) / System.Math.PI), 2);

                question = playerName + " is istructed to throw the rock inside the mouth of the Giant Worm who's guarding the exit of the cave. If the worm's head is moving 6m straight downward with the velocity of "
                     + bossV.ToString("f2") + qc.Unit(UnitOf.velocity) + ". How far from the worm should " + pronoun + " throw the stone to exactly hit the worm's mouth? After 1 second the stone is released with the velocity of "
                     + stoneV.ToString("f2") + qc.Unit(UnitOf.velocity) + ".";
                break;
            case 2:
                qc.SetUnitTo(UnitOf.time);
                gem[1].SetActive(true);
                labels.gameObject.SetActive(true);
                float allowanceTime;
                y = -6;
                qc.limit = 5;
                while (true)
                {
                    bossV = (float)System.Math.Round(Random.Range(4f, 5f), 2);
                    x = Random.Range(-10f, 10);
                    bossDistance = Mathf.Sqrt((x * x) + (y * y));
                    stuntTime = bossDistance / bossV;
                    angle = Mathf.Atan(x / y) * Mathf.Rad2Deg;
                    allowanceTime = Random.Range(0.75f, 1.5f);
                    if ((stuntTime < qc.limit) && (Mathf.Abs(angle) > 45) && (stuntTime > (allowanceTime + 2)))
                        break;
                }
                distance = Random.Range(18f, 23f);
                correctAnswer = (float)System.Math.Round(stuntTime - allowanceTime - 1, 2);
                stoneV = (distance + x) / (allowanceTime);

                myPlayer.transform.position = new Vector2(-distance + 0.5f, myPlayer.transform.position.y);
                indicators.distanceSpawnPnt = new Vector2(-distance + 1, 2.5f);
                indicators.timeSpawnPnt = new Vector2(myPlayer.transform.position.x + 0.5f, 1);
                indicators.showLines(distance, null, null, stoneV, stuntTime);
                indicators.UnknownIs('t');
                indicators.ResizeEndLines(0.5f, 0.25f, null, null, 0.25f, 1f);

                labels.startingAngle = 180;
                labels.SetSpawnPnt(bossHead.transform.position);
                labels.angleA = angle;
                labels.legB = bossDistance;
                labels.HideValuesOf(false, true, true, false, true, true);

                question = "Now, the worm is moving at " + Mathf.Abs(angle).ToString("f2") + qc.Unit(UnitOf.angle) + " rightmost downward with the velocity of "
                    + bossV.ToString("f2") + qc.Unit(UnitOf.velocity) + ". at after how many seconds should " + playerName +
                    " throw the stone to hit exactly inside the mouth of the worm, if " + pronoun + " is standing " + distance.ToString("f2") + qc.Unit(UnitOf.distance) +
                    " horizontally away and 6m vertically below from the worm? The stone is released after 1 second with the velocity of " + stoneV.ToString("f2") + qc.Unit(UnitOf.velocity) + ".";
                break;
            case 3:
                qc.SetUnitTo(UnitOf.velocity);
                ragdollSpawn.SetActive(false);
                gem[2].SetActive(true);
                labels.gameObject.SetActive(true);
                triangle.SetActive(true);
                bossHead.transform.position = new Vector2(bossStartPos.x, bossStartPos.y - 6);
                qc.limit = 40;
                while (true)
                {
                    bossV = (float)System.Math.Round(Random.Range(4f, 5f), 2);
                    x = Random.Range(-10f, 10f);
                    y = Random.Range(4f, 6f);
                    xS = Random.Range(16f, 17f);
                    dT = xS + x;
                    distance = Mathf.Sqrt((dT * dT) + (y * y));
                    bossDistance = Mathf.Sqrt((x * x) + (y * y));
                    stuntTime = bossDistance / bossV;
                    stoneV = distance / (stuntTime - 1);
                    angle = Mathf.Atan(x / y) * Mathf.Rad2Deg;
                    if ((stoneV < qc.limit) && (Mathf.Abs(x) > 5))
                        break;
                }
                myPlayer.transform.position = new Vector2(-xS + 0.5f, myPlayer.transform.position.y);
                indicators.distanceSpawnPnt = new Vector2(-xS + 1, 2.5f);

                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(xS, null, null, stoneV, stuntTime);
                indicators.UnknownIs('v');
                indicators.ResizeEndLines(0.5f, 0.25f, null, null, 0.25f, 1f);

                correctAnswer = (float)System.Math.Round(stoneV, 2);

                labels.startingAngle = 180;
                labels.SetSpawnPnt(new Vector2(bossHead.transform.position.x + x, bossHead.transform.position.y + y));
                labels.legA = y;
                labels.angleA = angle;
                labels.legB = bossDistance;
                labels.HideValuesOf(true, false, true, false, true, true);

                triangleAnnotaion.startingAngle = 180;
                triangleAnnotaion.SetSpawnPnt(new Vector2(bossHead.transform.position.x + x, bossHead.transform.position.y + y));
                triangleAnnotaion.legA = y;
                triangleAnnotaion.legB = Mathf.Sqrt((dT * dT) + (y * y));
                triangleAnnotaion.angleA = Mathf.Atan(dT / y) * Mathf.Rad2Deg;
                triangleAnnotaion.HideValuesOf(true, true, true, true, true, true);

                string direction;
                if (x / Mathf.Abs(x) == 1)
                    direction = " away ";
                else
                    direction = " towards ";
                question = "Finally, the worm is moving " + y.ToString("f2") + qc.Unit(UnitOf.distance) + " upward at "
                    + Mathf.Abs(angle).ToString("f2") + qc.Unit(UnitOf.angle) + direction + "from " + playerName + " with the velocity of "
                    + bossV.ToString("f2") + qc.Unit(UnitOf.velocity) +
                    ". At what velocity should " + pronoun +
                    " throw the stone to hit exactly inside the mouth of the worm? The stone is released after 1 second.";
                break;
        }
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
                    throwTime = (playerAnswer / stoneV) + 0.1f;
                else if (playerAnswer < correctAnswer)
                    throwTime = (playerAnswer / stoneV) - 0.1f;
                else
                    throwTime = correctAnswer / stoneV;
                break;
            case 2:
                isStartOfStunt = true;
                directorIsCalling = true;

                if (playerAnswer > correctAnswer)
                    throwTime = (stuntTime - 1 - playerAnswer) + 0.1f;
                else if (playerAnswer < correctAnswer)
                    throwTime = (stuntTime - 1 - playerAnswer) - 0.1f;
                else
                    throwTime = (stuntTime - 1 - correctAnswer);
                break;
            case 3:
                indicators.timeSpawnPnt = new Vector2(myPlayer.transform.position.x + 0.5f, myPlayer.transform.position.y - 1);
                // indicators.SetPlayerPosition(myPlayer.transform.position.x - );
                indicators.showLines(null, distanceTraveled, null, stoneV, stuntTime);
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
        yield return new WaitForSeconds(0.5f);
        bossRB.constraints = RigidbodyConstraints2D.FreezeAll;
        SetUp();
    }
    IEnumerator Retry()
    {
        qc.retried = false;
        yield return new WaitForSeconds(1);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        if (!ragdoll)
            stoneScript.destroyer = true;
        ragdoll = false;
        reset = true;
    }
    IEnumerator Next()
    {
        qc.nextStage = false;
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
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Camera!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Action!";
            yield return new WaitForSeconds(0.75f);
            if (stage == 2)
                isThrown = false;
            else isThrown = true;
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
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";
        }
    }
    IEnumerator BossCrumble()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var item in bossParts)
            item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        foreach (var item in joints)
            item.enabled = false;

        boss.SetVelocityOfTheHead(0, 0, 0);
        bossRB.constraints = RigidbodyConstraints2D.None;
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
        else if (stage == 1)
            stoneScript.SetVelocity(throwTime, distance, sY);
        else
            stoneScript.SetVelocity(throwTime, dT, y);
        indicators.ShowVelocityLabel(true);
        stoneIsPresent = true;
    }
}
