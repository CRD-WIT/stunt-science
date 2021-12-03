using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccMidSimulation : MonoBehaviour
{
    public Button playButton;
    public PlayerB thePlayer;
    public TruckManager theTruck;
    public Suv[] theSuv;
    public SubSuv[] theSubVan;
    public AccMediumOne theManagerOne;
    //public AccMediumTwo theManagerTwo;
    public AccMediumThree theManagerThree;
    public AccMediumTwo theManagerTwo;
    public TMP_InputField[] answerField;
    public TMP_Text questionTextBox, errorTextBox, levelText, diretorsSpeech;
    public static float playerAnswer;
    public static bool simulate;
    public static bool playerDead;
    public int stage;
    public QuestionControllerC theQuestion;

    public GameObject[] ground, dimension, arrow;
    bool directorIsCalling;
    public GameObject directorBubble;

    Vector2 playerstartPos;
    private Vector2 truckStartPos;


    private Quaternion TruckStartRot;
    private HeartManager theHeart;
    public AudioSource lightsSfx,cameraSfx,actionSfx,cutSfx;
    public AudioSource truckIdle,truckRunning,ChopperFlying;
    //string accelaration;
    // Start is called before the first frame update
    void Start()
    {
        simulate = false;
        theHeart = FindObjectOfType<HeartManager>();
        truckStartPos = theTruck.transform.position;
        TruckStartRot = theTruck.transform.rotation;
        PlayerPrefs.SetString("CurrentString", ("AccelarationMedium"));
        PlayerPrefs.SetInt("level", 4);
        playerstartPos = thePlayer.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    public void TriggerSimulation()
    {
        
        if (stage == 1)
        {
            playerAnswer = theQuestion.GetPlayerAnswer();
            //subChopper[0].SetActive(false);
            dimension[0].SetActive(false);
            if (playerAnswer > 10 || playerAnswer < 1)
            {
                StartCoroutine(theManagerOne.errorMesage());
                theQuestion.errorText = ("answer must not exceed between 1s to 10s");
            }
            else
            {              
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField[0].text = playerAnswer.ToString() + " s";
                }

            }
        }
        if (stage == 2)
        {
            playerAnswer = float.Parse(answerField[1].text);

            if (playerAnswer > 40)
            {
                StartCoroutine(theManagerTwo.errorMesage());
                theQuestion.errorText = ("answer must not exceed 40 meter");
            }
            else
            {
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField[1].text = playerAnswer.ToString() + " m";
                }

            }
        }
        if (stage == 3)
        {
            playerAnswer = float.Parse(answerField[2].text);
            if (answerField[2].text == "" || playerAnswer > 16)
            {
                StartCoroutine(theManagerThree.errorMesage());
                theQuestion.errorText = ("answers must not exceed 16 m/s²");
            }
            else
            {              
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField[2].text = playerAnswer.ToString() + " m/s²";
                }

            }
        }
    }
    public void retry()
    {
        playButton.interactable = true;
        theHeart.startbgentrance();
        playButton.interactable = true;
        playerAnswer = 0;
        simulate = false;
        truckRunning.Stop();
        ChopperFlying.Stop();


        if (stage == 1)
        {
            answerField[0].text = ("");
            theQuestion.isSimulating = false;
            dimension[0].SetActive(true);
            theTruck.transform.position = truckStartPos;
            theTruck.transform.rotation = TruckStartRot;
            thePlayer.transform.position = playerstartPos;
            theTruck.accelaration = 0;
            theTruck.accelerating = true;
            thePlayer.standup = false;
            theManagerOne.generateProblem();
        }
        if (stage == 2)
        {
            answerField[1].text = ("");
            theQuestion.isSimulating = false;
            theManagerTwo.generateProblem();
            thePlayer.standup = false;
            theSubVan[0].fade = false;

        }
        if (stage == 3)
        {
            answerField[2].text = ("");
            // theQuestion.isSimulating = false;
            theManagerThree.setAnswer = false;
            theSubVan[1].fade = false;
            theSuv[1].myCollider.enabled = true;
            theSubVan[1].gameObject.SetActive(true);
            theManagerThree.generateProblem();
        }
    }
    public void next()
    {
        truckRunning.Stop();
        ChopperFlying.Stop();
        playButton.interactable = true;
        if (stage == 1)
        {
            thePlayer.standup = false;
            theManagerOne.gameObject.SetActive(false);
            ground[0].SetActive(false);
            ground[1].SetActive(true);
            theManagerTwo.gameObject.SetActive(true);


        }
        if (stage == 2)
        {
            ground[1].SetActive(false);
            ground[2].SetActive(true);
            theManagerTwo.gameObject.SetActive(false);
            theManagerThree.gameObject.SetActive(true);
            thePlayer.ropeHang = false;

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
            theQuestion.isSimulating = true;
            truckIdle.Stop();
            truckRunning.Play();
            ChopperFlying.Play();
            directorIsCalling = false;
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


}
