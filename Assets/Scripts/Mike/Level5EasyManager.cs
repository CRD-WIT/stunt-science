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
    public TMP_InputField  AnswerField;
    [SerializeField]
    Button playButton, retryButton, nextButton;
    string pronoun, pPronoun, pNoun, playerName, playerGender;

    //Gameplay
    [SerializeField]
    GameObject gear, afterStuntMessage;
    Player myPlayer;
    public float gameTime, aVelocity, elapsed, currentPos, stage, playerAnswer;
    StageManager sm = new StageManager();
    HeartManager playerHeart;
    bool isAnswered, isAnswerCorrect;

    //Director
    [SerializeField]
    GameObject directorsBubble;
    TMP_Text directorsSpeech;
    bool directorIsCalling, isStartOfStunt;

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
        Lvl5EasySetUp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAnswered)
        {
            switch (stage)
            {
                case 1:
                    timerTxtBox.text = elapsed.ToString("f2") + "s";
                    elapsed += Time.fixedDeltaTime;
                    if (elapsed >= gameTime)
                    {
                        StartCoroutine(StuntResult());
                        isAnswered = false;
                        timerTxtBox.text = gameTime.ToString("f2") + "s";
                        if ((playerAnswer == aVelocity))
                        {
                            messageTxt.text = "<b><color=green>Stunt Successful!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " is <color=green>safe</color>!";
                            retryButton.gameObject.SetActive(false);
                        }
                        else
                        {
                            playerHeart.ReduceLife();
                            myPlayer.transform.position = new Vector2(-0.2f, myPlayer.transform.position.y);
                            nextButton.gameObject.SetActive(false);
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
    }

    void Lvl5EasySetUp()
    {
        elapsed = 0;
        timerTxtBox.text = "0.00s";
        aVelocity = 0;
        gameTime = 0;
        retryButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
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
                questionTxtBox.text = playerName+" is trying to go accross the other platform by hanging at the tooth or the rotating gear from the starting platform and letting it go after <color=#006400>" + gameTime.ToString()+ " seconds</color>. If the safe release point of the tooth is <color=red>210 degrees</color> from the grab point. At what <color=purple>angular velocity</color> should " + playerName + " set the spinning gear at?";
                break;
        }
    }
    public void ButtonManager()
    {
        switch (this.gameObject.name)
        {
            case "playButton":
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
                        isStartOfStunt = true;
                        directorIsCalling = true;
                        AnswerField.text = playerAnswer.ToString() + "°/s";
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
                break;

            case "retryButton":
                AnswerField.text = "";
                playButton.interactable = true;
                Lvl5EasySetUp();
                myPlayer.gameObject.SetActive(true);
                break;

            case "nextButton":
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
                break;
        }
    }
    public IEnumerator DirectorsCall()
    {
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
        SimulationManager.directorIsCalling = true;
        SimulationManager.isStartOfStunt = false;
        yield return new WaitForSeconds(3f);
        afterStuntMessage.SetActive(true);
    }
}
