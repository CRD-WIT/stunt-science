using System.Collections;
using UnityEngine;
using TMPro;

public class AccManagerOne : MonoBehaviour
{
    public GraphQLCloud graphQLCloud;
    string stuntResultMessage;
    bool answerIsCorrect;
    public TMP_Text debugAnswer;
    public float Vi;
    public float Vf;
    public float timer;
    public float time;
    public GameObject transitionCanvas;
    float generateTime;
    public float accelaration;
    public PlayerB thePlayer;
    private accSimulation theSimulation;
    private HeartManager theHeart;
    private BikeManager theBike;
    public bool gas = true;
    public float correctAns;
    float generateAns;
    float playerDistance;
    float playerVf;
    float currentPos;
    float correctDistance;
    string pronoun, pronoun2;
    string gender;
    public GameObject walls, bikeInitials, directionArrow;
    public TMP_Text Vitxt, Vftxt, Acctxt, timertxt, actiontxt;
    bool tooSlow, follow;
    private Vector2 bikeInitialsPos;
    public QuestionControllerAcceleration theQuestion;

    void Start()
    {
        theQuestion.SetGameLevel(2);
        bikeInitialsPos = bikeInitials.transform.position;
        theBike = FindObjectOfType<BikeManager>();
        gender = PlayerPrefs.GetString("Gender");
        theSimulation = FindObjectOfType<accSimulation>();
        theHeart = FindObjectOfType<HeartManager>();
        theBike.moveSpeed = 0;
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
        PlayerPrefs.SetInt("Life", 3);
    }
    IEnumerator Retry()
    {
        transitionCanvas.SetActive(true);
        theQuestion.retried = false;
        Debug.Log("Retry triggered.");
        yield return new WaitForSeconds(3f);
        transitionCanvas.SetActive(true);
        yield return new WaitForSeconds(1f);
        theSimulation.retry();
    }
    void FixedUpdate()
    {
        debugAnswer.SetText($"Answer: {correctAns}");
        generateAns = (Vf - Vi) / time;
        correctAns = (float)System.Math.Round(generateAns, 2);
        playerDistance = (time * time) * accelaration / 2;
        correctDistance = (time * time) * correctAns / 2;
        playerVf = accSimulation.playerAnswer * time;
        currentPos = theBike.transform.position.x;
        if (follow)
        {
            bikeInitials.transform.position = theBike.transform.position;
        }
        if (currentPos >= 35)
        {
            follow = false;
        }
        if (tooSlow)
        {
            theBike.moveSpeed -= 2 * Time.fixedDeltaTime;
            if (theBike.moveSpeed <= 0)
            {
                tooSlow = false;
                theBike.moveSpeed = 0;
                StartCoroutine(StuntResult(stuntResultMessage, answerIsCorrect));
                theHeart.losinglife();
            }
        }
        if (theQuestion.isSimulating)
        {
            directionArrow.SetActive(false);
            theQuestion.timer = timer.ToString("F2") + ("s");
            accelaration = accSimulation.playerAnswer;
            Acctxt.text = ("a = ") + accelaration.ToString("F2") + ("m/s²");
            Vitxt.text = ("v = ") + theBike.moveSpeed.ToString("F2") + ("m/s");

            if (accelaration != correctAns)
            {
                walls.SetActive(true);
                accSimulation.playerDead = true;
                answerIsCorrect = false;
                //theQuestion.SetModalTitle("Stunt failed");
                if (accelaration < correctAns & accelaration > correctAns - .5f)
                {
                    accelaration -= .5f;
                }
                if (accelaration > correctAns & accelaration < correctAns + 0.5f)
                {
                    accelaration += 1f;
                }

                if (accSimulation.playerAnswer > correctAns)
                {
                    stuntResultMessage = $"{PlayerPrefs.GetString("Name")} accelerated the motorcycle too fast and overshot the tunnel entrance. The correct answer is </color> {correctAns.ToString("F1")} m/s².";
                }
            }
            if (accelaration == correctAns)
            {
                stuntResultMessage = $"The correct answer is <b> {correctAns.ToString("F2")}</b> m/s². {PlayerPrefs.GetString("Name")} was able to enter tunnel succesfully!</color>";
                answerIsCorrect = true;
            }
            if (gas)
            {
                theBike.moveSpeed += accelaration * Time.fixedDeltaTime;
            }
            timer += Time.fixedDeltaTime;
            if (timer >= time)
            {
                theQuestion.isSimulating = false;
                timertxt.text = time.ToString("F2") + ("s");
                if (accelaration == correctAns)
                {
                    StartCoroutine(StuntResult(stuntResultMessage, answerIsCorrect));
                    playerVf = 15;
                    Acctxt.color = new Color32(10, 103, 0, 255);
                }
                else
                {
                    Acctxt.color = new Color32(10, 103, 0, 255);
                }
                gas = false;

                if (accelaration < correctAns)
                {
                    if (currentPos < 10)
                    {
                        tooSlow = true;
                    }
                    if (currentPos < 38)
                    {
                        stuntResultMessage = $"{PlayerPrefs.GetString("Name")} accelerated the motorcycle too slow and not able to cross tunnel entrance. The correct answer is </color> {correctAns.ToString("F1")} m/s².";
                    }
                    if (currentPos >= 38)
                    {
                        stuntResultMessage = $"{PlayerPrefs.GetString("Name")} accelerated the motorcycle too slow and undershot the tunnel entrance. The correct answer is </color> {correctAns.ToString("F1")} m/s².";
                    }
                    StartCoroutine(StuntResult(stuntResultMessage, answerIsCorrect));
                    playerVf = 0;
                }
                Vitxt.text = ("v = ") + playerVf.ToString("F2") + ("m/s");
            }
        }
    }
    public void generateProblem()
    {
        follow = true;
        bikeInitials.transform.position = bikeInitialsPos;
        timer = 0;
        timertxt.text = ("0.00s");
        generateTime = Random.Range(3.0f, 3.5f);
        time = (float)System.Math.Round(generateTime, 2);
        theQuestion.SetQuestion(PlayerPrefs.GetString("Name") + (" is instructed to enter the tunnel on the otherside of the platform where  ") + pronoun + (" is in, ") + pronoun + (" must drive his motorcycle from a complete stand still to a speed of <b>") + Vf.ToString("F2") + ("</b> m/s, after <b>") + time.ToString("F2") + ("</b> seconds. What should be ") + pronoun2 + (" acceleration in order to achieve the final velocity?"));
        theHeart.losslife = false;
        theBike.moveSpeed = 0;
        Acctxt.text = ("a = ?");
        Vitxt.text = ("vi = ") + Vi.ToString("F2") + ("m/s");
        Vftxt.text = ("vf = ") + Vf.ToString("F2") + ("m/s");
        directionArrow.SetActive(true);
    }
    IEnumerator StuntResult(string message, bool isCorrect)
    {        
        yield return new WaitForSeconds(5);
        StartCoroutine(theSimulation.DirectorsCall());
        walls.SetActive(false);
        theQuestion.isSimulating = false;
        theBike.moveSpeed = 0;
        theQuestion.ActivateResult(message, isCorrect);
    }
}
