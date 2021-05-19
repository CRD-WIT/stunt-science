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
    public TMP_InputField answerField;
    public static string question;
    public TMP_Text questionTextBox, errorTextBox, levelText, diretorsSpeech;
    public static float playerAnswer;
    public static bool simulate;
    public static bool playerDead;
    public int stage;

    public GameObject afterStuntMessage, retryButton, nextButton;
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
        
        questionTextBox.SetText(question);
        if(stage == 3)
        {
            arrow[0].transform.localScale = new Vector2(-5, arrow[0].transform.localScale.y);
            arrow[1].transform.localScale = new Vector2(-5, arrow[1].transform.localScale.y);
        }

    }
    public void PlayButton()
    {
        playerAnswer = float.Parse(answerField.text);
        
    
        if (stage == 1)
        {
            //subChopper[0].SetActive(false);
            dimension[0].SetActive(false);
            if (answerField.text == "" || playerAnswer > 7|| playerAnswer < 1)
            {
                errorTextBox.SetText("believe me! its too long!");
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
        if (stage == 2)
        {
            if (answerField.text == "" || playerAnswer > 100)
            {
                errorTextBox.SetText("Please enter a valid answer!");
            }
            else
            {
                directorIsCalling = true;
                StartCoroutine(DirectorsCall());
                playButton.interactable = false;
                {
                    answerField.text = playerAnswer.ToString() + "sec";
                }

            }
        }
        if (stage == 3)
        {
            if (answerField.text == "" || playerAnswer > 100)
            {
                errorTextBox.SetText("Please enter a valid answer!");
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
        
        afterStuntMessage.SetActive(false);
        
        playButton.interactable = true;
        playerAnswer = 0;
        answerField.text = ("");
        retryButton.SetActive(false);
        simulate = false;

        
        if (stage == 1)
        {
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
            theSubVan[1].fade = false;
            theSuv[1].myCollider.enabled = true;
            theSubVan[1].gameObject.SetActive(true);
            theManagerThree.generateProblem();
        }
    }
    public void next()
    {
        nextButton.SetActive(false);
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
