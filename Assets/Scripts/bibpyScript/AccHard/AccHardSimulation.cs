using System.Collections;
using System.Collections.Generic;
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
    private HeartManager theHeart;
    public static float playerAnswer;
    public static bool simulate;
    public int stage;
    public QuestionController theQuestion;
    bool directorIsCalling;
    public TMP_Text diretorsSpeech;
    

    // Start is called before the first frame update
    void Start()
    {
        theHeart = FindObjectOfType<HeartManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public void PlayButton()
    {
        
        
    
        if (stage == 1)
        {
            playerAnswer = float.Parse(answerField.text);
            if (answerField.text == "" || playerAnswer > 10|| playerAnswer < 1)
            {
                //StartCoroutine(theManagerOne.errorMesage());
                theQuestion.errorText = ("believe me! its too long!");
            }
            else
            {
                theQuestion.isSimulating = true;
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
            
            if (answerField.text == "" || playerAnswer > 100)
            {
                //StartCoroutine(theManagerTwo.errorMesage());
                
            }
            else
            {
                theQuestion.isSimulating = true;
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField.text = playerAnswer.ToString() + "m";
                }

            }
        }
        if (stage == 3)
        {
            playerAnswer = float.Parse(answerField.text);
            if (answerField.text == "" || playerAnswer > 16)
            {
               //StartCoroutine(theManagerThree.errorMesage());
               theQuestion.errorText =("exceed the average car acceleratoin");
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
        theHeart.startbgentrance(); 
        playButton.interactable = true;
        playerAnswer = 0;
        simulate = false;
        answerField.text = ("");
        theQuestion.isSimulating = false;
        if (stage == 1)
        {
            
            
        }
        if (stage == 2)
        {
    
            
        }
        if (stage == 3)
        {
           
            
        }
    }
     public void next()
     {
         
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
     public IEnumerator errorMesage()
    {
        theQuestion.popupVisible = true;
        yield return new WaitForSeconds(3);
        theQuestion.popupVisible = false;
    }
    public void action()
    {
        theQuestion.ToggleModal();
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
