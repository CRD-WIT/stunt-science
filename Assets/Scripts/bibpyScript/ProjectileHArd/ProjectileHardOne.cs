using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHardOne : MonoBehaviour
{
    public playerProjectile thePlayer;
    public golem theGolem;
    public ShootManager theShoot;
    public GameObject Mgear, arrowPoint, rangePoint, stone, puller, arrow, blastPrefab, deflector;
    public float arrowVi;
    public float generateAngle, generateGolemSpeed, golemSpeed, HRange, timer, time, generateTime;
    public float stoneDY, correctAnswer, stoneDyR;
    public float totalDistance,totalDistanceR, golemDistanceTravel, finalDistance,stoneGapOnX;
    public bool hit, answerIsCorrect;

    // Start is called before the first frame update
    void Start()
    {
        generateProblem();
        stoneGapOnX = stone.transform.position.x - theGolem.transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hit)
        {             
            
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
                ProjSimulationManager.simulate = false;
                theGolem.moveSpeed = 0;
                theGolem.transform.position = new Vector2 (theShoot.transform.position.x + finalDistance + stoneGapOnX, theGolem.transform.position.y);
                timer = 0;
                ShootArrow();
                if (ProjSimulationManager.playerAnswer == correctAnswer)
                {
                    Shoot();
                    answerIsCorrect = true;
                    deflector.GetComponent<Collider2D>().isTrigger = true;
                }
            }



        }


    }
    public void generateProblem()
    {
        thePlayer.aim = true;
        generateAngle = Random.Range(50, 47);
        generateGolemSpeed = Random.Range(1f, 2f);
        golemSpeed = (float)System.Math.Round(generateGolemSpeed, 2);
        generateTime = Random.Range(4f, 5f);
        time = (float)System.Math.Round(generateTime, 2);
        //Mgear.transform.rotation = Quaternion.Euler(Mgear.transform.rotation.x, Mgear.transform.rotation.y, generateAngle);
        totalDistance = stone.transform.position.x - theShoot.transform.position.x;
        totalDistanceR = (float)System.Math.Round(totalDistance, 2);
        stoneDY = stone.transform.position.y - theShoot.transform.position.y;
        stoneDyR = (float)System.Math.Round(stoneDY, 2);


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
        arrow.transform.position = arrowPoint.transform.position;
        arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (ProjSimulationManager.playerAnswer);
        GameObject explosion = Instantiate(blastPrefab);
        explosion.transform.position = transform.position;

    }
}
