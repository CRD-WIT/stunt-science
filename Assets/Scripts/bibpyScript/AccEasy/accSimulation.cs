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
    public AccManagerThree theManagerThree;
    public TMP_InputField answerField;
    public static string question;
    public TMP_Text diretorsSpeech;
    public static float playerAnswer;
    public static bool simulate;
    public static bool playerDead;
    public int stage;
    public Quaternion startRotation;
    public Vector2 startPosition;

    public GameObject driver, truck, player, director;
    public GameObject[] ground;
    bool directorIsCalling;
    public GameObject directorBubble;

    private ragdollScript theRagdoll;
    Vector2 playerstartPos;
    public QuestionControllerB theQuestion;
    public HeartManager theHeart;
    //string accelaration;
    // Start is called before the first frame update
    void Start()
    {
        simulate = false;
        //accelaration = "accelaration";
        theBike = FindObjectOfType<BikeManager>();
        //stage = 1;
        startRotation = theBike.transform.rotation;
        startPosition = theBike.transform.position;
        PlayerPrefs.SetString("CurrentString", ("Accelaration"));
        PlayerPrefs.SetInt("level", 3);
        playerstartPos = player.transform.position;
        theBike.moveSpeed = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        theRagdoll = FindObjectOfType<ragdollScript>();
        playerAnswer = float.Parse(answerField.text);
    }
    
    public void PlayButton()
    {
        
        
       
        if (stage == 1)
        {
           
            if (answerField.text == "" || playerAnswer > 10 || playerAnswer < 1)
            {
                theQuestion.errorText = ("exceed the maximum accelaration of motorcycles");
                StartCoroutine(errorMesage());
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
        if (stage == 2)
        {
           
            if (answerField.text == "" || playerAnswer > 100)
            {
                theQuestion.errorText = ("Please enter a valid answer!");
            }
            else
            {
                theQuestion.isSimulating = true;
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
            
            if (answerField.text == "" || playerAnswer > 70)
            {
                theQuestion.errorText = ("Please enter a valid answer!");
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
        
        StartCoroutine(exit());
       
    }
    public void next()
    {
        StartCoroutine(entrance());
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
    IEnumerator entrance()
    {
        StartCoroutine(theHeart.endBGgone());
        yield return new WaitForSeconds(1);
        StartCoroutine(theHeart.endBGgone());
        theQuestion.isSimulating = false;
        answerField.text = ("");
        playButton.interactable = true;
        theQuestion.answerIsCorrect = false;
        if(stage == 1)
        {
            theManagerOne.gameObject.SetActive(false);
            theManagerTwo.gameObject.SetActive(true);
            ground[0].SetActive(false);
            ground[1].SetActive(true);
           
            //director.transform.position = new Vector2(-1.31f, 4.98f);
            
            
        }
         if(stage == 2)
        {
            theManagerTwo.gameObject.SetActive(false);
            theManagerThree.gameObject.SetActive(true);
            ground[1].SetActive(false);
            ground[2].SetActive(true);
            truck.SetActive(true);
            theBike.brake = false;
            
            //director.transform.position = new Vector2(1.1f, 4.98f);
           
        }
    }
    IEnumerator exit()
    {
        StartCoroutine(theHeart.endBGgone());
        yield return new WaitForSeconds(1.2f);
        theQuestion.answerIsCorrect = false;
        theQuestion.isSimulating = false;
        theBike.transform.rotation = startRotation;
        theBike.moveSpeed = 0;
        driver.SetActive(true);
        theBike.stopBackward = false;
        theBike.stopForward = false;
        playButton.interactable = true;
        playerAnswer = 0;
        answerField.text = ("");
        player.transform.position = playerstartPos;
        if(theBike.collided == true)
        {
            //Destroy(theRagdoll.gameObject);
            theBike.collided = false;
        }
         if (stage == 3)
        {
            theManagerThree.generateProblem();
            theManagerThree.timer = 0;
            theBike.brake = false;
            theBike.transform.position = new Vector2(-15, 0.2f);
            
        }
        if (stage == 2)
        {
            
            theManagerTwo.timer = 0;
            theBike.brake = false;
            theBike.transform.position = new Vector2(-10, 0.2f);
            
            theManagerTwo.generateProblem();
        }
        if (stage == 1)
        {

            theManagerOne.generateProblem();
            theManagerOne.gas = true;
            theManagerOne.timer = 0;
            theBike.transform.position = startPosition;
            theManagerOne.walls.SetActive(false);
            
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
        if(theQuestion.answerIsCorrect == false)
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
        int I = stage - 1;
        string[] num;
        num = answerField.text.Split('.');
        answerField.characterLimit = num[0].Length + 3;
    }

}
