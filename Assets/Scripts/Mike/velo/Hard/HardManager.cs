using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameConfig;

public class HardManager : MonoBehaviour
{
    AngleAnnotaion labels;
    IndicatorManagerV1_1 indicators;
    BossScript boss;
    Player myPlayer;
    QuestionControllerVThree qc;
    public GameObject directorsBubble, bossHead, stonePrefab, bossAtk;
    public TMP_Text directorsSpeech;
    float x, y, bossV, playerAnswer, stuntTime, elapsed, bossDistance, stoneV, correctAnswer, angle, distance, throwTime, stonePosX, initialDistance;
    bool isAnswered, isEndOfStunt, isStartOfStunt, directorIsCalling, isAnswerCorrect, isThrown, stage1Flag;
    string messageTxt, question, playerName, playerGender, pronoun, pPronoun;
    public int stage;
    Vector2 bossStartPos;
    public Rigidbody2D bossRB;
    GameObject stone;
    Rigidbody2D thrownStone;
    public Transform stoneObject;
    // Start is called before the first frame update
    void Start()
    {
        indicators = FindObjectOfType<IndicatorManagerV1_1>();
        labels = FindObjectOfType<AngleAnnotaion>();
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
        bossStartPos = bossHead.transform.position;
        qc.stage = stage;
        SetUp();
    }
    // Update is called once per frame
    void Update()
    {
        if (stage1Flag)
        {
            if (myPlayer.transform.position.x < (-playerAnswer + 0.5f))
            {
                initialDistance = 1 - playerAnswer;
                myPlayer.moveSpeed = 1.99f;
                indicators.gameObject.SetActive(true);
                indicators.distanceSpawnPnt = new Vector2(initialDistance, 5);
                indicators.timeSpawnPnt = new Vector2(myPlayer.transform.position.x + 0.5f, 3);
                // indicators.SetPlayerPosition(stone.transform.position);
                indicators.showLines(playerAnswer, null, 3, stoneV, stuntTime);
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
        if (isAnswered)
        {
            qc.timer = elapsed.ToString("f2") + "s";
            elapsed += Time.deltaTime;
            bossRB.constraints = RigidbodyConstraints2D.None;
            bossRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (isThrown)
            {
                StartCoroutine(Throw());
            }
            // indicators.SetPlayerPosition(new Vector2((-distance + 1) + (stoneV * (elapsed - 1)), 5));
            stonePosX = playerAnswer + stone.transform.position.x;

            switch (stage)
            {
                case 1:
                    if ((boss.transform.position.y) <= (4))//stuntTime)
                    {
                        stonePosX = playerAnswer;
                        isAnswered = false;
                        elapsed = stuntTime;
                        bossRB.constraints = RigidbodyConstraints2D.FreezeAll;
                        isEndOfStunt = true;
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
                    indicators.IsRunning(playerAnswer, stonePosX, elapsed, null);
                    break;
                case 2:
                    if ((boss.transform.position.y) <= (4))//stuntTime)
                    {
                        isAnswered = false;
                        elapsed = stuntTime;
                        bossRB.constraints = RigidbodyConstraints2D.FreezeAll;
                        if (playerAnswer == correctAnswer)
                        {
                            Debug.Log("correct");
                        }
                        else
                        {
                            Debug.Log("wrong");
                        }
                    }
                    break;
                case 3:
                    if ((boss.transform.position.y) <= (0))//stuntTime)
                    {
                        isAnswered = false;
                        elapsed = stuntTime;
                        bossRB.constraints = RigidbodyConstraints2D.FreezeAll;
                        if (playerAnswer == correctAnswer)
                        {
                            Debug.Log("correct");
                            if (stage < 3)
                                stage++;
                        }
                        else
                        {
                            Debug.Log("wrong");
                        }
                    }
                    break;
            }
        }
        // stuntTime = boss.t;
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
        stage = qc.stage;
        isThrown = true;
        stoneV = 0;
        elapsed = 0;
        correctAnswer = 0;
        switch (stage)
        {
            case 1:
                y = -3;
                x = 0;
                qc.limit = 20.5f;
                bossDistance = 3;
                bossHead.transform.position = bossStartPos;
                // distance = (float)System.Math.Round(Random.Range(14.5f, 20.5f), 2);
                while (true)
                {
                    stoneV = (float)System.Math.Round(Random.Range(18f, 20f), 2);
                    bossV = (float)System.Math.Round(Random.Range(1.5f, 2f), 2);
                    stuntTime = (bossDistance / bossV);
                    // stoneV = 20.5f / (stuntTime - 1);
                    distance = stoneV * (stuntTime - 1);
                    if ((distance < qc.limit) && (distance > 16.5f))
                        break;
                }
                indicators.showLines(null, null, bossDistance, 0, 0);
                indicators.UnknownIs('n');
                indicators.heightSpawnPnt = new Vector2(1, 5);
                indicators.ShowVelocityLabel(false);
                indicators.ResizeEndLines(null, 6.5f, 0.25f, 0.25f);

                labels.gameObject.SetActive(false);

                correctAnswer = (float)System.Math.Round(distance, 2);
                Debug.Log(correctAnswer + " ans");
                boss.SetVelocityOfTheHead(stuntTime, x, y);
                angle = (float)System.Math.Round(((System.Math.Atan2(x, y) * 180) / System.Math.PI), 2);
                question = "The target is the gem inside the mouth of the golem. If the golem is moving at straight downward with the velocity of " + bossV + qc.Unit(UnitOf.velocity) + ". at what velocity should " + playerName + " throw the stone to hit exactly at the gem?";
                break;
            case 2:
                bossHead.transform.position = bossStartPos;
                y = -3;
                qc.limit = 5;
                while (true)
                {
                    bossV = (float)System.Math.Round(Random.Range(1f, 2.5f), 2);
                    x = Random.Range(-5f, 5f);
                    bossDistance = Mathf.Sqrt((x * x) + (y * y));
                    stuntTime = bossDistance / bossV;//boss.SetVelocityOfTheHead(x, y, -bossV);
                    // indicators.timeSpawnPnt = myPlayer.transform.position;
                    if (stuntTime < qc.limit)
                        break;
                }
                Debug.Log(stuntTime + " ans");
                stoneV = (20.5f + x) / (stuntTime - 1);
                distance = 20.5f + x;
                Debug.Log("dist " + distance);
                indicators.distanceSpawnPnt = new Vector2(-19.5f, 2);
                indicators.SetPlayerPosition(myPlayer.transform.position);
                indicators.showLines(20.5F + x, null, null, playerAnswer, stuntTime);
                indicators.UnknownIs('v');

                correctAnswer = (float)System.Math.Round(stoneV, 2);
                boss.SetVelocityOfTheHead(stuntTime, x, y);
                angle = Mathf.Atan(x / y) * Mathf.Rad2Deg;//(float)System.Math.Round(((System.Math.Atan2(x, y) * 180) / System.Math.PI), 2);
                labels.arc = angle;
                labels.legB = bossDistance;
                question = "The target is the gem inside the mouth of the golem. If the golem is moving at " + angle + qc.Unit(UnitOf.angle) + " with the velocity of " + bossV + qc.Unit(UnitOf.velocity) + ". at what velocity should " + playerName + " throw the stone to hit exactly at the gem?";
                break;
            case 3:
                float sX, sY = -2, sDist;
                y = -5;
                while (correctAnswer < qc.limit)
                {
                    x = Random.Range(-8f, 10f);
                    bossDistance = (float)System.Math.Round(Mathf.Sqrt((x * x) + (y * y)));
                    stuntTime = bossDistance / bossV;//boss.SetVelocityOfTheHead(x, y, -bossV);
                    stoneV = (distance + x) / stuntTime;
                    Debug.Log(stoneV);
                }

                sX = 20.5f + x;
                sDist = Mathf.Sqrt((sX * sX) + (sY * sY));
                correctAnswer = (float)System.Math.Round(stoneV, 2);
                boss.SetVelocityOfTheHead(stuntTime, x, y);
                angle = (float)System.Math.Round(((System.Math.Atan2(x, y) * 180) / System.Math.PI), 2);
                labels.arc = angle;
                labels.legB = bossDistance;
                question = "The target is the gem inside the mouth of the golem. If the golem is moving at " + angle + qc.Unit(UnitOf.angle) + " with the velocity of " + bossV + qc.Unit(UnitOf.velocity) + ". at what velocity should " + playerName + " throw the stone to hit exactly at the gem?";
                break;
        }
        qc.question = question;
        Debug.Log(stuntTime + "st");
        Debug.Log(x + "," + y);
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
                    throwTime = (playerAnswer / stoneV) - 0.5f;
                else if (playerAnswer < correctAnswer)
                    throwTime = (playerAnswer / stoneV) + 0.5f;
                else
                    throwTime = playerAnswer / stoneV;
                break;
            case 2:
                isStartOfStunt = true;
                directorIsCalling = true;
                break;
            case 3:
                break;
        }
    }
    IEnumerator Retry()
    {
        qc.retried = false;
        yield return new WaitForSeconds(1);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        SetUp();
    }
    IEnumerator Next()
    {
        qc.nextStage = false;
        myPlayer.happy = false;
        yield return new WaitForSeconds(1f);
        // yield return new WaitForSeconds(2.8f);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
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
            isThrown = true;
            directorsSpeech.text = "";
            directorsBubble.SetActive(false);
            isAnswered = true;
        }
        else
        {
            // yield return new WaitForSeconds((35 / playerVelocity) - stuntTime);
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
        stone.transform.position = new Vector2(-distance + 1, 5);
        // stone.GetComponent<Rigidbody2D>().gravityScale = 0;
        stone.GetComponent<Rock>().SetVelocity(throwTime, distance, 0);
        indicators.ShowVelocityLabel(true);
        // stone.GetComponent<Rigidbody2D>().velocity = new Vector2(playerAnswer, 0);
    }
    IEnumerator BossAttack()
    {
        boss.SetVelocityOfTheHead(0.5f, -x, 2);
        yield return new WaitForEndOfFrame();
        bossRB.constraints = RigidbodyConstraints2D.None;
        bossRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.5f);
        bossRB.constraints = RigidbodyConstraints2D.FreezeAll;
        GameObject BAtk = Instantiate(bossAtk);
        BAtk.transform.position = bossHead.transform.position;
        BAtk.GetComponent<Rigidbody2D>().gravityScale = 0;
        BAtk.GetComponent<Rigidbody2D>().velocity = new Vector2(15, 0);
    }
    public void GetStonePos()
    {
        stonePosX = playerAnswer;
    }
}
