using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class Level_3_Stage_1_Medium : MonoBehaviour
{
    // Start is called before the first frame update
    string question;
    public TMP_Text questionText, levelName, timerText;
    public GameObject AfterStuntMessage;
    Animator thePlayerAnimation;
    public GameObject HookAttachmentCollider;
    public GameObject hook;
    bool isSimulating = false;
    public GameObject thePlayer;
    Vector3 thePlayer_position;
    public TMP_InputField playerAnswer;
    Vector2 gravityGiven;
    public GameObject hookLauncher;
    public GameObject thePlayerRunning;
    float distanceGiven;
    float angleGiven;
    float correctAnswer;
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
    string playerName = "Junjun";
    string pronoun = "he";
    public QuestionController questionController;
    public CameraScript cameraScript;
    void Start()
    {
        // Given            
        distanceGiven = (float)System.Math.Round(UnityEngine.Random.Range(22f, 25f), 2);
        angleGiven = (float)System.Math.Round(UnityEngine.Random.Range(40f, 45f), 2);
        gravityGiven = Physics2D.gravity;

        //Problem
        question = $"{playerName} is instructed to cross another diff using this climbing device. If {playerName} shoot its gripping projectile at an angle of {angleGiven} degrees up to precisely get the corner of the cliff {distanceGiven} meters away horizontally from the shooting device, at what velocity should the projectile be shot to hit the gripping part?";

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
    }
    IEnumerator StuntResult(Action callback)
    {
        //messageFlag = false;
        yield return new WaitForSeconds(2f);
        callback();
    }
    void GenerateInitialVelocities()
    {
        velocityX = Mathf.Sqrt(Mathf.Abs((distanceGiven * gravityGiven.y) / (2 * Mathf.Tan(angleGiven * Mathf.Deg2Rad))));

        velocityInitial = float.Parse(playerAnswer.text);
        velocityY = Mathf.Abs(velocityInitial * Mathf.Sin(angleGiven * Mathf.Deg2Rad));

        correctAnswer = Mathf.Abs((velocityX / Mathf.Cos(angleGiven * Mathf.Deg2Rad)));

        Debug.Log($"VelocityX: {velocityX}");   
        Debug.Log($"VelocityY: {velocityY}");
        Debug.Log($"Correct Answer: {correctAnswer}");
    }

    void RegenerateVelocities()
    {
        velocityX = Mathf.Sqrt(Mathf.Abs((distanceGiven * gravityGiven.y) / (2 * Mathf.Tan(angleGiven * Mathf.Deg2Rad))));

        velocityInitial = float.Parse(playerAnswer.text);
        velocityY = Mathf.Abs(velocityInitial * Mathf.Sin(angleGiven * Mathf.Deg2Rad));

        correctAnswer = Mathf.Abs((velocityX / Mathf.Cos(angleGiven * Mathf.Deg2Rad)));

        Debug.Log($"VelocityX: {velocityX}");
        Debug.Log($"VelocityY: {velocityY}");
        Debug.Log($"Correct Answer: {correctAnswer}");
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
        isSimulating = true;
    }

    void FixedUpdate()
    {
        if (thePlayerRunning.GetComponent<RunningPlayer>().isCollided)
        {
            thePlayer.SetActive(true);
            thePlayerRunning.SetActive(false);
            transform.Find("Annotation").GetComponent<Annotation>().Show();
            transform.Find("CircularAnnotation").GetComponent<CircularAnnotation>().Show();
            transform.Find("AngularAnnotation").GetComponent<AngularAnnotation>().Show();
        }

        Annotation line = transform.Find("Annotation").GetComponent<Annotation>();
        dynamicPlatform.transform.position = new Vector3(line.distance + 32.34f, -15.69f, 1);
        grapplingPointIndicator.transform.position = new Vector3(line.distance + 4.4f, 1, 1);

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

        if (questionController.isSimulating)
        {
            if (playerAnswer.text.Length > 0)
            {
                transform.Find("Annotation").GetComponent<Annotation>().Hide();
                transform.Find("CircularAnnotation").GetComponent<CircularAnnotation>().Hide();
                transform.Find("AngularAnnotation").GetComponent<AngularAnnotation>().Hide();

                elapsed += Time.fixedDeltaTime;
                timerText.text = elapsed.ToString("f2") + "s";

                RegenerateVelocities();

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

                        cameraScript.isStartOfStunt = false;
                        questionController.answerIsCorrect = true;
                        StartCoroutine(StuntResult(() => questionController.ToggleModal($"<b>Stunt Success!!!</b>", $"{playerName} safely grabbed the pole!", "Next")));
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
                        hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        hook.GetComponent<Rigidbody2D>().WakeUp();
                        hook.GetComponent<Rigidbody2D>().velocity = new Vector3(float.Parse(playerAnswer.text), velocityY, 0) / (hook.GetComponent<Rigidbody2D>().mass);
                        doneFiring = true;
                    }

                    if (hook.GetComponent<Hook>().isCollidedToFailCollider)
                    {
                        cameraScript.isStartOfStunt = false;
                        questionController.answerIsCorrect = true;
                        // todo 
                        StartCoroutine(StuntResult(() => questionController.ToggleModal($"<b>Stunt Success!!!</b>", $"{playerName} safely grabbed the pole!", "Next")));
                        questionController.isSimulating = false;
                    }
                }
            }
        }
    }
}
