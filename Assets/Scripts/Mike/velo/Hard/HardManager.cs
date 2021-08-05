using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameConfig;

public class HardManager : MonoBehaviour
{
    SlantingIndicator labels;
    BossScript boss;
    Player myPlayer;
    QuestionControllerVThree qc;
    public GameObject directorsBubble, bossHead, stonePrefab;
    public TMP_Text directorsSpeech;
    float x, y, bossV, playerAnswer, stuntTime, elapsed, distance, stoneV, correctAnswer, angle;
    bool isAnswered, isEndOfStunt, isStartOfStunt, directorIsCalling, isAnswerCorrect, isThrown;
    string messageTxt, question, playerName, playerGender, pronoun, pPronoun;
    public int stage;
    public Rigidbody2D bossRB, stone;
    Rigidbody2D thrownStone;
    public Transform stoneObject;
    // Start is called before the first frame update
    void Start()
    {
        labels = FindObjectOfType<SlantingIndicator>();
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
        SetUp();
    }
    // Update is called once per frame
    void Update()
    {
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        switch (stage)
        {
            case 1:
                if (isAnswered)
                {
                    playerAnswer = qc.GetPlayerAnswer();
                    qc.timer = elapsed.ToString("f2") + "s";
                    elapsed += Time.deltaTime;
                    bossRB.constraints = RigidbodyConstraints2D.None;
                    bossRB.constraints = RigidbodyConstraints2D.FreezeRotation;
                    if (isThrown)
                    {
                        StartCoroutine(Throw());
                    }

                    if ((boss.transform.position.y) <= (0))//stuntTime)
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
                }
                break;
            case 2:
                break;
            case 3:
                break;
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
    void SetUp()
    {
        isThrown = true;
        stoneV = 0;
        elapsed = 0;
        switch (stage)
        {
            case 1:
                qc.limit = 10;
                while (true)
                {
                    bossV = Random.Range(2f, 4f);
                    x = Random.Range(-8f, -3f);
                    y = -3;
                    distance = Mathf.Sqrt((x * x) + (y * y));
                    stuntTime = distance / bossV;//boss.SetVelocityOfTheHead(x, y, -bossV);
                    stoneV = (20.5f + x) / stuntTime;
                    Debug.Log(stoneV);
                    if (stoneV < qc.limit)
                        break;
                }
                correctAnswer = (float)System.Math.Round(stoneV, 2);
                boss.SetVelocityOfTheHead(stuntTime, x, y);
                angle = (float)((System.Math.Atan2(x, y) * 180) / System.Math.PI);
                labels.SetDistance(distance, angle, x, y);
                labels.SetSpawnPnt(bossHead.transform.position);
                question = "The target is the gem inside the mouth of the golem. If the golem is moving at " + -angle + qc.Unit(UnitOf.angle) + " with the velocity of " + bossV + ". at what velocity should " +playerName+" throw the stone ti hit exactly at the gem?";
                break;
            case 2:
                break;
            case 3:
                break;
        }
        qc.question = question;
        Debug.Log(x + "," + y);
        Debug.Log(distance + ", " + angle);

    }
    void Play()
    {
        qc.isSimulating = false;
        isStartOfStunt = true;
        directorIsCalling = true;
    }
    IEnumerator Retry()
    {
        qc.retried = false;
        yield return new WaitForSeconds(3);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        SetUp();
    }
    IEnumerator Next()
    {
        qc.nextStage = false;
        myPlayer.happy = false;
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(2.8f);
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
        yield return new WaitForSeconds(1f);
        myPlayer.thrown = false;
        GameObject stone = Instantiate(stonePrefab);
        stone.transform.position = new Vector2(-19.5f, 2);
        stone.GetComponent<Rigidbody2D>().gravityScale = 0;
        stone.GetComponent<Rigidbody2D>().velocity = new Vector2(playerAnswer, 0);
    }
}
