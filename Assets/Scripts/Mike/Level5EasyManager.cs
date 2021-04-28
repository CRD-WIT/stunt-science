using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class Level5EasyManager : MonoBehaviour
{
    [SerializeField] TMP_Text questionTxtBox1, questionTxtBox2, timerTxtBox, levelTxtBox, noAnswerMessage1, noAnswerMessage2, messageTxt, directorsSpeech;
    [SerializeField] TMP_InputField answerField1, answerField2;
    [SerializeField] Button playButton1, playButton2, retryButton, nextButton;
    string pronoun, pNoun, playerName, playerGender;
    [SerializeField] GameObject gear, afterStuntMessage, playerHangerTrigger1, playerHangerTrigger2, ragdollPrefab, stage1Layout, stage2Layout, gearSet, directorsBubble, ragdoll, directorPlatform;
    public Player myPlayer;
    [SerializeField] float elapsed, aVelocity, stage, gameTime;
    Vector2 playerPos, currentPos;
    StageManager sm = new StageManager();
    HeartManager playerHeart;
    [SerializeField] Animator crank;
    bool directorIsCalling, isStartOfStunt, stuntReady, DC, isCranking, crankingDone, crankReset, DCisOn, ragdollActive;
    public static bool isHanging, cranked, isAnswered;
    [SerializeField] Rigidbody2D gearRB, player;
    public static float playerAnswer, gear2Speed;
    void Start()
    {
        myPlayer = FindObjectOfType<Player>();
        playerHeart = FindObjectOfType<HeartManager>();
        //stage = 1;
        isAnswered = false;
        sm.SetGameLevel(5);
        sm.SetDifficulty(1);
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        levelTxtBox.text = sm.GetGameLevel();
        playerPos = myPlayer.transform.position;
        gearRB = gear.gameObject.GetComponent<Rigidbody2D>();
        player = myPlayer.gameObject.GetComponent<Rigidbody2D>();
        Lvl5EasySetUp();
    }
    void Update()
    {
        if (directorIsCalling)
        {
            StartCoroutine(DirectorsCall());
        }
        if (isAnswered)
        {
            switch (stage)
            {
                case 1:
                    timerTxtBox.text = elapsed.ToString("f2") + "s";
                    CurvedLineFollower.arc = playerAnswer * elapsed;
                    if (elapsed < gameTime)
                    {
                        elapsed = GearHangers.hangTime;
                        myPlayer.isHanging = true;
                        myPlayer.gameObject.transform.localScale = new Vector2(-0.4f, 0.4f);
                    }
                    else //(elapsed >= gameTime)
                    {
                        isHanging = false;
                        myPlayer.isHanging = false;
                        if (playerAnswer == aVelocity)
                        {
                            elapsed = gameTime;
                            messageTxt.text = "<b><color=green>Stunt Successful!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " has landed <color=green>safely</color> at the other platform!";
                            nextButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            if (!ragdollActive)
                            {
                                RagdollSpawn();
                            }
                            playerHeart.ReduceLife();
                            if (playerAnswer < aVelocity)
                            {
                                messageTxt.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " spinned the gear too slow and " + pronoun + " fell down too soon before the release point.\nThe correct answer is <color=red>" + aVelocity + "°/s</color>.";
                            }
                            else //if(playerAnswer > Speed)
                            {
                                messageTxt.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " spinned the gear too fast and " + pronoun + " fell down too late after the release point.\nThe correct answer is <color=red>" + aVelocity + "°/s</color>.";
                            }
                            retryButton.gameObject.SetActive(true);
                        }
                        StartCoroutine(StuntResult());
                        isAnswered = false;
                    }
                    break;

                case 2:
                    timerTxtBox.text = elapsed.ToString("f2") + "s";
                    CurvedLineFollower.arc = aVelocity * elapsed;
                    if (elapsed < playerAnswer)
                    {
                        elapsed = GearHangers.hangTime;
                        myPlayer.isHanging = true;
                        // myPlayer.gameObject.transform.localScale = new Vector2(-0.4f, 0.4f);
                    }
                    else //(elapsed >= gameTime)
                    {
                        isHanging = false;
                        myPlayer.isHanging = false;
                        if (playerAnswer == gameTime)
                        {
                            elapsed = gameTime;
                            messageTxt.text = "<b><color=green>Stunt Successful!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " has crossed <color=green>safely</color> at the other platform!";
                            nextButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            elapsed = playerAnswer;
                            if (!ragdollActive)
                            {
                                RagdollSpawn();
                            }
                            playerHeart.ReduceLife();
                            if (playerAnswer < aVelocity)
                            {
                                messageTxt.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " tried to grab the pipe too soon and " + pronoun + " fell down.\nThe correct answer is <color=red>" + gameTime + "s</color>.";
                            }
                            else //if(playerAnswer > Speed)
                            {
                                messageTxt.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " tried to grab the pipe too late and " + pronoun + " fell down.\nThe correct answer is <color=red>" + gameTime + "s</color>.";
                            }
                            retryButton.gameObject.SetActive(true);
                        }
                        StartCoroutine(StuntResult());
                        isAnswered = false;
                    }
                    break;

                case 3:
                    break;
            }
        }
        else
        {
            if (stuntReady)
            {
                switch (stage)
                {
                    case 1:
                        crank.SetBool("crank", cranked);
                        crank.SetBool("crankReset", crankReset);
                        if (isCranking)
                        {
                            StartCoroutine(Cranking());
                        }
                        if (myPlayer.transform.position.x < -1.25)
                        {
                            isCranking = true;
                        }
                        else
                        {
                            myPlayer.moveSpeed = 0f;
                            if (DC)
                            {
                                DC = false;
                                isStartOfStunt = true;
                                directorIsCalling = true;
                            }
                        }
                        break;
                    case 2:
                        if (myPlayer.transform.position.x < -9.25)
                        {
                            myPlayer.moveSpeed = 1.99f;
                        }
                        else
                        {
                            playButton2.interactable = true;
                            myPlayer.moveSpeed = 0;
                        }
                        break;
                }

            }
            else
            {
                DC = true;
                crankingDone = false;
            }
        }
    }

    void Lvl5EasySetUp()
    {
        stage1Layout.SetActive(false);
        stage2Layout.SetActive(false);
        CurvedLineFollower.arc = 0;

        playerHeart.losslife = false;
        playerAnswer = 0;
        myPlayer.happy = false;
        DC = true;
        DCisOn = false;
        myPlayer.lost = false;
        myPlayer.standup = false;
        elapsed = 0;
        timerTxtBox.text = "0.00s";
        aVelocity = 0;
        gameTime = 0;
        retryButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        myPlayer.gameObject.SetActive(true);
        ragdollActive = false;
        playerHangerTrigger1.SetActive(false);
        playerHangerTrigger2.SetActive(false);
        gear2Speed = 0;
        gearRB.angularVelocity = 0;
        gearRB.rotation = 0f;
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
        switch (stage)
        {
            case 1:
                stage1Layout.SetActive(true);

                float t = Random.Range(3.1f, 3.7f);
                gameTime = (float)System.Math.Round(t, 2);
                aVelocity = (float)System.Math.Round((210 / gameTime), 2);
                questionTxtBox1.text = playerName + " is trying to go accross the other platform by hanging at the tooth or the rotating gear from the starting platform and letting it go after <color=#006400>" + gameTime.ToString() + " seconds</color>. If the safe release point of the tooth is <color=red>210 degrees</color> from the grab point. At what <color=purple>angular velocity</color> should " + playerName + " set the spinning gear at?";



                CurvedLineFollower.stage = 1;
                myPlayer.transform.position = playerPos;
                gearSet.transform.position = new Vector3(gearSet.transform.position.x, gearSet.transform.position.y, gearSet.transform.position.z);
                crankReset = true;
                break;
            case 2:
                directorPlatform.transform.position = new Vector3(-17, 2, 0);
                directorPlatform.transform.localScale = new Vector3(-1.20f, 01.20f, 0);
                directorsSpeech.transform.localScale = new Vector3(directorsSpeech.transform.localScale.x * -1,directorsSpeech.transform.localScale.y, directorsSpeech.transform.localScale.z);
                playButton2.interactable = false;
                stage2Layout.SetActive(true);

                float av = Random.Range(30f, 40f);
                aVelocity = (float)System.Math.Round(av, 2);
                gameTime = (float)System.Math.Round((118 / aVelocity), 2);
                questionTxtBox2.text = playerName + " is trying to cross the other platform by hanging at the rotating gear from the starting platform and grabbing the pipe above upon reaching the highest point of the gear. If the release point of the gear is <color=red>118 degrees</color> from teh grab point and the release point, and the angular velocity of the gear is <color=purple>" + aVelocity + " degrees per second</color>, how <color=#006400>long</color> " + pronoun + " should hold on to the gear before reaching for the pipe?";



                CurvedLineFollower.stage = 2;
                myPlayer.transform.position = new Vector2(playerPos.x - 12, playerPos.y);
                gearSet.transform.position = new Vector3(-6.15f, gearSet.transform.position.y, gearSet.transform.position.z);
                gear2Speed = aVelocity * 1.61f;
                gearRB.angularVelocity = -aVelocity;
                stuntReady = true;
                break;
        }
    }
    public void PlayButton()
    {
        switch (stage)
        {
            case 1:
                if (answerField1.text == "")
                {
                    noAnswerMessage1.text = "Please enter your playerAnswer!";
                }
                else
                {
                    playerAnswer = float.Parse(answerField1.text);
                    playButton1.interactable = false;
                    stuntReady = true;
                    answerField1.text = playerAnswer.ToString() + "°/s";
                }
                break;
            case 2:
                if (answerField2.text == "")
                {
                    noAnswerMessage2.text = "Please enter your playerAnswer!";
                }
                else
                {
                    playerAnswer = float.Parse(answerField2.text);
                    playButton2.interactable = false;
                    isStartOfStunt = true;
                    directorIsCalling = true;
                    answerField2.text = playerAnswer.ToString() + "s";
                }
                break;
            case 3:
                if (answerField2.text == "")
                {
                    noAnswerMessage2.text = "Please enter your playerAnswer!";
                }
                else
                {
                    answerField1.text = playerAnswer.ToString() + "°";
                }
                break;
        }
    }
    public void RetryButton()
    {
        if (stage == 1)
        {
            answerField1.text = "";
            playButton1.interactable = true;
        }
        else //if (stage == 2)
        {
            answerField2.text = "";
            playButton2.interactable = true;
        }
        Lvl5EasySetUp();
        myPlayer.gameObject.SetActive(true);
        afterStuntMessage.SetActive(false);
    }
    public void NextButton()
    {
        myPlayer.SetEmotion("");
        if (stage == 1)
        {
            stage = 2;
        }
        else if (stage == 2)
        {
            stage = 3;
        }
        else
        {
            sm.SetDifficulty(2);
        }
        StartCoroutine(ExitStage());
    }
    public IEnumerator DirectorsCall()
    {
        DCisOn = true;
        stuntReady = false;
        directorIsCalling = false;
        if (isStartOfStunt)
        {
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Lights!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Camera!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Action!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "";
            directorsBubble.SetActive(false);
            if (stage == 1)
                playerHangerTrigger1.SetActive(true);
            else if (stage == 2)
                playerHangerTrigger2.SetActive(true);
            isAnswered = true;
        }
        else
        {
            myPlayer.brake = true;
            yield return new WaitForSeconds(1f);
            myPlayer.brake = false;
            yield return new WaitForSeconds(1.25f);
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";
            myPlayer.gameObject.transform.position = new Vector2(myPlayer.gameObject.transform.position.x + 0.4f, myPlayer.gameObject.transform.position.y);
            myPlayer.gameObject.transform.localScale = new Vector2(0.4f, 0.4f);
            yield return new WaitForEndOfFrame();
            myPlayer.happy = true;
        }
    }
    IEnumerator ExitStage()
    {
        myPlayer.standup = false;
        afterStuntMessage.SetActive(false);
        myPlayer.moveSpeed = 5;
        yield return new WaitForSeconds(3f);
        StartCoroutine(playerHeart.endBGgone());
        yield return new WaitForSeconds(2.8f);
        myPlayer.transform.position = new Vector2(0f, myPlayer.transform.position.y);
        myPlayer.moveSpeed = 0;
        Lvl5EasySetUp();
        answerField1.text = "";
        playButton1.interactable = true;
        playerAnswer = 0;
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(1f);
        directorIsCalling = true;
        isStartOfStunt = false;
        yield return new WaitForSeconds(3f);
        afterStuntMessage.SetActive(true);
        Destroy(ragdoll);
    }
    IEnumerator Cranking()
    {
        isCranking = false;
        crankReset = false;
        if (!crankingDone)
        {
            cranked = true;
            yield return new WaitForSeconds(1.17f);
            gear2Speed = playerAnswer * 1.612f;
            gearRB.angularVelocity = -playerAnswer;
            cranked = false;
            yield return new WaitForEndOfFrame();
            myPlayer.moveSpeed = 1.99f;
            crankingDone = true;
        }
    }
    IEnumerator Hanging()
    {
        myPlayer.isHanging = true;
        yield return new WaitForSeconds(gameTime);
        myPlayer.isHanging = false;
    }
    public void RagdollSpawn()
    {
        myPlayer.gameObject.SetActive(false);
        ragdoll = Instantiate(ragdollPrefab);
        ragdoll.transform.position = myPlayer.gameObject.transform.position * 1.05f;
        ragdollActive = true;
    }
}