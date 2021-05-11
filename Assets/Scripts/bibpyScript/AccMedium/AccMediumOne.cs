using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccMediumOne : MonoBehaviour
{
    public GameObject hangingRagdoll, ropeTip, playerInTruck, ragdollPrefab, stickmanPoint, AfterStuntMessage, retry, next, grabline,playerGrabline;
    public Player thePlayer;
    public Hellicopter theChopper;
    public SubHellicopter theSubChopper;
    public TruckManager theTruck;
    private AccMidSimulation theSimulate;
    public bool chase;
    public float timer, correctAnswer;
    public float velocity, accelaration;
    float generateVelocity, generateAccelaration, generateCorrectAnswer;
    public float chopperPos, truckPos, answer;
    public bool readyToJump;
    public TMP_Text timertxt, stuntMessageTxt, vHtxt, viTtxt, aTtxt;
    float grabLineDistance, playerGrabLineDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        theSimulate = FindObjectOfType<AccMidSimulation>();
        generateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vHtxt.text = ("V = ")+ velocity.ToString("F2")+("m/s");
        viTtxt.text = ("Vi = 0m/s ");
        aTtxt.text = ("a = ")+ accelaration.ToString("F2")+("m/s²");
        playerGrabLineDistance = velocity * AccMidSimulation.playerAnswer;
        
       
        if (AccMidSimulation.simulate == true)
        {
            answer = AccMidSimulation.playerAnswer;
            truckPos = theTruck.transform.position.x;
            chopperPos = theChopper.transform.position.x;
            hangingRagdoll.transform.position = ropeTip.transform.position;
            theChopper.flySpeed = velocity;
            playerGrabline.transform.position = new Vector2(playerGrabLineDistance + 2.67f, playerGrabline.transform.position.y);
            if (chase)
            {
                timertxt.text = timer.ToString("F2") + "s";
                theTruck.accelerating = true;
                theTruck.accelaration = accelaration;


                thePlayer.transform.position = playerInTruck.transform.position;
                timer += Time.fixedDeltaTime;
                if (answer == correctAnswer)
                {
                    if (timer >= answer - .45f)
                    {
                        if (readyToJump)
                        {
                            StartCoroutine(StuntResult());
                            StartCoroutine(jump());
                        }
                        stuntMessageTxt.text = "<b><color=green>Your Answer is Correct!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " has grabbed the rope and is now succesfully hanging from a hellicopter</color>";
                    }
                    next.SetActive(true);
                }
                if (answer < correctAnswer)
                {
                    AccMidSimulation.playerDead = true;
                    if (timer >= answer - .45f)
                    {
                        if (readyToJump)
                        {
                            StartCoroutine(StuntResult());
                            StartCoroutine(jump());
                            playerGrabline.SetActive(true);
                            grabline.SetActive(true);
                        }
                        
                    }
                    retry.SetActive(true);
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " grab the rope too soon. The correct answer is </color>" + correctAnswer.ToString("F2") +"seconds.";
                }
                if (answer > correctAnswer)
                {
                    AccMidSimulation.playerDead = true;
                    if (timer >= answer - .7f)
                    {
                        if (readyToJump)
                        {
                            StartCoroutine(StuntResult());
                            StartCoroutine(jump());
                            playerGrabline.SetActive(true);
                            grabline.SetActive(true);
                        }
                    }
                    retry.SetActive(true);
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " grab the rope too soon. The correct answer is </color>" + correctAnswer.ToString("F2") +"seconds.";
                }
            }
        }
    }
    public void generateProblem()
    {
        timer = 0;
        timertxt.text = timer.ToString("F2") + "s";
        readyToJump = true;
        generateAccelaration = Random.Range(4f, 8f);
        generateVelocity = Random.Range(8f,10f);
        accelaration = (float)System.Math.Round(generateAccelaration, 2);
        velocity = (float)System.Math.Round(generateVelocity, 2);
        generateCorrectAnswer = (2 * velocity) / accelaration;
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);
        grabLineDistance = velocity * correctAnswer;
        grabline.transform.position = new Vector2(grabLineDistance + 2.67f, grabline.transform.position.y);
        AccMidSimulation.question = (("<b>") + PlayerPrefs.GetString("Name") + ("</b> is standing in top of a non moving truck, waiting for the hellicopter to pass by, chase it with the truck, and grab the rope hanging from it, If the hellicopter flies forward at a constant speed of <b>") + velocity.ToString("F2") + ("</b> m/s and the truck follows to catch up with an accelaration of <b>") + accelaration.ToString("F2") + ("</b> m/s² the moment the rope passes by <b>") + PlayerPrefs.GetString("Name") + ("</b>, after how many seconds should <b>") + PlayerPrefs.GetString("Name") + ("</b> precisely grab the rope the moment the truck starts moving?"));
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(4);
        StartCoroutine(theSimulate.DirectorsCall());
        yield return new WaitForSeconds(1);
        AfterStuntMessage.SetActive(true);
        playerGrabline.SetActive(false);
        
    }
    IEnumerator jump()
    {
        readyToJump = false;
        thePlayer.toReach= true;
        yield return new WaitForSeconds(.5f);
        thePlayer.toReach = false;
        thePlayer.gameObject.SetActive(false);
        if (answer == correctAnswer)
        {
            hangingRagdoll.SetActive(true);
        }
        if (answer != correctAnswer)
        {
            ragdollSpawn();
        }
        thePlayer.standup = true;
        chase = false;
        timertxt.text = answer.ToString("F2")+"s";
        yield return new WaitForSeconds(5f);
        velocity = 0;
        theTruck.accelerating = false;
        theTruck.moveSpeed = 0;
        //AccMidSimulation.simulate = false;

    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(ragdollPrefab);
        stick.transform.position = stickmanPoint.transform.position;
    }

}
