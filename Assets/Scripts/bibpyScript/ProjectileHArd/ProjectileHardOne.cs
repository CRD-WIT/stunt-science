using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHardOne : MonoBehaviour
{
    public playerProjectile thePlayer;
    public ProjSimulationManager theSimulate;
    public QuestionControllerB theQuestion;
    public golem theGolem;
    public Arrow theArrow;
    public GameObject Mgear, stone, puller, arrow, blastPrefab, deflector,trail;
    public float arrowVi;
    public float generateAngle, generateGolemSpeed, golemSpeed, HRange, timer, time, generateTime;
    public float stoneDY, correctAnswer, stoneDyR;
    public float totalDistance, totalDistanceR, golemDistanceTravel, finalDistance, stoneGapOnX;
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
        if (!hit)
        {
            stoneDY = stone.transform.position.y - this.transform.position.y;
            stoneDyR = (float)System.Math.Round(stoneDY, 2);
            trail.transform.position = this.transform.position;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, generateAngle);
            //HRange = ((arrowVi*arrowVi) * (Mathf.Sin((generateAngle*2) * Mathf.Deg2Rad)))/ Mathf.Abs(Physics2D.gravity.y);
            //rangePoint.transform.position = new Vector2(theShoot.transform.position.x +stoneDH, theShoot.transform.position.y);
            //theShoot.speed = Mathf.Sqrt((stoneDH*Mathf.Abs(Physics2D.gravity.y)) / (Mathf.Sin((generateAngle*2) * Mathf.Deg2Rad)));
            golemDistanceTravel = golemSpeed * time;
            finalDistance = totalDistanceR - golemDistanceTravel;
            arrowVi = ((Mathf.Sqrt((Mathf.Abs(Physics2D.gravity.y) / (2 * ((finalDistance * (Mathf.Tan((generateAngle) * Mathf.Deg2Rad))) - stoneDyR))))) * finalDistance) / (Mathf.Cos((generateAngle) * Mathf.Deg2Rad));
            correctAnswer = (float)System.Math.Round(arrowVi, 2);
        }
        if (ProjSimulationManager.simulate == true)
        {
            timer += Time.fixedDeltaTime;
            theGolem.moveSpeed = golemSpeed;

            if (timer >= time)
            {
                trail.SetActive(true);
                ProjSimulationManager.simulate = false;
                theGolem.moveSpeed = 0;
                theGolem.transform.position = new Vector2(this.transform.position.x + finalDistance + stoneGapOnX, theGolem.transform.position.y);
                timer = 0;
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
                    StartCoroutine(StuntResult());
                }
            }



        }


    }
    public void generateProblem()
    {
        theGolem.transform.position = new Vector2(17, theGolem.transform.position.y);
        hit = false;
        arrow.SetActive(false);
        trail.SetActive(false);
        stoneGapOnX =   theGolem.transform.position.x - stone.transform.position.x;
        arrow.transform.position = this.transform.position;
        theArrow.line.SetActive(true);
        theArrow.getAngle = false;
        theArrow.generateLine = true;
        generateAngle = Random.Range(47, 50);
        generateGolemSpeed = Random.Range(1f, 2f);
        golemSpeed = (float)System.Math.Round(generateGolemSpeed, 2);
        generateTime = Random.Range(3f, 4f);
        time = (float)System.Math.Round(generateTime, 2);
        //Mgear.transform.rotation = Quaternion.Euler(Mgear.transform.rotation.x, Mgear.transform.rotation.y, generateAngle);
        totalDistance = stone.transform.position.x - this.transform.position.x;
        totalDistanceR = (float)System.Math.Round(totalDistance, 2);
        


    }
    IEnumerator ropePull()
    {
        generateAngle = 20;
        yield return new WaitForSeconds(3f);

        puller.GetComponent<Rigidbody2D>().velocity = transform.right * arrowVi;
        thePlayer.airdive = true;
        hit = true;
        thePlayer.aim = false;

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
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (ProjSimulationManager.playerAnswer -.3f);
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
