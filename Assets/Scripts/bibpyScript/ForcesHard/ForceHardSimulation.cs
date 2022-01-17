using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForceHardSimulation : MonoBehaviour
{
    // Start is called before the first frame update
   public HeartManager theHeart;
    public PlayerContForcesMed thePlayer;
    public ForceHardManagerOne theManagerOne;
    public ForceManagerHardTwo theManagerTwo;
    public ForceManagerHardThree theManagerThree;
    public PrisonerManager thePrisoner;
    public bool simulate;
    public float stage;
    public float playerAnswer;
    public TMP_InputField answerField;
    public TMP_Text diretorsSpeech;
    public QuestionContForcesMed theQuestion;
    bool directorIsCalling;
    public Button playButton;
    public GameObject directorBubble;
    public bool destroyGlass;
    public Vector2 playerStartPoint, zombie1StartPoint, zombie2StartPoint, boxStartPoint;
    //public GameObject dimensionOne, dimensionTwo,dimensionThree, groundOne,groundTwo;
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
            if (answerField.text == "" || playerAnswer > 500|| playerAnswer < (-theManagerOne.resultantDownhillForce)+50)
            {
                StartCoroutine(errorMesage());
                theQuestion.errorText = ("answer must not exceed 500N or not less than"+ ((-theManagerOne.resultantDownhillForce)+50).ToString("F2")+ "N ") ;
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
            if (answerField.text == "" )
            {
                StartCoroutine(errorMesage());
                theQuestion.errorText = ("answer must not exceed ") ;
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
        if (stage == 3)
        {
            playerAnswer = float.Parse(answerField.text);
            if (answerField.text == "" )
            {
                StartCoroutine(errorMesage());
                theQuestion.errorText = ("answer must not exceed ") ;
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
        destroyGlass = true;

        if (stage == 1)
        {
            theManagerOne.showProblem();
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
            directorIsCalling = false;
            destroyGlass = false;
             thePrisoner.ragdollReady = true;
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
