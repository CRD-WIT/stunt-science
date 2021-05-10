using System.Collections;
using UnityEngine;
using TMPro;

public class Level_4_Stage_1 : MonoBehaviour
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

    void Start()
    {
        // Given        
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(20f, 25f), 2);
        distanceGiven = transform.Find("Annotation1").GetComponent<Annotation>().distance;
        angleGiven = (float)System.Math.Round(UnityEngine.Random.Range(35f, 40f), 2);
        //angleGiven = 40f;
        gravityGiven = Physics2D.gravity;

        //Problem
        levelName.SetText("Free Fall | Stage 1");
        question = $"[player] wants to cross to the cliff at the other side using his climbing device. If [pronoun] shoots its gripping projectile at a velocity of [vo] at an angle of [a] degrees, at what precise time should [name] trigger the gripper if it will grip at the exact moment when it will touch the gripping point of the cliff accross which is at the same horizontal level of the shooting device.";

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

        spawnPointValue = transform.Find("Annotation1").GetComponent<Annotation>().SpawnPointValue();

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

    void GenerateVelocities()
    {
        velocityX = Mathf.Sqrt(Mathf.Abs((distanceGiven * gravityGiven.y) / (2 * Mathf.Tan(angleGiven * Mathf.Deg2Rad))));
        velocityInitial = Mathf.Abs((velocityX / Mathf.Cos(angleGiven * Mathf.Deg2Rad)));
        velocityY = Mathf.Abs(velocityInitial * Mathf.Sin(angleGiven * Mathf.Deg2Rad));

        initalVelociyText.SetText($"{System.Math.Round(velocityInitial, 2)} m/s");

        // Formula
        timeOfFlight = Mathf.Abs((2 * velocityInitial * Mathf.Sin(angleGiven * Mathf.Deg2Rad)) / gravityGiven.y);

        correctAnswer = timeOfFlight;

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
    }

    public void StartSimulation()
    {
        isSimulating = true;
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

        if (isSimulating)
        {

            if (playerAnswer.text.Length > 0)
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
                            isSimulating = false;

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
                            isSimulating = false;

                        }

                    }
                }
            }
            else
            {
                Debug.Log("No value was added");
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
