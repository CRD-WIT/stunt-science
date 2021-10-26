using System.Collections;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using GameConfig;
using UnityEngine.UI;
public class Level_3_Stage_2_Easy : MonoBehaviour
{
    // Start is called before the first frame update

    float height = 5.0f;
    float targetTime = 0f;
    string question;
    public float elapsed;
    public TMP_Text calloutText;
    public GameObject AfterStuntMessage;
    Animator thePlayerAnimation;
    public TMP_InputField playerAnswer;
    bool isSimulating = false, directorIsCalling, isStartOfStunt, answerIsCorrect, isEndOfStunt;
    public GameObject playerHingeJoint;
    public GameObject thePlayer;
    public GameObject playerHangingFixed;
    Vector3 thePlayer_position;
    public GameObject accurateCollider;
    public Button play;

    public GameObject platformBarBottom;
    public GameObject platformBarTop, callout;

    Vector3 playerOnRopeTransform;

    float timeGiven;
    Vector2 gravityGiven;
    float correctAnswer;
    Vector2 spawnPointValue;
    float distance, answer;
    float distanceGiven;
    string playerName = "Junjun", messageTxt;
    string pronoun = "he", pPronoun;
    public QuestionControllerVThree questionController;
    public CameraScript cameraScript;
    StageManager sm = new StageManager();
    public Annotation annotation;
    public HeartManager life;
    public TMP_Text debugAnswer;
    bool ShowResult;
    void Start()
    {
        // Given 
        ShowResult = true;
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(1f, 1.5f), 2);
        distanceGiven = (float)System.Math.Round(UnityEngine.Random.Range(5.0f, 12.5f), 2);
        gravityGiven = Physics2D.gravity;

        // Formula
        correctAnswer = Mathf.Sqrt(Mathf.Abs((2 * distanceGiven) / gravityGiven.y));

        questionController.limit = correctAnswer + 1;

        annotation.SetDistance(distanceGiven);
        annotation.revealValue = true;
        annotation.SetSpawningPoint(new Vector2(15, playerHingeJoint.transform.position.y - distanceGiven));
        // annotation.SetSpawningPoint(new Vector2(15, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - correctAnswer));
        // transform.Find("Annotation1").GetComponent<Annotation>().SetDistance(distanceGiven);
        //transform.Find("Annotation1").GetComponent<Annotation>().SetSpawningPoint(new Vector2(15, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - correctAnswer));

        //Debug.Log($"Hinge: {playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y}");
        // Debug.Log($"Distance Generated: {distanceGiven}");
        // Debug.Log($"Correct Answer: {correctAnswer}");
        // Debug.Log($"Correct Answer Rounded: {System.Math.Round(correctAnswer, 2)}");

        //Problem
        // levelName.SetText("Free Fall | Stage 2");
        sm.SetGameLevel(3);
        questionController.stage = 2;
        questionController.levelDifficulty = Difficulty.Easy;
        playerName = PlayerPrefs.GetString("Name");
        String playerGender = PlayerPrefs.GetString("Gender");
        if (playerGender == "Male")
        {
            pronoun = "he";
            pPronoun = "him";
        }
        else
        {
            pronoun = "she";
            pPronoun = "her";
        }
        //question = $"{playerName} is hanging on a horizontal pole and {pronoun} is instructed to let go of it, drop down, and hang again to another pole below. If the hands of {playerName} is exactly <color=#006A11>{distanceGiven}</color> meters above the second pole, how long should [name] fall down before {pronoun} grabs the second pole?";
        question = $"<b>{playerName}</b> now is instructed to let go of the first branch and grab the second branch below as she falls. If the distance between two branches is exactly <b>{distanceGiven}</b> meters vertically, how long should {pronoun} fall down before grabbing the second branch?";

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

        distance = annotation.distance;

        // playerOnRopeTransform = playerOnRope.transform.position;

        spawnPointValue = annotation.SpawnPointValue();
        // playerHingeJoint.transform.position = new Vector3(spawnPointValue.x - 1, distanceGiven, 0);
        playerHingeJoint.transform.position = new Vector3(spawnPointValue.x - 1, platformBarTop.transform.position.y, 0);
        platformBarBottom.transform.position = new Vector3(spawnPointValue.x - 9, spawnPointValue.y, 0);
        // platformBarTop.transform.position = new Vector3(spawnPointValue.x - 9, distanceGiven, 0);
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

