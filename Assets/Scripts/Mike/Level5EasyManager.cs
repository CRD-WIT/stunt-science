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
    string pronoun, pPronoun, pNoun, playerName, playerGender;

    //Gameplay
    [SerializeField]
    GameObject gear, afterStuntMessage, playerHangerTrigger;
    Player myPlayer;
    [SerializeField]
    float gameTime, aVelocity, elapsed, currentPos, stage, playerAnswer, currentTime;
    Vector2 playerPos;
    StageManager sm = new StageManager();
    HeartManager playerHeart;
    bool isAnswered, isAnswerCorrect;
    [SerializeField]
    Animator crank;

    //Director
    [SerializeField]
    GameObject directorsBubble;
    [SerializeField]
    TMP_Text directorsSpeech;
    [SerializeField]
    bool directorIsCalling, isStartOfStunt, stuntReady, DC, isCranking, crankingDone, crankReset, DCisOn;
    public static bool isHanging, cranked;
    [SerializeField]
    float a;
    Rigidbody2D gearRB;
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
                    //gear.transform.rotation = Quaternion.Slerp(gear.transform.rotation, Quaternion.(0, 0, -a), playerAnswer * Time.deltaTime);
                    //a = playerAnswer * gameTime;
                    //gear.transform.RotateAround(gear.transform.position, Vector3.back, playerAnswer * elapsed);
                    //gear.transform.eulerAngles = (Vector3.back * playerAnswer * elapsed);
                    //gearRB.angularVelocity = -playerAnswer;
                    myPlayer.isHanging = true;
                    elapsed += Time.fixedDeltaTime;
                    timerTxtBox.text = elapsed.ToString("f2") + "s";
                    if (elapsed >= gameTime)
                    {
                        StartCoroutine(StuntResult());
                        myPlayer.isHanging = false;
                        isAnswered = false;
                        timerTxtBox.text = gameTime.ToString("f2") + "s";
                        isHanging = false;
                        if ((playerAnswer == aVelocity))
                        {
                            messageTxt.text = "<b><color=green>Stunt Successful!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " is <color=green>safe</color>!";
                            nextButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            playerHeart.ReduceLife();
                            myPlayer.transform.position = new Vector2(-0.2f, myPlayer.transform.position.y);
                            retryButton.gameObject.SetActive(true);
                            if (playerAnswer < aVelocity)
                            {
                                messageTxt.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " ran too slow and " + pronoun + " stopped before the safe spot.\nThe correct playerAnswer is <color=red>" + aVelocity + "m/s</color>.";
                            }
                            else //if(playerAnswer > Speed)
                            {
                                messageTxt.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " ran too fast and " + pronoun + " stopped after the safe spot.\nThe correct playerAnswer is <color=red>" + aVelocity + "m/s</color>.";
                            }
                        }
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
            float angle = (float)((playerAnswer * gameTime) * (System.Math.PI / 180));
            //a = angle / gameTime;
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
        //isHanging = true;

        DC = true;
        DCisOn = false;
        crankReset = true;

        myPlayer.lost = false;
        myPlayer.standup = false;
        //gear.transform.eulerAngles = (Vector3.back * 0);
        gearRB.rotation = 0;
        elapsed = 0;
        timerTxtBox.text = "0.00s";
        aVelocity = 0;
        gameTime = 0;
        playerHangerTrigger.SetActive(false);
        retryButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        myPlayer.transform.position = playerPos;
        if (playerGender == "Male")
        {
            pronoun = "he";
            pPronoun = "him";
            pNoun = "his";
        }
        else
        {
            pronoun = "she";
            pPronoun = "her";
            pNoun = "her";
        }
        switch (stage)
        {
            case 1:
                float t = Random.Range(3.1f, 3.7f);
                gameTime = (float)System.Math.Round(t, 2);
                aVelocity = (float)System.Math.Round((210 / gameTime), 2);
                questionTxtBox.text = playerName + " is trying to go accross the other platform by hanging at the tooth or the rotating gear from the starting platform and letting it go after <color=#006400>" + gameTime.ToString() + " seconds</color>. If the safe release point of the tooth is <color=red>210 degrees</color> from the grab point. At what <color=purple>angular velocity</color> should " + playerName + " set the spinning gear at?";
                gearRB.angularVelocity = -20;
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
            yield return new WaitForSeconds(1.25f);
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";
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
    IEnumerator Hanging(){
        myPlayer.isHanging =true;
        yield return new WaitForSeconds(gameTime);
        myPlayer.isHanging = false;
    }
}
