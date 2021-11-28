using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForceMedSimulation : MonoBehaviour
{
    public HeartManager theHeart;
    public PlayerContForcesMed thePlayer;
    public ForceMedOne theManagerOne;
    public ForceMedTwo theManagerTwo;
    public ForceMedThree theManagerThree;
    public bool simulate;
    public float stage;
    public static float playerAnswer;
    public TMP_InputField answerField;
    public TMP_Text diretorsSpeech;
    public QuestionContForcesMed theQuestion;
    bool directorIsCalling;
    public Button playButton;
    public GameObject directorBubble;
    // Start is called before the first frame update
void Start()
    {
        PlayerPrefs.SetString("CurrentString", ("Forces"));
        PlayerPrefs.SetInt("level", 4);
        theHeart = FindObjectOfType<HeartManager>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    public void PlayButton()
    {

        if (stage == 1)
        {
            playerAnswer = float.Parse(answerField.text);
            if (answerField.text == "" )
            {
                StartCoroutine(errorMesage());
                theQuestion.errorText = ("Invalid strength of a glass");
            }
            else
            {
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField.text = playerAnswer.ToString() + "N";
                }

            }
        }
        if (stage == 2)
        {
            playerAnswer = float.Parse(answerField.text);
            if (answerField.text == "" || playerAnswer > 12.42)
            {
                StartCoroutine(errorMesage());
                theQuestion.errorText = ("exceed human capabilities!");
            }
            else
            {
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField.text = playerAnswer.ToString() + "m/s²";
                }

            }
        }
        if (stage == 3)
        {
            playerAnswer = float.Parse(answerField.text);
            if (answerField.text == "" || playerAnswer > 100)
            {
                StartCoroutine(errorMesage());
                theQuestion.errorText = ("too heavy for a human!");
            }
            else
            {
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField.text = playerAnswer.ToString() + "kg";
                }

            }
        }
    }
    public void next()
    {
        theQuestion.isModalOpen = false;
        StartCoroutine(theHeart.startBGgone());
        StartCoroutine(nextStage());


    }
    public void retry()
    {
        simulate = false;
        playButton.interactable = true;
        playerAnswer = 0;
        answerField.text = ("");
        thePlayer.moveSpeed = 0;
        thePlayer.gameObject.SetActive(true);
        theHeart.losslife = false;

        if (stage == 1)
        {

        }
        if (stage == 2)
        {

        }
        if (stage == 3)
        {

        }

    }
    public IEnumerator DirectorsCall()
    {
        if (directorIsCalling)
        {
            directorBubble.SetActive(true);
            diretorsSpeech.text = "Lights!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Camera!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Action!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "";
            directorBubble.SetActive(false);
            simulate = true;
            //theManagerOne.startRunning = true;
            directorIsCalling = false;
        }
        else
        {
            directorBubble.SetActive(true);
            diretorsSpeech.text = "Cut!";
            yield return new WaitForSeconds(1);
            directorBubble.SetActive(false);
            diretorsSpeech.text = "";
        }
    }
    IEnumerator nextStage()
    {
        yield return new WaitForSeconds(.1f);
        playButton.interactable = true;
        answerField.text = ("");
        if (stage == 2)
        {
            stage = 3;
            theManagerTwo.gameObject.SetActive(false);
            theManagerThree.gameObject.SetActive(true);
            //theManagerTwo.GenerateProblem();

        }
        if (stage == 1)
        {
            stage = 2;
            theManagerOne.gameObject.SetActive(false);
            theManagerTwo.gameObject.SetActive(true);
            //theManagerTwo.GenerateProblem();

        }

        StartCoroutine(theHeart.startBGgone());
    }
    public IEnumerator errorMesage()
    {
        theQuestion.popupVisible = true;
        yield return new WaitForSeconds(3);
        theQuestion.popupVisible = false;
    }
    public void action()
    {
        theQuestion.isModalOpen = false;
        if (theQuestion.answerIsCorrect == false)
        {
            retry();

        }
        else
        {
            next();
        }
    }

}

