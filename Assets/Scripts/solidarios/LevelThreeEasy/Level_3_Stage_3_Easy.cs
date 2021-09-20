using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Level_3_Stage_3_Easy : MonoBehaviour
{
    // Start is called before the first frame update
    StageManager sm = new StageManager();
    string question;
    public float elapsed;
    public GameObject platformCollider;
    public TMP_Text questionText, levelName;
    Animator thePlayerAnimation;
    public GameObject playerHingeJoint;
    public GameObject thePlayer;
    public GameObject playerHangingFixed;
    public GameObject FirstCamera;
    public GameObject SecondCamera;
    Vector3 thePlayer_position;
    public GameObject accurateCollider;
    public GameObject platformBarTop;
    string playerName, pronoun, pPronoun;
    public GameObject playerHangingBottom;
    float timeGiven;
    Vector2 gravityGiven;
    float correctAnswer;
    Vector2 spawnPointValue;
    float distance;
    float distanceGiven;
    // public QuestionController questionController;
    QuestionControllerVThree questionController;
    public CameraScript cameraScript;
    int stage = 3;

    void Start()
    {
        questionController = FindObjectOfType<QuestionControllerVThree>();
        questionController.stage = stage;
        playerName = PlayerPrefs.GetString("Name");
        if (PlayerPrefs.GetString("Gender") == "Male")
        {
            pronoun = "he";
            pPronoun = "him";
        }
        else
        {
            pronoun = "she";
            pPronoun = "her";
        }
        // Given        
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(20f, 25f), 2);
        distanceGiven = (float)System.Math.Round(UnityEngine.Random.Range(25f, 28.0f), 2);
        gravityGiven = Physics2D.gravity;

        // Formula
        correctAnswer = Mathf.Sqrt(Mathf.Abs((2 * distanceGiven) / gravityGiven.y));

        questionController.limit = correctAnswer + 1;
        transform.Find("Annotation1").GetComponent<Annotation>().SetDistance(distanceGiven);

        Debug.Log($"Correct Answer: {System.Math.Round(correctAnswer, 2)}");

        //Problem
        // levelName.SetText("Free Fall | Stage 3");
        question = $"{playerName} is hanging on a horizontal pole and {pronoun} is instructed to let go of it, drop down, and grab the elastic cord below to slow down his fall and safely land him into the ground. If the hands of {playerName} is exactly <color=#006A11>{distanceGiven}</color> meters above the second pole, <color=red>how long</color> should {playerName} fall down before {pronoun} grabs the second pole?";

        // if (questionText != null)
        // {
        //     questionController.SetQuestion(question);
        // }
        // else
        // {
        //     Debug.Log("QuestionText object not loaded.");
        // }
        questionController.SetQuestion(question);

        thePlayerAnimation = thePlayer.GetComponent<Animator>();
        thePlayerAnimation.SetBool("isHangingInBar", true);
        thePlayer_position = thePlayer.transform.position;

        distance = transform.Find("Annotation1").GetComponent<Annotation>().distance;

        // playerOnRopeTransform = playerOnRope.transform.position;

        spawnPointValue = transform.Find("Annotation1").GetComponent<Annotation>().SpawnPointValue();
        playerHingeJoint.transform.position = new Vector3(spawnPointValue.x - 2, distanceGiven - (spawnPointValue.y * -1), 0);

        platformBarTop.transform.position = new Vector3(spawnPointValue.x - 8, distanceGiven - (spawnPointValue.y * -1), 0);

        sm.SetGameLevel(3);
        questionController.levelDifficulty=GameConfig.Difficulty.Easy;
    }

    IEnumerator StuntResult(Action callback)
    {
        //messageFlag = false;
        yield return new WaitForSeconds(2f);
        callback();
    }

    public void GotoNextScene()
    {

    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // public void CallAction()
    // {
    //     Debug.Log(questionController.answerIsCorrect);
    //     if (questionController.answerIsCorrect)
    //     {
    //         GotoNextScene();
    //     }
    //     else
    //     {
    //         ResetLevel();
    //     }
    // }

    public void StartSimulation()
    {
        cameraScript.directorIsCalling = true;
        cameraScript.isStartOfStunt = true;
        // questionController.SetAnswer();
    }

    void FixedUpdate()
    {
        if (questionController.isSimulating)
        {
            float answer = questionController.GetPlayerAnswer();//float.Parse(playerAnswer.text.Split(new string[] { questionController.GetUnit() }, System.StringSplitOptions.None)[0]);
            transform.Find("Annotation1").GetComponent<Annotation>().Hide();
            elapsed += Time.fixedDeltaTime;

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
                    platformCollider.SetActive(false);
                    playerHangingBottom.SetActive(true);
                    thePlayer.SetActive(false);
                    playerHangingFixed.SetActive(true);
                    playerHangingFixed.GetComponentInChildren<Animator>().SetBool("isHangingInBar", true);
                    // elapsed -= 0.02f;
                    elapsed = correctAnswer;

                    cameraScript.isStartOfStunt = false;
                    questionController.answerIsCorrect = true;
                    StartCoroutine(StuntResult(() => questionController.ActivateResult($"{playerName} safely grabbed the pole!", true)));
                    //ToggleModal($"<b>Stunt Success!!!</b>", $"{playerName} safely grabbed the pole!", "Next")));
                }
            }
            else
            {
                if (answer < System.Math.Round(correctAnswer, 2))
                {
                    if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                    {
                        Debug.Log("Distance is too short!");
                        questionController.answerIsCorrect = false;
                        questionController.isSimulating = false;
                        cameraScript.directorIsCalling = true;
                        StartCoroutine(StuntResult(() => questionController.ActivateResult($"{playerName} grabbed the pole too soon!", false)));
                        //ToggleModal($"<b>Stunt Failed!!!</b>", $"{playerName} grabbed the pole too soon!", "Retry")));
                    }
                }
                else
                {
                    if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                    {
                        Debug.Log("Distance is too long!");
                        questionController.answerIsCorrect = false;
                        questionController.isSimulating = false;
                        cameraScript.directorIsCalling = true;
                        StartCoroutine(StuntResult(() => questionController.ActivateResult("{playerName} grabbed the pole too late!", false)));
                        //ToggleModal($"<b>Stunt Failed!!!</b>", $"{playerName} grabbed the pole too late!", "Retry")));
                    }
                }
            }
            questionController.timer = elapsed.ToString("f2") + "s";
        }
        else
        {
            //platformBarBottom.transform.position = new Vector3(spawnPointValue.x - 9, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - distance, 1);            
            questionController.isSimulating = false;

            // timerText.text = $"{(elapsed).ToString("f2")}s";
        }
        if (questionController.isSimulating)
            StartSimulation();
        else if (questionController.retried)
            ResetLevel();
        else if (questionController.nextStage)
            GotoNextScene();
    }
}
