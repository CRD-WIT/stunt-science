using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHardOne : MonoBehaviour
{
    public playerProjectile thePlayer;
    public ProjSimulationManager theSimulate;
    public QuestionControllerB theQuestion;
    public golem theGolem;
    public BoulderProjectile theBoulder;
    public DistanceMeter[] theMeter;
    public Arrow theArrow;
    public GameObject Mgear, stone, target, puller, arrow, blastPrefab, deflector, trail, lineAngle, boulder,angleArrow;
    public GameObject lineVertical;
    public float arrowVi;
    public float generateAngle, stoneAngle, stoneOpp,  HRange, timer, time, generateTime;
    public float stoneDY, correctAnswer, stoneDyR;
    public float totalDistance, totalDistanceR;
    public bool hit, answerIsCorrect;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer.aim = true;
        generateProblem();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lineAngle.transform.position = stone.transform.position;
        if (!hit)
        {
            angleArrow.transform.position = this.transform.position;
            angleArrow.transform.rotation = this.transform.rotation;
            totalDistance = stone.transform.position.x - this.transform.position.x;
            totalDistanceR = (float)System.Math.Round(totalDistance, 2);
            stoneDY = stone.transform.position.y - this.transform.position.y;
            stoneDyR = (float)System.Math.Round(stoneDY, 2);
            stoneAngle = Mathf.Atan((totalDistance / stoneDyR)) * Mathf.Rad2Deg;
            stoneOpp = (Mathf.Tan(stoneAngle * Mathf.Deg2Rad)) * stoneDyR;
            stone.transform.position = new Vector2(stone.transform.position.x , target.transform.position.y);
            trail.transform.position = this.transform.position;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, generateAngle);
            //HRange = ((arrowVi*arrowVi) * (Mathf.Sin((generateAngle*2) * Mathf.Deg2Rad)))/ Mathf.Abs(Physics2D.gravity.y);
            //rangePoint.transform.position = new Vector2(theShoot.transform.position.x +stoneDH, theShoot.transform.position.y);
            //theShoot.speed = Mathf.Sqrt((stoneDH*Mathf.Abs(Physics2D.gravity.y)) / (Mathf.Sin((generateAngle*2) * Mathf.Deg2Rad)));
            arrowVi = ((Mathf.Sqrt((Mathf.Abs(Physics2D.gravity.y) / (2 * ((stoneOpp * (Mathf.Tan((generateAngle) * Mathf.Deg2Rad))) - stoneDyR))))) * stoneOpp) / (Mathf.Cos((generateAngle) * Mathf.Deg2Rad));
            correctAnswer = (float)System.Math.Round(arrowVi, 2);
            lineVertical.transform.position = new Vector2(stone.transform.position.x, this.transform.position.y);
            theMeter[0].positionX = lineVertical.transform.position.x+3;
            theMeter[0].positionY = this.transform.position.y;
            theMeter[0].distance = stoneDyR;

        }
        if (ProjSimulationManager.simulate == true)
        {


            trail.SetActive(true);
            ProjSimulationManager.simulate = false;
            ShootArrow();
            if (ProjSimulationManager.playerAnswer == correctAnswer)
            {
                theQuestion.answerIsCorrect = true;
                Shoot();
                answerIsCorrect = true;
                deflector.GetComponent<Collider2D>().isTrigger = true;
            }
            if (ProjSimulationManager.playerAnswer != correctAnswer)
            {
                StartCoroutine(golemThrow());
                StartCoroutine(StuntResult());
            }




        }


    }
    public void generateProblem()
    {
        theGolem.transform.position = new Vector2(20, theGolem.transform.position.y);
        hit = false;
        arrow.SetActive(false);
        trail.SetActive(false);
        arrow.transform.position = this.transform.position;
        theArrow.line.SetActive(true);
        theArrow.getAngle = false;
        theArrow.generateLine = true;
        generateAngle = Random.Range(47, 50);
        generateTime = Random.Range(3f, 4f);
        time = (float)System.Math.Round(generateTime, 2);
        //Mgear.transform.rotation = Quaternion.Euler(Mgear.transform.rotation.x, Mgear.transform.rotation.y, generateAngle);
        
        



    }
    IEnumerator ropePull()
    {
        generateAngle = 20;
        
        yield return new WaitForSeconds(1.5f);
        theGolem.throwing = true;
        yield return new WaitForSeconds(.55f);
        boulder.SetActive(false);
        theGolem.throwing = false;
        theBoulder.boulderThrow();
        puller.GetComponent<Rigidbody2D>().velocity = transform.right * arrowVi;
        thePlayer.airdive = true;
        hit = true;
        thePlayer.aim = false;

    }
    IEnumerator golemThrow()
    {
        yield return new WaitForSeconds(1.5f);
        theGolem.throwing = true;
        yield return new WaitForSeconds(.55f);
        boulder.SetActive(false);
        theGolem.throwing = false;
        theBoulder.boulderThrow();
    }
    public void Shoot()
    {
        StartCoroutine(ropePull());
    }
    public void reShoot()
    {
        puller.GetComponent<Rigidbody2D>().velocity = transform.right * 10;
    }
    public void ShootArrow()
    {
        arrow.SetActive(true);
        arrow.transform.position = transform.position;
        if (ProjSimulationManager.playerAnswer < correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (ProjSimulationManager.playerAnswer - .3f);
        }
        if (ProjSimulationManager.playerAnswer > correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (ProjSimulationManager.playerAnswer + .3f);
        }
        if (ProjSimulationManager.playerAnswer == correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (ProjSimulationManager.playerAnswer);
        }
        GameObject explosion = Instantiate(blastPrefab);
        explosion.transform.position = transform.position;

    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(theSimulate.DirectorsCall());
        theQuestion.ToggleModal();
    }

}
