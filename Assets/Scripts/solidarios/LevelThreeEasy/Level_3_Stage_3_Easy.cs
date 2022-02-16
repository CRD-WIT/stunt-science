using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using GameConfig;
using UnityEngine.UI;

public class Level_3_Stage_3_Easy : MonoBehaviour
{
    // Stunt Guide
    public GameObject[] stuntGuideObjectPrefabs;
    public Image stuntGuideImage;
    public Sprite stuntGuideImageSprite;
    // End of Stunt Guide
    string question;
    public float elapsed;
    public GameObject platformCollider, callout;
    public TMP_Text calloutText;
    public GameObject AfterStuntMessage;
    Animator thePlayerAnimation;
    public TMP_InputField playerAnswer;
    bool isSimulating = false, isEndOfStunt, directorIsCalling, isStartOfStunt, answerIsCorrect, isComplete;
    public GameObject playerHingeJoint;
    public GameObject thePlayer, timer, shadow;
    public GameObject playerHangingFixed;
    public GameObject FirstCamera;
    public GameObject SecondCamera;
    Vector3 thePlayer_position;
    public GameObject accurateCollider;
    public GameObject platformBarTop;
    string playerName = "Junjun";
    string pronoun = "he", messageTxt, pPronoun;
    public GameObject playerHangingBottom;
    public Button play;
    float timeGiven;
    Vector2 gravityGiven;
    float correctAnswer;
    Vector2 spawnPointValue;
    float distance, answer;
    float distanceGiven;
    public QuestionControllerVThree questionController;
    public CameraScript cameraScript;
    public Annotation annotation;
    public HeartManager life;
    public TMP_Text debugAnswer;
    bool ShowResult;
    public AudioSource lightsSfx, cameraSfx, actionSfx, cutSfx, hitSfx;
    public FirebaseManager firebaseManager;
    float adjustedAnswer;
    public bool timestart;
    public hitCollide theCollide;

    void Start()
    {

        theCollide.collide = true;
        firebaseManager.GameLogMutation(3, 3, "Easy", Actions.Started, 0);
        // Given 
        ShowResult = true;
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(20f, 25f), 2);
        distanceGiven = (float)System.Math.Round(UnityEngine.Random.Range(25f, 28.0f), 2);
        gravityGiven = Physics2D.gravity;

        // Formula
        correctAnswer = Mathf.Sqrt(Mathf.Abs((2 * distanceGiven) / gravityGiven.y));
        questionController.limit = 5;

        annotation.SetDistance(distanceGiven);

        //Debug.Log($"Correct Answer: {System.Math.Round(correctAnswer, 2)}");

        //Problem
        // levelName.SetText("Free Fall | Stage 3"); sm.SetGameLevel(3);
        questionController.stage = 3;
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
        //question = $"{playerName} is hanging on a horizontal pole and {pronoun} is instructed to let go of it, drop down, and grab the elastic cord below to slow down his fall and safely land him into the ground. If the hands of {playerName} is exactly <color=#006A11>{distanceGiven}</color> meters above the second pole, <color=red>how long</color> should {playerName} fall down before {pronoun} grabs the second pole?";
        question = $"<b>{playerName}</b> is finally instructed to let go of the branch that {pronoun} is hanging from and grab the crossing vine below to soften {pronoun} landing into the ground. If the distance between the branch and the vine is exactly {distanceGiven} meters vertically, how long should {pronoun} fall before grabbing the vine?";
        Debug.Log(question);

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
        playerHingeJoint.transform.position = new Vector3(spawnPointValue.x - 2, distanceGiven - (spawnPointValue.y * -1), 0);

