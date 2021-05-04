using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccMediumOne : MonoBehaviour
{
    public GameObject hangingRagdoll, ropeTip, playerInTruck, ragdollPrefab, stickmanPoint, AfterStuntMessage, retry, next;
    public Player thePlayer;
    public Hellicopter theChopper;
    public TruckManager theTruck;
    private AccMidSimulation theSimulate;
    public bool chase;
    public float timer, correctAnswer;
    public float velocity, accelaration;
    float generateVelocity, generateAccelaration, generateCorrectAnswer;
    public float chopperPos, truckPos, answer;
    public bool readyToJump;
    public TMP_Text timertxt;



    // Start is called before the first frame update
    void Start()
    {
        theSimulate = FindObjectOfType<AccMidSimulation>();
        generateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        if (AccMidSimulation.simulate == true)
        {
            answer = AccMidSimulation.playerAnswer;
            truckPos = theTruck.transform.position.x;
            chopperPos = theChopper.transform.position.x;
            hangingRagdoll.transform.position = ropeTip.transform.position;
            theChopper.flySpeed = velocity;
            if (chase)
            {
                timertxt.text = timer.ToString("F2") + "s";
                theTruck.accelerating = true;
                theTruck.accelaration = accelaration;


                thePlayer.transform.position = playerInTruck.transform.position;
                timer += Time.fixedDeltaTime;
                if (answer == correctAnswer)
                {
                    if (timer >= answer - .8f)
                    {
                        if (readyToJump)
                        {

                            StartCoroutine(jump());
                        }
                    }
                    next.SetActive(true);
                }
                if (answer < correctAnswer)
                {
                    AccMidSimulation.playerDead = true;
                    if (timer >= answer - .9f)
                    {
                        if (readyToJump)
                        {
                            StartCoroutine(StuntResult());
                            StartCoroutine(jump());
                        }
                        
                    }
                    retry.SetActive(true);
                }
                if (answer > correctAnswer)
                {
                    AccMidSimulation.playerDead = true;
                    if (timer >= answer - .75f)
                    {
                        if (readyToJump)
                        {
                            StartCoroutine(StuntResult());
                            StartCoroutine(jump());
                        }
                    }
                    retry.SetActive(true);
                }
            }
        }
    }
    public void generateProblem()
    {
        generateAccelaration = Random.Range(4f, 8f);
        generateVelocity = Random.Range(8f,10f);
        accelaration = (float)System.Math.Round(generateAccelaration, 2);
        velocity = (float)System.Math.Round(generateVelocity, 2);
        generateCorrectAnswer = (2 * velocity) / accelaration;
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(4);
        StartCoroutine(theSimulate.DirectorsCall());
        yield return new WaitForSeconds(1);
        AfterStuntMessage.SetActive(true);
        
    }
    IEnumerator jump()
    {
        readyToJump = false;
        thePlayer.toJump = true;
        yield return new WaitForSeconds(.8f);
        thePlayer.toJump = false;
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

    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(ragdollPrefab);
        stick.transform.position = stickmanPoint.transform.position;
    }

}
