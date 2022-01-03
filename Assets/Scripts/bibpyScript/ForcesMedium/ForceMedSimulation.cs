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
    public float playerAnswer;
    public TMP_InputField answerField;
    public TMP_Text diretorsSpeech;
    public QuestionContForcesMed theQuestion;
    bool directorIsCalling;
    public Button playButton;
    public GameObject directorBubble;
    public Vector2 playerStartPoint, zombie1StartPoint, zombie2StartPoint, boxStartPoint;
    public GameObject dimensionOne, dimensionTwo;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("CurrentString", ("Forces"));
        PlayerPrefs.SetInt("level", 4);
        theHeart = FindObjectOfType<HeartManager>();
        playerStartPoint = thePlayer.transform.position;


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
            if (answerField.text == "" || playerAnswer > (theManagerOne.correctAnswer + 10) || playerAnswer < 150)
            {
                StartCoroutine(errorMesage());
                theQuestion.errorText = ("answer must not exceed ") + (theManagerOne.correctAnswer + 10).ToString("F2") + (" N and not less than 150 N");
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
            if (answerField.text == "" || playerAnswer > (theManagerTwo.correctAnswer + 6) || playerAnswer < 150)
            {
                StartCoroutine(errorMesage());
                theQuestion.errorText = ("answer must not exceed ") + (theManagerTwo.correctAnswer +6).ToString("F2") + (" N and not less than 150 N");
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
            if (answerField.text == "")
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
            theManagerOne.preset = true;
            theManagerOne.showProblem();
        }
        if (stage == 2)
        {
            theManagerTwo.preset = true;
            theManagerTwo.showProblem();
        }
        if (stage == 3)
        {
            theManagerThree.thePlayer.transform.position = theManagerThree.playerStartPoint;
            theManagerThree.showProblem();
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
            if (stage == 1)
            {
                 theManagerOne.startRunning = true;
            }
            if (stage == 2)
            {
                theManagerTwo.startRunning = true;
            }
            if (stage == 3)
            {
                theManagerTwo.startRunning = true;
            }
           
            
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
            zombie1StartPoint = theManagerThree.theZombie[0].transform.position;
            zombie2StartPoint = theManagerThree.theZombie[1].transform.position;
            //theManagerTwo.GenerateProblem();

        }
        if (stage == 1)
        {
            zombie1StartPoint = theManagerTwo.theZombie[0].transform.position;
            zombie2StartPoint = theManagerTwo.theZombie[1].transform.position;
            boxStartPoint = theManagerTwo.box2.transform.position;
            stage = 2;
            theManagerOne.gameObject.SetActive(false);
            theManagerTwo.gameObject.SetActive(true);
            theManagerTwo.showProblem();
            dimensionOne.SetActive(false);
            dimensionTwo.SetActive(true);

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
        theHeart.startbgentrance();
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

