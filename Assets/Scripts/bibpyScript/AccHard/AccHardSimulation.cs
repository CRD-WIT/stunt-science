using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccHardSimulation : MonoBehaviour
{
    public Button playButton;
    public TMP_InputField answerField;
    public GameObject directorBubble;
    public TruckManager theTruck;
    public AccHardOne theManagerOne;
    public AccHardTwo theManagerTwo;
    public AccHardThree theManagerThree;
    public HeartManager theHeart;
    public static float playerAnswer;
    public static bool simulate;
    public int stage;
    public QuestionControllerC theQuestion;
    bool directorIsCalling;
    public TMP_Text diretorsSpeech;
    private Vector2 truckStartPoint;
    public bool posCheck, answerIsCorrect;
    public string take;
    public int takeNumber;
    public AudioSource lightsSfx,cameraSfx,actionSfx,cutSfx;
    public AudioSource chopperEngine, truckIdle,truckRunning,GunShot;
    public GameObject indicator, hit, missed;



    // Start is called before the first frame update
    void Start()
    {
        theHeart = FindObjectOfType<HeartManager>();
        truckStartPoint = theTruck.transform.position;
        takeNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (takeNumber == 1)
        {
            take = "One";
        }
        if (takeNumber == 2)
        {
            take = "Two";
        }
        if (takeNumber == 3)
        {
            take = "three";
        }
        if(answerIsCorrect)
        {
            hit.SetActive(true);
            missed.SetActive(false);
        }
        else
        {
            hit.SetActive(false);
            missed.SetActive(true);
        }
    }
    public void PlayButton()
    {



        if (stage == 1)
        {
            playerAnswer = theQuestion.GetPlayerAnswer();
            if (answerField.text == "" || playerAnswer > 4)
            {

                theQuestion.errorText = ("answer must not exceed 4 seconds");
                StartCoroutine(errorMesage());
            }
            else
            {
                theQuestion.isSimulating = true;
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField.text = playerAnswer.ToString() + " s";
                }

            }
        }
        if (stage == 2)
        {
            playerAnswer = float.Parse(answerField.text);

            if (answerField.text == "" || playerAnswer > 100)
            {

                theQuestion.errorText = ("answer must not exceed 100 m/s");
                StartCoroutine(errorMesage());
            }
            else
            {
                theQuestion.isSimulating = true;
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField.text = playerAnswer.ToString() + " m/s";
                }

            }
        }
        if (stage == 3)
        {
            playerAnswer = float.Parse(answerField.text);
            if (answerField.text == "" || playerAnswer > 20.98)
            {
                //StartCoroutine(theManagerThree.errorMesage());
                theQuestion.errorText = ("answers must not exceeds 20 m/s²");
                StartCoroutine(errorMesage());
            }
            else
            {
                theQuestion.isSimulating = true;
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField.text = playerAnswer.ToString() + " m/s²";
                }

            }
        }
    }
    public void retry()
    {

        answerIsCorrect = false;
        playerAnswer = 0;
        simulate = false;
        answerField.text = ("");
        theHeart.losslife = false;
        if (stage == 1)
        {
            StartCoroutine(theManagerOne.positioningTwo());



        }
        if (stage == 2)
        {
            StartCoroutine(theManagerTwo.positioningTwo());

        }
        if (stage == 3)
        {
            StartCoroutine(theManagerThree.positioningTwo());

        }
    }
    public void next()
    {
        answerIsCorrect = false;
        playerAnswer = 0;
        if (stage == 1)
        {
            theManagerOne.gameObject.SetActive(false);
            theManagerTwo.gameObject.SetActive(true);
            //theQuestion.isSimulating = false;
            playButton.interactable = true;
        }
        if (stage == 2)
        {
            theManagerTwo.gameObject.SetActive(false);
            theManagerThree.gameObject.SetActive(true);
            //theQuestion.isSimulating = false;
            playButton.interactable = true;
        }
    }
    public IEnumerator DirectorsCall()
    {
        if (directorIsCalling)
        {
            directorBubble.SetActive(true);
            // diretorsSpeech.text = "Take " + take + ("!");
            // yield return new WaitForSeconds(0.75f);
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
            simulate = true;
            truckIdle.Stop();
            truckRunning.Play();
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
    public IEnumerator errorMesage()
    {
        theQuestion.popupVisible = true;
        yield return new WaitForSeconds(3);
        theQuestion.popupVisible = false;
    }
    public void action()
    {
        indicator.SetActive(false);
        //theQuestion.ToggleModal();
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
    public void answerLimiter()
    {

        string[] num;
        num = answerField.text.Split('.');
        answerField.characterLimit = num[0].Length + 3;
    }
}
