using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHardOne : MonoBehaviour
{
    public playerProjectile thePlayer;
    public ShootManager theShoot;
    public GameObject Mgear, arrowPoint, rangePoint, stone, puller, arrow, blastPrefab, deflector;
    public float arrowVi;
    public float generateAngle, HRange;
    public float stoneDY, stoneDX, correctAnswer,stoneDyR, stoneDxR;
    public bool hit, answerIsCorrect;

    // Start is called before the first frame update
    void Start()
    {
        generateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hit)
        {
            stoneDX = stone.transform.position.x - theShoot.transform.position.x;
            stoneDY = stone.transform.position.y - theShoot.transform.position.y;
            stoneDxR = (float)System.Math.Round(stoneDX, 2);
            stoneDyR = (float)System.Math.Round(stoneDY, 2);
            Mgear.transform.rotation = Quaternion.Euler(Mgear.transform.rotation.x, Mgear.transform.rotation.y, generateAngle);
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, generateAngle);
            //HRange = ((arrowVi*arrowVi) * (Mathf.Sin((generateAngle*2) * Mathf.Deg2Rad)))/ Mathf.Abs(Physics2D.gravity.y);
            //rangePoint.transform.position = new Vector2(theShoot.transform.position.x +stoneDH, theShoot.transform.position.y);
            //theShoot.speed = Mathf.Sqrt((stoneDH*Mathf.Abs(Physics2D.gravity.y)) / (Mathf.Sin((generateAngle*2) * Mathf.Deg2Rad)));
            arrowVi = ((Mathf.Sqrt((Mathf.Abs(Physics2D.gravity.y) / (2 * ((stoneDxR * (Mathf.Tan((generateAngle) * Mathf.Deg2Rad))) - stoneDyR))))) * stoneDxR) / (Mathf.Cos((generateAngle) * Mathf.Deg2Rad));
            correctAnswer = (float)System.Math.Round(arrowVi, 2);
        }
        if (ProjSimulationManager.simulate == true)
        {
            if (ProjSimulationManager.playerAnswer == correctAnswer)
            {
                
                Shoot();
                answerIsCorrect = true;
                deflector.GetComponent<Collider2D>().isTrigger = true;
            }
            ShootArrow();
            
        }


    }
    public void generateProblem()
    {
        thePlayer.aim = true;
        generateAngle = Random.Range(50, 45);

    }
    IEnumerator ropePull()
    {
        generateAngle = 25;
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
        ProjSimulationManager.simulate = false;
    }
}
