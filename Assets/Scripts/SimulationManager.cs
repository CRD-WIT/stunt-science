using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimulationManager : MonoBehaviour
{
    public GameObject directorsBubble;
    //public GameObject jumpers;
    public GameObject ragdollSpawn;
    public VelocityEasyStage1 VelocityEasyStage1;
    public StageTwoManager theManager2;

    public StageThreeManager StageThreeManager;
    public Player thePlayer;
    //public GameObject PlayerObject;
    public Button answerButton, retryButton, nextButton;
    public TMP_InputField answerField;
    public TMP_Text questionTextBox, errorTextBox, diretorsSpeech, levelText;
    public static string question;
    public static float playerAnswer;
    public static bool isSimulating, isAnswerCorrect, directorIsCalling, isStartOfStunt, playerDead;
    public static int stage;
    public bool destroyPrefab;
    private HeartManager theHeart;


    StageManager sm = new StageManager();
    // Start is called before the first frame update
    void Start()
    {
        stage = 1;
        thePlayer = FindObjectOfType<Player>();
        theHeart = FindObjectOfType<HeartManager>();
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
        if (answerField.text == "")
        {
            errorTextBox.SetText("Please enter your answer!");
        }
        else
        {
            isStartOfStunt = true;
            directorIsCalling = true;
            playerAnswer = float.Parse(answerField.text);
            answerButton.interactable = false;
            if (stage == 1)
            {
                answerField.text = playerAnswer.ToString() + "m/s";
            }
            else if (stage == 2)
            {
                answerField.text = playerAnswer.ToString() + "s";
            }
            else
            {
                answerField.text = playerAnswer.ToString() + "m";
            }
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
            RumblingManager.shakeON = false;
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
            StageThreeManager.Stage3SetUp();
        }
        thePlayer.gameObject.SetActive(true);
    }
    public void NextButton()
    {
        //jumpers.SetActive(true);
        thePlayer.SetEmotion("");
        ragdollSpawn.SetActive(false);
        StartCoroutine(resetPrefab());
        if (stage == 1)
        {
            stage = 2;
            VelocityEasyStage1.gameObject.SetActive(false);
            theManager2.gameObject.SetActive(true);
        }
        else if (stage == 2)
        {
            stage = 3;
            VelocityEasyStage1.gameObject.SetActive(false);
            theManager2.gameObject.SetActive(false);
            StageThreeManager.gameObject.SetActive(true);
        }
        StartCoroutine(ExitStage());
    }
    IEnumerator ExitStage()
    {
        VelocityEasyStage1.AfterStuntMessage.SetActive(false);
        thePlayer.moveSpeed = 5;
        yield return new WaitForSeconds(3f);
        StartCoroutine(theHeart.endBGgone());
        yield return new WaitForSeconds(2.8f);
        thePlayer.transform.position = new Vector2(0f, thePlayer.transform.position.y);
        thePlayer.moveSpeed = 0;
        if (stage == 2)
        {
            theManager2.generateProblem();
        }
        if (stage == 3)
        {
            StageThreeManager.Stage3SetUp();
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
