using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForceHardManagerThree : MonoBehaviour
{
    public TMP_Text debugAnswer;
    public ForceHardSimulation theSimulate;
    public PlayerContForcesMed thePlayer;
    public PrisonerManager thePrisoner;
    // public BoxManager theBox;
    public GameObject boxOne, pushOrigin, ragdollPrefab, prisoner, prisonGlass, dimensions,platform,elevator;
    public float breakingForce, resultantDownhillForce, downhillForce, frictionForce, normalForce, appliedForceCorrect, appliedForcePlayer, mu, angle, massBox, accBoxCorrect, accBoxPlayer, finalForce, finalForceCorrect;
    public float boxSpeed, correctAnswer, playerMassBox, downhillForcePlayer, normalForcePlayer, frictionForcePlayer,elevatorSpeed;
    public bool answerIsCorrect, answerIsMorethan, answerIsLessthan;
    public BoxCollisionManager theCollision;
    public QuestionContForcesMed theQuestion;
    public HeartManager theHeart;
    public Vector2 playerStartPoint, boxStartPoint;
    public Quaternion boxStartRot;
    public TMP_Text massBoxTxt, frictionTxt, angleTxt, breakingForceTxt, forceAppliedTxt;

    // Start is called before the first frame update
    void Start()
    {
        playerStartPoint = new Vector2(-11.81f, -.66f);
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        boxStartPoint = boxOne.transform.position;
        boxStartRot = boxOne.transform.rotation;
        showProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        debugAnswer.SetText($"Answer: {correctAnswer}");
        elevator.GetComponent<Rigidbody2D>().velocity = new Vector2(0,elevatorSpeed);
        if (theSimulate.simulate == true)
        {
            
            dimensions.SetActive(false);
            playerMassBox = 50 + theSimulate.playerAnswer;
            massBoxTxt.text = playerMassBox.ToString("F2") + "kg";
            downhillForcePlayer = (playerMassBox * 9.81f) * Mathf.Sin(angle * Mathf.Deg2Rad);
            normalForcePlayer = (playerMassBox * 9.81f) * Mathf.Cos(angle * Mathf.Deg2Rad);
            frictionForcePlayer = mu * normalForcePlayer;
            resultantDownhillForce = downhillForce - frictionForce;
            dimensions.SetActive(false);
            accBoxCorrect = finalForceCorrect / massBox;
            // appliedForcePlayer = accBoxCorrect * playerMassBox;
            // appliedForcePlayer = frictionForcePlayer + finalForce - downhillForcePlayer;
            finalForce = appliedForceCorrect + downhillForcePlayer - frictionForcePlayer;
            accBoxPlayer = finalForce / playerMassBox;
            thePlayer.push = true;
            boxOne.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            boxSpeed += accBoxCorrect * Time.fixedDeltaTime;
            boxOne.GetComponent<Rigidbody2D>().velocity = pushOrigin.transform.right * (boxSpeed);
            thePlayer.myRigidbody.velocity = pushOrigin.transform.right * (boxSpeed);

            if (theSimulate.playerAnswer == correctAnswer)
            {
                answerIsCorrect = true;
                theQuestion.answerIsCorrect = true;
            }
            if (theSimulate.playerAnswer > correctAnswer)
            {
                answerIsMorethan = true;
            }
            if (theSimulate.playerAnswer < correctAnswer)
            {
                answerIsLessthan = true;
            }
        }

    }
    public void showProblem()
    {
        answerIsMorethan = false;
        answerIsLessthan = false;
        massBoxTxt.gameObject.SetActive(true);
        thePrisoner.ragdollReady = true;
        boxSpeed = 0;
        dimensions.SetActive(true);
        prisonGlass.SetActive(true);
        theCollision.breakReady = true;
        thePlayer.transform.position = playerStartPoint;
        boxOne.transform.position = boxStartPoint;
        boxOne.transform.rotation = boxStartRot;
        boxOne.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        appliedForceCorrect = Random.Range(350, 400);
        // massBox = Random.Range(110, 130);
        // mu = (float)System.Math.Round((Random.Range(.6f, .7f)), 2);
        breakingForce = (float)System.Math.Round((Random.Range(110f, 120f)), 1);

        // frictionForce = mu * normalForce;
        frictionForce = Random.Range(700, 800);

        Debug.Log($"Log: {frictionForce}, {normalForce} => {frictionForce / normalForce}");
        downhillForce = +breakingForce + frictionForce - appliedForceCorrect;
        // downhillForce = (massBox * 9.81f) * Mathf.Sin(angle * Mathf.Deg2Rad);
        correctAnswer = (float)System.Math.Round((downhillForce / (9.81f * (Mathf.Sin(angle * Mathf.Deg2Rad)))) - 50, 2);
        massBox = correctAnswer + 50;
        normalForce = (massBox * 9.81f) * Mathf.Cos(angle * Mathf.Deg2Rad);
        prisoner.SetActive(true);
        finalForceCorrect = appliedForceCorrect + downhillForce - frictionForce;
        massBoxTxt.text = "50kg";
        mu = frictionForce / normalForce;
        forceAppliedTxt.text = "Force Applied = " + appliedForceCorrect.ToString("F2");
        breakingForceTxt.text = "breaking Force =" + breakingForce.ToString("F2") + "N";
        frictionTxt.text = "Ff =" + frictionForce.ToString("F2");
        angleTxt.text = " θ =" + angle.ToString("F2") + "°";
        theQuestion.SetQuestion(("<b>" + PlayerPrefs.GetString("Name") + ("</b> is instructed to constantly push the box(C) to break the prison glass with a breaking force of <b>") + breakingForce.ToString("f2") + ("N</b>, If the platform is inclined at <b>") + angle.ToString("F2") + ("</b>°. and the force needed by ") + PlayerPrefs.GetString("Name") + (", to break the prison glass is <b>") + appliedForceCorrect.ToString("F2") + ("N</b> that will result to platform surface friction force of <b>")+frictionForce.ToString("F2")+("N</b>. Currently the box(C) has a mass of <b>50kg</b>. How many kilogram is ")+ PlayerPrefs.GetString("Name") +("needed to fill in the box in order to break the prison glass without hitting the captive?") ));




    }
    public IEnumerator overForce()
    {
        yield return new WaitForSeconds(.5f);
        theSimulate.simulate = false;
        thePlayer.push = false;

    }
    public void ragdollSpawn()
    {
        GameObject ragdoll = Instantiate(ragdollPrefab);
        ragdoll.transform.position = prisoner.transform.position;
        prisoner.SetActive(false);
    }
    public IEnumerator StuntResult()
    {
        if (theSimulate.playerAnswer == correctAnswer)
        {
            platform.GetComponent<BoxCollider2D>().enabled = false;
            theQuestion.answerIsCorrect = true;
            yield return new WaitForSeconds(2);
            theSimulate.destroyGlass = true;
            theHeart.startbgentrance();
            boxOne.SetActive(false);
            StartCoroutine(exitPlayer());
            StartCoroutine(thePrisoner.startRun2());
        }
        StartCoroutine(theSimulate.DirectorsCall());
        if (theSimulate.playerAnswer != correctAnswer)
        {
            theHeart.losinglife();
            yield return new WaitForSeconds(4);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has failed to performed the stunt and not able to save the captive"), false, false);

        }
    }
    public IEnumerator exitPlayer()
    {
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        thePlayer.transform.rotation = Quaternion.Euler(0,0,0);
        thePlayer.moveSpeed = 4;
        thePlayer.run = true;
        yield return new WaitForSeconds(7f);
        thePlayer.moveSpeed = 0;
        thePlayer.run = false;
        yield return new WaitForSeconds(1.5f);
        platform.GetComponent<BoxCollider2D>().enabled = true;
        thePlayer.moveSpeed = -4;
        thePlayer.run = true;
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        yield return new WaitForSeconds(8);
        thePlayer.moveSpeed = 0;
        thePlayer.run = false;
        elevatorSpeed = 1;
        theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and freed the captive"), true, true);
        
    }
}
