using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VelocityMediumManager : MonoBehaviour
{
    StageManager sm = new StageManager();
    [SerializeField] GameObject boulder, directorsBubble, floor, afterStuntMessage, ragdollPrefab, boulderA;
    [SerializeField] LineRenderer EndOfAnnotation, correctAnswerIndicator, playerAnswerIndicator;
    Annotation distanceMeassure;
    Player myPlayer;
    CeillingGenerator createCeilling;
    [SerializeField] TMP_Text timer, level, errorTxtBox, directorsSpeech;
    [SerializeField] TMP_InputField answerField;
    [SerializeField] Button answerButton, nextButton, retryButton;
    [SerializeField] float playerVelocity, boulderVelocity, boulder2Velocity, stuntTime, distance, jumpDistance, correctAnswer;
    public int stage;
    Rigidbody2D boulderRB;
    GameObject ragdoll;
    float playerPos, playerAnswer, elapsed, currentPlayerPos, jumpTime, jumpForce;
    string question, playerName, playerGender, pronoun, pPronoun, messageTxt;
    bool isStartOfStunt, directorIsCalling, isAnswered, isAnswerCorrect, isEndOfStunt;
    // Start is called before the first frame update
    void Start()
    {
        myPlayer = FindObjectOfType<Player>();
        distanceMeassure = FindObjectOfType<Annotation>();
        createCeilling = FindObjectOfType<CeillingGenerator>();
        boulderRB = boulder.GetComponent<Rigidbody2D>();
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        sm.SetGameLevel(1);
        playerPos = myPlayer.gameObject.transform.position.x;
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
        level.text = sm.GetGameLevel();
        VeloMediumSetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        currentPlayerPos = myPlayer.transform.position.x;
        if (isAnswered)
        {
            timer.text = elapsed.ToString("f2") + "s";
            elapsed += Time.deltaTime;
            myPlayer.moveSpeed = playerVelocity;
            boulderRB.velocity = new Vector2(-boulderVelocity, 0);
            if (playerAnswer < elapsed)
            {
                elapsed = playerAnswer;
                if (playerAnswer == stuntTime)
                {
                    myPlayer.jumpforce = jumpForce;
                    nextButton.gameObject.SetActive(true);
                    messageTxt = "<b><color=green>Stunt Successful!</color></b>\n\n\n" + playerName + " has jumped over the boulder <color=green>safely</color>!";
                }
                else
                {
                    myPlayer.jumpforce = jumpForce - 0.13f;
                    retryButton.gameObject.SetActive(true);
                    if (playerAnswer < stuntTime)
                    {
                        messageTxt = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + playerName + " jumped too soon and hit the boulder.\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                    }

                    else
                        messageTxt = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + playerName + " jumped too late and hit the boulder.\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                }
                isEndOfStunt = true;
                StartCoroutine(Jump());
            }
        }
        if (isEndOfStunt)
        {
            isEndOfStunt = false;
            StartCoroutine(StuntResult());
        }
    }
    void VeloMediumSetUp()
    {
        float Va = 0, Vb = 0, d = 0, Da = 0, Db = 0, Dj = 0, t = 0;//, tm = 0;
        playerAnswer = 0;
        elapsed = 0;
        //Destroy(ragdollPrefab);
        nextButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        timer.text = "0.00s";
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
                //t = (2 * distance) / (3 * (playerVelocity + boulderVelocity)); //this is divided by 2
                Dj = ((Va + Vb) / 2) + 0.53f;
                t = (d - Dj) / (Va + Vb);
                //t = (4 * distance) / (3 * (playerVelocity + boulderVelocity)); //this is divided by 4
                //tm = distance / (playerVelocity + boulderVelocity);
                //t = tm - Random.Range(0.375f, 0.625f);
                stuntTime = (float)System.Math.Round(t, 2);
                Da = playerVelocity * stuntTime;
                Db = boulderVelocity * stuntTime;

                myPlayer.transform.position = new Vector2(playerPos, 0);

                boulder.transform.position = new Vector2(distance, boulder.transform.position.y);
                boulderRB.freezeRotation = false;

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

                question = playerName + " is instructed to run until the end of the scene while jumping over the rolling boulder. If " + pronoun + " is running at a velocity of <color=purple>" + playerVelocity + " meters per second</color> while an incoming fast moving boulder <color=red>" + distance + " meters</color> away is catchind up from behind with a velocity of <color=purple>" + boulderVelocity + "meters per second</color>, at after how many <color=red>meters</color> should " + playerName + " be jumping if " + pronoun + " has to jump at exactly <color=red>" + jumpDistance + " meters</color> away from the boulder in order to jump over it safely?";
                break;
            case 3:
                boulder.SetActive(true);
                boulderA.SetActive(true);
                float Vp, Dp, Tp, Dac = Random.Range(19f, 22f);
                Va = Random.Range(7f, 8f);
                Vb = Random.Range(3f, 4f);
                d = Random.Range(29f, 30f);

                boulderVelocity = (float)System.Math.Round(Va, 2);
                boulder2Velocity = (float)System.Math.Round(Vb, 2);
                distance = (float)System.Math.Round(d, 2);

                t = d / (Va + Vb);
                Tp = t - 0.5f;

                //Dj = (Vb - Va) / 2;
                //t = (distance - Dj) / (boulderVelocity - playerVelocity);
                stuntTime = (float)System.Math.Round(t, 2);
                Db = boulder2Velocity * stuntTime;
                Dp = Dac - Db;

                Vp = Dp / Tp;

                correctAnswer = (float)System.Math.Round(Vp, 2);

                boulder.transform.position = new Vector2(0, 0);
                boulderA.transform.position = new Vector2(boulder.transform.position.x + d, 0);
                myPlayer.transform.position = new Vector2(boulderA.transform.position.x - Dac, 0);

                boulderRB.rotation = 180;
                boulderRB.freezeRotation = false;

                question = playerName + " is instructed to run until the end of the scene while jumping over the rolling boulder. If " + pronoun + " is running at a velocity of <color=purple>" + playerVelocity + " meters per second</color> while an incoming fast moving boulder <color=red>" + distance + " meters</color> away is catchind up from behind with a velocity of <color=purple>" + boulderVelocity + "meters per second</color>, at after how many <color=red>meters</color> should " + playerName + " be jumping if " + pronoun + " has to jump at exactly <color=red>" + jumpDistance + " meters</color> away from the boulder in order to jump over it safely?";
                break;
        }
        jumpTime = Dj / Va;
        jumpForce = 1.07f / jumpTime;
        jumpDistance = (float)System.Math.Round(Dj, 2);
        distanceMeassure.distance = distance;

        EndOfAnnotation.SetPosition(0, new Vector2(distance, 0));
        EndOfAnnotation.SetPosition(1, new Vector2(distance, 1.5f));

        correctAnswerIndicator.SetPosition(0, new Vector2(Da, 0));
        correctAnswerIndicator.SetPosition(1, new Vector2(Da, 2));
        correctAnswerIndicator.gameObject.SetActive(true);
        playerAnswerIndicator.gameObject.SetActive(false);

        myPlayer.moveSpeed = 0;
        boulderRB.velocity = new Vector2(0, 0);
        //createCeilling.mapWitdh = distance;
        // createCeilling.createQuadtilemap2();
    }
    public void PlayButton()
    {
        if (answerField.text == "")
        {
            errorTxtBox.text = "Please enter your answer!";
        }
        else
        {
            playerAnswer = float.Parse(answerField.text);
            answerButton.interactable = false;
            distanceMeassure.gameObject.SetActive(false);
            if (stage == 1)
            {
                isStartOfStunt = true;
                directorIsCalling = true;
                answerField.text = playerAnswer.ToString() + "s";
                playerAnswerIndicator.SetPosition(0, new Vector2((playerAnswer * playerVelocity), 0));
                playerAnswerIndicator.SetPosition(1, new Vector2((playerAnswer * playerVelocity), 1.5f));
            }
            else if (stage == 2)
            {
                isStartOfStunt = true;
                directorIsCalling = true;
                answerField.text = playerAnswer.ToString() + "s";
            }
            // else
            // {
            //     stage3Flag = true;
            //     answerField.text = playerAnswer.ToString() + "m";
            // }
        }
    }
    public void RetryButton()
    {
        isAnswered = false;
        answerField.text = "";
        answerButton.interactable = true;
        myPlayer.gameObject.SetActive(true);
        afterStuntMessage.SetActive(false);
        VeloMediumSetUp();
    }
    public void NextButton()
    {
        myPlayer.SetEmotion("");
        //ragdollSpawn.SetActive(false);
        //StartCoroutine(resetPrefab());
        if (stage == 1)
        {
            stage = 2;
            //VelocityEasyStage1.gameObject.SetActive(false);
            //theManager2.gameObject.SetActive(true);
            VeloMediumSetUp();
        }
        else if (stage == 2)
        {
            stage = 3;
            VeloMediumSetUp();
            //theManager2.gameObject.SetActive(false);
        }
        else
        {
            sm.SetDifficulty(2);
        }
        //StartCoroutine(ExitStage());
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
            //boulder.transform.position = new Vector2(boulder.transform.position.x - 0.5f, boulder.transform.position.y);
            isAnswered = true;
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
            if (isAnswerCorrect)
            {
                correctAnswerIndicator.gameObject.SetActive(true);
            }
            else
            {
                correctAnswerIndicator.gameObject.SetActive(true);
                playerAnswerIndicator.gameObject.SetActive(true);
                myPlayer.moveSpeed = 0;
                myPlayer.transform.position = ragdoll.transform.position;
                myPlayer.gameObject.SetActive(true);
                Destroy(ragdollPrefab);
                myPlayer.standup = true;
            }
            boulderVelocity = 0;
        }
    }
    IEnumerator StuntResult()
    {
        //yield return new WaitForSeconds(1);
        directorIsCalling = true;
        isStartOfStunt = false;
        yield return new WaitForSeconds(4f);
        afterStuntMessage.SetActive(true);
    }
    IEnumerator Jump()
    {
        myPlayer.jump();
        yield return new WaitForEndOfFrame();
        //if(isAnswerCorrect)
        playerAnswerIndicator.gameObject.SetActive(true);
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
