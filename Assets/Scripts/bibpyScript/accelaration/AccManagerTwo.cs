using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AccManagerTwo : MonoBehaviour
{

    public float Vi;
    public float Vf;
    public float timer;
    public float time;
    float generateAcceleration;
    public float deacceleration;
    private Player thePlayer;
    private accSimulation theSimulation;
    private BikeManager theBike;
    public bool gas, follow;
    public float correctAns;
    float generateAns;
    float playerDistance;
    float playerVf;
    float currentPos;
    public float correctDistance;
    string pronoun;
    string gender;
    public GameObject walls, bikeInitials;
    public GameObject AfterStuntMessage;
    public TMP_Text stuntMessageTxt, Vitxt, Vftxt, Acctxt, timertxt;
    public Button retry, next;
    private HeartManager theHeart;
    private Vector2 bikeInitialStartPos;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        theBike = FindObjectOfType<BikeManager>();
        gender = PlayerPrefs.GetString("Gender");
        theSimulation = FindObjectOfType<accSimulation>();
        theHeart = FindObjectOfType<HeartManager>();
        bikeInitialStartPos = bikeInitials.transform.position;
        theSimulation.stage = 2;
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

        Vftxt.text = ("Vf = ") + Vf.ToString("F2") + ("m/s");
        playerVf = (-deacceleration * accSimulation.playerAnswer) + Vi;
        currentPos = theBike.transform.position.x;
        if (theBike.brake == true)
        {
            
            if(timer <= accSimulation.playerAnswer)
            {
                timer += Time.fixedDeltaTime;
                timertxt.text = timer.ToString("F2") + ("s");
            }
            if(timer >= accSimulation.playerAnswer)
            {
                timertxt.text = accSimulation.playerAnswer.ToString("F2") + ("s");
            }
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

        if (accSimulation.simulate)
        {
            gas = true;
            time = accSimulation.playerAnswer;
            if (time != correctAns)
            {
                walls.SetActive(true);
                retry.gameObject.SetActive(true);
                accSimulation.playerDead = true;

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
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " deaccelerates the motorcycle too fast and overshot the tunnel entrance. The correct answer is </color>" + correctAns.ToString("F2") + "seconds.";
                }
                if (time > correctAns)
                {
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " deaccelerates the motorcycle too slow and undershot the tunnel entrance. The correct answer is </color>" + correctAns.ToString("F2") + "seconds.";
                }
            }
            if (time == correctAns)
            {
                next.gameObject.SetActive(true);
                stuntMessageTxt.text = "<b><color=green>Your Answer is Correct!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " went through the tunnel</color>";


            }
            if (theBike.brake == true)
            {

                Acctxt.text = ("a = -") + deacceleration.ToString("F2") + ("m/s²");
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
                        Vitxt.text = ("V = ") + theBike.moveSpeed.ToString("F2") + ("m/s");
                        theBike.moveSpeed -= deacceleration * Time.fixedDeltaTime;
                        if (theBike.moveSpeed <= 1f)
                        {
                            theBike.moveSpeed = theBike.myRigidbody.velocity.x;
                        }
                    }
                    if (timer >= time)
                    {
                        Vitxt.text = ("V = ") + playerVf.ToString("F2") + ("m/s");
                    }
                }
            }
            if (currentPos > 32)
            {
                theBike.moveSpeed = theBike.myRigidbody.velocity.x;
                accSimulation.simulate = false;
                //theBike.moveSpeed = theBike.myRigidbody.velocity.x;
                if (time == correctAns)
                {
                    StartCoroutine(StuntResult());
                }

            }


        }




    }
    public void generateProblem()
    {
        bikeInitials.transform.position = bikeInitialStartPos;
        Vi = Random.Range(20, 30);
        generateAns = 60 / (Vi + 10);
        correctAns = (float)System.Math.Round(generateAns, 2);
        deacceleration = -(10 - Vi) / correctAns;
        //time = (float)System.Math.Round(generateTime, 2);
        accSimulation.question = (PlayerPrefs.GetString("Name") + ("</b> is initially speeding is motorcycle at <b>") + Vi.ToString("F1") + ("</b> m/s and must deacelerate to a final velocity of 10 m/s right before dropping the ledge so it could safely enter the tunnel across it. How long should <b>") + pronoun + ("</b> apply brake if braking deaccelerates the motorcycle by <b>") + deacceleration.ToString("F1") + ("</b> m/s²?"));
        theBike.transform.position = new Vector2(-10, theBike.transform.position.y);
        theHeart.losslife = false;
        walls.SetActive(false);
        Vitxt.text = ("Vi = ") + Vi.ToString("F2") + ("m/s");
        Acctxt.text = ("a = -") + deacceleration.ToString("F2") + ("m/s²");
        timertxt.text = timer.ToString("F2") + ("s");
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(theSimulation.DirectorsCall());
        yield return new WaitForSeconds(2);
        AfterStuntMessage.SetActive(true);
        walls.SetActive(false);
        theBike.moveSpeed = 0;
    }
}