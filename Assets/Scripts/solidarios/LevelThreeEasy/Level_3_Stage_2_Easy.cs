using System.Collections;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using GameConfig;
public class Level_3_Stage_2_Easy : MonoBehaviour
{
    // Start is called before the first frame update

    float height = 5.0f;
    float targetTime = 0f;
    string question;
    public float elapsed;
    public TMP_Text playerNameText, stuntMessageText, timerText, questionText, levelName;
    public GameObject AfterStuntMessage;
    Animator thePlayerAnimation;
    public TMP_InputField playerAnswer;
    bool isSimulating = false;
    public GameObject playerHingeJoint;
    public GameObject thePlayer;
    public GameObject playerHangingFixed;
    public GameObject FirstCamera;
    public GameObject SecondCamera;
    Vector3 thePlayer_position;
    public GameObject accurateCollider;

    public GameObject platformBarBottom;
    public GameObject platformBarTop;

    Vector3 playerOnRopeTransform;

    float timeGiven;
    Vector2 gravityGiven;
    float correctAnswer;
    Vector2 spawnPointValue;
    float distance;
    float distanceGiven;
    string playerName = "Junjun";
    string pronoun = "he";
    public QuestionControllerVThree questionController;
    public CameraScript cameraScript;
    StageManager sm = new StageManager();
    public Annotation annnotation;
    void Start()
    {
        // Given        
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(0.8f, 1.0f), 2);
        distanceGiven = (float)System.Math.Round(UnityEngine.Random.Range(5.0f, 5.55f), 2);
        gravityGiven = Physics2D.gravity;

        // Formula
        correctAnswer = Mathf.Sqrt(Mathf.Abs((2 * distanceGiven) / gravityGiven.y));


        annotation.SetDistance(distanceGiven);
        annotation.revealValue = true;
        annotation.SetSpawningPoint(new Vector2(15, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - correctAnswer));
        // transform.Find("Annotation1").GetComponent<Annotation>().SetDistance(distanceGiven);
        //transform.Find("Annotation1").GetComponent<Annotation>().SetSpawningPoint(new Vector2(15, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - correctAnswer));

        //Debug.Log($"Hinge: {playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y}");
        Debug.Log($"Distance Generated: {distanceGiven}");
        Debug.Log($"Correct Answer: {correctAnswer}");
        Debug.Log($"Correct Answer Rounded: {System.Math.Round(correctAnswer, 2)}");

        //Problem
        // levelName.SetText("Free Fall | Stage 2");
        sm.SetGameLevel(3);
        questionController.stage = 2;
        questionController.levelDifficulty = Difficulty.Easy;
        question = $"{playerName} is hanging on a horizontal pole and {pronoun} is instructed to let go of it, drop down, and hang again to another pole below. If the hands of {playerName} is exactly <color=#006A11>{distanceGiven}</color> meters above the second pole, how long should [name] fall down before {pronoun} grabs the second pole?";

        // if (questionText != null)
        // {
            questionController.SetQuestion(question);
        // }
        // else
        // {
        //     Debug.Log("QuestionText object not loaded.");
        // }

        thePlayerAnimation = thePlayer.GetComponent<Animator>();
        thePlayerAnimation.SetBool("isHangingInBar", true);
        thePlayer_position = thePlayer.transform.position;

        distance = transform.Find("Annotation1").GetComponent<Annotation>().distance;

        // playerOnRopeTransform = playerOnRope.transform.position;

