using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjSimulationManager : MonoBehaviour
{
    public Button playButton;
    public TMP_InputField answerField;
    public GameObject directorBubble;
    public ProjectileHardOne theManagerOne;
    public ProjectileHardTwo theManagerTwo;
    public ProjectileHardThree theManagerThree;
    public HeartManager theHeart;
    public static float playerAnswer;
    public static bool simulate;
    public int stage;
    public QuestionControllerC theQuestion;
    bool directorIsCalling;
    public TMP_Text diretorsSpeech;
    public string take;
    public int takeNumber;
    public bool answerIsCorrect;
    // Start is called before the first frame update
    void Start()
    {
        stage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(takeNumber == 1)
        {
            take = "One";
        }
        if(takeNumber == 2)
        {
            take = "Two";
        }
        if(takeNumber == 3)
        {
            take = "three";
        }
        
    }
    public void PlayButton()
    {
        if (stage == 1)
        {
           playerAnswer = theQuestion.GetPlayerAnswer();
            if (answerField.text == "" || playerAnswer > 50 || playerAnswer < 1)
            {
               
                theQuestion.errorText = ("believe me! its too long!");
                StartCoroutine(theManagerOne.errorMesage());
            }
            else
            {
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField.text = playerAnswer.ToString() + "s";
                }

            }
        }
        if (stage == 2)
        {
            playerAnswer = float.Parse(answerField.text);

            if (answerField.text == "" || playerAnswer > 111.67)
            {
                
                theQuestion.errorText = ("fastest helicopter flies at 111.67 m/s only");
                 StartCoroutine(errorMesage());
            }
            else
            {
                theQuestion.isSimulating = true;
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField.text = playerAnswer.ToString() + "m/s";
                }

            }
        }
        if (stage == 3)
        {
            playerAnswer = float.Parse(answerField.text);
            if (answerField.text == "" || playerAnswer > 20.98)
            {
                //StartCoroutine(theManagerThree.errorMesage());
                theQuestion.errorText = ("exceeds the helicopter's fastest acceleration");
                
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
    }
    public void retry()
    {
        
        playerAnswer = 0;
        simulate = false;
        answerField.text = ("");
        theQuestion.isSimulating = false;
        if (stage == 1)
        {
            theManagerOne.generateProblem();



        }
        if (stage == 2)
        {
            //StartCoroutine(theManagerTwo.positioningTwo());

        }
        if (stage == 3)
        {
            //StartCoroutine(theManagerThree.positioningTwo());
            
        }
    }
    public void next()
    {
        playerAnswer = 0;
        if (stage == 1)
        {
            theManagerOne.gameObject.SetActive(false);
            theManagerTwo.gameObject.SetActive(true);
            theQuestion.isSimulating = false;
            playButton.interactable = true;
        }
        if (stage == 2)
        {
            theManagerTwo.gameObject.SetActive(false);
            theManagerThree.gameObject.SetActive(true);
            theQuestion.isSimulating = false;
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
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Camera!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Action!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "";
            directorBubble.SetActive(false);
            simulate = true;
            theQuestion.isSimulating = true;
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
        theHeart.startbgentrance();
        //theQuestion.ToggleModal();
        playButton.interactable = true;
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
