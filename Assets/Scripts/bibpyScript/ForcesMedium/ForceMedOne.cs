using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForceMedOne : MonoBehaviour
{
    public ForceMedSimulation theSimulate;
    public PlayerContForcesMed thePlayer;
    public float accelerationPlayer, time, timer, totalDistance, massBox, forcePlayer, massPlayer;
    public float weightBox, accelerationBox, correctAnswer, friction, Fn,mu;
    public bool preset, startRunning;
    public BoxManager theBox;
    public GameObject  stopper2, dimensions;
    public GameObject box1;
    public QuestionContForcesMed theQuestion;
    public HeartManager theHeart;

    public ZombieMedium[] theZombie;
    Vector2 playerStartPoint, boxStartPoint,zombie0StartPoint,zombie1StartPoint,zombie2StartPoint,zombie3StartPoint;
     public TMP_Text boxMassTxt,frictionTxt;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Life",3);
        playerStartPoint = thePlayer.transform.position;
        boxStartPoint = box1.transform.position;
        zombie0StartPoint = theZombie[0].transform.position;
        zombie1StartPoint = theZombie[1].transform.position;
        zombie2StartPoint = theZombie[2].transform.position;
        zombie3StartPoint = theZombie[3].transform.position;
        showProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (preset)
        {
            weightBox = massBox * 9.81f;
            accelerationBox = ((2 * totalDistance) / (time * time)) + .02f;
            correctAnswer = (float)System.Math.Round(((massBox * (accelerationBox - 0.02f)) + friction), 2);

        }
        if (startRunning)
        {
            StartCoroutine(zombieChase());
            startRunning = false;

        }
        if (theSimulate.simulate == true)
        {
            dimensions.SetActive(false);
            thePlayer.push = true;
            accelerationPlayer = (theSimulate.playerAnswer-friction) / massBox;
            timer += Time.fixedDeltaTime;
            if (theSimulate.playerAnswer == correctAnswer)
            {
                thePlayer.moveSpeed += accelerationBox * Time.fixedDeltaTime;
                theBox.boxSpeed1 += accelerationBox * Time.fixedDeltaTime;
                stopper2.SetActive(true);
            }
            if (theSimulate.playerAnswer != correctAnswer)
            {
                if (theSimulate.playerAnswer < correctAnswer)
                {
                    thePlayer.moveSpeed += (accelerationPlayer-0.1f) * Time.fixedDeltaTime;
                    theBox.boxSpeed1 += (accelerationPlayer-0.1f) * Time.fixedDeltaTime;
                }
                if (theSimulate.playerAnswer > correctAnswer)
                {
                    thePlayer.moveSpeed += (accelerationPlayer + 0.1f) * Time.fixedDeltaTime;
                    theBox.boxSpeed1 += (accelerationPlayer + 0.1f) * Time.fixedDeltaTime;
                }

            }

            if (timer >= time)
            {
                thePlayer.moveSpeed = 0;
                theBox.boxSpeed1 = 0;
                thePlayer.push = false;
                theSimulate.simulate = false;
                StartCoroutine(StuntResult());
                if(theSimulate.playerAnswer == correctAnswer)
                {
                    box1.transform.position = new Vector2(8.05f,1.483791f);
                }
            }



        }
    }
    public void showProblem()
    {
        dimensions.SetActive(true);
        thePlayer.transform.position = playerStartPoint;
        theZombie[0].transform.position = zombie0StartPoint;
        theZombie[1].transform.position = zombie1StartPoint;
        theZombie[2].transform.position = zombie2StartPoint;
        theZombie[3].transform.position = zombie3StartPoint;
        box1.transform.position = boxStartPoint;
        massBox = (float)System.Math.Round((Random.Range(45f,50f)), 2);
        massPlayer = thePlayer.GetComponent<Rigidbody2D>().mass;
        Fn = massBox * 9.81f;
        friction = Fn * mu;
        timer = 0;
        frictionTxt.text = "Ff = "+friction.ToString("F2")+ "N";
        boxMassTxt.text = "m = "+ massBox.ToString("F2") + "kg";
        theQuestion.SetQuestion(("<b>" + PlayerPrefs.GetString("Name") + ("</b> is instructed to push the box(A) starting at rest using constant Force for <b>") + time + ("</b> seconds. If the target location is <b>") + totalDistance.ToString("F2") + ("</b> meter from the box starting position, How much Force should the box 'A' needed to reach the target location with the given time,if the surface has an oppossing friction force of <b>")+ friction.ToString("F2") +("N</b> and the box has a mass of <b>")+ massBox.ToString("F2")+("</b>kg. After the given time, ") + PlayerPrefs.GetString("Name") + (" will stop pushing and the box will stop moving. Fail to perform the task and zombies will eat your brain.")));
    }
    public IEnumerator zombieChase()
    {
        yield return new WaitForSeconds(3);
        theZombie[0].moveSpeed = 1;
        theZombie[0].zombieRun = true;
        theZombie[1].moveSpeed = -1;
        theZombie[1].zombieRun = true;
        theZombie[2].moveSpeed = 1;
        theZombie[2].zombieRun = true;
        theZombie[3].moveSpeed = 1;
        theZombie[3].zombieRun = true;
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(2f);
        ForceSimulation.simulate = false;
        theZombie[2].moveSpeed = 0;
        theZombie[2].zombieRun = false;
        theZombie[3].moveSpeed = 0;
        theZombie[3].zombieRun = false;

        if (theSimulate.playerAnswer == correctAnswer)
        {
            theQuestion.answerIsCorrect = true;
            yield return new WaitForSeconds(4);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and safely escaped from zombies"), true, false);
        }
        StartCoroutine(theSimulate.DirectorsCall());
        if (theSimulate.playerAnswer != correctAnswer)
        {
            // theZombie[0].moveSpeed = 0;
            // theZombie[1].moveSpeed = 0;
            theHeart.losinglife();
            yield return new WaitForSeconds(4);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has failed to performed the stunt and not able to positioned the box on the target"), false, false);

        }
    }
}
