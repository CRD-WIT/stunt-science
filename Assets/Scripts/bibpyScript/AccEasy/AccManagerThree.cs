using System.Collections;
using UnityEngine;
using TMPro;

public class AccManagerThree : MonoBehaviour
{
    public float Vi;
    public float Vf;
    public float timer;
    public float time;
    float generateTime;
    float generateAcceleration;
    public float deacceleration;
    private PlayerB thePlayer;
    private accSimulation theSimulation;
    private BikeManager theBike;
    public bool gas;
    bool addingAcc;
    public float correctAns;
    float generateAns;
    float playerDistance;
    float playerVf;
    float currentPos;
    public float correctDistance;
    string pronoun2,pronoun;
    string gender;
    public GameObject walls, stopper, bikeInitials, actionButton, cam, directionArrow;

    public TMP_Text velocitytxt, Vftxt, Acctxt, timertxt, actiontxt, vitxt;

    public TruckManager theTruck;
    bool answerIsCorrect;
    string stuntResultMessage;
    Vector2 truckPosition;
    private HeartManager theHeart;
    private ScoreManager theScorer;
    int currentLevel;
    int currentStar;
    private Vector2 bikeInitialStartPos;
    public TMP_Text debugAnswer;
    public QuestionControllerAcceleration theQuestion;
    Quaternion truckStartRot;
    public AudioSource engineIdle, engineRunning,truckEngine;


