using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForceHardManagerOne : MonoBehaviour
{
    public ForceHardSimulation theSimulate;
    public PlayerContForcesMed thePlayer;
    public PrisonerManager thePrisoner;
    public BoxManager theBox;
    public GameObject boxOne, pushOrigin, ragdollPrefab, prisoner,prisonGlass,dimensions;
    public float breakingForce, resultantDownhillForce, downhillForce, frictionForce, normalForce, appliedForce, mu, angle, massBox, accBox, finalForce;
    public float boxSpeed, correctAnswer;
    public bool answerIsCorrect, answerIsMorethan, answerIsLessthan;
    public BoxCollisionManager theCollision;
    public QuestionContForcesMed theQuestion;
    public HeartManager theHeart;
    public Vector2 playerStartPoint, boxStartPoint;
    public Quaternion boxStartRot;
    public TMP_Text massBoxTxt,muTxt,angleTxt,breakingForceTxt;

    // Start is called before the first frame update
    void Start()
    {
        playerStartPoint = thePlayer.transform.position;
        boxStartPoint = boxOne.transform.position;
        boxStartRot = boxOne.transform.rotation;
        showProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (theSimulate.simulate == true)
        {
            dimensions.SetActive(false);
            finalForce = theSimulate.playerAnswer + downhillForce - frictionForce;
            accBox = finalForce / massBox;
            thePlayer.push = true;
            boxOne.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            boxSpeed += accBox * Time.fixedDeltaTime;
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
        thePrisoner.ragdollReady = true;
        boxSpeed = 0;
        dimensions.SetActive(true);
        prisonGlass.SetActive(true);
        theCollision.breakReady = true;
        thePlayer.transform.position = playerStartPoint;
        boxOne.transform.position = boxStartPoint;
        boxOne.transform.rotation = boxStartRot;
        boxOne.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        massBox = Random.Range(110, 130);
        mu = (float)System.Math.Round((Random.Range(.6f, .7f)), 2);
        breakingForce = Random.Range(90, 110);
        normalForce = (massBox * 9.81f) * Mathf.Cos(angle * Mathf.Deg2Rad);
        frictionForce = mu * normalForce;
        downhillForce = (massBox * 9.81f) * Mathf.Sin(angle * Mathf.Deg2Rad);
        resultantDownhillForce = downhillForce - frictionForce;
        correctAnswer = (float)System.Math.Round(breakingForce - resultantDownhillForce, 2);
        prisoner.SetActive(true);
        massBoxTxt.text = massBox.ToString("F2") + "kg";
        breakingForceTxt.text = "breaking Force ="+ breakingForce.ToString("F2")+"N";
        muTxt.text = "μ ="+mu.ToString("F2");
        angleTxt.text = " θ ="+angle.ToString("F2")+"°";
        theQuestion.SetQuestion(("<b>" + PlayerPrefs.GetString("Name") + ("</b> is instructed to constantly push the box(A) in an inclined platform downward to break the prison glass, If the platform is inclined at <b>")+angle.ToString("f2")+("°</b> and has a coeffiecient of friction(μ) of <b>")+mu.ToString("F2")+("</b>. How much force is needed to push the box to break the prison glass with a breaking force of <b>")+breakingForce.ToString("F2")+("N</b>, if the box has a mass of <b>")+massBox.ToString("F2")+("kg</b>.Too much force will also hit the captive.")));




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
            theQuestion.answerIsCorrect = true;
            yield return new WaitForSeconds(4);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and freed the captive"), true, false);
        }
        StartCoroutine(theSimulate.DirectorsCall());
        if (theSimulate.playerAnswer != correctAnswer)
        {
            theHeart.losinglife();
            yield return new WaitForSeconds(4);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has failed to performed the stunt and not able to save the captive"), false, false);

        }
    }
}
