using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VelocityMediumManager : MonoBehaviour
{
    StageManager sm = new StageManager();
    [SerializeField] GameObject boulder, directorsBubble, floor;
    [SerializeField] LineRenderer EndOfAnnotation;
    [SerializeField] Annotation ditanceMeassure = new Annotation();
    [SerializeField] Player myPlayer = new Player();
    [SerializeField] TMP_Text question, timer, level, errorTxtBox, directorsSpeech;
    [SerializeField] TMP_InputField answerField;
    [SerializeField] Button answerButton;
    [SerializeField] float playerVelocity, boulderVelocity, stuntTime, distance, jumpDistance;
    public int stage;
    Rigidbody2D boulderRB;
    float playerPos, playerAnswer, elapsed, currentPlayerPos;
    string playerName, playerGender;
    bool isStartOfStunt, directorIsCalling, isAnswered, isAnswerCorrect;
    // Start is called before the first frame update
    void Start()
    {
        boulderRB = boulder.GetComponent<Rigidbody2D>();
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        sm.SetGameLevel(1);
        playerPos = myPlayer.gameObject.transform.position.x;
        VeloMediumSetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        if (isAnswered)
        {
            timer.text = elapsed.ToString("f2");
            elapsed += Time.deltaTime;
            myPlayer.moveSpeed = playerVelocity;
            boulderRB.velocity = new Vector2(-boulderVelocity, 0);
            if(playerAnswer < elapsed){
                myPlayer.jump();
                //floor.layer = 0;
                isAnswered =false;
            }
        }
    }
    void VeloMediumSetUp()
    {
        string pronoun, pPronoun;
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

        timer.text = "0.00s";

        switch (stage)
        {
            case 1:
                float Va = Random.Range(8f, 9f),
                Vb = Random.Range(4f, 6f),
                d = Random.Range(27f, 30f),
                Da, Db, Dj, t;
                playerVelocity = (float)System.Math.Round(Va, 2);
                boulderVelocity = (float)System.Math.Round(Vb, 2);
                distance = (float)System.Math.Round(d, 2);
                t = (2 * distance) / (3 * (playerVelocity + boulderVelocity));
                stuntTime = (float)System.Math.Round(t, 2);
                Da = playerVelocity * stuntTime;
                Db = boulderVelocity * stuntTime;
                Dj = (Da + Db) / 2;
                jumpDistance = (float)System.Math.Round(Dj, 2);
                ditanceMeassure.distance = distance;

                EndOfAnnotation.SetPosition(0, new Vector2(distance - 15, 0));
                EndOfAnnotation.SetPosition(1, new Vector2(distance - 15, 1.5f));

                boulder.transform.position = new Vector2(distance - 15, boulder.transform.position.y);

                question.text = playerName + " is instructed to run until the end of the scene while jumping over the rolling boulder. If " + pronoun + " is running at a velocity of <color=purple>" + playerVelocity + " meters per second</color> while an incoming boulder <color=red>" + distance + " meters</color> away is rolling at the velocity of <color=purple>" + boulderVelocity + "meters per second</color>, at how many <color=#006400>seconds</color> after will " + playerName + " jump if " + pronoun + " has to jump at exactly <color=red>" + jumpDistance + " meters</color> away from the boulder in order to jump over it safely?";
                break;
        }



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
            if (stage == 1)
            {
                isStartOfStunt = true;
                directorIsCalling = true;
                answerField.text = playerAnswer.ToString() + "m/s";
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
            RumblingManager.shakeON = false;
            yield return new WaitForSeconds(1.25f);
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";
            if (isAnswerCorrect)
            {
                if (stage == 3)
                    myPlayer.slide = true;
                else
                    myPlayer.happy = true;
            }
            else
            {
                myPlayer.standup = true;
            }
        }
    }
}
