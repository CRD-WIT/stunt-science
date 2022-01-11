using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceHardManagerOne : MonoBehaviour
{
    public ForceHardSimulation theSimulate;
    public PlayerContForcesMed thePlayer;
    public BoxManager theBox;
    public GameObject boxOne,pushOrigin;
    public float breakingForce, resultantDownhillForce, downhillForce, frictionForce, normalForce,appliedForce, mu, angle, massBox,accBox,finalForce;
    public float boxSpeed, correctAnswer;
    public bool answerIsCorrect;
    public BoxCollisionManager theCollision;

    // Start is called before the first frame update
    void Start()
    {
        showProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(theSimulate.simulate == true)
        {
            finalForce = theSimulate.playerAnswer + downhillForce - frictionForce;
            accBox = finalForce/massBox;
            thePlayer.push = true;
            boxOne.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            boxSpeed += accBox * Time.fixedDeltaTime;
            boxOne.GetComponent<Rigidbody2D>().velocity = pushOrigin.transform.right * (boxSpeed*massBox);
            thePlayer.myRigidbody.velocity = pushOrigin.transform.right * (boxSpeed*massBox);
            if(theSimulate.playerAnswer == correctAnswer)
            {
                answerIsCorrect = true;
            }
        }
        
    }
    public void showProblem()
    {
        theCollision.breakReady = true;
        boxOne.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        massBox = (float)System.Math.Round((Random.Range(190f, 200f)), 2);
        mu = (float)System.Math.Round((Random.Range(.6f, .7f)), 2);
        breakingForce = Random.Range(90, 110);
        normalForce = (massBox*9.81f) * Mathf.Cos(angle * Mathf.Deg2Rad);
        frictionForce = mu*normalForce;
        downhillForce = (massBox*9.81f) * Mathf.Sin(angle * Mathf.Deg2Rad);
        resultantDownhillForce = downhillForce - frictionForce;
        correctAnswer = (float)System.Math.Round(breakingForce - resultantDownhillForce,2);



    }
}
