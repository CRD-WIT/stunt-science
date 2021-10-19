using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_3_Stage_1_Medium : MonoBehaviour
{
    // Start is called before the first frame update
    string question;
    public TMP_Text questionText, levelName, timerText;
    public Hook theHook;
    public GameObject AfterStuntMessage;
    Animator thePlayerAnimation;
    public GameObject HookAttachmentCollider, shootPosTriger, puller;
    public GameObject hook, trail;
    bool isSimulating = false;
    public GameObject thePlayer;
    Vector3 thePlayer_position;
    Vector2 gravityGiven;
    public GameObject hookLauncher;
    public GameObject thePlayerRunning;
    float distanceGiven;
    float angleGiven;
    public float correctAnswer;
    public GameObject movingToHookHand;
    float velocityX = 0;
    public GameObject thinRope;
    bool doneFiring = false;
    float velocityY = 0;
    float velocityInitial = 0;
    public GameObject hookLine;
    public GameObject Rope2HookEnd;
    bool isMovingToHook;
    public float elapsed;
    public GameObject dynamicPlatform;
    public GameObject grapplingPointIndicator;
    string playerName;
    string pronoun = "he";
    public QuestionContProjMed questionController;
    //public CameraScript cameraScript;
    public playerProjectileMed thePlayerProjMed;
    public GameObject directorBubble;
    bool directorIsCalling;
    public TMP_Text diretorsSpeech;
    public TMP_InputField answerField;
    static float playerAnswer;
    public Button playButton;
    public HeartManager theHeart;

    void Start()
    {
        // Given 
        theHeart.startbgentrance();           
        distanceGiven = (float)System.Math.Round(UnityEngine.Random.Range(22f, 25f), 2);
        angleGiven = (float)System.Math.Round(UnityEngine.Random.Range(40f, 45f), 2);
        gravityGiven = Physics2D.gravity;
        questionController.isModalOpen = false;


        //Problem
        question = $"{PlayerPrefs.GetString("Name")} is instructed to cross another diff using this climbing device. If {PlayerPrefs.GetString("Name")} shoot its gripping projectile at an angle of {angleGiven} degrees up to precisely get the corner of the cliff {distanceGiven} meters away horizontally from the shooting device, at what velocity should the projectile be shot to hit the gripping part?";

        questionController.SetQuestion(question);

        Annotation line = transform.Find("Annotation").GetComponent<Annotation>();

        line.SetDistance(distanceGiven);

        thePlayerAnimation = thePlayer.GetComponent<Animator>();
        thePlayer_position = thePlayer.transform.position;

        transform.Find("CircularAnnotation").GetComponent<CircularAnnotation>().SetAngle(angleGiven);

        transform.Find("AngularAnnotation").GetComponent<AngularAnnotation>().SetAngle(angleGiven);

        hook.GetComponent<Rigidbody2D>().Sleep();
        hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        hookLauncher.transform.Rotate(new Vector3(0, 0, angleGiven));

        GenerateInitialVelocity();
    }
    public IEnumerator DirectorsCall()
    {
        if (directorIsCalling)
        {
            directorBubble.SetActive(true);
            //diretorsSpeech.text = "Take " + take + ("!");
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Lights!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Camera!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "Action!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "";
            directorBubble.SetActive(false);
            questionController.isSimulating = true;
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
        questionController.popupVisible = true;
        yield return new WaitForSeconds(3);
        questionController.popupVisible = false;
    }
    IEnumerator StuntResult()
    {
        //messageFlag = false;
        yield return new WaitForSeconds(2f);
        if(playerAnswer == correctAnswer)
        {
            questionController.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and able to hit the target"), true, false);
        } 
         if(playerAnswer != correctAnswer)
        {
            questionController.ActivateResult((PlayerPrefs.GetString("Name") + " has unable to hit and grab the target"), false, false);
            theHeart.ReduceLife();
        }        
    }
    // void GenerateInitialVelocities()
    // {
    //     velocityX = Mathf.Sqrt(Mathf.Abs((distanceGiven * gravityGiven.y) / (2 * Mathf.Tan(angleGiven * Mathf.Deg2Rad))));

    //     velocityInitial = questionController.GetPlayerAnswer();
    //     velocityY = Mathf.Abs(velocityInitial * Mathf.Sin(angleGiven * Mathf.Deg2Rad));

    //     correctAnswer = Mathf.Abs((velocityX / Mathf.Cos(angleGiven * Mathf.Deg2Rad)));

    //     Debug.Log($"VelocityX: {velocityX}");
    //     Debug.Log($"VelocityY: {velocityY}");
    //     Debug.Log($"Correct Answer: {correctAnswer}");
    // }

    void GenerateInitialVelocity()
    {
        velocityX = Mathf.Sqrt(Mathf.Abs((distanceGiven * gravityGiven.y) / (2 * Mathf.Tan(angleGiven * Mathf.Deg2Rad))));
        correctAnswer = (float)System.Math.Round(Mathf.Abs((velocityX / Mathf.Cos(angleGiven * Mathf.Deg2Rad))), 2);
        Debug.Log($"Correct Answer: {correctAnswer}");
        trail.GetComponent<TrailRenderer>().time = 3000;
        theHook.isTrailing = true;
        thePlayerProjMed.aim = true;

    }

    void RegenerateVelocities()
    {

        velocityInitial = questionController.GetPlayerAnswer();
        velocityY = Mathf.Abs(velocityInitial * Mathf.Sin(angleGiven * Mathf.Deg2Rad));
        Debug.Log($"VelocityX: {velocityX}");
        Debug.Log($"VelocityY: {velocityY}");
        //hookLine.SetActive(true);
    }

    IEnumerator DropRope()
    {
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator PullRope()
    {
        yield return new WaitForSeconds(1.3f);
    }

    IEnumerator ShowAnnotations()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void StartSimulation()
    {

        playerAnswer = questionController.GetPlayerAnswer();
        if (answerField.text == "" || playerAnswer > 30 || playerAnswer < 1)
        {

            questionController.errorText = ("answer is not valid");
            StartCoroutine(errorMesage());
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
    public void action()
    {
        if (questionController.GetPlayerAnswer() == correctAnswer)
        {
            SceneManager.LoadScene("LevelThreeStage2Medium");
        }
        else
        {
            SceneManager.LoadScene("LevelThreeStage1Medium");
        }


    }


    void FixedUpdate()
    {
        hookLine.GetComponent<LineRenderer>().SetPosition(0, movingToHookHand.transform.position);
        hookLine.GetComponent<LineRenderer>().SetPosition(1, hook.transform.position);
        if (thePlayerRunning.GetComponent<RunningPlayer>().isCollided)
        {
            thePlayer.SetActive(true);
            thePlayerRunning.SetActive(false);
            transform.Find("Annotation").GetComponent<Annotation>().Show();
            transform.Find("CircularAnnotation").GetComponent<CircularAnnotation>().Show();
            transform.Find("AngularAnnotation").GetComponent<AngularAnnotation>().Show();
            shootPosTriger.SetActive(false);
            puller.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;


        }

        Annotation line = transform.Find("Annotation").GetComponent<Annotation>();
        dynamicPlatform.transform.position = new Vector3(line.distance + 32.34f, -15.69f, 1);
        grapplingPointIndicator.transform.position = new Vector3(line.distance + 4.4f, 1, 1);

        /*if (isMovingToHook && !isSimulating)
        {
            hookLine.GetComponent<LineRenderer>().SetPosition(0, movingToHookHand.transform.position);
            hookLine.GetComponent<LineRenderer>().SetPosition(1, hook.transform.position);
        }
        else
        {
            hookLine.GetComponent<LineRenderer>().SetPosition(0, hookLauncher.transform.position);
            hookLine.GetComponent<LineRenderer>().SetPosition(1, hook.transform.position);
        }*/

        if (questionController.isSimulating)
        {
            if (questionController.GetPlayerAnswer() > 0)
            {
                RegenerateVelocities();
                theHook.correctAnswer = (float)System.Math.Round(correctAnswer, 2);
                transform.Find("Annotation").GetComponent<Annotation>().Hide();
                transform.Find("CircularAnnotation").GetComponent<CircularAnnotation>().Hide();
                transform.Find("AngularAnnotation").GetComponent<AngularAnnotation>().Hide();

                elapsed += Time.fixedDeltaTime;
                timerText.text = elapsed.ToString("f2") + "s";

                // Correct Answer
                if (System.Math.Round(questionController.GetPlayerAnswer(), 2) == System.Math.Round(correctAnswer, 2))
                {
                    if (!doneFiring)
                    {
                        hook.SetActive(true);
                        hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        hook.GetComponent<Rigidbody2D>().WakeUp();
                        hookLine.SetActive(true);
                        //hook.GetComponent<Rigidbody2D>().velocity = new Vector3(velocityX, velocityY, 0) / (hook.GetComponent<Rigidbody2D>().mass);
                        hook.GetComponent<Rigidbody2D>().velocity = hookLauncher.transform.right * (questionController.GetPlayerAnswer());
                        doneFiring = true;
                    }

                    if (theHook.isCollided)
                    {
                        //thinRope.gameObject.SetActive(true);
                        //hookLine.SetActive(false);
                        elapsed -= 0.01f;
                        isSimulating = false;
                        //cameraScript.isStartOfStunt = false;
                        questionController.answerIsCorrect = true;
                        StartCoroutine(StuntResult());
                        questionController.isSimulating = false;
                    }
                }
                else
                {
                    // Too long
                    Debug.Log("Too long");
                    HookAttachmentCollider.SetActive(false);

                    if (!doneFiring)
                    {
                        hook.SetActive(true);
                        hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        hook.GetComponent<Rigidbody2D>().WakeUp();
                        hookLine.SetActive(true);
                        //hook.GetComponent<Rigidbody2D>().velocity = new Vector3(questionController.GetPlayerAnswer(), velocityY, 0) / (hook.GetComponent<Rigidbody2D>().mass);
                        if (questionController.GetPlayerAnswer() < correctAnswer)
                        {
                            hook.GetComponent<Rigidbody2D>().velocity = hookLauncher.transform.right * (questionController.GetPlayerAnswer() - .5f);
                        }
                        if (questionController.GetPlayerAnswer() > correctAnswer)
                        {
                            hook.GetComponent<Rigidbody2D>().velocity = hookLauncher.transform.right * (questionController.GetPlayerAnswer() + .5f);
                        }

                        doneFiring = true;
                    }

                    if (theHook.isCollided)
                    {
                        //cameraScript.isStartOfStunt = false;
                        questionController.answerIsCorrect = false;
                        // todo 
                        StartCoroutine(StuntResult());
                        questionController.isSimulating = false;
                    }
                }
            }
        }
    }
}