    void ResetLevel()
    {
        play.interactable = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void GotoNextScene()
    {
        SceneManager.LoadScene("LevelThreeStage3");
    }

    IEnumerator StuntResult(Action callback)
    {
        //messageFlag = false;
        isEndOfStunt = false;
        yield return new WaitForSeconds(1f);
        isStartOfStunt = false;
        directorIsCalling = true;
        yield return new WaitForSeconds(1f);
        callback();
    }

    void StartSimulation()
    {
        // cameraScript.directorIsCalling = true;
        // cameraScript.isStartOfStunt = true;
        // questionController.SetAnswer();
        answer = questionController.GetPlayerAnswer();
        questionController.isSimulating = false;
        directorIsCalling = true;
        isStartOfStunt = true;
    }
    void FixedUpdate()
    {
        questionController.errorText = "answer is not valid for simulation";
        debugAnswer.SetText($"Answer: {System.Math.Round(correctAnswer, 2)}");

        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        if (isSimulating)
        {
            play.interactable = false;         // float answer = questionController.GetPlayerAnswer();//float.Parse(playerAnswer.text.Split(new string[] { questionController.GetUnit() }, System.StringSplitOptions.None)[0]);
            annotation.Hide();
            elapsed += Time.fixedDeltaTime;
            questionController.timer = elapsed.ToString("f2");

            playerHingeJoint.GetComponent<HingeJoint2D>().enabled = false;
            thePlayerAnimation.SetBool("isFalling", true);

            // Correct Answer
            if (System.Math.Round(answer, 2) == System.Math.Round(correctAnswer, 2))
            {
                answerIsCorrect = true;
                Debug.Log("Time is correct!");
                if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                {
                    thePlayer.SetActive(false);
                    playerHangingFixed.SetActive(true);
                    playerHangingFixed.transform.position = new Vector3(spawnPointValue.x - 1f, platformBarBottom.transform.position.y - 1.2f, 1);
                    platformBarBottom.GetComponent<Animator>().SetBool("collided", true);
                    playerHangingFixed.GetComponent<Animator>().SetBool("isHangingInBar", true);
                    elapsed -= 0.01f;
                    isSimulating = false;
                    cameraScript.isStartOfStunt = false;
                    questionController.answerIsCorrect = true;
                    messageTxt = $"{PlayerPrefs.GetString("Name")} safely grabbed the pole!";
                    // StartCoroutine(StuntResult(() => questionController.ActivateResult($"{PlayerPrefs.GetString("Name")} safely grabbed the pole!",true,false)));
                    //ToggleModal($"<b>Stunt Success!!!</b>", $"{PlayerPrefs.GetString("Name")} safely grabbed the pole!", "Next")));
                    isSimulating = false;
                    isEndOfStunt = true;
                }
            }
            else
            {
                answerIsCorrect = false;
                if (answer < System.Math.Round(correctAnswer, 2))
                {
                    if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                    {
                        Debug.Log("Distance is too short!");
                        cameraScript.isStartOfStunt = false;
                        questionController.answerIsCorrect = false;
                        cameraScript.directorIsCalling = true;
                        messageTxt = $"{playerName} grabbed the pole too soon. The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.";
                        // StartCoroutine(StuntResult(() => questionController.ActivateResult($"{playerName} grabbed the pole too soon. The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.", false, false)));
                        //ToggleModal($"<b>Stunt Failed!!!</b>", $"{playerName} grabbed the pole too soon. The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.", "Retry")));
                    }
                }
                else
                {
                    if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                    {
                        cameraScript.isStartOfStunt = false;
                        questionController.answerIsCorrect = false;
                        cameraScript.directorIsCalling = true;
                        messageTxt = $"{playerName} grabbed the pole too late! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.";
                        // StartCoroutine(StuntResult(() => questionController.ActivateResult($"{playerName} grabbed the pole too late! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.",false,false)));
                        //ToggleModal($"<b>Stunt Failed!!!</b>", $"{playerName} grabbed the pole too late! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.", "Retry")));
                    }
                }
                isEndOfStunt = true;
                isSimulating = false;
                life.ReduceLife();
            }
        }
        else
        {                //platformBarBottom.transform.position = new Vector3(spawnPointValue.x - 9, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - distance, 1);            
            isSimulating = false;
            questionController.timer = $"{(elapsed).ToString("f2")}s";
        }
        if (isEndOfStunt)
        {
            if (ShowResult)
            {
                if (answerIsCorrect == true)
                {
                    StartCoroutine(StuntResult(() => questionController.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and able grab at the branch"), true, false)));
                }
                if (answerIsCorrect == false)
                {
                    StartCoroutine(StuntResult(() => questionController.ActivateResult((PlayerPrefs.GetString("Name") + " has failed to performed the stunt and not able grab at the branch"), false, false)));
                }
                ShowResult = false;
            }

        }
        if (questionController.isSimulating)
            StartSimulation();
        else if (questionController.nextStage)
            GotoNextScene();
        else if (questionController.retried)
            ResetLevel();
        else
        {
            questionController.nextStage = false;
            questionController.retried = false;
        }
    }
    public IEnumerator DirectorsCall()
    {
        directorIsCalling = false;
        if (isStartOfStunt)
        {
            // isStartOfStunt = false;
            callout.SetActive(true);
            calloutText.SetText("Lights!");
            yield return new WaitForSeconds(0.75f);
            calloutText.SetText("Camera!");
            yield return new WaitForSeconds(0.75f);
            calloutText.SetText("Action!");
            yield return new WaitForSeconds(0.75f);
            calloutText.SetText("");
            callout.SetActive(false);
            isSimulating = true;
        }
        else
        {
            calloutText.SetText("Cut!");
            callout.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            callout.SetActive(false);
        }
    }
}
