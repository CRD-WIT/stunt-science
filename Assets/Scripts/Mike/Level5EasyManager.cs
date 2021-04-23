using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Level5EasyManager : MonoBehaviour
{
    //UI
    [SerializeField]
    TMP_Text questionTxtBox, timerTxtBox, levelTxtBox, noAnswerMessage, messageTxt;
    public TMP_InputField AnswerField;
    [SerializeField]
    Button playButton, retryButton, nextButton;
    string pronoun, pNoun, playerName, playerGender;

    //Gameplay
    [SerializeField]
    GameObject gear, afterStuntMessage, playerHangerTrigger, ragdollPrefab;
    public Player myPlayer;
    [SerializeField]
    float gameTime, aVelocity, elapsed, stage, playerAnswer, currentTime;
    Vector2 playerPos, currentPos;
    StageManager sm = new StageManager();
    HeartManager playerHeart;
    bool isAnswered, isAnswerCorrect;
    [SerializeField]
    Animator crank;

    //Director
    [SerializeField]
    GameObject directorsBubble, ragdoll;
    [SerializeField]
    TMP_Text directorsSpeech;
    [SerializeField]
    bool directorIsCalling, isStartOfStunt, stuntReady, DC, isCranking, crankingDone, crankReset, DCisOn, ragdollActive;
    public static bool isHanging, cranked;
    [SerializeField]
    float a;
    Rigidbody2D gearRB, player;
    void Start()
    {
        myPlayer = FindObjectOfType<Player>();
        playerHeart = FindObjectOfType<HeartManager>();
        stage = 1;
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
    void FixedUpdate()
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
                    myPlayer.isHanging = true;
                    elapsed += Time.fixedDeltaTime;
                    timerTxtBox.text = elapsed.ToString("f2") + "s";
                    currentPos = myPlayer.transform.position;
                    myPlayer.gameObject.transform.localScale = new Vector2(-0.4f, 0.4f);
                    if (elapsed >= gameTime)
                    {
                        timerTxtBox.text = gameTime.ToString("f2") + "s";
                        isHanging = false;
                        myPlayer.isHanging = false;
                        if (playerAnswer == aVelocity)
                        {
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
                                messageTxt.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " spinned the gear too slow and " + pronoun + " fell doen too soon before the release point.\nThe correct answer is <color=red>" + aVelocity + "°/s</color>.";
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
                    break;

                case 3:
                    break;
            }
        }
        else
        {
            crank.SetBool("crank", cranked);
            crank.SetBool("crankReset", crankReset);
            if (stuntReady)
            {
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
        myPlayer.transform.position = playerPos;
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
                myPlayer.gameObject.transform.localScale = new Vector2(0.4f, 0.4f);
                ragdollActive = false;
                float t = Random.Range(3.1f, 3.7f);
                gameTime = (float)System.Math.Round(t, 2);
                aVelocity = (float)System.Math.Round((210 / gameTime), 2);
                questionTxtBox.text = playerName + " is trying to go accross the other platform by hanging at the tooth or the rotating gear from the starting platform and letting it go after <color=#006400>" + gameTime.ToString() + " seconds</color>. If the safe release point of the tooth is <color=red>210 degrees</color> from the grab point. At what <color=purple>angular velocity</color> should " + playerName + " set the spinning gear at?";
                gearRB.angularVelocity = -20;
                GearHangers.gameTime = gameTime;
                playerHangerTrigger.SetActive(false);
                gearRB.rotation = 0;
                crankReset = true;
                break;
        }
    }
    public void PlayButton()
    {

        if (AnswerField.text == "")
        {
            noAnswerMessage.text = "Please enter your playerAnswer!";
        }
        else
        {
            playerAnswer = float.Parse(AnswerField.text);
            playButton.interactable = false;
            if (stage == 1)
            {
                stuntReady = true;
                AnswerField.text = playerAnswer.ToString() + "°/s";
                //StartCoroutine(DirectorsCall());
            }
            else if (stage == 2)
            {
                isStartOfStunt = true;
                directorIsCalling = true;
                AnswerField.text = playerAnswer.ToString() + "s";
            }
            else
            {
                AnswerField.text = playerAnswer.ToString() + "°";
            }
        }
        //currentTime = Time.fixedTime;
    }
    public void RetryButton()
    {
        AnswerField.text = "";
        playButton.interactable = true;
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
            isAnswered = true;
            playerHangerTrigger.SetActive(true);
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
        AnswerField.text = "";
        playButton.interactable = true;
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
        ragdoll.transform.position = currentPos;
        ragdollActive = true;
    }
}
