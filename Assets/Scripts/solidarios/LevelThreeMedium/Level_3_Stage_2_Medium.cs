using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class Level_3_Stage_2_Medium : MonoBehaviour
{
    // Start is called before the first frame update
    string question;
    public float elapsed;

    public TMP_Text initalVelociyText, playerNameText, stuntMessageText, timerText, questionText, levelName;
    public GameObject AfterStuntMessage;
    public GameObject movingToHookHand;
    Animator thePlayerAnimation;
    bool isMovingToHook;
    public Animator levelAnimations;
    public TMP_InputField playerAnswer;
    public GameObject hook;
    bool isSimulating = false;
    public float speed = 1f;
    public GameObject thePlayer;
    public GameObject FirstCamera;
    public GameObject thinRope;
    public GameObject SecondCamera;
    Vector3 thePlayer_position;
    public GameObject Rope2HookEnd;
    public GameObject HookAttachmentCollider;
    [SerializeField] CameraScript cameraScript;
    float timeGiven;
    Vector2 gravityGiven;
    float correctAnswer;
    Vector2 spawnPointValue;
    public GameObject hookLauncher;
    public GameObject hookLine;
    public GameObject hookIndicator;
    public TextMeshPro timeIndicator;
    float distance;
    float distanceGiven;
    bool doneFiring = false;
    float angleGiven;
    float velocityX = 0;
    float velocityY = 0;
    float velocityInitial = 0;
    float totalRopeMass = 0f;
    float timeOfFlight = 0f;
    public GameObject angularAnnotation;
    string playerName = "Junjun";
    string pronoun = "he";
    public GameObject annotation;

    public QuestionControllerVX questionController;
    void Start()
    {
        // Given        
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(20f, 25f), 2);
        distanceGiven = annotation.GetComponent<Annotation>().distance;
        angleGiven = (float)System.Math.Round(UnityEngine.Random.Range(35f, 40f), 2);
        //angleGiven = 40f;
        gravityGiven = Physics2D.gravity;

        thePlayerAnimation = thePlayer.GetComponent<Animator>();
        thePlayer_position = thePlayer.transform.position;

        spawnPointValue = annotation.GetComponent<Annotation>().SpawnPointValue();

        transform.Find("CircularAnnotation").GetComponent<CircularAnnotation>().SetAngle(angleGiven);

        transform.Find("AngularAnnotation").GetComponent<AngularAnnotation>().SetAngle(angleGiven);

        hook.GetComponent<Rigidbody2D>().Sleep();
        hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        hookLauncher.transform.Rotate(new Vector3(0, 0, angleGiven));
    }

    IEnumerator StuntResult(Action callback)
    {
        //messageFlag = false;
        yield return new WaitForSeconds(2f);
        callback();
    }

    void GenerateVelocities()
    {
        velocityX = Mathf.Sqrt(Mathf.Abs((distanceGiven * gravityGiven.y) / (2 * Mathf.Tan(angleGiven * Mathf.Deg2Rad))));
        velocityInitial = Mathf.Abs((velocityX / Mathf.Cos(angleGiven * Mathf.Deg2Rad)));
        velocityY = Mathf.Abs(velocityInitial * Mathf.Sin(angleGiven * Mathf.Deg2Rad));

        initalVelociyText.SetText($"{System.Math.Round(velocityInitial, 2)} m/s");

        // Formula
        timeOfFlight = Mathf.Abs((2 * velocityInitial * Mathf.Sin(angleGiven * Mathf.Deg2Rad)) / gravityGiven.y);

        correctAnswer = timeOfFlight;

        //Problem
        question = $"<b>{playerName}</b> wants to cross to the cliff at the other side using his climbing device. If {pronoun} shoots its gripping projectile at a velocity of <b><color=red>{System.Math.Round(velocityInitial, 2)}</color></b> at an angle of <b><color=orange>{angleGiven}</color></b> degrees, <b><color=purple>at what precise time</color></b> should <b>{playerName}</b> <b>trigger the gripper</b> if it will grip at the exact moment when it will touch the gripping point of the cliff accross which is at the same horizontal level of the shooting device.";

        if (questionText != null)
        {
            questionController.SetQuestion(question);
        }
        else
        {
            Debug.Log("QuestionText object not loaded.");
        }

        Debug.Log($"Gravity: {gravityGiven.y}");
        Debug.Log($"Angle: {angleGiven}");
        Debug.Log($"VelocityX: {velocityX}");
        Debug.Log($"VelocityY: {velocityY}");
        Debug.Log($"VelocityInitial: {velocityInitial}");
        Debug.Log($"TimeofFlight: {timeOfFlight}");
    }

    IEnumerator DropRope()
    {
        yield return new WaitForSeconds(0.3f);
        hookLauncher.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        hookLauncher.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        thePlayerAnimation.SetBool("failed", true);
    }

    IEnumerator PullRope()
    {
        yield return new WaitForSeconds(0.3f);
        isMovingToHook = true;
        thinRope.gameObject.SetActive(false);
        hookLine.SetActive(true);
        levelAnimations.SetBool("isMoving", true);
        thePlayerAnimation.SetBool("isPullingRope", true);
        yield return new WaitForSeconds(1.59f);
        thePlayerAnimation.SetBool("happy", true);
        hookLine.SetActive(false);
        timeIndicator.enabled = false;

        cameraScript.isStartOfStunt = false;
        questionController.answerIsCorrect = true;
        StartCoroutine(StuntResult(() => questionController.ToggleModal($"<b>Stunt Success!!!</b>", $"{playerName} safely hooked on the grabbing point!", "Next")));
        questionController.isSimulating = false;
    }

    public void StartSimulation()
    {
        cameraScript.directorIsCalling = true;
        cameraScript.isStartOfStunt = true;
        questionController.SetAnswer();
    }

    void FixedUpdate()
    {

        if (velocityX == 0 || velocityY == 0 || velocityInitial == 0)
        {
            GenerateVelocities();
        }

        if (isMovingToHook && !isSimulating)
        {
            hookLine.GetComponent<LineRenderer>().SetPosition(0, movingToHookHand.transform.position);
            hookLine.GetComponent<LineRenderer>().SetPosition(1, hook.transform.position);
        }
        else
        {
            hookLine.GetComponent<LineRenderer>().SetPosition(0, hookLauncher.transform.position);
            hookLine.GetComponent<LineRenderer>().SetPosition(1, hook.transform.position);
        }
        if (playerAnswer.text.Length > 0)
        {
            if (questionController.isSimulating)
            {
                timeIndicator.transform.position = new Vector3(hook.transform.position.x, hook.transform.position.y + 1, 1);
                transform.Find("Annotation1").GetComponent<Annotation>().Hide();
                transform.Find("CircularAnnotation").GetComponent<CircularAnnotation>().Hide();
                transform.Find("AngularAnnotation").GetComponent<AngularAnnotation>().Hide();

                elapsed += Time.fixedDeltaTime;
                timerText.text = elapsed.ToString("f2") + "s";
                timeIndicator.text = elapsed.ToString("f2") + "s";

                // Correct Answer
                if (System.Math.Round(float.Parse(playerAnswer.text), 2) == System.Math.Round(correctAnswer, 2))
                {
                    if (!doneFiring)
                    {
                        hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        hook.GetComponent<Rigidbody2D>().WakeUp();
                        hook.GetComponent<Rigidbody2D>().velocity = new Vector3(velocityX, velocityY, 0) / (hook.GetComponent<Rigidbody2D>().mass);
                        doneFiring = true;
                    }

                    if (hook.GetComponent<Hook>().isCollided)
                    {
                        thinRope.gameObject.SetActive(true);
                        hookLine.SetActive(false);
                        elapsed -= 0.01f;
                        isSimulating = false;

                        Rope2HookEnd.GetComponent<Rigidbody2D>().velocity = new Vector3(2, 0, 0);
                        StartCoroutine(PullRope());

                    }
                }
                else
                {
                    if (System.Math.Round(correctAnswer, 2) > float.Parse(playerAnswer.text))
                    {
                        // Too short
                        Debug.Log("Too short");
                        HookAttachmentCollider.SetActive(false);

                        if (!doneFiring)
                        {
                            hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            hook.GetComponent<Rigidbody2D>().WakeUp();
                            hook.GetComponent<Rigidbody2D>().velocity = new Vector3(velocityX, velocityY, 0) / (hook.GetComponent<Rigidbody2D>().mass);
                            doneFiring = true;
                        }

                        if (hook.GetComponent<Hook>().isCollidedToFailCollider)
                        {
                            questionController.answerIsCorrect = false;
                            questionController.isSimulating = false;
                            cameraScript.directorIsCalling = true;
                            StartCoroutine(StuntResult(() => questionController.ToggleModal($"<b>Stunt Failed!!!</b>", $"{playerName} hook was grabbed too soon! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.", "Retry")));
                        }
                    }
                    else
                    {
                        // Too long
                        Debug.Log("Too long");
                        HookAttachmentCollider.SetActive(false);

                        if (!doneFiring)
                        {
                            hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            hook.GetComponent<Rigidbody2D>().WakeUp();
                            hook.GetComponent<Rigidbody2D>().velocity = new Vector3(velocityX, velocityY, 0) / (hook.GetComponent<Rigidbody2D>().mass);
                            doneFiring = true;
                        }

                        if (hook.GetComponent<Hook>().isCollidedToFailCollider)
                        {
                            cameraScript.isStartOfStunt = false;
                            questionController.answerIsCorrect = false;
                            questionController.isSimulating = false;
                            cameraScript.directorIsCalling = true;
                            StartCoroutine(StuntResult(() => questionController.ToggleModal($"<b>Stunt Failed!!!</b>", $"{playerName} hook was grabbed too late! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.", "Retry")));
                        }
                    }
                }
            }
            else
            {
                //platformBarBottom.transform.position = new Vector3(spawnPointValue.x - 9, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - distance, 1);            
                if (hook.GetComponent<Hook>().isCollidedToFailCollider)
                {
                    hook.transform.position -= new Vector3(0.1f, -0.1f, 0);
                    hookIndicator.SetActive(false);
                    StartCoroutine(DropRope());
                }

                timerText.text = $"{(elapsed).ToString("f2")}s";
                timeIndicator.text = $"{(elapsed).ToString("f2")}s";
            }
        }
    }
}
