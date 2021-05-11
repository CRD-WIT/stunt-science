using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccMediumThree : MonoBehaviour
{
    public GameObject edge, hangingRagdoll, ropeTip, ragdollPrefab, stickmanPoint, playerPos;
    public SubSuv theSubVan;
    public SubHellicopter theSubChopper;
    public Suv theSuv;
    public Player thePlayer;
    public Hellicopter theChopper;
    float correctAnswer, accH, accV, velocity, dv, dx, dh = 40;
    float time, suvPos, chopperPos, generateDv, generateVelocity, generateAccH, generateCorrectAnswer;
    bool repos, ragdollReady;

    // Start is called before the first frame update
    void Start()
    {
        generateProblem();


    }

    // Update is called once per frame
    void Update()
    {
        thePlayer.myRigidbody.mass = 0;
        suvPos = theSuv.transform.position.x;
        chopperPos = theChopper.transform.position.x;
        accV = AccMidSimulation.playerAnswer;
        if (AccMidSimulation.simulate == true)
        {
            thePlayer.transform.position = playerPos.transform.position;
            if (repos)
            {
                theChopper.flySpeed = -velocity;
                theSuv.moveSpeed = -velocity;
                if (suvPos <= theSubVan.transform.position.x)
                {
                    theChopper.deaccelaration = true;
                    theSuv.accelarating = true;
                    theChopper.accelaration = accH;
                    theSuv.accelaration = accV;
                    theSubVan.fade = true;
                    theSubChopper.fade = true;
                    repos = false;
                   

                }
            }
            if (chopperPos <= 15)
            {
                theChopper.deaccelaration = false;
                theChopper.accelaration = accH * 3;
                theChopper.accelarating = true;
                theChopper.myRigidbody.constraints = RigidbodyConstraints2D.None;
                theChopper.flyUp = 4;
                 if (theChopper.flySpeed >= 0)
                    {
                        theChopper.accelarating = false;
                        theChopper.flySpeed = 0;
                        theChopper.flyUp = 0;
                        theChopper.myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
                    }
            }
            if (suvPos <= 15)
            {
                if (accV == correctAnswer)
                {
                    hangingRagdoll.SetActive(true);
                    hangingRagdoll.transform.position = ropeTip.transform.position;
                }
                if (accV != correctAnswer)
                {
                    if (ragdollReady)
                    {
                        ragdollSpawn();
                        ragdollReady = false;
                    }
                }
                //Time.timeScale = 0;
                theSuv.myCollider.enabled = false;
                thePlayer.gameObject.SetActive(false);
                theSuv.accelarating = false;
                theSuv.deaccelarating = true;
                theSuv.accelaration = accV * 3;


                if (theSuv.moveSpeed >= 0)
                {
                    theSuv.moveSpeed = 0;
                }


            }
        }

    }
    public void generateProblem()
    {
        repos = true;
        ragdollReady = true;
        generateDv = Random.Range(22f, 25f);
        generateVelocity = Random.Range(5f, 7f);
        generateAccH = Random.Range(6f, 9f);
        accH = (float)System.Math.Round(generateAccH, 2);
        dv = (float)System.Math.Round(generateDv, 2);
        velocity = (float)System.Math.Round(generateVelocity, 2);
        dx = dh - dv;
        theSubVan.transform.position = new Vector2(edge.transform.position.x + dv, theSubVan.transform.position.y);
        theSubChopper.transform.position = new Vector2(theSubVan.transform.position.x + dx, theSubChopper.transform.position.y);
        theChopper.transform.position = new Vector2(theSuv.transform.position.x + dx, theChopper.transform.position.y);
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        time = (-velocity + Mathf.Sqrt((velocity * velocity) - (4 * (accH / 2) * (-dh)))) / (2 * (accH / 2));
        generateCorrectAnswer = (2 * (dv - (velocity * time))) / (time * time);
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);


    }
    IEnumerator hover()
    {
        yield return new WaitForSeconds(1);
        theChopper.deaccelaration = false;
        theChopper.accelaration = 10;
        theChopper.accelarating = true;

    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(ragdollPrefab);
        stick.transform.position = stickmanPoint.transform.position;
        stick.transform.localScale = new Vector2(-stick.transform.localScale.x, stick.transform.localScale.y);
    }

}