        platformBarTop.transform.position = new Vector3(spawnPointValue.x - 8, distanceGiven - (spawnPointValue.y * -1), 0);
        questionController.answerUnit = " sec";
    }

    IEnumerator StuntResult(Action callback)
    {
        //messageFlag = false;
        yield return new WaitForSeconds(2f);

        callback();
    }

    public void ResetLevel()
    {
        play.interactable = true;
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
        // cameraScript.directorIsCalling = true;
        // cameraScript.isStartOfStunt = true;
        // questionController.SetAnswer();
        answer = questionController.GetPlayerAnswer();
        adjustedAnswer = questionController.AnswerTolerance(correctAnswer);
        questionController.isSimulating = false;
        directorIsCalling = true;
        isStartOfStunt = true;
    }

    void FixedUpdate()
    {
        //Stunt Guide        
        stuntGuideObjectPrefabs[0].SetActive(false);
        stuntGuideObjectPrefabs[1].SetActive(false);
        stuntGuideObjectPrefabs[2].SetActive(true);

        questionController.errorText = "answer must not exceed 5 seconds";
        debugAnswer.SetText($"Answer: {System.Math.Round(correctAnswer, 2)}");
        if (timestart)
        {
            elapsed += Time.fixedDeltaTime;
            if (elapsed >= answer)
            {

                // if(answer != correctAnswer)
                // {
                //     isSimulating = false;
                // }
                shadow.SetActive(true);
                shadow.transform.position = thePlayer.transform.position;
                elapsed = answer;
                timestart = false;
            }
        }
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        if (isSimulating)
        {
            play.interactable = false;
            // float answer = float.Parse(playerAnswer.text.Split(new string[] { questionController.GetUnit() }, System.StringSplitOptions.None)[0]);
            annotation.Hide();
            timestart = true;
            questionController.timer = elapsed.ToString("f2") + "s";
            timer.transform.position = thePlayer.transform.position;
            playerHingeJoint.GetComponent<HingeJoint2D>().enabled = false;
            thePlayerAnimation.SetBool("isFalling", true);

            // Correct Answer
            if (System.Math.Round(adjustedAnswer, 2) == System.Math.Round(correctAnswer, 2))
            {
                answerIsCorrect = true;
                Debug.Log("Time is correct!");
                if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                {
                    hitSfx.Play();
                    platformCollider.SetActive(false);
                    playerHangingBottom.SetActive(true);
                    thePlayer.SetActive(false);
                    playerHangingFixed.SetActive(true);
                    playerHangingFixed.GetComponent<Animator>().SetBool("isHangingInBar", true);
                    elapsed -= 0.02f;

                    cameraScript.isStartOfStunt = false;
                    questionController.answerIsCorrect = true;
                    //messageTxt = $"<b>{playerName}</b> safely grabbed the pole!";
                    messageTxt = $"{playerName} grabbed the vine at the precise time and successfully hangs from it! Stunt successfully executed!";
                    isEndOfStunt = true;
                    isSimulating = false;
                    isComplete = true;
                    // StartCoroutine(StuntResult(() => questionController.ToggleModal($"<b>Stunt Success!!!</b>", $"{playerName} safely grabbed the pole!", "Next")));
                }
            }
            else
            {
                answerIsCorrect = false;
                if (adjustedAnswer < System.Math.Round(correctAnswer, 2))
                {
                    if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                    {
                        Debug.Log("Distance is too short!");
                        questionController.answerIsCorrect = false;
                        cameraScript.directorIsCalling = true;
                        //messageTxt = $"<b>{playerName}</b> grabbed the pole too soon!";
                        messageTxt = $"{playerName} grabbed vine too soon and missed grabbing it. Stunt failed! The correct answer is {System.Math.Round(correctAnswer, 2)} seconds.";
                        // StartCoroutine(StuntResult(() => questionController.ToggleModal($"<b>Stunt Failed!!!</b>", $"{playerName} grabbed the pole too soon!", "Retry")));
                    }
                }
                else
                {
                    if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                    {
                        Debug.Log("Distance is too long!");
                        questionController.answerIsCorrect = false;
                        cameraScript.directorIsCalling = true;
                        //messageTxt = $"<b>{playerName}</b> grabbed the pole too late!";
                         messageTxt = $"{playerName} grabbed vine too late and missed grabbing it. Stunt failed! The correct answer is {System.Math.Round(correctAnswer, 2)} seconds.";
                        // StartCoroutine(StuntResult(() => questionController.ToggleModal($"<b>Stunt Failed!!!</b>", $"{playerName} grabbed the pole too late!", "Retry")));
                    }
                }
                StartCoroutine(ShowModal());
            }
        }
        else
        {
            //platformBarBottom.transform.position = new Vector3(spawnPointValue.x - 9, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - distance, 1);            
            // isSimulating = false;

            questionController.timer = $"{(elapsed).ToString("f2")}s";
        }
        if (isEndOfStunt)
        {
            if (ShowResult)
            {
                if (answerIsCorrect == true)
                {
                     StartCoroutine(StuntResult(() => questionController.ActivateResult((PlayerPrefs.GetString("Name") + " grabbed the lower branch at the precise time and successfully hang from it."), true, true)));
                    ShowResult = false;
                }
                if (answerIsCorrect == false)
                {
                    if (answer > correctAnswer)
                    {
                        StartCoroutine(StuntResult(() => questionController.ActivateResult((PlayerPrefs.GetString("Name") + " grabbed the lower branch too late and missed grabbing it. The correct answer is <b>" + correctAnswer.ToString("F2") + " seconds</b>."), false, false)));
                    }
                    if (answer < correctAnswer)
                    {
                        StartCoroutine(StuntResult(() => questionController.ActivateResult((PlayerPrefs.GetString("Name") + " grabbed the lower branch too soon and missed grabbing it. The correct answer is <b>" + correctAnswer.ToString("F2") + " seconds</b>."), false, false)));
                    }
                }

            }
        }
        if (questionController.isSimulating)
            StartSimulation();
        else if (questionController.retried)
            ResetLevel();
        else
        {
            questionController.nextStage = false;
            questionController.retried = false;
        }
    }
    public IEnumerator ShowModal()
    {
        yield return new WaitForSeconds(2);
        isSimulating = false;
        if (answerIsCorrect == false)
        {
            life.ReduceLife();
        }
        isComplete = false;
        yield return new WaitForSeconds(.2f);
        isEndOfStunt = true;


    }
    public IEnumerator DirectorsCall()
    {
        directorIsCalling = false;
        if (isStartOfStunt)
        {
            // isStartOfStunt = false;
            callout.SetActive(true);
            calloutText.SetText("Lights!");
            lightsSfx.Play();
            yield return new WaitForSeconds(0.75f);
            calloutText.SetText("Camera!");
            cameraSfx.Play();
            yield return new WaitForSeconds(0.75f);
            calloutText.SetText("Action!");
            actionSfx.Play();
            yield return new WaitForSeconds(0.75f);
            calloutText.SetText("");
            callout.SetActive(false);
            isSimulating = true;
            hitSfx.Play();
        }
        else
        {
            calloutText.SetText("Cut!");
            cutSfx.Play();
            callout.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            callout.SetActive(false);
        }
    }
}