        spawnPointValue = transform.Find("Annotation1").GetComponent<Annotation>().SpawnPointValue();
        playerHingeJoint.transform.position = new Vector3(spawnPointValue.x - 1, distanceGiven, 0);
        platformBarBottom.transform.position = new Vector3(spawnPointValue.x - 9, spawnPointValue.y, 0);
        platformBarTop.transform.position = new Vector3(spawnPointValue.x - 9, distanceGiven, 0);
    }

    public void CallAction()
    {
        Debug.Log(questionController.answerIsCorrect);
        if (questionController.answerIsCorrect)
        {
            GotoNextScene();
        }
        else
        {
            ResetLevel();
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GotoNextScene()
    {

    }

    IEnumerator StuntResult(Action callback)
    {
        //messageFlag = false;
        yield return new WaitForSeconds(2f);
        callback();
    }

    public void StartSimulation()
    {
        cameraScript.directorIsCalling = true;
        cameraScript.isStartOfStunt = true;
        questionController.SetAnswer();
    }
    void FixedUpdate()
    {
        // if (playerAnswer.text.Length > 0)
        // {

            if (questionController.isSimulating)
            {

                float answer = questionController.GetPlayerAnswer();//float.Parse(playerAnswer.text.Split(new string[] { questionController.GetUnit() }, System.StringSplitOptions.None)[0]);
                transform.Find("Annotation1").GetComponent<Annotation>().Hide();
                elapsed += Time.fixedDeltaTime;
                timerText.text = elapsed.ToString("f2") + "s";

                playerHingeJoint.GetComponent<HingeJoint2D>().enabled = false;
                thePlayerAnimation.SetBool("isFalling", true);

                // Correct Answer
                if (System.Math.Round(answer, 2) == System.Math.Round(correctAnswer, 2))
                {
                    Debug.Log("Time is correct!");
                    if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                    {
                        FirstCamera.SetActive(false);
                        SecondCamera.SetActive(true);

                        thePlayer.SetActive(false);
                        playerHangingFixed.SetActive(true);
                        playerHangingFixed.transform.position = new Vector3(spawnPointValue.x - 1f, platformBarBottom.transform.position.y - 1.5f, 1);
                        platformBarBottom.GetComponent<Animator>().SetBool("collided", true);
                        playerHangingFixed.GetComponent<Animator>().SetBool("isHangingInBar", true);
                        elapsed -= 0.01f;
                        isSimulating = false;

                        cameraScript.isStartOfStunt = false;
                        questionController.answerIsCorrect = true;
                        StartCoroutine(StuntResult(() => questionController.ActivateResult($"{PlayerPrefs.GetString("Name")} safely grabbed the pole!",true,false)));
                        //ToggleModal($"<b>Stunt Success!!!</b>", $"{PlayerPrefs.GetString("Name")} safely grabbed the pole!", "Next")));
                        questionController.isSimulating = false;

                    }
                }
                else
                {
                    if (answer < System.Math.Round(correctAnswer, 2))
                    {
                        if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                        {
                            Debug.Log("Distance is too short!");
                            cameraScript.isStartOfStunt = false;
                            questionController.answerIsCorrect = false;
                            questionController.isSimulating = false;
                            cameraScript.directorIsCalling = true;
                            StartCoroutine(StuntResult(() => questionController.ActivateResult($"{playerName} grabbed the pole too soon. The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.", false, false)));
                            //ToggleModal($"<b>Stunt Failed!!!</b>", $"{playerName} grabbed the pole too soon. The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.", "Retry")));
                        }
                    }
                    else
                    {
                        if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                        {
                            cameraScript.isStartOfStunt = false;

                            questionController.answerIsCorrect = false;
                            questionController.isSimulating = false;
                            cameraScript.directorIsCalling = true;
                            StartCoroutine(StuntResult(() => questionController.ActivateResult($"{playerName} grabbed the pole too late! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.",false,false)));
                            //ToggleModal($"<b>Stunt Failed!!!</b>", $"{playerName} grabbed the pole too late! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.", "Retry")));
                        }
                    }
                }
            }
            else
            {
                //platformBarBottom.transform.position = new Vector3(spawnPointValue.x - 9, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - distance, 1);            
                questionController.isSimulating = false;
                timerText.text = $"{(elapsed).ToString("f2")}s";
            }
        // }
    }
}
