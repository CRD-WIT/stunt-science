using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForceMedTwo : MonoBehaviour
{
    public TMP_Text debugAnswer;
    public ForceMedSimulation theSimulate;
    public PlayerContForcesMed thePlayer;
    public float accelerationPlayer, time, timer, totalDistance, massBox, forcePlayer, massPlayer;
    public float weightBox, accelerationBox, correctAnswer, friction, Fn,mu;
    public float playerAnswer,min,max;
    public bool preset, startRunning;
    public BoxManager theBox;
    public GameObject stopper2, dimensions, elevator;
    public GameObject box2, targetPos;
    public QuestionContForcesMed theQuestion;
    public HeartManager theHeart;

    public ZombieMedium[] theZombie;
    Vector2 playerStartPoint, boxStartPoint, zombie0StartPoint, zombie1StartPoint, zombie2StartPoint, zombie3StartPoint;
    public TMP_Text boxMassTxt, frictionTxt, muTxt;
    public AudioSource dragSfx;
    // Start is called before the first frame update
    void Start()
    {

        theSimulate.stage = 2;
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        targetPos.transform.position = new Vector2(-16.22f, 1.139412f);
        theZombie[0].transform.localScale = new Vector2(-theZombie[0].transform.localScale.x, theZombie[0].transform.localScale.y);
        theZombie[1].transform.localScale = new Vector2(-theZombie[1].transform.localScale.x, theZombie[1].transform.localScale.y);
        showProblem();
        theQuestion.stage = 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (preset)
        {
            weightBox = massBox * 9.81f;
            
            correctAnswer = (float)System.Math.Round(((massBox * (accelerationBox - 0.02f)) + friction), 2);

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
            StartCoroutine(zombieChase());
            startRunning = false;

        }
        if (theSimulate.simulate == true)
        {
            dimensions.SetActive(false);
            thePlayer.push = true;
            accelerationPlayer = (theSimulate.playerAnswer - friction) / massBox;
            timer += Time.fixedDeltaTime;
            if (playerAnswer == correctAnswer)
            {
                thePlayer.moveSpeed -= accelerationBox * Time.fixedDeltaTime;
                theBox.boxSpeed2 -= accelerationBox * Time.fixedDeltaTime;
                stopper2.SetActive(true);
            }
            if (playerAnswer != correctAnswer)
            {
                if (playerAnswer < correctAnswer)
                {
                    thePlayer.moveSpeed -= (accelerationPlayer - 0.1f) * Time.fixedDeltaTime;
                    theBox.boxSpeed2 -= (accelerationPlayer - 0.1f) * Time.fixedDeltaTime;
                }
                if (playerAnswer > correctAnswer)
                {
                    thePlayer.moveSpeed -= (accelerationPlayer + 0.1f) * Time.fixedDeltaTime;
                    theBox.boxSpeed2 -= (accelerationPlayer + 0.1f) * Time.fixedDeltaTime;
                }

            }

            if (timer >= time)
            {
                dragSfx.Stop();
                thePlayer.moveSpeed = 0;
                theBox.boxSpeed2 = 0;
                thePlayer.push = false;
                theSimulate.simulate = false;
                StartCoroutine(StuntResult());
                if(playerAnswer == correctAnswer)
                {
                    box2.transform.position = new Vector2(-16.0f, 1.480934f);
                }
            }



        }
    }
    public void showProblem()
    {

        dimensions.SetActive(true);
        thePlayer.transform.position = theSimulate.playerStartPoint;
        theZombie[0].transform.position = theSimulate.zombie1StartPoint;
        theZombie[1].transform.position = theSimulate.zombie2StartPoint;
        box2.transform.position = theSimulate.boxStartPoint;
        massPlayer = thePlayer.GetComponent<Rigidbody2D>().mass;
        friction = (float)System.Math.Round((Random.Range(140f, 150f)), 2);
        Fn = friction / mu;
        massBox = Fn / 9.81f;
        timer = 0;
        accelerationBox = ((2 * totalDistance) / (time * time)) + .02f;
        frictionTxt.text = "Ff = "+friction.ToString("F2")+ "N";
        boxMassTxt.text = "m = ?";
        muTxt.text = "?? = " + mu.ToString("F2");
        theQuestion.SetQuestion(("<b>" + PlayerPrefs.GetString("Name") + ("</b> is instructed to push the box(B) starting at rest using constant Force for <b>") + time + ("</b> seconds. If the target location is <b>") + totalDistance.ToString("F2") + ("</b> meter from the box starting position, How much Force should the box 'A' needed to reach the target location with the given time,if the surface coefficient of friction(??) is <b>") + mu.ToString("F2") + ("</b> and has an oppossing friction force of <b>") + friction.ToString("F2") + ("N</b>. After the given time, ") + PlayerPrefs.GetString("Name") + (" will stop pushing and the box will stop moving. Fail to perform the task and zombies will eat your brain.")));
    }
    public IEnumerator zombieChase()
    {
        yield return new WaitForSeconds(1);
        theZombie[0].moveSpeed = -2;
        theZombie[0].zombieRun = true;
        theZombie[1].moveSpeed = -2;
        theZombie[1].zombieRun = true;

    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(4f);
        ForceSimulation.simulate = false;
        theZombie[0].moveSpeed = 0;
        theZombie[0].zombieRun = false;
        theZombie[1].moveSpeed = 0;
        theZombie[1].zombieRun = false;

        if (playerAnswer == correctAnswer)
        {
            theQuestion.answerIsCorrect = true;
            yield return new WaitForSeconds(1);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and safely escaped from zombies"), true, false);
        }
        StartCoroutine(theSimulate.DirectorsCall());
        if (playerAnswer != correctAnswer)
        {
            theHeart.losinglife();
            yield return new WaitForSeconds(2);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has failed to performed the stunt and not able to positioned the box on the target"), false, false);

        }
    }
}

