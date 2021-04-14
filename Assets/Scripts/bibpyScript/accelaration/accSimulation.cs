using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class accSimulation : MonoBehaviour
{
    public Button playButton;
    private BikeManager theBike;
    public AccManagerOne theManagerOne;
    public AccManagerTwo theManagerTwo;
    public TMP_InputField answerField;
    public static string question;
    public TMP_Text questionTextBox, errorTextBox, levelText, diretorsSpeech;
    public static float playerAnswer;
    public static bool simulate;
    public static bool playerDead;
    public int stage;
    public Quaternion startRotation;
    public Vector2 startPosition;

    public GameObject driver, afterStuntMessage;
    bool directorIsCalling;
    public GameObject directorBubble;
    // Start is called before the first frame update
    void Start()
    {
        theBike = FindObjectOfType<BikeManager>();
        //stage = 1;
        startRotation = theBike.transform.rotation;
        startPosition = theBike.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        questionTextBox.SetText(question);

    }
    public void PlayButton()
    {
        playerAnswer = float.Parse(answerField.text);
        if (stage == 1)
        {
            if (answerField.text == "" || playerAnswer > 200)
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
                    answerField.text = playerAnswer.ToString() + "sec";
                }

            }
        }
    }
    public void retry()
    {
        theBike.transform.rotation = startRotation;
        theBike.moveSpeed = 0;
        driver.SetActive(true);
        afterStuntMessage.SetActive(false);
        theBike.stopBackward = false;
        theBike.collided = false;
        playButton.interactable = true;
        playerAnswer = 0;
        answerField.text = ("");
        if (stage == 1)
        {
            theManagerOne.generateProblem();
            theManagerOne.gas = true;
            theManagerOne.timer = 0;
            theBike.transform.position = startPosition;
        }
        if (stage == 2)
        {
            theManagerTwo.generateProblem();
            theManagerTwo.timer = 0;
            theBike.brake = false;
            theBike.transform.position = new Vector2(-10, 0.1f);
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
            yield return new WaitForSeconds(0.75f);
            directorBubble.SetActive(false);
            diretorsSpeech.text = "";
        }
    }
}
