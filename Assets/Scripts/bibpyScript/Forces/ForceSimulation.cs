using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForceSimulation : MonoBehaviour
{
    public Button playButton;
    public Player thePlayer;
    public ForceManagerOne theManagerOne;
    public ForceManagerTwo theManagerTwo;
    public ForceManagerThree theManagerThree;
    public TMP_InputField answerField;
    public static string question;
    public TMP_Text questionTextBox, errorTextBox, levelText, diretorsSpeech;
    public static float playerAnswer;
    public static bool simulate;
    public bool playerDead;
    public int stage;
    public Quaternion startRotation;
    public Vector2 startPosition;

    public GameObject afterStuntMessage, retryButton, nextButton;
    public GameObject[] ground;
    bool directorIsCalling;
    public GameObject directorBubble;
    private ragdollScript theRagdoll;
    //string accelaration;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("CurrentString", ("Forces"));
        PlayerPrefs.SetInt("level", 4);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        theRagdoll = FindObjectOfType<ragdollScript>();
        questionTextBox.SetText(question);

    }
    public void PlayButton()
    {
        playerAnswer = float.Parse(answerField.text);
        if (stage == 1)
        {
            if (answerField.text == "" || playerAnswer < 200 || playerAnswer < 1)
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
        thePlayer.standup = false;
        simulate = false;
        afterStuntMessage.SetActive(false);
        playButton.interactable = true;
        playerAnswer = 0;
        answerField.text = ("");
        retryButton.SetActive(false);
        thePlayer.brake = false;
        thePlayer.moveSpeed = 0;
        thePlayer.gameObject.SetActive(true);
        thePlayer.transform.position = new Vector2(1, -0.6f);
        if(stage == 1)
        {
                
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
