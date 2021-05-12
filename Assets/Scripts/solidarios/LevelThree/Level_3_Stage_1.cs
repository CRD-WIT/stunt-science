using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Level_3_Stage_1 : MonoBehaviour
{
    // Start is called before the first frame update
    string question;
    float lastHitYPosition;
    GameObject[] ropeBones;
    public float elapsed;
    public GameObject timer;
    public TMP_Text playerNameText, stuntMessageText, timerText, questionText, levelName;
    public GameObject AfterStuntMessage;
    Animator thePlayerAnimation;
    public TMP_InputField playerAnswer;
    public GameObject playerOnRope;
    bool isSimulating = false;
    public GameObject playerHingeJoint;
    public GameObject thePlayer;
    public GameObject playerHangingFixed;
    public GameObject FirstCamera;
    public GameObject SecondCamera;
    Vector3 thePlayer_position;
    public GameObject accurateCollider;
    public GameObject platformBar;
    Vector3 playerOnRopeTransform;
    float timeGiven;
    Vector2 gravityGiven;
    float correctAnswer;
    Vector2 spawnPointValue;
    bool respositioned = false;
    float distance;
    bool letGoRope = false;
    float playerOnRopeInitialY;
    float accurateColliderInitialPointY;
    public GameObject playButton;
    public GameObject timerAnnotation;

    String playerName = "Junjun";
    String pronoun = "he";
    bool metTargetTime = false;
    void Start()
    {
        ropeBones = GameObject.FindGameObjectsWithTag("RopeBones");

        // Given        
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(0.8f, 1.0f), 2);
        gravityGiven = Physics2D.gravity;
        // Formula
        correctAnswer = Mathf.Abs((gravityGiven.y / 2) * Mathf.Pow(timeGiven, 2));

        transform.Find("Annotation1").GetComponent<Annotation>().SetDistance(correctAnswer);
        transform.Find("Annotation1").GetComponent<Annotation>().SetSpawningPoint(new Vector2(15, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - correctAnswer));

        Debug.Log($"Distance: {correctAnswer}");
        Debug.Log($"Hinge: {playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y}");
        Debug.Log($"Time Generated: {timeGiven}");
        Debug.Log($"Correct Answer: {System.Math.Round(correctAnswer, 2)}");

        //Problem
        levelName.SetText("Stage 1");
        question = $"{playerName} is hanging from a rope and {pronoun} is instructed to let go of it, drop down, and hang again to the horizontal pole below. If {playerName} will let go ang grab the pole after exactly <b><color=#006d00>{timeGiven} sec</color></b>, at what <b><color=#006d00>distance</color></b> should {pronoun} hands above the pole before letting go?";

        if (questionText != null)
        {
            questionText.SetText(question);
        }
        else
        {
            Debug.Log("QuestionText object not loaded.");
        }

        thePlayerAnimation = thePlayer.GetComponent<Animator>();

        thePlayerAnimation.SetBool("isHanging", true);

        thePlayer_position = thePlayer.transform.position;

        distance = transform.Find("Annotation1").GetComponent<Annotation>().distance;

        playerOnRopeTransform = playerOnRope.transform.position;

        spawnPointValue = transform.Find("Annotation1").GetComponent<Annotation>().SpawnPointValue();

        platformBar.transform.position = new Vector3(spawnPointValue.x - 9, spawnPointValue.y, 0);

        accurateColliderInitialPointY = accurateCollider.transform.position.y;

        playerOnRopeInitialY = (float)Math.Round(playerOnRope.transform.position.y, 2);
    }

    IEnumerator StuntResult()
    {
        //messageFlag = false;
        yield return new WaitForSeconds(4f);
        AfterStuntMessage.SetActive(true);
    }

    void RepositionRopeComplete()
    {
        this.respositioned = true;
    }

    void FallFromRope()
    {
        this.letGoRope = true;
    }

    public void StartSimulation()
    {
        isSimulating = true;
    }
    void FixedUpdate()
    {


        if (isSimulating)
        {
            timer.SetActive(true);
            playButton.SetActive(false);

            if (playerAnswer.text.Length > 0)
            {
                float answer = float.Parse(playerAnswer.text);
                float correct = (float)Math.Round(correctAnswer, 2);
                float playerOnRopeY = (float)Math.Round(playerOnRope.transform.position.y, 2);

                if (answer != correct)
                {
                    if (answer > correct)
                    {
                        float diff = answer - correct;
                        float target = playerOnRopeInitialY + diff;
                        if (playerOnRopeY < target)
                        {
                            Debug.Log($"{playerOnRopeY < target} | {playerOnRopeY} < {target}");
                            playerOnRope.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 1f, 0);
                        }
                        else
                        {
                            playerOnRope.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                            RepositionRopeComplete();
                        }
                        //Debug.Log($"rope: {playerOnRopeY} | correct: {correct} | answer: {answer}");
                    }
                    else
                    {
                        float diff = correct - answer;
                        float target = playerOnRopeInitialY - diff;
                        if (playerOnRopeY > target)
                        {
                            playerOnRope.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -1f, 0);
                        }
                        else
                        {
                            playerOnRope.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                            RepositionRopeComplete();
                        }
                        //Debug.Log($"rope: {playerOnRopeY} | correct: {correct} | answer: {answer}");
                    }
                }
                else
                {
                    RepositionRopeComplete();
                    if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                    {
                        FirstCamera.SetActive(false);
                        SecondCamera.SetActive(true);

                        thePlayer.SetActive(false);
                        playerHangingFixed.SetActive(true);
                        playerHangingFixed.GetComponent<Animator>().SetBool("isHangingInBar", true);
                        isSimulating = false;
                        stuntMessageText.text = $"<b>Stunt Success!!!</b>\n\n{PlayerPrefs.GetString("Name")} safely grabbed the pole!";
                    }
                }

                transform.Find("Annotation1").GetComponent<Annotation>().Hide();

                if (respositioned)
                {
                    elapsed += Time.fixedDeltaTime;

                    playerHingeJoint.GetComponent<HingeJoint2D>().enabled = false;
                    thePlayerAnimation.SetBool("isFalling", true);

                    // Correct Answer
                    if (System.Math.Round(float.Parse(playerAnswer.text), 2) == System.Math.Round(correctAnswer, 2))
                    {
                        Debug.Log("Distance is correct!");
                        if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                        {
                            FirstCamera.SetActive(false);
                            SecondCamera.SetActive(true);

                            thePlayer.SetActive(false);
                            playerHangingFixed.SetActive(true);
                            playerHangingFixed.transform.position = new Vector3(spawnPointValue.x - 0.2f, platformBar.transform.position.y - 1.5f, 1);
                            platformBar.GetComponent<Animator>().SetBool("collided", true);
                            playerHangingFixed.GetComponent<Animator>().SetBool("isHangingInBar", true);
                            isSimulating = false;
                            stuntMessageText.text = $"<b>Stunt Success!!!</b>\n\n{PlayerPrefs.GetString("Name")} safely grabbed the pole!";
                            //StartCoroutine(StuntResult());
                        }
                    }
                    else
                    {
                        if (float.Parse(playerAnswer.text) < System.Math.Round(correctAnswer, 2))
                        {
                            if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                            {
                                Debug.Log("Distance is too short!");
                                isSimulating = false;
                                stuntMessageText.text = $"<b><color=red>Stunt Failed!!!</b>\n\n{PlayerPrefs.GetString("Name")} hand distance to the pole is shorter.</color>";
                                //StartCoroutine(StuntResult());
                            }
                        }
                        else
                        {
                            if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                            {
                                Debug.Log("Distance is too long!");
                                isSimulating = false;
                                stuntMessageText.text = $"<b><color=red>Stunt Failed!!!</b>\n\n{PlayerPrefs.GetString("Name")} hand distance to the pole is longer.</color>";
                                //StartCoroutine(StuntResult());
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("No value was added");
            }
            timerText.text = $"{(elapsed).ToString("f2")}s";
            timerAnnotation.GetComponent<TMP_Text>().text = $"t={(elapsed).ToString("f2")}s";

        }
        else
        {
            // platformBar.transform.position = new Vector3(spawnPointValue.x - 9, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - distance, 1);           
            if (respositioned)
            {
                timerText.text = $"{(timeGiven).ToString("f2")}s";
                // timerAnnotation.GetComponent<TMP_Text>().text = $"t={(timeGiven).ToString("f2")}s";
            }

        }

        if (elapsed == timeGiven)
        {
            timerAnnotation.transform.position = new Vector3(17, lastHitYPosition, 1);
        }
        else
        {
            timerAnnotation.transform.position = new Vector3(17, playerHingeJoint.transform.position.y, 1);
            lastHitYPosition = playerHingeJoint.transform.position.y;
        }
    }
}
