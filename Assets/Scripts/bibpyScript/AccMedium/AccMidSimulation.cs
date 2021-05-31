using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccMidSimulation : MonoBehaviour
{
    public Button playButton;
    public Player thePlayer;
    public TruckManager theTruck;
    public Suv [] theSuv;
    public SubSuv[] theSubVan;
    public AccMediumOne theManagerOne;
    //public AccMediumTwo theManagerTwo;
    public AccMediumThree theManagerThree;
    public TMP_InputField[] answerField;
    public static string question;
    public TMP_Text questionTextBox, errorTextBox, levelText, diretorsSpeech;
    public static float playerAnswer;
    public static bool simulate;
    public static bool playerDead;
    public int stage;
    public QuestionController[] theQuestion;

    
    public GameObject[] ground, dimension, arrow;
    bool directorIsCalling;
    public GameObject directorBubble;
    
    Vector2 playerstartPos;
    private Vector2 truckStartPos;
    

    private Quaternion TruckStartRot;
    //string accelaration;
    // Start is called before the first frame update
    void Start()
    {
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
    public void PlayButton()
    {
        
        
    
        if (stage == 1)
        {
            playerAnswer = float.Parse(answerField[0].text);
            //subChopper[0].SetActive(false);
            dimension[0].SetActive(false);
            if (answerField[0].text == "" || playerAnswer > 10|| playerAnswer < 1)
            {
                StartCoroutine(theManagerOne.errorMesage());
                theQuestion[0].errorText = ("believe me! its too long!");
            }
            else
            {
                theQuestion[0].isSimulating = true;
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField[0].text = playerAnswer.ToString() + "s";
                }

            }
        }
        if (stage == 2)
        {
            
            if (answerField[1].text == "" || playerAnswer > 100)
            {
                errorTextBox.SetText("Please enter a valid answer!");
            }
            else
            {
                theQuestion[1].isSimulating = true;
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField[1].text = playerAnswer.ToString() + "sec";
                }

            }
        }
        if (stage == 3)
        {
            playerAnswer = float.Parse(answerField[2].text);
            if (answerField[2].text == "" || playerAnswer > 16)
            {
                errorTextBox.SetText("exceed the average car acceleratoin");
            }
            else
            {
                theQuestion[2].isSimulating = true;
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField[2].text = playerAnswer.ToString() + "m/s²";
                }

            }
        }
    }
    public void retry()
    {  
        playButton.interactable = true;
        playerAnswer = 0;
        simulate = false;

        
        if (stage == 1)
        {
            answerField[0].text = ("");
            theQuestion[0].isSimulating = false;
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
            
        }
        if (stage == 3)
        {
            answerField[2].text = ("");
            theQuestion[2].isSimulating = false;
            theSubVan[1].fade = false;
            theSuv[1].myCollider.enabled = true;
            theSubVan[1].gameObject.SetActive(true);
            theManagerThree.generateProblem();
        }
    }
    public void next()
    {
        if(stage == 1)
        {
            theManagerOne.gameObject.SetActive(false);
            ground[0].SetActive(false);
            ground[1].SetActive(true);
            
            
        }
         if(stage == 2)
        {
            ground[1].SetActive(false);
            ground[2].SetActive(true);
           
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
    
    
}
