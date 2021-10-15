using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjHardSimulation : MonoBehaviour
{
    public Button playButton;
    public TMP_InputField answerField;
    public GameObject directorBubble, trail, projectTrail;
    public ProjectileHardOne theManagerOne;
    public ProjectileHardTwo theManagerTwo;
    public ProjectileHardThree theManagerThree;
    public HeartManager theHeart;
    public static float playerAnswer;
    public static bool simulate;
    public int stage;
    public QuestionContProJHard theQuestion;
    bool directorIsCalling;
    public TMP_Text diretorsSpeech;
    public string take;
    public int takeNumber;
    public bool answerIsCorrect;
    public GameObject  hit, miss;
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
            if (answerField.text == "" || playerAnswer > 70)
            {
                //StartCoroutine(theManagerThree.errorMesage());
                theQuestion.errorText = ("exceeds the helicopter's fastest acceleration");
                
            }
            else
            {
                theQuestion.isSimulating = true;
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
        StartCoroutine(theHeart.startBGgone());
        playerAnswer = 0;
        simulate = false;
        answerField.text = ("");
        theQuestion.isSimulating = false;
        trail.GetComponent<TrailRenderer>().time = 0.05f;
        if (stage == 1)
        {
            theManagerOne.generateProblem();
        }
        if (stage == 2)
        {
            theManagerTwo.generateProblem();

        }
        if (stage == 3)
        {
            theManagerThree.generateProblem();
            
        }
    }
    public void next()
    {
        playerAnswer = 0;
        trail.GetComponent<TrailRenderer>().time = 0.05f;
        if (stage == 1)
        {
            theManagerOne.gameObject.SetActive(false);
            theManagerTwo.gameObject.SetActive(true);
            theQuestion.isSimulating = false;
            playButton.interactable = true;
            StartCoroutine(theManagerTwo.positioning());
        }
        if (stage == 2)
        {
            theManagerTwo.gameObject.SetActive(false);
            theManagerThree.gameObject.SetActive(true);
            theQuestion.isSimulating = false;
            playButton.interactable = true;
            StartCoroutine(theManagerThree.positioning());
        }
    }
    public IEnumerator DirectorsCall()
    {
        if (directorIsCalling)
        {
            directorBubble.SetActive(true);
            diretorsSpeech.text = "Take " + take + ("!");
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Lights!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Camera!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Action!";
            projectTrail.GetComponent<TrailRenderer>().time = 0;
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
        //theQuestion.ToggleModal();
        theHeart.startbgentrance();
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
