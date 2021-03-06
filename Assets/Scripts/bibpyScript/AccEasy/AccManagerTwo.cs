using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccManagerTwo : MonoBehaviour
{
    // Stunt Guide
    public GameObject[] stuntGuideObjectPrefabs;
    public Image stuntGuideImage;
    public Sprite stuntGuideImageSprite;
    // End of Stunt Guide
    AnswerGuards answerGuards = new AnswerGuards();
    string stuntResultMessage;
    public float Vi;
    public float Vf;
    public float timer;
    public float time;
    float generateAcceleration;
    public float deacceleration;
    private PlayerB thePlayer;
    bool answerIsCorrect;
    private accSimulation theSimulation;
    private BikeManager theBike;
    public bool gas, follow;
    public float correctAns;
    float generateAns;
    float playerDistance;
    float playerVf;
    float currentPos;
    public float correctDistance;
    string pronoun, pronoun2;
    string gender;
    public GameObject walls, bikeInitials, cam, directionArrow;
    public TMP_Text Vitxt, Vftxt, Acctxt, actiontxt;
    private HeartManager theHeart;
    private Vector2 bikeInitialStartPos;
    public QuestionControllerAcceleration theQuestion;
    public TMP_Text debugAnswer;
    public AudioSource engineIdle, engineRunning;
    bool setAnswer;
    float min, max;

    // Start is called before the first frame update
    void Start()
    {
        engineRunning.Stop();
        cam.transform.position = new Vector3(17.5f, cam.transform.position.y, cam.transform.position.z);
        thePlayer = FindObjectOfType<PlayerB>();
        theBike = FindObjectOfType<BikeManager>();
        gender = PlayerPrefs.GetString("Gender");
        theSimulation = FindObjectOfType<accSimulation>();
        theHeart = FindObjectOfType<HeartManager>();
        bikeInitialStartPos = bikeInitials.transform.position;
        theSimulation.stage = 2;
        if (gender == "Male")
        {
            pronoun = ("he");
            pronoun2 = ("his");
        }
        if (gender == "Female")
        {
            pronoun = ("she");
            pronoun2 = ("her");
        }
        generateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Stunt Guide        
        stuntGuideObjectPrefabs[0].SetActive(false);
        stuntGuideObjectPrefabs[1].SetActive(true);
        stuntGuideObjectPrefabs[2].SetActive(false);
        stuntGuideImage.sprite = stuntGuideImageSprite;

        min = correctAns - 0.02f;
        max = correctAns + 0.02f;
        debugAnswer.SetText($"Answer: {correctAns}");
        Vftxt.text = ("Vf = ") + Vf.ToString("F2") + ("m/s");
        playerVf = (-deacceleration * theQuestion.GetPlayerAnswer()) + Vi;
        currentPos = theBike.transform.position.x;
        if (theBike.brake == true)
        {
            timer += Time.fixedDeltaTime;
            theQuestion.timer = timer.ToString("F2") + ("s");
            // if (float.Parse(theQuestion.timer) <= theQuestion.GetPlayerAnswer())
            // {

            //     // timertxt.text = timer.ToString("F2") + ("s");
            // }
            // if (timer >= theQuestion.GetPlayerAnswer())
            // {
            //     // timertxt.text = theQuestion.GetPlayerAnswer().ToString("F2") + ("s");
            // }
        }
        if (follow)
        {
            bikeInitials.transform.position = theBike.transform.position;
        }
        if (currentPos >= 28)
        {
            follow = false;
        }

        if (gas)
        {
            theBike.moveSpeed = Vi;
        }

        if (theQuestion.isSimulating)
        {
            if (accSimulation.playerAnswer < max & accSimulation.playerAnswer > min)
            {
                time = correctAns;
                Debug.Log("inRange");

            }
            else
            {
                time = theQuestion.GetPlayerAnswer();
            }
            setAnswer = true;
        }
        if (setAnswer)
        {
            theQuestion.isSimulating = false;
            Debug.Log("Now simulating...");
            directionArrow.SetActive(false);
            gas = true;

            if (time != correctAns)
            {
                // actiontxt.text = "retry";
                answerIsCorrect = false;
                walls.SetActive(true);
                accSimulation.playerDead = true;
                //theQuestion.SetModalTitle("Stunt failed");

                if (time < correctAns & time > correctAns - 0.5f)
                {
                    time -= 0.5f;
                }
                if (time > correctAns & time < correctAns + 0.5f)
                {
                    time += 0.5f;
                }
                if (time < correctAns)
                {
                    stuntResultMessage = $"{PlayerPrefs.GetString("Name")} deccelerates the motorcycle too fast and overshot the tunnel entrance. The correct answer is </color> {correctAns.ToString("F2")} seconds.";
                }
                if (time > correctAns)
                {
                    stuntResultMessage = $"{PlayerPrefs.GetString("Name")} deccelerates the motorcycle too slow and undershot the tunnel entrance. The correct answer is </color> {correctAns.ToString("F2")} seconds.";
                }
            }

            if (answerGuards.AnswerIsInRange(correctAns, time, 0.01f))
            {
                actiontxt.text = "Next";
                stuntResultMessage = $"The correct answer is <b> {correctAns.ToString("F2")} </b> seconds. {PlayerPrefs.GetString("Name")} was able to enter tunnel succesfully!</color>";
                answerIsCorrect = true;

            }
            if (theBike.brake == true)
            {

                Acctxt.text = ("a = -") + deacceleration.ToString("F2") + ("m/s??");
                gas = false;

                if (time > correctAns)
                {

                    if (timer < time)
                    {

                        if (currentPos <= 32)
                        {
                            theBike.moveSpeed -= deacceleration * Time.fixedDeltaTime;
                            if (theBike.moveSpeed <= 1f)
                            {
                                theBike.moveSpeed = theBike.myRigidbody.velocity.x;
                            }
                            if (timer >= time - .5f)
                            {
                                Vitxt.text = ("V = ") + playerVf.ToString("F2") + ("m/s");
                            }
                            if (timer <= time - .5f)
                            {
                                Vitxt.text = ("V = ") + theBike.moveSpeed.ToString("F2") + ("m/s");
                            }
                        }
                    }
                }
                if (time <= correctAns)
                {
                    if (timer < time)
                    {
                        Vitxt.text = ("v = ") + theBike.moveSpeed.ToString("F2") + ("m/s");
                        theBike.moveSpeed -= deacceleration * Time.fixedDeltaTime;
                        if (theBike.moveSpeed <= 1f)
                        {
                            theBike.moveSpeed = theBike.myRigidbody.velocity.x;
                        }
                    }
                    if (timer >= time)
                    {
                        Vitxt.text = ("v = ") + playerVf.ToString("F2") + ("m/s");
                    }
                }
            }
            if (currentPos > 32)
            {
                theBike.moveSpeed = theBike.myRigidbody.velocity.x;
                setAnswer = false;
                //theBike.moveSpeed = theBike.myRigidbody.velocity.x;
                if (answerGuards.AnswerIsInRange(correctAns, time, 0.01f))
                {
                    StartCoroutine(StuntResult(stuntResultMessage, answerIsCorrect));
                }
                else
                {
                    StartCoroutine(StuntResult(stuntResultMessage, answerIsCorrect));
                }
            }
            //Debug.Log($"Current Pos: {currentPos}");

        }
        else
        {
            theQuestion.timer = "0.00s";
        }
    }
    public void generateProblem()
    {
        engineIdle.Play();
        bikeInitials.transform.position = bikeInitialStartPos;
        Vi = Random.Range(15, 20);
        generateAns = 60 / (Vi + 10);
        correctAns = (float)System.Math.Round(generateAns, 2);
        deacceleration = -(10 - Vi) / correctAns;
        //time = (float)System.Math.Round(generateTime, 2);
        string question = ("<b>" + PlayerPrefs.GetString("Name") + ("</b> is instructed to drive ") + pronoun2 + (" motorcycle into another tunnel across the platform. To do this,") + pronoun2 + (" motorcycle must have an exact velocity of <b>10.00</b> m/s before it jumps off the edge. If ") + PlayerPrefs.GetString("Name") + (" is currently cruising at constant velocity of <b>") + Vi.ToString("F1") + ("</b> m/s and apply the brakes will constantly deaccelerate the motorcycle by <b>") + deacceleration.ToString("F1") + ("</b> m/s??,how long should ") + PlayerPrefs.GetString("Name") + (" apply the brakes before jumping off to successfully perform the stunt?"));
        theQuestion.SetQuestion(question);
        Debug.Log(question);
        theBike.transform.position = new Vector2(-17, .2f);
        theHeart.losslife = false;
        walls.SetActive(false);
        Vitxt.text = ("vi = ") + Vi.ToString("F2") + ("m/s");
        Acctxt.text = ("a = -") + deacceleration.ToString("F2") + ("m/s??");
        // timertxt.text = timer.ToString("F2") + ("s");
        directionArrow.SetActive(true);
    }
    IEnumerator StuntResult(string message, bool isCorrect)
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(theSimulation.DirectorsCall());
        yield return new WaitForSeconds(2);
        engineRunning.Stop();
        theQuestion.ActivateResult(message, isCorrect);
        walls.SetActive(false);
        theBike.moveSpeed = 0;
    }
}