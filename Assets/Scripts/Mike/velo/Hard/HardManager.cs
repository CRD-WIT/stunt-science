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
    Player myPlayer;
    QuestionControllerVThree qc;
    public GameObject directorsBubble, bossHead, stonePrefab, bossAtk, triangle;
    public GameObject[] gem, bossParts;
    public HingeJoint2D[] joints;
    public TMP_Text directorsSpeech;
    float x, y, bossV, playerAnswer, stuntTime, elapsed, bossDistance, stoneV, correctAnswer, angle, distance, throwTime, stonePosX, initialDistance,
        initialPlayerPos, sX, sY, sDist, dT, xS;
    bool isAnswered, isEndOfStunt, isStartOfStunt, directorIsCalling, isAnswerCorrect, isThrown, stage1Flag, stoneIsPresent, reset;
    string messageTxt, question, playerName, playerGender, pronoun, pPronoun;
    public int stage;
    Vector2 bossStartPos, currentBossPos;
    public Rigidbody2D bossRB;
    GameObject stone;
    Rigidbody2D thrownStone;
    Rock stoneScript;
    public Transform stoneObject;
    // Start is called before the first frame update
    void Start()
    {
        indicators = FindObjectOfType<IndicatorManagerV1_1>();
        labels = FindObjectOfType<AngleAnnotaion>();
        triangleAnnotaion = triangle.GetComponent<AngleAnnotaion1>();
        boss = FindObjectOfType<BossScript>();
        myPlayer = FindObjectOfType<Player>();
        qc = FindObjectOfType<QuestionControllerVThree>();
        bossRB = bossHead.GetComponent<Rigidbody2D>();
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
        initialPlayerPos = myPlayer.transform.position.x;
        bossStartPos = bossHead.transform.position;
        qc.stage = stage;
        SetUp();
    }
    // Update is called once per frame
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
                // indicators.SetPlayerPosition(stone.transform.position);
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
            qc.timer = elapsed.ToString("f2") + "s";
            elapsed += Time.deltaTime;
            bossRB.constraints = RigidbodyConstraints2D.None;
            // bossRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            // indicators.SetPlayerPosition(new Vector2((-distance + 1) + (stoneV * (elapsed - 1)), 5));
            switch (stage)
            {
                case 1:
                    // stonePosX = playerAnswer + stone.transform.position.x -1;
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
                        Debug.Log("correct");
                    }
                    else
                    {
                        isAnswerCorrect = false;
                        Debug.Log("wrong");
                    }
                    // indicators.IsRunning(playerAnswer, stonePosX, elapsed, null);
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
                            Debug.Log("correct");
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            Debug.Log("wrong");
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
                            Debug.Log("correct");
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            Debug.Log("wrong");
                        }
                    }
                    break;
            }
            currentBossPos = bossHead.transform.position;
        }
        if (stoneIsPresent)
        {
            if (stoneScript.hit != null)
            {
                isAnswered = false;
                elapsed = stuntTime;
                bossRB.constraints = RigidbodyConstraints2D.FreezeAll;
                bool hit = (bool)stoneScript.hit;
                isEndOfStunt = true;
                stoneIsPresent = false;
                if (hit)
                {
                    switch (stage)
                    {
                        case 1:
                            gem[0].SetActive(false);
                            gem[1].SetActive(true);
                            break;
                        case 2:
                            gem[1].SetActive(false);
                            gem[2].SetActive(true);
                            break;
                        case 3:
                            gem[2].SetActive(false);
                            foreach (var item in bossParts)
                            {
                                item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            }
                            foreach(var item in joints)
                                item.enabled =false;
                            // bossRB.constraints = RigidbodyConstraints2D.None;
                            break;
                    }
                }
                else
                {
                    Debug.Log("Wrong");
                }
            }
        }
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
    public void SetUp()
    {
        foreach (var item in gem)
            item.SetActive(false);
        labels.gameObject.SetActive(false);
        triangle.SetActive(false);
        if (stoneIsPresent)
            StartCoroutine(DestroyRock());
        bossHead.transform.position = bossStartPos;
        stage = qc.stage;
        isThrown = false;
        stoneV = 0;
        elapsed = 0;
        correctAnswer = 0;
        sY = 0;
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
                    if ((stoneV < 40f))// && (distance > 16.5f))
                        break;
                }
                indicators.showLines(null, null, bossDistance, 0, 0);
                indicators.UnknownIs('n');
                indicators.heightSpawnPnt = new Vector2(1, 3);
                indicators.ShowVelocityLabel(false);
                indicators.ResizeEndLines(null, 6.5f, 0.25f, 0.25f, null, null);

                correctAnswer = distance;
                boss.SetVelocityOfTheHead(stuntTime, x, y);
                angle = (float)System.Math.Round(((System.Math.Atan2(x, y) * 180) / System.Math.PI), 2);

                question = "The target is the gem inside the mouth of the golem. If the golem is moving straight downward with the velocity of "
                     + bossV.ToString("f2") + qc.Unit(UnitOf.velocity) + ". How far from the boss should " + playerName + " throw the stone to hit exactly at the gem? After 1 second the stone is released with the speed of "
                     + stoneV.ToString("f2") + qc.Unit(UnitOf.velocity) + ".";
                break;
            case 2:
                gem[1].SetActive(true);
                labels.gameObject.SetActive(true);
                float allowanceTime;
                y = -6;
                qc.limit = 7;
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
                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(distance, distance + x, null, stoneV, stuntTime);
                indicators.UnknownIs('t');
                indicators.ResizeEndLines(0.5f, 0.25f, null, null, 0.25f, 1f);

                boss.SetVelocityOfTheHead(stuntTime, x, y);
                labels.startingAngle = 180;
                labels.SetSpawnPnt(bossHead.transform.position);
                labels.angleA = angle;
                labels.legB = bossDistance;
                labels.HideValuesOf(false, true, true, false, true, true);

                question = "The target is the gem inside the mouth of the golem. If the golem is moving at "
                    + angle.ToString("f2") + qc.Unit(UnitOf.angle) + " with the velocity of " + bossV.ToString("f2") + " " + qc.Unit(UnitOf.velocity) +
                    ". at what velocity should " + playerName + " throw the stone to hit exactly at the gem?";
                break;
            case 3:
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
                boss.SetVelocityOfTheHead(stuntTime, x, y);

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

                question = "The target is the gem inside the mouth of the golem. If the golem is moving at "
                    + (-angle).ToString("f2") + qc.Unit(UnitOf.angle) + " with the velocity of " + bossV.ToString("f2") + qc.Unit(UnitOf.velocity) +
                    ". at what velocity should " + playerName + " throw the stone to hit exactly at the gem?";
                break;
        }
        sX = x;
        qc.question = question;
        Debug.Log(correctAnswer + " ans");
        Debug.Log(stuntTime + "st");
        Debug.Log(x + "," + y + "," + stoneV);
        Debug.Log(bossDistance + ", " + angle);
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
    IEnumerator DestroyRock()
    {
        yield return new WaitForEndOfFrame();
        Destroy(stonePrefab);
        stoneIsPresent = false;
    }
    IEnumerator Retry()
    {
        qc.retried = false;
        yield return new WaitForSeconds(1);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        stoneScript.destroyer = true;
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
