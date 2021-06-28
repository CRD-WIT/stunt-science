using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AccManagerOne : MonoBehaviour
{

    public float Vi;
    public float Vf;
    public float timer;
    public float time;
    float generateTime;
    public float accelaration;
    public Player thePlayer;
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
    string pronoun;
    string gender;
    public GameObject walls, bikeInitials,directionArrow;
    public TMP_Text Vitxt, Vftxt, Acctxt, timertxt, actiontxt;
    bool tooSlow, follow;
    private Vector2 bikeInitialsPos;
    public QuestionController2 theQuestion;
    // Start is called before the first frame update
    void Start()
    {
        theQuestion.stageNumber = 1;
        theQuestion.levelDifficulty = GameConfig.Difficulty.Medium;
        theQuestion.levelName = "Acceleration";
        bikeInitialsPos = bikeInitials.transform.position;
        //thePlayer = FindObjectOfType<Player>();
        theBike = FindObjectOfType<BikeManager>();
        gender = PlayerPrefs.GetString("Gender");
        theSimulation = FindObjectOfType<accSimulation>();
        theHeart = FindObjectOfType<HeartManager>();
        theBike.moveSpeed = 0;
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

        generateAns = (Vf - Vi) / time;
        correctAns = (float)System.Math.Round(generateAns, 2);
        playerDistance = (time * time) * accelaration / 2;
        correctDistance = (time * time) * correctAns / 2;
        playerVf = accSimulation.playerAnswer * time;
        currentPos = theBike.transform.position.x;
        if(follow)
        {
             bikeInitials.transform.position = theBike.transform.position;
        }
        if(currentPos >= 35)
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
                StartCoroutine(StuntResult());
                theHeart.losinglife();
            }
        }

        if (accSimulation.simulate)
        {
            directionArrow.SetActive(false);
            timertxt.text = timer.ToString("F2") + ("s");
            accelaration = accSimulation.playerAnswer;
            Acctxt.text = ("a = ") + accelaration.ToString("F2") + ("m/s²");
            Vitxt.text = ("v = ") + theBike.moveSpeed.ToString("F2") + ("m/s");
            
           

            if (accelaration != correctAns)
            {
                walls.SetActive(true);
                accSimulation.playerDead = true;
                theQuestion.SetModalTitle("Stunt failed");
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
                   theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " accelerated the motorcycle too fast and overshot the tunnel entrance. The correct answer is </color>" + correctAns.ToString("F1") + "m/s².");
                }
                

            }
            if (accelaration == correctAns)
            {
                actiontxt.text = "Next";
                theQuestion.answerIsCorrect = true;
                theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " went through the tunnel</color>");
                theQuestion.SetModalTitle("Stunt Success");

            }
            if (gas)
            {
                theBike.moveSpeed += accelaration * Time.fixedDeltaTime;
            }
            timer += Time.fixedDeltaTime;
            if (timer >= time)
            {
                accSimulation.simulate = false;
                timertxt.text = time.ToString("F2") + ("s");
                if (accelaration == correctAns)
                {
                    StartCoroutine(StuntResult());
                    playerVf = 15;
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
                        theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " accelerated the motorcycle too slow and not able to cross tunnel entrance. The correct answer is </color>" + correctAns.ToString("F1") + "m/s².");
                    }
                    if (currentPos >= 38)
                    {
                        theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " accelerated the motorcycle too slow and undershot the tunnel entrance. The correct answer is </color>" + correctAns.ToString("F1") + "m/s².");
                    }

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
        theQuestion.SetQuestion(("In order for <b>") + PlayerPrefs.GetString("Name") + ("</b> to enter the tunnel on the otherside of the platform where  <b>") + pronoun + ("</b>is in, <b>") + pronoun + ("</b> must drive his motorcycle from a complete standstill to a speed of <b>") + Vf.ToString("F2") + ("</b> m/s, after ") + time.ToString("F2") + ("seconds. What should be ") + pronoun + (" accelaration in order to achieve the final velocity?"));
        theHeart.losslife = false;
        theBike.moveSpeed = 0;
        Acctxt.text = ("a = ?");
        Vitxt.text = ("vi = ") + Vi.ToString("F2") + ("m/s");
        Vftxt.text = ("vf = ") + Vf.ToString("F2") + ("m/s");
        directionArrow.SetActive(true);


    }
    IEnumerator StuntResult()
    {

        
        yield return new WaitForSeconds(3);
        StartCoroutine(theSimulation.DirectorsCall());
        walls.SetActive(false);
        accSimulation.simulate = false;
        theBike.moveSpeed = 0;
        theQuestion.ToggleModal();
    }


}
