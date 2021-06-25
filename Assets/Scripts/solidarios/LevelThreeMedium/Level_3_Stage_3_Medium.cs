using System.Collections;
using UnityEngine;
using TMPro;

public class Level_3_Stage_3_Medium : MonoBehaviour
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
    public GameObject grappingPointIndicator;
    public QuestionController questionController;
    public CameraScript cameraScript;
    void Start()
    {
        // Given   
        velocityInitial =  (float)System.Math.Round(UnityEngine.Random.Range(12f, 14f), 2);        
        distanceGiven = 0;
        angleGiven = (float)System.Math.Round(UnityEngine.Random.Range(70f, 75f), 2);
        gravityGiven = Physics2D.gravity;

        Annotation line = transform.Find("Annotation").GetComponent<Annotation>();

        line.SetDistance(distanceGiven);

        //Problem
        levelName.SetText("Projectile Motion | Stage 2");
        question = $"[name] is instructed to cross the last cliff. If [name] shoots the gripping projectile at an angle of [angle] degrees up at a velocity of [vo] meters per second, how far precisely should [name] be away horizontally from the target gripping point in order to hit it?";

        if (questionText != null)
        {
            questionText.SetText(question);
        }
        else
        {
            Debug.Log("QuestionText object not loaded.");
        }

        thePlayerAnimation = thePlayer.GetComponent<Animator>();
        thePlayer_position = thePlayer.transform.position;

        transform.Find("CircularAnnotation").GetComponent<CircularAnnotation>().SetAngle(angleGiven);

        transform.Find("AngularAnnotation").GetComponent<AngularAnnotation>().SetAngle(angleGiven);

        hook.GetComponent<Rigidbody2D>().Sleep();
        hook.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        hookLauncher.transform.Rotate(new Vector3(0, 0, angleGiven));
    }

    IEnumerator StuntResult()
    {
        //messageFlag = false;
        yield return new WaitForSeconds(4f);
        AfterStuntMessage.SetActive(true);
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
        grappingPointIndicator.transform.position = new Vector3(line.distance + 4.4f, 1, 1);

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

        if (isSimulating)
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
                        isSimulating = false;
                    }
                }
            }
        }
    }
}