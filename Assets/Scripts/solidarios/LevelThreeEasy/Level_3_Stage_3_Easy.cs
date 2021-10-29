using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using GameConfig;
using UnityEngine.UI;

public class Level_3_Stage_3_Easy : MonoBehaviour
{
    // Start is called before the first frame update
    string question;
    public float elapsed;
    public GameObject platformCollider, callout;
    public TMP_Text calloutText;
    public GameObject AfterStuntMessage;
    Animator thePlayerAnimation;
    public TMP_InputField playerAnswer;
    bool isSimulating = false, isEndOfStunt, directorIsCalling, isStartOfStunt, answerIsCorrect, isComplete;
    public GameObject playerHingeJoint;
    public GameObject thePlayer;
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
    public AudioSource lightsSfx, cameraSfx, actionSfx, cutSfx;
    public FirebaseManager firebaseManager;

    void Start()
    {
        firebaseManager.GameLogMutation(3,3, "Easy", Actions.Started, 0); 
        // Given 
        ShowResult = true;
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(20f, 25f), 2);
        distanceGiven = (float)System.Math.Round(UnityEngine.Random.Range(25f, 28.0f), 2);
        gravityGiven = Physics2D.gravity;

        // Formula
        correctAnswer = Mathf.Sqrt(Mathf.Abs((2 * distanceGiven) / gravityGiven.y));
        questionController.limit = correctAnswer + 1;

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
        question = $"{playerName} is finally instructed to let go of the branch that {pronoun} is hanging from and grab the crossing vine below to soften {pronoun} landing into the ground. If the distance between the branch and the vine is exactly {distanceGiven} meters vertically, how long should {pronoun} fall down before grabbing the vine?";

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
            play.interactable = false;
            // float answer = float.Parse(playerAnswer.text.Split(new string[] { questionController.GetUnit() }, System.StringSplitOptions.None)[0]);
            annotation.Hide();
            elapsed += Time.fixedDeltaTime;
            questionController.timer = elapsed.ToString("f2") + "s";

            playerHingeJoint.GetComponent<HingeJoint2D>().enabled = false;
            thePlayerAnimation.SetBool("isFalling", true);

            // Correct Answer
            if (System.Math.Round(answer, 2) == System.Math.Round(correctAnswer, 2))
            {
                answerIsCorrect = true;
                Debug.Log("Time is correct!");
                if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                {
                    platformCollider.SetActive(false);
                    playerHangingBottom.SetActive(true);
                    thePlayer.SetActive(false);
                    playerHangingFixed.SetActive(true);
                    playerHangingFixed.GetComponent<Animator>().SetBool("isHangingInBar", true);
                    elapsed -= 0.02f;

                    cameraScript.isStartOfStunt = false;
                    questionController.answerIsCorrect = true;
                    messageTxt = $"{playerName} safely grabbed the pole!";
                    isEndOfStunt = true;
                    isSimulating = false;
                    isComplete = true;
                    // StartCoroutine(StuntResult(() => questionController.ToggleModal($"<b>Stunt Success!!!</b>", $"{playerName} safely grabbed the pole!", "Next")));
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
                        questionController.answerIsCorrect = false;
                        cameraScript.directorIsCalling = true;
                        messageTxt = $"{playerName} grabbed the pole too soon!";
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
                        messageTxt = $"{playerName} grabbed the pole too late!";
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
                    StartCoroutine(StuntResult(() => questionController.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and able grab at the branch"), true, true)));
                    ShowResult = false;
                }
                if (answerIsCorrect == false)
                {
                    StartCoroutine(StuntResult(() => questionController.ActivateResult((PlayerPrefs.GetString("Name") + " has failed to performed the stunt and not able grab at the branch"), false, false)));
                    ShowResult = false;
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
