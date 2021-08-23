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
    public GameObject Mgear, stone, target, puller, arrow, blastPrefab, deflector, trail, lineAngle, boulder, angleArrow;
    public GameObject lineVertical, dimension;
    public float vi, generateVG, vG;
    public float generateAngle, stoneAngle, stoneOpp, HRange, timer, projectileTime, golemTravelTime;
    public float stoneDY, correctAnswer, stoneDyR, generateAnswer;
    public float generateDistance, distance, golemDistanceToTravel;
    public bool timeStart, answerIsCorrect, shootReady;

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
        if (!timeStart)
        {
            stone.transform.position = new Vector2(this.transform.position.x + distance, target.transform.position.y);
            angleArrow.transform.position = this.transform.position;
            angleArrow.transform.rotation = this.transform.rotation;
            golemDistanceToTravel = target.transform.position.x - stone.transform.position.x;
            golemTravelTime = golemDistanceToTravel / vG;
            //generateDistance = stone.transform.position.x - this.transform.position.x;
            //distance = (float)System.Math.Round(generateDistance, 2);
            stoneDY = stone.transform.position.y - this.transform.position.y;
            stoneDyR = (float)System.Math.Round(stoneDY, 2);
            stoneAngle = Mathf.Atan((generateDistance / stoneDyR)) * Mathf.Rad2Deg;
            stoneOpp = (Mathf.Tan(stoneAngle * Mathf.Deg2Rad)) * stoneDyR;
            stone.transform.position = new Vector2(stone.transform.position.x, target.transform.position.y);
            trail.transform.position = this.transform.position;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, generateAngle);
            //HRange = ((arrowVi*arrowVi) * (Mathf.Sin((generateAngle*2) * Mathf.Deg2Rad)))/ Mathf.Abs(Physics2D.gravity.y);
            //rangePoint.transform.position = new Vector2(theShoot.transform.position.x +stoneDH, theShoot.transform.position.y);
            //theShoot.speed = Mathf.Sqrt((stoneDH*Mathf.Abs(Physics2D.gravity.y)) / (Mathf.Sin((generateAngle*2) * Mathf.Deg2Rad)));
            vi = ((Mathf.Sqrt((Mathf.Abs(Physics2D.gravity.y) / (2 * ((stoneOpp * (Mathf.Tan((generateAngle) * Mathf.Deg2Rad))) - stoneDyR))))) * stoneOpp) / (Mathf.Cos((generateAngle) * Mathf.Deg2Rad));
            generateAnswer = golemTravelTime - projectileTime;
            correctAnswer = (float)System.Math.Round(generateAnswer, 2);
            projectileTime = stoneOpp / (vi * (Mathf.Cos(generateAngle * Mathf.Deg2Rad)));
            lineVertical.transform.position = new Vector2(stone.transform.position.x, this.transform.position.y);
            theMeter[0].positionX = lineVertical.transform.position.x + 1;
            theMeter[0].positionY = this.transform.position.y;
            theMeter[0].distance = stoneDyR;

        }
        if (timeStart)
        {
            timer += Time.fixedDeltaTime;

        }

        if (ProjSimulationManager.simulate == true)
        {
            timeStart = true;
            theGolem.moveSpeed = vG;
            trail.SetActive(true);
            dimension.SetActive(false);
            if (shootReady)
            {
                if (timer >= ProjSimulationManager.playerAnswer+.1f)
                {
                    ShootArrow();
                    ProjSimulationManager.simulate = false;
                    if (ProjSimulationManager.playerAnswer == correctAnswer)
                    {
                        theQuestion.answerIsCorrect = true;
                        Shoot();
                        answerIsCorrect = true;
                        deflector.GetComponent<Collider2D>().isTrigger = true;
                    }
                    shootReady = false;
                }
            }


            if (ProjSimulationManager.playerAnswer != correctAnswer)
            {
                //StartCoroutine(golemThrow());
                StartCoroutine(StuntResult());
            }




        }


    }
    public void generateProblem()
    {
        dimension.SetActive(true);
        generateVG = Random.Range(2f, 3f);
        vG = (float)System.Math.Round(generateVG, 2);
        generateDistance = Random.Range(25f, 30);
        distance = (float)System.Math.Round(generateDistance, 2);
        theGolem.transform.position = new Vector2(20, theGolem.transform.position.y);
        timeStart = false;
        arrow.SetActive(false);
        trail.SetActive(false);
        arrow.transform.position = this.transform.position;
        theArrow.line.SetActive(true);
        theArrow.getAngle = false;
        theArrow.generateLine = true;
        generateAngle = Random.Range(50, 60);
        //Mgear.transform.rotation = Quaternion.Euler(Mgear.transform.rotation.x, Mgear.transform.rotation.y, generateAngle);





    }
    IEnumerator ropePull()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 20);

        yield return new WaitForSeconds(projectileTime);
        //theGolem.throwing = true;
        theGolem.moveSpeed = 0;
        yield return new WaitForSeconds(.55f);
        boulder.SetActive(false);
        //theGolem.throwing = false;
        //theBoulder.boulderThrow();
        puller.GetComponent<Rigidbody2D>().velocity = transform.right * 30;
        thePlayer.airdive = true;
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
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (vi);
        }
        if (ProjSimulationManager.playerAnswer > correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (vi);
        }
        if (ProjSimulationManager.playerAnswer == correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (vi);
        }
        GameObject explosion = Instantiate(blastPrefab);
        explosion.transform.position = transform.position;

    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(theSimulate.DirectorsCall());
        //theQuestion.ToggleModal();
    }

}
