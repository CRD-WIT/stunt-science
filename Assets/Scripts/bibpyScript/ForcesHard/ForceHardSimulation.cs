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
    public ForceHardManagerTwo theManagerTwo;
    public ForceHardManagerThree theManagerThree;
    // public ForceManagerHardThree theManagerThree;
    public PrisonerManager[] thePrisoner;
    public bool simulate;
    public float stage;
    public float playerAnswer;
    public TMP_InputField answerField;
    public TMP_Text diretorsSpeech;
    public QuestionContForcesMed theQuestion;
    bool directorIsCalling;
    public Button playButton;
    public GameObject directorBubble,weightBox;
    public GameObject[] box;
    public bool destroyGlass;
    public Vector2 playerStartPoint, zombie1StartPoint, zombie2StartPoint, boxStartPoint;
    public AudioSource dragSfx;
    public AudioSource lightsSfx,cameraSfx,actionSfx,cutSfx;
    
    //public GameObject dimensionOne, dimensionTwo,dimensionThree, groundOne,groundTwo;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("CurrentString", ("Forces"));
        PlayerPrefs.SetInt("level", 4);
        theHeart.life = 3;
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
            if (answerField.text == "" || playerAnswer > 600|| playerAnswer < (-theManagerOne.resultantDownhillForce)+50)
            {
                StartCoroutine(errorMesage());
                theQuestion.errorText = ("answer must not exceed 600N or not less than"+ ((-theManagerOne.resultantDownhillForce)+50).ToString("F2")+ "N ") ;
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
            if (answerField.text == ""|| playerAnswer > 8 )
            {
                StartCoroutine(errorMesage());
                theQuestion.errorText = ("answer must not exceed 8 sec!") ;
            }
            else
            {
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField.text = playerAnswer.ToString() + "sec";
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
        destroyGlass = true;

        if (stage == 1)
        {
            theManagerOne.showProblem();
        }
        if (stage == 2)
        {    
            theManagerTwo.showProblem();
        }
        if (stage == 3)
        {
            theManagerThree.showProblem();
            weightBox.SetActive(true);
        }

    }
    public IEnumerator DirectorsCall()
    {
        if (directorIsCalling)
        {
            directorBubble.SetActive(true);
            diretorsSpeech.text = "Lights!";
            lightsSfx.Play();
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Camera!";
             cameraSfx.Play();
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Action!";
             actionSfx.Play();
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "";
            directorBubble.SetActive(false);
            if(stage == 3)
            {
                thePlayer.fillWeight = true;
                yield return new WaitForSeconds(3f);
                thePlayer.fillWeight = false;
                weightBox.SetActive(false);
                dragSfx.Play();
            }
            simulate = true;  
            directorIsCalling = false;
            destroyGlass = false;
            if(stage == 2)
            {
                dragSfx.Play();
                StartCoroutine(theManagerTwo.zombieChase());
            }
            if(stage == 1)
            {
                dragSfx.Play();
            }
        }
        else
        {
            directorBubble.SetActive(true);
            diretorsSpeech.text = "Cut!";
            cutSfx.Play();
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
            
            theManagerTwo.gameObject.SetActive(false);
            theManagerThree.gameObject.SetActive(true);
            // thePlayer.transform.position = new Vector2(19.32f, -.22f);
            thePlayer.transform.rotation = Quaternion.Euler(0,0,20);
            // destroyGlass = true;
            // box[0].SetActive(false);
            // StartCoroutine(thePrisoner.startRun());
            stage = 3;
        }
        if(stage == 1)
        {
            
            theManagerOne.gameObject.SetActive(false);
            theManagerTwo.gameObject.SetActive(true);
            thePlayer.transform.position = new Vector2(19.32f, -.22f);
            thePlayer.transform.rotation = Quaternion.Euler(0,0,0);
            destroyGlass = true;
            box[0].SetActive(false);
            StartCoroutine(thePrisoner[0].startRun());
            stage = 2;

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
