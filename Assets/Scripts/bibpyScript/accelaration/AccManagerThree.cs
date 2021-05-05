using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AccManagerThree : MonoBehaviour
{
    public float Vi;
    public float Vf;
    public float timer;
    public float time;
    float generateTime;
    float generateAcceleration;
    public float deacceleration;
    private Player thePlayer;
    private accSimulation theSimulation;
    private BikeManager theBike;
    public bool gas;
    public float correctAns;
    float generateAns;
    float playerDistance;
    float playerVf;
    float currentPos;
    public float correctDistance;
    string pronoun;
    string gender;
    public GameObject walls, stopper;
    public GameObject AfterStuntMessage;
    public TMP_Text stuntMessageTxt, Vitxt, Vftxt, Acctxt;
    public Button retry, next;
    private TruckManager theTruck;
    Vector2 truckPosition;
    private HeartManager theHeart;
    private ScoreManager theScorer;
    int currentLevel;
    int currentStar;

    // Start is called before the first frame update
    void Start()
    {
        //thePlayer = FindObjectOfType<Player>();
        theBike = FindObjectOfType<BikeManager>();
        theTruck = FindObjectOfType<TruckManager>();
        gender = PlayerPrefs.GetString("Gender");
        theSimulation = FindObjectOfType<accSimulation>();
        theHeart = FindObjectOfType<HeartManager>();
        theScorer = FindObjectOfType<ScoreManager>();
        truckPosition = theTruck.transform.position;
        currentLevel = PlayerPrefs.GetInt("level");
        currentStar = PlayerPrefs.GetInt("AcstarE");
        theSimulation.stage = 3;
        if (gender == "Male")
        {
            pronoun = ("he");
        }
        if (gender == "Female")
        {
            pronoun = ("she");
        }
        generateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vitxt.text = ("Vi = ?");
        Vftxt.text = ("Vf = ") +Vf.ToString("F2") + ("m/s");
        Acctxt.text = ("accelaration = ")+ deacceleration.ToString("F2") + ("m/s²");
        generateAns = 60 / time;
        correctAns = (float)System.Math.Round(generateAns, 2);
        playerDistance = (time * time) * deacceleration / 2;
        playerVf = (2 * playerDistance) / time;
        currentPos = theBike.transform.position.x;
        deacceleration = generateAns / time;
        Vi = accSimulation.playerAnswer;
        if (gas)
        {
            theBike.moveSpeed = Vi;
        }

        if (accSimulation.simulate)
        {
            gas = true;

            if (Vi != correctAns)
            {
                if (timer >= time)
                {
                    theBike.moveSpeed = 0;
                    accSimulation.simulate = false;
                    StartCoroutine(truckWillGo());
                }
                walls.SetActive(true);
                retry.gameObject.SetActive(true);

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
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " drive too slow and unable to park the motorcycle  inside the truck and got left behind! The correct answer is </color>" + correctAns.ToString("F2") + "seconds.";
                }
                if (Vi > correctAns)
                {
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " drive too fast and unable to park and crashed the motorcycle  inside the truck and got left behind! The correct answer is </color>" + correctAns.ToString("F2") + "seconds.";
                }
            }
            if (Vi == correctAns)
            {
                if (timer >= time)
                {
                    theBike.moveSpeed = 0.5f;
                    if (currentPos >= 30)
                    {
                        theBike.moveSpeed = 0;
                        //StartCoroutine(StuntResult());
                        accSimulation.simulate = false;
                        StartCoroutine(truckWillGo());
                    }

                }
                
                stuntMessageTxt.text = "<b><color=green>Your Answer is Correct!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " has succesfully parked inside the truck! </color>";


            }
            if (theBike.brake == true)
            {
                gas = false;
                timer += Time.fixedDeltaTime;
                if (timer < time)
                {
                    theBike.moveSpeed -= deacceleration * Time.fixedDeltaTime;
                    if (theBike.moveSpeed <= 0)
                    {
                        theBike.moveSpeed = 0;

                    }
                }
            }



        }




    }
    public void generateProblem()
    {
        generateTime = Random.Range(2.5f, 4.5f);
        theBike.transform.position = new Vector2(-10, theBike.transform.position.y);
        time = (float)System.Math.Round(generateTime, 2);
        theTruck.transform.position = truckPosition;
        theTruck.moveSpeed = 0;
        stopper.SetActive(false);
        accSimulation.question = (PlayerPrefs.GetString("Name") + ("</b> must park his motorcycle perfectly at the back of truck. If braking the motorcycle deaccelerates it by <b>") + deacceleration.ToString("F1") + ("</b> m/s, what should be its initial velocity(Vi) so it will come into complete stop after braking it for exactly  <b>") + time.ToString("F2") + ("</b> seconds?"));

        theHeart.losslife = false;

    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(2);
         if (Vi == correctAns)
        {
            theScorer.finalstar();
            if(theHeart.life > currentStar)
            {
                PlayerPrefs.SetInt("AcstarE", theHeart.life);
            }
            if (currentLevel < 4)
            {
                PlayerPrefs.SetInt("level", currentLevel + 1);
            }
        }
        StartCoroutine(theSimulation.DirectorsCall());
        yield return new WaitForSeconds(1);
        AfterStuntMessage.SetActive(true);
        yield return new WaitForSeconds(1);
        
        walls.SetActive(false);
    }
    IEnumerator truckWillGo()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(StuntResult());
        theTruck.moveSpeed = 10;
        if (Vi == correctAns)
        {
            theBike.moveSpeed = 10.5f;
        }
        if (Vi != correctAns)
        {
            theHeart.losinglife();
            if (Vi < correctAns)
            {
                if (currentPos > 28)
                {
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " drive too slow, unable to park the motorcycle properly inside the truck and got left behind! The correct answer is </color>" + correctAns.ToString("F2") + "seconds.";

                    theBike.moveSpeed = 4;
                }

            }
        }
    }
}
