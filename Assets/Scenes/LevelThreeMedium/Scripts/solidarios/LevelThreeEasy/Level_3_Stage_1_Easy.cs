using System;
using System.Collections;
using GameConfig;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_3_Stage_1_Easy : MonoBehaviour
{
    // Stunt Guide
    public Text stuntGuideTextObject;
    public string stuntGuideText;
    public GameObject stuntGuideObjectPrefab;
    public Image stuntGuideImage;
    public Sprite stuntGuideImageSprite;
    // End of Stunt Guide
    string question;

    float lastHitYPosition;

    GameObject[] ropeBones;

    public float elapsed;

    // public TMP_Text playerNameText, timerText, questionText, levelName,
    public TMP_Text timeGivenContainer, calloutText;

    public GameObject AfterStuntMessage;

    Animator thePlayerAnimation;

    // public TMP_InputField playerAnswer;
    public GameObject playerOnRope;

    public Annotation annotation;

    public GameObject playerHingeJoint;

    public GameObject thePlayer;

    public GameObject playerHangingFixed;

    public GameObject callout;

    Vector3 thePlayer_position;

    public GameObject accurateCollider;

    public GameObject platformBar;

    Vector3 playerOnRopeTransform;

    float timeGiven;

    Vector2 gravityGiven;

    float correctAnswer;

    Vector2 spawnPointValue;

    bool respositioned = false, isAnswerCorrect;

    float distance;

    bool letGoRope = false;

    float playerOnRopeInitialY;

    float accurateColliderInitialPointY;

    public GameObject timerAnnotation;
    public Button play;

    public QuestionControllerVThree questionController;

    string playerName = "Junjun";

    string pronoun, pPronoun;

    string answerUnit, messageTxt;

    bool metTargetTime = false, isStartOfStunt, directorIsCalling, isSimulating, isEndOfStunt;

    float globalTime, answer;
    public HeartManager life;
    bool showResult;

    StageManager sm = new StageManager();

    public TMP_Text debugAnswer;
    public AudioSource lightsSfx, cameraSfx, actionSfx, cutSfx;
    public FirebaseManager firebaseManager;

    float adjustedAnswer;


    void Start()
    {
        firebaseManager.GameLogMutation(3, 1, "Easy", Actions.Started, 0);

        showResult = true;
        ropeBones = GameObject.FindGameObjectsWithTag("RopeBones");
        // Given
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(1f, 1.5f), 2);
        gravityGiven = Physics2D.gravity;

        // Formula
        correctAnswer = Mathf.Abs((gravityGiven.y / 2) * Mathf.Pow(timeGiven, 2));


        annotation.SetDistance(correctAnswer);
        annotation.revealValue = false;
        annotation.SetSpawningPoint(new Vector2(15, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - correctAnswer));


        // Debug.Log($"Distance: {correctAnswer}");
        // Debug.Log($"Hinge: {playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y}");
        // Debug.Log($"Time Generated: {timeGiven}");
        // Debug.Log($"Correct Answer: {System.Math.Round(correctAnswer, 2)}");

        //Problem
        // levelName.SetText("Stage 1");
        sm.SetGameLevel(3);
        questionController.stage = 1;
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
        //question = $"{playerName} is hanging from a rope and {pronoun} is instructed to let go of it, drop down, and hang again to the horizontal pole below. If {playerName} will let go ang grab the pole after exactly <b><color=#006d00>{timeGiven} sec</color></b>, at what <b><color=#006d00>distance</color></b> should {pronoun} hands above the pole before letting go?";
        question = $"<b>{playerName}</b> is instructed to hand from a rope, let go of it, and grab the branch below as {pronoun} falls. If {pronoun} will let go of the rope and will grab the branch at exactly <b>{timeGiven}</b> sec after with the hands in the same position while falling, at what distance should {pPronoun} hands above the branch before letting go?";

        // if (questionText != null)
        // {
        questionController.SetQuestion(question);

        // }
        // else
        // {
        //     Debug.Log("QuestionText object not loaded.");
        // }
        thePlayerAnimation = thePlayer.GetComponent<Animator>();

        thePlayerAnimation.SetBool("isHanging", true);

        thePlayer_position = thePlayer.transform.position;

        distance = annotation.distance;

        playerOnRopeTransform = playerOnRope.transform.position;

        spawnPointValue = annotation.SpawnPointValue();

        platformBar.transform.position = new Vector3(spawnPointValue.x - 9, spawnPointValue.y, 0);

        accurateColliderInitialPointY = accurateCollider.transform.position.y;

        playerOnRopeInitialY = (float)Math.Round(playerOnRope.transform.position.y, 2);

        timeGivenContainer.SetText($"Time: {timeGiven}s");
        questionController.limit = playerHingeJoint.transform.position.y - platformBar.transform.position.y;
        questionController.answerUnit = " m";
        // questionController.timer = timeGiven.ToString("f2");
    }

    void ResetLevel()
    {
        play.interactable = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void GotoNextScene()
    {
        SceneManager.LoadScene("LevelThreeStage2");
    }

    void Play()
    {
        answer = questionController.GetPlayerAnswer();
        adjustedAnswer = questionController.AnswerTolerance(correctAnswer);
        questionController.isSimulating = false;
        directorIsCalling = true;
        isStartOfStunt = true;
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

    void RepositionRopeComplete()
    {
        this.respositioned = true;
    }

    void FallFromRope()
    {
        this.letGoRope = true;
    }

    // public void StartSimulation()
    // {
    //     cameraScript.directorIsCalling = true;
    //     cameraScript.isStartOfStunt = true;
    //     questionController.SetAnswer();
    // }
    public string GetUnit()
    {
        return answerUnit;
    }

    void FixedUpdate()
    {
        //Stunt Guide
        stuntGuideImage.sprite = stuntGuideImageSprite;
        stuntGuideTextObject.text = stuntGuideText;
        
        questionController.errorText = "answer must not exceed your current distance from the branch";
        debugAnswer.SetText($"Answer: {System.Math.Round(correctAnswer, 2)}");

        if (directorIsCalling)
            StartCoroutine(DirectorsCall());

        float correct = (float)Math.Round(correctAnswer, 2);

        if (isSimulating)
        {
            play.interactable = false;
            elapsed += Time.fixedDeltaTime;
            float playerOnRopeY = (float)Math.Round(playerOnRope.transform.position.y, 2);

            if (adjustedAnswer != correct)
            {
                if (adjustedAnswer > correct)
                {
                    float diff = adjustedAnswer - correct;
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
                    //Debug.Log($"rope: {playerOnRopeY} | correct: {correct} | adjustedAnswer: {adjustedAnswer}");
                }
                else
                {
                    float diff = correct - adjustedAnswer;
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
                    thePlayer.SetActive(false);
                    playerHangingFixed.SetActive(true);
                    playerHangingFixed.GetComponent<Animator>().SetBool("isHangingInBar", true);
                }
            }

            annotation.Hide();

            if (respositioned)
            {
                playerHingeJoint.GetComponent<HingeJoint2D>().enabled = false;
                thePlayerAnimation.SetBool("isFalling", true);

                // Correct Answer
                if (System.Math.Round(adjustedAnswer, 2) == System.Math.Round(correctAnswer, 2))
                {
                    Debug.Log("Distance is correct!");
                    if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                    {
                        thePlayer.SetActive(false);
                        playerHangingFixed.SetActive(true);
                        playerHangingFixed.transform.position = new Vector3(spawnPointValue.x - 0.2f, platformBar.transform.position.y - 1f, 1);
                        platformBar.GetComponent<Animator>().SetBool("collided", true);
                        playerHangingFixed.GetComponent<Animator>().SetBool("isHangingInBar", true);
                        questionController.answerIsCorrect = true;
                        messageTxt = $"<b>{playerName}</b> safely grabbed the pole!";
                        isAnswerCorrect = true;
                        isEndOfStunt = true;
                        isSimulating = false;
                        // StartCoroutine(StuntResult(() => questionController.ActivateResult($"{playerName} safely grabbed the pole!", true, false)));

                        //ToggleModal($"<b>Stunt Success!!!</b>", $"{playerName} safely grabbed the pole!", "Next")));
                    }
                }
                else
                {
                    isAnswerCorrect = false;
                    isEndOfStunt = true;
                    isSimulating = false;
                    if (adjustedAnswer < System.Math.Round(correctAnswer, 2))
                    {
                        if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                        {
                            Debug.Log("Distance is too short!");
                            questionController.answerIsCorrect = false;
                            messageTxt = $"<b>{playerName}</b> hand distance to the pole is shorter! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.";
                            // StartCoroutine(StuntResult(() => questionController.ActivateResult($"{playerName} hand distance to the pole is shorter! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.", false, false)));
                            //ToggleModal($"<b>Stunt Failed!!!</b>", $"{playerName} hand distance to the pole is shorter! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.", "Retry")));
                        }
                    }
                    else
                    {
                        if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                        {
                            Debug.Log("Distance is too long!");
                            questionController.answerIsCorrect = false;
                            messageTxt = $"<b>{playerName}</b> hand distance to the pole is longer! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.";
                            // StartCoroutine(StuntResult(() => questionController.ActivateResult($"{playerName} hand distance to the pole is longer! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.", false, false)));
                            //ToggleModal($"<b>Stunt Failed!!!</b>", $"{playerName} hand distance to the pole is longer! The correct answer is <b>{System.Math.Round(correctAnswer, 2)}</b>.", "Retry")));
                        }
                    }
                    life.ReduceLife();
                }
            }
            questionController.timer = $"{(elapsed).ToString("f2")}s";
        }
        else
        {
            // platformBar.transform.position = new Vector3(spawnPointValue.x - 9, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - distance, 1);
            if (respositioned)
            {
                questionController.timer = $"{(timeGiven).ToString("f2")}s";
                // timerAnnotation.GetComponent<TMP_Text>().text = $"t={(timeGiven).ToString("f2")}s";
            }
            // isSimulating = false;
        }

        if (respositioned)
        {
            if (System.Math.Round(globalTime / 1000, 2) <= timeGiven)
            {
                globalTime += Time.fixedTime;

                // timerAnnotation.GetComponent<TMP_Text>().text = $"t={(globalTime / 1000).ToString("f2")}s";
                //timerAnnotation.GetComponent<TMP_Text>().text = $"- Grabbed here!";
                if (questionController.answerIsCorrect)
                {
                    if (!accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                    {
                        timerAnnotation.transform.position = new Vector3(17, platformBar.transform.position.y, 1);
                    }
                    else
                    {
                        timerAnnotation.transform.position = new Vector3(17, playerHingeJoint.transform.position.y, 1);
                    }
                }
                else
                {
                    timerAnnotation.transform.position = new Vector3(17, playerHingeJoint.transform.position.y, 1);
                }
            }
            else
            {
                timerAnnotation.SetActive(true);
            }

            // timerAnnotation.GetComponent<TMP_Text>().text = $"t={(timeGiven).ToString("f2")}s";
        }
        if (isEndOfStunt)
        {
            if (showResult)
            {
                if (isAnswerCorrect == true)
                {
                    StartCoroutine(StuntResult(() => questionController.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and able grab at the branch"), true, false)));
                    showResult = false;
                }
                if (isAnswerCorrect == false)
                {
                    StartCoroutine(StuntResult(() => questionController.ActivateResult((PlayerPrefs.GetString("Name") + " has failed to performed the stunt and not able grab at the branch"), false, false)));
                    showResult = false;
                }

            }
        }
        if (questionController.isSimulating)
            Play();
        else if (questionController.nextStage)
            GotoNextScene();
        else if (questionController.retried)
            ResetLevel();
        else
        {
            questionController.nextStage = false;
            questionController.retried = false;
        }

        // if (elapsed == timeGiven)
        // {
        //     Debug.Log("Freeze");
        //     //timerAnnotation.transform.position = new Vector3(17, lastHitYPosition, 1);
        // }
        // else
        // {
        //     //timerAnnotation.transform.position = new Vector3(17, playerHingeJoint.transform.position.y, 1);
        //     //lastHitYPosition = playerHingeJoint.transform.position.y;
        // }
        // }
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
