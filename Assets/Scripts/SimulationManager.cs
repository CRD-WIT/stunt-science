using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimulationManager : MonoBehaviour
{
    public GameObject transition, directorsBubble;
    public GameObject jumpers;
    public GameObject ragdollSpawn;
    public VelocityEasyStage1 VelocityEasyStage1;
    public StageTwoManager theManager2;


    public Player thePlayer;
    //public GameObject PlayerObject;
    public Button answerButton, retryButton, nextButton;
    public TMP_InputField answerField;
    public TMP_Text questionTextBox, errorTextBox, diretorsSpeech, levelText;
    public static string question;
    public static float playerAnswer;
    public static bool isSimulating, isAnswerCorrect, directorIsCalling, isStartOfStunt;
    int stage;
    public static bool playerDead;
    public bool destroyPrefab;


    StageManager sm = new StageManager();
    // Start is called before the first frame update
    void Start()
    {
        stage = 1;
        thePlayer = FindObjectOfType<Player>();

    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        levelText.text = sm.GetGameLevel();
        questionTextBox.SetText(question);
        if (isAnswerCorrect)
        {
            retryButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);
        }
        else
        {
            retryButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
        }

        if (directorIsCalling)
        {
            StartCoroutine(DirectorsCall());
        }
        else
        {
            directorIsCalling = false;
        }
    }

    public void PlayButton()
    {
        //string errorMessage = answerField.text != "" ? "":"Please enter a value";

        if (answerField.text == "")
        {
            errorTextBox.SetText("Please enter your answer!");
        }
        else
        {
            isStartOfStunt = true;
            directorIsCalling = true;
            //answerField.placeholder = playerAnswer.ToString()+"m/s";
            playerAnswer = float.Parse(answerField.text);
            answerField.text = playerAnswer.ToString() + "m/s";
            answerButton.interactable = false;
        }
    }
    public IEnumerator DirectorsCall()
    {
        directorIsCalling = false;
        if (isStartOfStunt)
        {
            directorsBubble.SetActive(true);
            diretorsSpeech.text = "Lights!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Camera!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Action!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "";
            directorsBubble.SetActive(false);
            isSimulating = true;
        }
        else
        {
            directorsBubble.SetActive(true);
            diretorsSpeech.text = "Cut!";
            yield return new WaitForSeconds(0.75f);
            directorsBubble.SetActive(false);
            diretorsSpeech.text = "";
        }
    }
    public void RetryButton()
    {
        answerField.text = "";
        answerButton.interactable = true;
        StartCoroutine(resetPrefab());
        if (stage == 1)
        {
            VelocityEasyStage1.VelocityEasyStage1SetUp();
        }
        else if (stage == 2)
        {
            theManager2.reset();
        }
        else
        {
        }
        thePlayer.gameObject.SetActive(true);
    }
    public void NextButton()
    {
        jumpers.SetActive(true);
        thePlayer.SetEmotion("");
        ragdollSpawn.SetActive(false);
        StartCoroutine(resetPrefab());
        if (stage == 1)
        {
            stage = 2;
            StartCoroutine(ExitStage());
            VelocityEasyStage1.gameObject.SetActive(false);
            theManager2.gameObject.SetActive(true);
        }
        else if (stage == 2)
        {
            stage = 3;
        }
    }
    IEnumerator ExitStage()
    {
        VelocityEasyStage1.AfterStuntMessage.SetActive(false);
        thePlayer.moveSpeed = 3;
        yield return new WaitForSeconds(3);
        transition.SetActive(true);
        yield return new WaitForSeconds(1);
        thePlayer.moveSpeed = 0;
        yield return new WaitForSeconds(0.5f);
        transition.SetActive(false);
        thePlayer.transform.position = new Vector2(0f, thePlayer.transform.position.y);
        if (stage == 2)
        {
            theManager2.generateProblem();
        }
        if (stage == 3)
        {

        }
        answerField.text = "";
        answerButton.interactable = true;
        playerAnswer = 0;

    }
    IEnumerator resetPrefab()
    {
        destroyPrefab = true;
        yield return new WaitForEndOfFrame();
        destroyPrefab = false;
    }




}
