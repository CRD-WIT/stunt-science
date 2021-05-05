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
    public Hellicopter theChopper;
    public AccMediumOne theManagerOne;
    //public AccMediumTwo theManagerTwo;
    //public AccMediumThree theManagerThree;
    public TMP_InputField answerField;
    public static string question;
    public TMP_Text questionTextBox, errorTextBox, levelText, diretorsSpeech;
    public static float playerAnswer;
    public static bool simulate;
    public static bool playerDead;
    public int stage;
    
    public Vector2 ChopperStartPosition;
    public Vector2 TruckStartPosition;

    public GameObject afterStuntMessage, retryButton, nextButton,subChopper;
    public GameObject[] ground;
    bool directorIsCalling;
    public GameObject directorBubble;
    
    Vector2 playerstartPos;
    private Vector2 truckStartPos;
    private Vector2 chopperStartpos;
    private Quaternion TruckStartRot;
    //string accelaration;
    // Start is called before the first frame update
    void Start()
    {
        truckStartPos = theTruck.transform.position;
        TruckStartRot = theTruck.transform.rotation;
        chopperStartpos = theChopper.transform.position;      
        TruckStartPosition = theTruck.transform.position;
        ChopperStartPosition = theChopper.transform.position;
        PlayerPrefs.SetString("CurrentString", ("AccelarationMedium"));
        PlayerPrefs.SetInt("level", 4);
        playerstartPos = thePlayer.transform.position;
       
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        questionTextBox.SetText(question);

    }
    public void PlayButton()
    {
        playerAnswer = float.Parse(answerField.text);
        subChopper.SetActive(false);
        if (stage == 1)
        {
            if (answerField.text == "" || playerAnswer > 200 || playerAnswer < 1)
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
        subChopper.SetActive(true);
        playButton.interactable = true;
        playerAnswer = 0;
        answerField.text = ("");
        retryButton.SetActive(false);
        simulate = false;

        
        if (stage == 1)
        {
            theManagerOne.generateProblem();
            theChopper.transform.position = chopperStartpos;
            theTruck.transform.position = truckStartPos;
            theTruck.transform.rotation = TruckStartRot;
            thePlayer.transform.position = playerstartPos;
            theTruck.accelaration = 0;
            theTruck.accelerating = true;
            theChopper.flySpeed = 0;
            thePlayer.standup = false;
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
