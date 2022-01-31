using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForceMedThree : MonoBehaviour
{
    public ForceMedSimulation theSimulate;
    public PlayerContForcesMed thePlayer;
    public float accelerationPlayer, time, timer, totalDistance, massBox, forcePlayer, massPlayer, elevatorSpeed;
    public float weightBox, accelerationBox, correctAnswer, friction, Fn, mu, zombieForce, initialForce, zombieFinalForce;
    public float playerAnswer,min,max;
    public bool preset, startRunning;
    public BoxManager theBox;
    public GameObject dimensions, elevator;
    public GameObject box3, targetPos;
    public QuestionContForcesMed theQuestion;
    public HeartManager theHeart;

    public ZombieMedium[] theZombie;
    public Vector2 playerStartPoint, boxStartPoint, zombie0StartPoint, zombie1StartPoint, zombie2StartPoint, zombie3StartPoint,elevatorStartPoint;
    public TMP_Text boxMassTxt, frictionTxt, muTxt, zombieForceTxt;
    public AudioSource dragSfx;
    // Start is called before the first frame update
    void Start()
    {
        elevatorStartPoint = elevator.transform.position;
        theSimulate.stage = 3;
        targetPos.transform.position = new Vector2(6.33f, 1.139412f);
        zombie0StartPoint = theZombie[0].transform.position;
        zombie1StartPoint = theZombie[1].transform.position;
        box3.SetActive(true);
        boxStartPoint = box3.transform.position;
        thePlayer.transform.position = new Vector2(1.1f,0);
        //theZombie[0].transform.localScale = new Vector2(-theZombie[0].transform.localScale.x,theZombie[0].transform.localScale.y);
        //theZombie[1].transform.localScale = new Vector2(-theZombie[1].transform.localScale.x,theZombie[1].transform.localScale.y);
        showProblem();
        theQuestion.stage = 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         elevator.GetComponent<Rigidbody2D>().velocity = new Vector2(0,elevatorSpeed);
        if (preset)
        {
            accelerationBox = ((2 * totalDistance) / (time * time));
            initialForce = massBox * accelerationBox;
            zombieFinalForce = zombieForce - friction;
            correctAnswer = (float)System.Math.Round(zombieFinalForce - initialForce, 2);

        }
        min = correctAnswer - .01f;
        max = correctAnswer +.01f;
        if(theSimulate.playerAnswer == min)
        {
            playerAnswer = correctAnswer;
        }
        if(theSimulate.playerAnswer == max)
        {
            playerAnswer = correctAnswer;
        }
         if(theSimulate.playerAnswer == correctAnswer)
        {
            playerAnswer = correctAnswer;
        }
        if(theSimulate.playerAnswer > max)
        {
            playerAnswer = theSimulate.playerAnswer;
        }
        if( theSimulate.playerAnswer < min)
        {
            playerAnswer = theSimulate.playerAnswer;
        }
        if (startRunning)
        {
            zombieChase();
            startRunning = false;

        }
        if (theSimulate.simulate == true)
        {
            dimensions.SetActive(false);
            thePlayer.push = true;
            accelerationPlayer = (zombieFinalForce - theSimulate.playerAnswer) / massBox;
            timer += Time.fixedDeltaTime;
            if (theSimulate.playerAnswer == correctAnswer)
            {
                thePlayer.moveSpeed += accelerationBox * Time.fixedDeltaTime;
                theBox.boxSpeed3 += accelerationBox * Time.fixedDeltaTime;
                theZombie[0].moveSpeed = theBox.boxSpeed3;
                //stopper2.SetActive(true);
            }
            if (theSimulate.playerAnswer != correctAnswer)
            {
                if (theSimulate.playerAnswer < correctAnswer)
                {
                    thePlayer.moveSpeed += (accelerationPlayer + 0.1f) * Time.fixedDeltaTime;
                    theBox.boxSpeed3 += (accelerationPlayer + 0.1f) * Time.fixedDeltaTime;
                    theZombie[0].moveSpeed = thePlayer.moveSpeed;
                }
                if (theSimulate.playerAnswer > correctAnswer)
                {
                    thePlayer.moveSpeed += (accelerationPlayer - 0.1f) * Time.fixedDeltaTime;
                    theBox.boxSpeed3 += (accelerationPlayer - 0.1f) * Time.fixedDeltaTime;
                    theZombie[0].moveSpeed = thePlayer.moveSpeed;
                }

            }

            if (timer >= time)
            {
                thePlayer.moveSpeed = 0;
                theBox.boxSpeed3 = 0;
                thePlayer.push = false;
                dragSfx.Stop();
                theZombie[0].push = false;
                theZombie[0].moveSpeed = 0;
                StartCoroutine(stopElevator());
                StartCoroutine(StuntResult());
                if (theSimulate.playerAnswer == correctAnswer)
                {
                    box3.transform.position = new Vector2(6.453878f, 1.480934f);
                    theZombie[0].transform.position = new Vector2(theZombie[0].transform.position.x - .2f,theZombie[0].transform.position.y);
                    elevatorSpeed = 1;
                }
                if (theSimulate.playerAnswer > correctAnswer)
                {
                    elevatorSpeed = 1;
                }
               
                theSimulate.simulate = false;
            }



        }
    }
    public void showProblem()
    {
        elevator.GetComponent<Rigidbody2D>().bodyType =  RigidbodyType2D.Dynamic;
        elevatorSpeed = 0;
        elevator.transform.position = elevatorStartPoint;
        dimensions.SetActive(true);
        //thePlayer.transform.position = playerStartPoint;
        theZombie[0].transform.position = zombie0StartPoint;
        theZombie[1].transform.position = zombie1StartPoint;
        box3.transform.position = boxStartPoint;
        massPlayer = thePlayer.GetComponent<Rigidbody2D>().mass;
        time = (float)System.Math.Round((Random.Range(5f, 7f)), 2);
        friction = (float)System.Math.Round((Random.Range(140f, 150f)), 2);
        zombieForce = (float)System.Math.Round((Random.Range(400f, 450f)), 2);
        Fn = massBox * 9.81f;
        massBox = (float)System.Math.Round((Random.Range(45f, 50f)), 2);
        timer = 0;
        frictionTxt.text = "Ff = " + friction.ToString("F2") + "N";
        boxMassTxt.text = "m = " + massBox.ToString("F2") + "kg";
        muTxt.text = "μ = " + mu.ToString("F2");
        zombieForceTxt.text = "F = " + zombieForce.ToString("F2") + "N";
        theQuestion.SetQuestion(("<b>" + PlayerPrefs.GetString("Name") + ("</b> is instructed to push the box(B) starting at rest using constant Force for <b>") + time + ("</b> seconds. If the target location is <b>") + totalDistance.ToString("F2") + ("</b> meter from the box starting position, How much Force should the box 'C' needed to reach the target location with the given time,if the surface coefficient of friction(μ) is <b>") + mu.ToString("F2") + ("</b> and has an oppossing friction force of <b>") + friction.ToString("F2") + ("N</b>. After the given time, ") + PlayerPrefs.GetString("Name") + (" will stop pushing and the box will stop moving. Fail to perform the task and zombies will eat your brain.")));
    }
    public void zombieChase()
    {
        theZombie[0].push = true;
        theZombie[1].moveSpeed = 2;
        theZombie[1].zombieRun = true;

    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(4f);
        ForceSimulation.simulate = false;
        theZombie[1].moveSpeed = 0;
        theZombie[1].zombieRun = false;

        if (theSimulate.playerAnswer == correctAnswer)
        {
            theQuestion.answerIsCorrect = true;
            yield return new WaitForSeconds(1);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and safely escaped from zombies"), true, true);
        }
        StartCoroutine(theSimulate.DirectorsCall());
        if (theSimulate.playerAnswer != correctAnswer)
        {
            theHeart.losinglife();
            yield return new WaitForSeconds(2);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has failed to performed the stunt and not able to positioned the box on the target"), false, false);

        }
    }
    IEnumerator stopElevator()
    {
        yield return new WaitForSeconds(8.5f);
        elevatorSpeed = 0;
        elevator.GetComponent<Rigidbody2D>().bodyType =  RigidbodyType2D.Static;
    }
}
