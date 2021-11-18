using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForceSimulation : MonoBehaviour
{
    public Button playButton;
    public PlayerB thePlayer;
    public QuestionContForces theQuestion;
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
    public GameObject[] glassHolder, otherGlassHolder;
    public Quaternion startRotation;
    public Vector2 startPosition;

    public GameObject  fadeOut, fadeIn;
    public GameObject[] ground;
    bool directorIsCalling;
    public GameObject directorBubble,zombiePrefab;
    private ragdollScript theRagdoll;
    public bool zombieChase, destroyZombies;

    //string accelaration;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("CurrentString", ("Forces"));
        PlayerPrefs.SetInt("level", 4);
        theHeart = FindObjectOfType<HeartManager>();
        StartCoroutine(theManagerOne.createZombies());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        theRagdoll = FindObjectOfType<ragdollScript>();

    }
    public void PlayButton()
    {
       
        if (stage == 1)
        {
            playerAnswer = float.Parse(answerField.text);
            if (answerField.text == "" || playerAnswer < 10 || playerAnswer > 1000)
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

       StartCoroutine(theHeart.startBGgone());
        StartCoroutine(nextStage());
        
       
    }
    public void retry()
    {
         destroyZombies = true;
        StartCoroutine(theHeart.startBGgone());
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
            StartCoroutine(theManagerOne.createZombies());
            theManagerOne.GenerateProblem();
            theManagerOne.tooStrong = false;
            theManagerOne.tooWeak = false;
            thePlayer.transform.position = new Vector2(.16f, 3.86f);
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
        zombieChase = false;
        destroyZombies = true;
        yield return new WaitForSeconds(.1f);
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
        theQuestion.isModalOpen = false;
        if(theQuestion.answerIsCorrect == false)
        {
            destroyZombies = true;
            retry();
            
        }
        else
        {
            zombieChase = false;
            destroyZombies = true;
            next();
        }
    }
    
}