    // Start is called before the first frame update
    void Start()
    {
        engineRunning.Stop();
        timer = 0;
        //theQuestion.stageNumber = 3;
        cam.transform.position = new Vector3(18f, cam.transform.position.y, cam.transform.position.z);
        //thePlayer = FindObjectOfType<Player>();
        theBike = FindObjectOfType<BikeManager>();
        gender = PlayerPrefs.GetString("Gender");
        theSimulation = FindObjectOfType<accSimulation>();
        theHeart = FindObjectOfType<HeartManager>();
        theScorer = FindObjectOfType<ScoreManager>();
        truckPosition = theTruck.transform.position;
        currentLevel = PlayerPrefs.GetInt("level");
        currentStar = PlayerPrefs.GetInt("AcstarE");
        bikeInitialStartPos = bikeInitials.transform.position;
        theSimulation.stage = 3;
        truckStartRot = theTruck.transform.rotation;
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
        correctAns = (float)System.Math.Round(generateAns, 2);
        debugAnswer.SetText($"Answer: {correctAns}");
        playerDistance = (time * time) * deacceleration / 2;
        playerVf = (2 * playerDistance) / time;
        currentPos = theBike.transform.position.x;

        Vi = theQuestion.GetPlayerAnswer();
        if (gas)
        {
            theBike.moveSpeed = Vi;
            velocitytxt.text = ("v = ") + theBike.moveSpeed.ToString("F2") + ("m/s");
            vitxt.text = ("vi = ") + Vi.ToString("F2") + ("m/s");
        }

        if (theQuestion.isSimulating)
        {
            gas = true;

            if (Vi != correctAns)
            {
                actiontxt.text = "retry";
                accSimulation.playerDead = true;
                if (timer >= time)
                {
                    theBike.moveSpeed = 0;
                    theQuestion.isSimulating = false;
                    StartCoroutine(truckWillGo());
                }
                walls.SetActive(true);
                if (addingAcc)
                {
                    if (Vi < correctAns & Vi > correctAns - 1f)
                    {
                        deacceleration += 0.2f;
                        stopper.SetActive(true);
                    }
                    if (Vi > correctAns & Vi < correctAns + 1f)
                    {
                        deacceleration -= 0.5f;
                    }
                    if (Vi < correctAns)
                    {
                        answerIsCorrect = false;
                        stuntResultMessage = $"{PlayerPrefs.GetString("Name")} drive too slow and unable to park the motorcycle  inside the truck and got left behind! The correct answer is </color> {correctAns.ToString("F2")} seconds.";
                    }
                    if (Vi > correctAns)
                    {
                        answerIsCorrect = false;
                        stuntResultMessage = $"{PlayerPrefs.GetString("Name")} drive too fast and unable to park and crashed the motorcycle  inside the truck and got left behind! The correct answer is </color> {correctAns.ToString("F2")} seconds.";
                    }
                    addingAcc = false;
                }

            }
            if (Vi == correctAns)
            {
                //theQuestion.SetModalTitle("Stunt Success");
                if (timer >= time)
                {
                    //actionButton.SetActive(false);
                    theQuestion.answerIsCorrect = true;
                    //theQuestion.SetModalTitle("Stunt Success");
                    theBike.moveSpeed = 0.5f;
                    if (currentPos >= 30)
                    {
                        theBike.moveSpeed = 0;
                        //StartCoroutine(StuntResult(stuntResultMessage, answerIsCorrect));
                        theQuestion.isSimulating = false;
                        vitxt.color = new Color32(10, 103, 0, 255);
                        StartCoroutine(truckWillGo());
                        PlayerPrefs.SetInt("levelAccelerationEasy",theHeart.life);
                    }

                }
                answerIsCorrect = true;
                stuntResultMessage = $" The correct answer is <b> {correctAns.ToString("F2")} </b> m/s. {PlayerPrefs.GetString("Name")} have parked inside the truck succesfully! </color>";

            }
            if (theBike.brake == true)
            {
                gas = false;
                directionArrow.SetActive(false);

                // timer += Time.fixedDeltaTime;
                // theQuestion.timer = timer.ToString("F2") + ("s");

                //timertxt.text = timer.ToString("F2") + ("s");

                bikeInitials.transform.position = theBike.transform.position;
                if (timer < time)
                {
                    timer += Time.fixedDeltaTime;
                    velocitytxt.text = ("v = ") + theBike.moveSpeed.ToString("F2") + ("m/s");
                    theBike.moveSpeed -= deacceleration * Time.fixedDeltaTime;

                }
                if (timer >= time)
                {
                    timer = time;
                }
                if (theBike.moveSpeed <= 0)
                {
                    theBike.moveSpeed = 0;
                    engineRunning.Stop();
                    if (Vi == correctAns)
                    {
                        velocitytxt.text = ("vf = 0.00 m/s");
                    }

                }
                theQuestion.timer = timer.ToString("F2") + ("s");
            }



        }

    }
    public void generateProblem()
    {
        truckEngine.Stop();        engineIdle.Play();
        walls.SetActive(false);
        theTruck.transform.rotation = truckStartRot;
        bikeInitials.transform.position = bikeInitialStartPos;
        addingAcc = true;
        generateTime = Random.Range(2.5f, 4.5f);
        theBike.transform.position = new Vector2(-17, 0.2f);
        time = (float)System.Math.Round(generateTime, 2);
        theTruck.transform.position = truckPosition;
        theTruck.moveSpeed = 0;
        stopper.SetActive(false);
        generateAns = 60 / time;
        generateAcceleration = generateAns / time;
        deacceleration = (float)System.Math.Round(generateAcceleration, 2);
        theQuestion.SetQuestion("<b>"+PlayerPrefs.GetString("Name") + ("</b> is instructed to park ") + pronoun2 + (" motorcycle perfectly at the back of truck. If braking the motorcycle constantly deaccelerates it by <b>") + deacceleration.ToString("F2") + ("</b> m/s², what should be the velocity(Vi) of the motorcycle prioe braking it if ")+ pronoun +(" has to apply brakes to the motorcycle for exactly <b>") + time.ToString("F2") + ("</b> seconds only to stop it for the stunt?"));
        vitxt.text = ("vi = ?");
        theHeart.losslife = false;
        velocitytxt.text = ("v = 0 m/s");
        Vftxt.text = ("vf = ") + Vf.ToString("F2") + ("m/s");
        Acctxt.text = ("a = -") + deacceleration.ToString("F2") + ("m/s²");
        timertxt.text = timer.ToString("F2") + ("s");
        directionArrow.SetActive(true);

    }
    IEnumerator StuntResult(string message, bool isCorrect)
    {
        yield return new WaitForSeconds(2);
        if (Vi == correctAns)
        {
             truckEngine.Stop();
            theQuestion.ActivateResult(message, isCorrect, true);

            // theScorer.finalstar();
            // if (theHeart.life > currentStar)
            // {
            //     PlayerPrefs.SetInt("AcstarE", theHeart.life);
            // }
            // if (currentLevel < 4)
            // {
            //     PlayerPrefs.SetInt("level", currentLevel + 1);
            // }
        }
        else
        {
            theQuestion.ActivateResult(message, isCorrect, false);
        }
        StartCoroutine(theSimulation.DirectorsCall());
        yield return new WaitForSeconds(1);
        walls.SetActive(false);
    }
    IEnumerator truckWillGo2()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(truckWillGo());
    }
    IEnumerator truckWillGo()
    {
        truckEngine.Play();
        yield return new WaitForSeconds(2);
        StartCoroutine(StuntResult(stuntResultMessage, answerIsCorrect));
        theTruck.moveSpeed = 10;
        if (Vi == correctAns)
        {
            theBike.moveSpeed = 10f;
        }
        if (Vi != correctAns)
        {
            theHeart.losinglife();
            if (Vi < correctAns)
            {
                if (currentPos > 28)
                {
                    answerIsCorrect = false;
                    stuntResultMessage = $"{PlayerPrefs.GetString("Name")} drive too slow, unable to park the motorcycle properly inside the truck and got left behind! The correct answer is </color> {correctAns.ToString("F2")} seconds.";
                    theBike.moveSpeed = 4;
                }

            }
        }
    }
}
