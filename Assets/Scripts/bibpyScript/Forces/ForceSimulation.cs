using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForceSimulation : MonoBehaviour
{
    public Button playButton;
    public Player thePlayer;
    public QuestionController theQuestion;
    public ForceManagerOne theManagerOne;
    public ForceManagerTwo theManagerTwo;
    public ForceManagerThree theManagerThree;
    private HeartManager theHeart;
    public TMP_InputField answerField;
    public TMP_Text diretorsSpeech;
    public static float playerAnswer;
    public static bool simulate;
    public bool playerDead;
    public int stage;
    public Quaternion startRotation;
    public Vector2 startPosition;

    public GameObject  fadeOut, fadeIn;
    public GameObject[] ground;
    bool directorIsCalling;
    public GameObject directorBubble;
    private ragdollScript theRagdoll;
    //string accelaration;
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
        theRagdoll = FindObjectOfType<ragdollScript>();

    }
    public void PlayButton()
    {
        playerAnswer = float.Parse(answerField.text);
        if (stage == 1)
        {
            if (answerField.text == "" || playerAnswer < 200 || playerAnswer < 1)
            {
                StartCoroutine(errorMesage());
               theQuestion.errorText = ("Please enter a valid answer!");
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
            if (answerField.text == "" || playerAnswer > 100)
            {
                StartCoroutine(errorMesage());
                theQuestion.errorText = ("too heavy for you!");
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

        StartCoroutine(theHeart.endBGgone());
        StartCoroutine(nextStage());
       
    }
    public void retry()
    {
        thePlayer.standup = false;
        simulate = false;
        playButton.interactable = true;
        playerAnswer = 0;
        answerField.text = ("");
        thePlayer.brake = false;
        thePlayer.moveSpeed = 0;
        thePlayer.gameObject.SetActive(true);
        theHeart.losslife = false;

        if (stage == 1)
        {
            theManagerOne.GenerateProblem();
            theManagerOne.tooStrong = false;
            theManagerOne.tooWeak = false;
            thePlayer.transform.position = new Vector2(0, -0.6f);
        }
        if (stage == 2)
        {
            theManagerTwo.GenerateProblem();
            theManagerTwo.tooStrong = false;
            theManagerTwo.tooWeak = false;
            
        }
         if (stage == 3)
        {
            theManagerThree.GenerateProblem();
            theManagerThree.tooStrong = false;
            theManagerThree.tooWeak = false;
            
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
        yield return new WaitForSeconds(2.5f);
        playButton.interactable = true;
        answerField.text = ("");
        if(stage == 2)
        {
            stage = 3;
            ground[1].SetActive(false);
            ground[2].SetActive(true);
            theManagerTwo.gameObject.SetActive(false);
            theManagerThree.gameObject.SetActive(true);
            //theManagerTwo.GenerateProblem();
            
        }
        if(stage == 1)
        {
            stage = 2;
            ground[0].SetActive(false);
            ground[1].SetActive(true);
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
        theQuestion.ToggleModal();
        if(theQuestion.answerIsCorrect == false)
        {
            retry();
            
        }
        else
        {
            next();
        }
    }
}
