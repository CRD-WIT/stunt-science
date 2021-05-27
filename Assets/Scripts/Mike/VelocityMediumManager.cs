using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GameConfig;

public class VelocityMediumManager : MonoBehaviour
{
    QuestionControllerVThree qc;
    IndicatorManager labels;
    StageManager sm = new StageManager();
    [SerializeField] GameObject boulder, directorsBubble, floor, ragdollPrefab, boulderA;
    [SerializeField] LineRenderer EndOfAnnotation, correctAnswerIndicator, playerAnswerIndicator;
    Annotation distanceMeassure;
    Player myPlayer;
    CeillingGenerator createCeilling;
    [SerializeField] TMP_Text directorsSpeech;
    [SerializeField] float playerVelocity, boulderVelocity, boulder2Velocity, stuntTime, distance, jumpDistance, correctAnswer;
    int stage;
    Rigidbody2D boulderRB, boulder2RB;
    GameObject ragdoll;
    float playerPos, playerAnswer, elapsed, distanceTraveled, currentPlayerPos, jumpTime, jumpForce;
    string question, playerName, playerGender, pronoun, pPronoun, messageTxt;
    bool isStartOfStunt, directorIsCalling, isAnswered, isAnswerCorrect, isEndOfStunt;
    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        labels = FindObjectOfType<IndicatorManager>();
        myPlayer = FindObjectOfType<Player>();
        distanceMeassure = FindObjectOfType<Annotation>();
        createCeilling = FindObjectOfType<CeillingGenerator>();
        boulderRB = boulder.GetComponent<Rigidbody2D>();
        boulder2RB = boulderA.GetComponent<Rigidbody2D>();

        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        sm.SetGameLevel(1);
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
        qc.levelName = sm.GetGameLevel();
        qc.levelDifficulty = Difficulty.Medium;
        VeloMediumSetUp();
    }
    // Update is called once per frame
    void Update()
    {
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        if (isAnswered)
        {
            playerAnswer = qc.GetPlayerAnswer();
            currentPlayerPos = myPlayer.transform.position.x;
            qc.timer = elapsed.ToString("f2") + "s";
            elapsed += Time.deltaTime;
            myPlayer.moveSpeed = playerVelocity;
            switch (stage)
            {
                case 1:
                    boulderRB.velocity = new Vector2(-boulderVelocity, 0);
                    if (playerAnswer <= elapsed)
                    {
                        elapsed = playerAnswer;
                        StartCoroutine(Jump());
                        if (playerAnswer == stuntTime)
                        {
                            isAnswerCorrect = true;
                            myPlayer.jumpforce = jumpForce;
                            messageTxt = playerName + " has jumped over the boulder <color=green>safely</color>!";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            myPlayer.jumpforce = jumpForce - 0.13f;
                            if (playerAnswer < stuntTime)
                            {
                                messageTxt = playerName + " jumped too soon and hit the boulder.\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                            }
                            else
                                messageTxt = playerName + " jumped too late and hit the boulder.\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                        }
                    }
                    break;
                case 2:
                    boulderRB.velocity = new Vector2(+boulderVelocity, 0);
                    if (stuntTime <= elapsed)
                    {
                        currentPlayerPos = myPlayer.transform.position.x;
                        if (playerAnswer == correctAnswer)
                        {
                            isAnswerCorrect = true;
                            elapsed = stuntTime;
                            myPlayer.jumpforce = jumpForce;
                            messageTxt = playerName + " has jumped over the boulder <color=green>safely</color>!";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            elapsed = playerAnswer;
                            myPlayer.jumpforce = jumpForce - 0.13f;
                            if (playerAnswer < stuntTime)
                            {
                                messageTxt = playerName + " jumped too far from the boulder, and hits it!\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                            }

                            else
                                messageTxt = playerName + " jumped too n from the boulder, and hits it!\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                        }
                        StartCoroutine(Jump());
                    }
                    break;
                case 3:
                    myPlayer.moveSpeed = playerAnswer;
                    boulderRB.velocity = new Vector2(boulderVelocity, 0);
                    boulder2RB.velocity = new Vector2(-boulder2Velocity, 0);
                    if (stuntTime <= elapsed)
                    {
                        myPlayer.jumpforce = jumpForce;
                        elapsed = stuntTime;
                        myPlayer.moveSpeed = 0;
                        boulderRB.velocity = new Vector2(boulderRB.velocity.x, boulderRB.velocity.y);
                        boulder2RB.velocity = new Vector2(boulder2RB.velocity.x, boulder2RB.velocity.y);
                        currentPlayerPos = myPlayer.transform.position.x;
                        if (playerAnswer == correctAnswer)
                        {
                            isAnswerCorrect = true;
                            elapsed = stuntTime;
                            myPlayer.jumpforce = jumpForce;
                            messageTxt = playerName + " has jumped over the boulder <color=green>safely</color>!";
                        }
                        else
                        {
                            isAnswerCorrect = false;
                            elapsed = playerAnswer;
                            myPlayer.jumpforce = jumpForce - 0.13f;
                            if (playerAnswer < stuntTime)
                            {
                                messageTxt = playerName + " jumped too far from the boulder, and hits it!\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                            }

                            else
                                messageTxt = playerName + " jumped too n from the boulder, and hits it!\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                        }
                        StartCoroutine(Jump());
                    }
                    break;
            }
        }
        if (qc.isSimulating)
            Play();
        if (qc.nextStage)
            Next();
    }
    void VeloMediumSetUp()
    {
        stage = qc.stage;
        myPlayer.isLanded = false;
        qc.isSimulating = false;
        float Va = 0, Vb = 0, d = 0, Da = 0, Db = 0, Dj = 0, t = 0;//, tm = 0;
        playerAnswer = 0;
        elapsed = 0;
        //Destroy(ragdollPrefab);
        qc.timer = "0.00s";
        RumblingManager.shakeON = true;
        distanceMeassure.gameObject.SetActive(true);

        boulder.SetActive(false);
        boulderA.SetActive(false);

        boulderVelocity = 0;
        myPlayer.standup = false;
        boulderRB.rotation = 0;
        boulderRB.freezeRotation = true;

        switch (stage)
        {
            case 1:
                boulder.SetActive(true);
                Va = Random.Range(8f, 9f);
                Vb = Random.Range(4f, 6f);
                d = Random.Range(27f, 30f);

                playerVelocity = (float)System.Math.Round(Va, 2);
                boulderVelocity = (float)System.Math.Round(Vb, 2);
                distance = (float)System.Math.Round(d, 2);
                Dj = ((Va + Vb) / 2) + 0.53f;
                t = (d - Dj) / (Va + Vb);
                stuntTime = (float)System.Math.Round(t, 2);
                Da = playerVelocity * stuntTime;
                Db = boulderVelocity * stuntTime;
                correctAnswer = stuntTime;

                myPlayer.transform.position = new Vector2(playerPos, 0);

                boulder.transform.position = new Vector2(distance, boulder.transform.position.y);
                boulderRB.freezeRotation = false;

                jumpDistance = (float)System.Math.Round(Dj, 2);
                jumpTime = Dj / Va;

                question = playerName + " is instructed to run until the end of the scene while jumping over the rolling boulder. If " + pronoun + " is running at a velocity of <color=purple>" + playerVelocity + " meters per second</color> while an incoming boulder at the front <color=red>" + distance + " meters</color> away is rolling at the velocity of <color=purple>" + boulderVelocity + "meters per second</color>, at after how many <color=#006400>seconds</color> will " + playerName + " jump if " + pronoun + " has to jump at exactly <color=red>" + jumpDistance + " meters</color> away from the boulder in order to jump over it safely?";
                break;
            case 2:
                boulder.SetActive(true);
                Va = Random.Range(8f, 9f);
                Vb = Random.Range(20f, 22f);
                d = Random.Range(14f, 16f);

                playerVelocity = (float)System.Math.Round(Va, 2);
                boulderVelocity = (float)System.Math.Round(Vb, 2);
                distance = (float)System.Math.Round(d, 2);
                Dj = (Vb - Va) / 2;
                t = (distance - Dj) / (boulderVelocity - playerVelocity);
                stuntTime = (float)System.Math.Round(t, 2);
                Da = playerVelocity * stuntTime;
                Db = boulderVelocity * stuntTime;

                correctAnswer = (float)System.Math.Round(Da, 2);

                boulder.transform.position = new Vector2(playerPos, 0);

                myPlayer.transform.position = new Vector2(distance, boulder.transform.position.y);
                boulderRB.rotation = 180;
                boulderRB.freezeRotation = false;

                jumpDistance = (float)System.Math.Round(Dj, 2);
                jumpTime = Dj / Va;

                question = playerName + " is instructed to run until the end of the scene while jumping over the rolling boulder. If " + pronoun + " is running at a velocity of <color=purple>" + playerVelocity + " meters per second</color> while an incoming fast moving boulder <color=red>" + distance + " meters</color> away is catchind up from behind with a velocity of <color=purple>" + boulderVelocity + "meters per second</color>, at after how many <color=red>meters</color> should " + playerName + " be jumping if " + pronoun + " has to jump at exactly <color=red>" + jumpDistance + " meters</color> away from the boulder in order to jump over it safely?";
                break;
            case 3:
                boulder.SetActive(true);
                boulderA.SetActive(true);
                float Vp, Dp, Tp, Dac = (float)System.Math.Round(Random.Range(19f, 22f), 2);
                Va = Random.Range(7f, 8f);
                Vb = Random.Range(3f, 4f);
                d = Random.Range(29f, 30f);

                boulderVelocity = (float)System.Math.Round(Va, 2);
                boulder2Velocity = (float)System.Math.Round(Vb, 2);
                distance = (float)System.Math.Round(d, 2);

                t = distance / (Va + Vb);
                Tp = t - 0.5f;
                Db = boulder2Velocity * t;
                Da = boulderVelocity * t;
                Dp = Dac - Db;

                stuntTime = (float)System.Math.Round(Tp, 2);
                Vp = Dp / stuntTime;

                correctAnswer = (float)System.Math.Round(Vp, 2);

                boulder.transform.position = new Vector2(0, 0);
                boulderA.transform.position = new Vector2(boulder.transform.position.x + d, 0);
                myPlayer.transform.position = new Vector2(boulderA.transform.position.x - Dac, 0);

                boulderRB.rotation = 180;
                boulderRB.freezeRotation = false;
                jumpTime = 0.5f;

                question = playerName + " is instructed to vertically jump over between two incoming boulders at precisely <color=#006400>0.5 seconds</color> before they collide. If the boulder in front is <color=red>" + Dac + " meters</color> away from " + playerName + " is approaching at <color=purple>" + boulder2Velocity + " meters per second</color>, and the boulder behind " + pPronoun + " is <color=red>" + distance + " meters</color> away from the first boulder and is approaching at <color=purple>" + boulderVelocity + "meters per second</color>, at what <color=purple>velocity</color> should " + pronoun + " run forward for <color=#006400>" + stuntTime + " seconds</color> before doing the vertical jump?";
                break;
        }
        qc.SetQuestion(question);
        jumpForce = 1.07f / jumpTime;
        distanceMeassure.distance = distance;

        EndOfAnnotation.SetPosition(0, new Vector2(distance, 0));
        EndOfAnnotation.SetPosition(1, new Vector2(distance, 1.5f));

        correctAnswerIndicator.SetPosition(0, new Vector2(Da, 0));
        correctAnswerIndicator.SetPosition(1, new Vector2(Da, 2));
        correctAnswerIndicator.gameObject.SetActive(true);
        playerAnswerIndicator.gameObject.SetActive(false);

        myPlayer.moveSpeed = 0;
        boulderRB.velocity = new Vector2(0, 0);
        createCeilling.mapWitdh = distance;
        createCeilling.createQuadtilemap2();
    }
    public void Play()
    {
        qc.isSimulating = false;
        if (stage == 1)
        {
            playerAnswerIndicator.SetPosition(0, new Vector2((playerAnswer * playerVelocity), 0));
            playerAnswerIndicator.SetPosition(1, new Vector2((playerAnswer * playerVelocity), 1.5f));
        }
        else if (stage == 2)
        {
        }
        else
        {
            // stage3Flag = true;
        }
        isStartOfStunt = true;
        directorIsCalling = true;
    }
    public void RetryButton()
    {
        myPlayer.gameObject.SetActive(true);
        VeloMediumSetUp();
    }
    public void Next()
    {
        qc.nextStage = false;
        myPlayer.SetEmotion("");
        //ragdollSpawn.SetActive(false);
        //StartCoroutine(resetPrefab());
        VeloMediumSetUp();
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
            //boulder.transform.position = new Vector2(boulder.transform.position.x - 0.5f, boulder.transform.position.y);
        }
        else
        {
            RumblingManager.shakeON = false;
            yield return new WaitForSeconds((35 / playerVelocity) - stuntTime);
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";
            // if (isAnswerCorrect)
            // {
            //     correctAnswerIndicator.gameObject.SetActive(true);
            // }
            // else
            // {
            //     correctAnswerIndicator.gameObject.SetActive(true);
            //     playerAnswerIndicator.gameObject.SetActive(true);
            //     myPlayer.moveSpeed = 0;
            //     myPlayer.transform.position = ragdoll.transform.position;
            //     myPlayer.gameObject.SetActive(true);
            //     Destroy(ragdollPrefab);
            //     myPlayer.standup = true;
            // }
            boulderVelocity = 0;
        }
    }
    IEnumerator StuntResult()
    {
        directorIsCalling = true;
        isStartOfStunt = false;
        yield return new WaitForSeconds(1f);
        qc.ActivateResult(messageTxt, isAnswerCorrect);
    }
    IEnumerator Jump()
    {
        myPlayer.jump();
        StartCoroutine(StuntResult());
        yield return new WaitForSeconds(jumpTime);
        isAnswered = false;
    }
    void OnTriggerEnter(Collider other)
    {
        other.enabled = false;
        if (other.gameObject.name == "Boulder")
        {
            StartCoroutine(RagdollSpawn());
        }
    }
    IEnumerator RagdollSpawn()
    {
        yield return new WaitForEndOfFrame();
        myPlayer.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        ragdoll = Instantiate(ragdollPrefab);
        ragdoll.transform.position = myPlayer.gameObject.transform.position;
    }
}
