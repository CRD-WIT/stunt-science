using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ForceHardManagerTwo : MonoBehaviour
{
    public ForceHardSimulation theSimulate;
    public QuestionContForcesMed theQuestion;
    public HeartManager theHeart;
    public float accBoxOne, accBoxTwo, massBox, frictionForceOne, frictionForceTwo, normalForce, muOne, muTwo, distanceOne, distanceTwo, appliedForce, ViOne, VfOne, VfTwo, finalForceOne, finalForceTwo;
    public float timeOne, timeTwo, timeTotal, boxStartPos, boxCurrentPos, boxDistanceTravel, time;
    public float boxSpeed, angle, correctAnswer, timer;
    public GameObject box, target, wallGlass, stopper, dimensions;
    public PlayerContForcesMed thePlayer;
    public bool answerIsCorrect;
    public Vector2 playerStartPoint, boxStartPoint, zombie0StartPoint, zombie1StartPoint;
    public BoxCollisionManager theCollision;
    public ZombieMedium[] theZombie;
    public TMP_Text muOnetxt, muTwoTxt, massBoxTxt, appliedForceTxt;

    // Start is called before the first frame update
    void Start()
    {
        theQuestion.stage = 2;
        zombie0StartPoint = theZombie[0].transform.position;
        zombie1StartPoint = theZombie[1].transform.position;
        normalForce = (massBox * 9.81f) * (Mathf.Cos(angle * Mathf.Deg2Rad));
        boxStartPos = box.transform.position.x;
        target.SetActive(true);
        playerStartPoint = thePlayer.transform.position;
        boxStartPoint = box.transform.position;
        showProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        correctAnswer = (float)System.Math.Round(timeTotal, 2);
        boxCurrentPos = box.transform.position.x;
        boxDistanceTravel = boxStartPos - boxCurrentPos;
        box.GetComponent<Rigidbody2D>().velocity = new Vector2(boxSpeed, box.GetComponent<Rigidbody2D>().velocity.y);
        if (theSimulate.simulate == true)
        {
            thePlayer.pull = true;

            timer += Time.fixedDeltaTime;

            if (theSimulate.playerAnswer == correctAnswer)
            {
                dimensions.SetActive(false);
                stopper.SetActive(true);
                if (boxDistanceTravel <= 6)
                {
                    boxSpeed -= (accBoxOne) * Time.fixedDeltaTime;
                    thePlayer.moveSpeed -= (accBoxOne) * Time.fixedDeltaTime;
                }
                if (boxDistanceTravel > 6)
                {
                    boxSpeed -= (accBoxTwo) * Time.fixedDeltaTime;
                    thePlayer.moveSpeed -= (accBoxTwo) * Time.fixedDeltaTime;
                }
                answerIsCorrect = true;
                if (timer >= timeTotal)
                {
                    boxSpeed = 0;
                    thePlayer.moveSpeed = 0;
                    theSimulate.simulate = false;
                    thePlayer.pull = false;
                    box.transform.position = new Vector2(8.01f, .98f);
                    StartCoroutine(StuntResult());
                }
            }
            if (theSimulate.playerAnswer > correctAnswer)
            {
                if (boxDistanceTravel <= 6)
                {
                    boxSpeed -= (accBoxOne + .1f) * Time.fixedDeltaTime;
                    thePlayer.moveSpeed -= (accBoxOne + .1f) * Time.fixedDeltaTime;
                }
                if (boxDistanceTravel > 6)
                {
                    boxSpeed -= (accBoxTwo + .1f) * Time.fixedDeltaTime;
                    thePlayer.moveSpeed -= (accBoxTwo + .1f) * Time.fixedDeltaTime;
                }
                if (timer >= theSimulate.playerAnswer)
                {
                    boxSpeed = 0;
                    thePlayer.moveSpeed = 0;
                    theSimulate.simulate = false;
                    thePlayer.pull = false;
                    StartCoroutine(StuntResult());
                }


            }
            if (theSimulate.playerAnswer < correctAnswer)
            {
                if (boxDistanceTravel <= 6)
                {
                    boxSpeed -= (accBoxOne - .02f) * Time.fixedDeltaTime;
                    thePlayer.moveSpeed -= (accBoxOne - .02f) * Time.fixedDeltaTime;
                }
                if (boxDistanceTravel > 6)
                {
                    boxSpeed -= (accBoxTwo - .02f) * Time.fixedDeltaTime;
                    thePlayer.moveSpeed -= (accBoxTwo - .02f) * Time.fixedDeltaTime;
                }
                if (timer >= theSimulate.playerAnswer)
                {
                    boxSpeed = 0;
                    thePlayer.moveSpeed = 0;
                    theSimulate.simulate = false;
                    thePlayer.pull = false;
                    StartCoroutine(StuntResult());
                }


            }


        }
    }
    public void showProblem()
    {
        dimensions.SetActive(true);
        theZombie[0].moveSpeed = 0;
        theZombie[1].moveSpeed = 0;
        theZombie[1].zombieRun = false;
        theZombie[0].zombieRun = false;
        stopper.SetActive(false);
        timer = 0;
        theZombie[0].transform.position = zombie0StartPoint;
        theZombie[1].transform.position = zombie1StartPoint;
        wallGlass.SetActive(true);
        theCollision.breakReady = true;
        thePlayer.transform.position = playerStartPoint;
        box.transform.position = boxStartPoint;
        massBox = (float)System.Math.Round((Random.Range(45, 50f)), 2);
        muOne = (float)System.Math.Round((Random.Range(.6f, 0.65f)), 2);
        muTwo = (float)System.Math.Round((Random.Range(0.66f, 0.7f)), 2);
        appliedForce = Random.Range(350, 360);
        normalForce = massBox * 9.81f;
        frictionForceOne = normalForce * muOne;
        frictionForceTwo = normalForce * muTwo;
        finalForceOne = appliedForce - frictionForceOne;
        finalForceTwo = appliedForce - frictionForceTwo;
        accBoxOne = finalForceOne / massBox;
        accBoxTwo = finalForceTwo / massBox;
        //VfOne = Mathf.Sqrt((ViOne * ViOne) + (2 * (accBoxOne * distanceOne)));

        //timeOne = (VfOne - ViOne) / accBoxOne;
        timeOne = Mathf.Sqrt(((2 * distanceOne) / accBoxOne));
        VfOne = accBoxOne * timeOne;
        VfTwo = Mathf.Sqrt((VfOne * VfOne) + (2 * (accBoxTwo * distanceTwo)));
        timeTwo = (VfTwo - VfOne) / accBoxTwo;
        timeTotal = timeOne + timeTwo;
        muOnetxt.text = "μ =" + muOne.ToString("F2");
        muTwoTxt.text = "μ =" + muOne.ToString("F2");
        massBoxTxt.text = "m = " + massBox.ToString("F2") + "kg";
        appliedForceTxt.text = appliedForce.ToString("F2") + "N";
        theQuestion.SetQuestion(("<b>" + PlayerPrefs.GetString("Name") + ("</b> is instructed to pull the box(B) in a horizontal plane with a different of surface friction, at a distnce of <b>") + distanceOne.ToString("F2") + ("</b> meters, wherein the suraface has <b>") + muOne.ToString("F2") + ("</b> coefficient of friction and must continue to move at a distance of <b>") + distanceTwo.ToString("F2") + ("</b> meters that has a coefficient friction of <b>") + muTwo.ToString("F2") + ("</b> applying force of <b>") + appliedForce.ToString("F2") + ("N</b>. If the box has a mass of <b>") + massBox.ToString("F2") + ("</b> kg, How long will it take for <b>") + PlayerPrefs.GetString("Name") + ("</b> to set the box in position?")));

    }
    public IEnumerator StuntResult()
    {
        if (theSimulate.playerAnswer == correctAnswer)
        {
            theQuestion.answerIsCorrect = true;
            yield return new WaitForSeconds(4);
            box.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            target.SetActive(false);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and closed the zombies way in"), true, false);
        }
        StartCoroutine(theSimulate.DirectorsCall());
        if (theSimulate.playerAnswer != correctAnswer)
        {
            theHeart.ReduceLife();
            yield return new WaitForSeconds(4);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has failed to performed the stunt and not able to lock in the zombies"), false, false);

        }
    }
    public IEnumerator zombieChase()
    {
        yield return new WaitForSeconds(.5f);
        theZombie[0].moveSpeed = -2.2f;
        theZombie[0].zombieRun = true;
        theZombie[1].moveSpeed = -2.2f;
        theZombie[1].zombieRun = true;
    }


}
