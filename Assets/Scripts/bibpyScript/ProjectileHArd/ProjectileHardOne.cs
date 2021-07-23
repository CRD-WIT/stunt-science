using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHardOne : MonoBehaviour
{
    public PlayerB thePlayer;
    public ShootManager theShoot;
    public GameObject Mgear, arrowPoint,rangePoint,stone;
    public float arrowVi;
    public float generateAngle, HRange;
    public float stoneDY,stoneDX,stoneAngle, stoneDH;
    public bool hit;

    // Start is called before the first frame update
    void Start()
    {
        generateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stoneDX = stone.transform.position.x - theShoot.transform.position.x;
        stoneDY = stone.transform.position.y - theShoot.transform.position.y;
        stoneDH = Mathf.Sqrt((stoneDX*stoneDX) + (stoneDY*stoneDY));
        stoneAngle = Mathf.Asin(stoneDY/stoneDH)* Mathf.Rad2Deg;
       
        Mgear.transform.rotation = Quaternion.Euler(Mgear.transform.rotation.x, Mgear.transform.rotation.y, generateAngle);
        transform.rotation =  Quaternion.Euler(transform.rotation.x, transform.rotation.y, generateAngle);
        //HRange = ((arrowVi*arrowVi) * (Mathf.Sin((generateAngle*2) * Mathf.Deg2Rad)))/ Mathf.Abs(Physics2D.gravity.y);
        rangePoint.transform.position = new Vector2(theShoot.transform.position.x +stoneDH, theShoot.transform.position.y);
        //theShoot.speed = Mathf.Sqrt((stoneDH*Mathf.Abs(Physics2D.gravity.y)) / (Mathf.Sin((generateAngle*2) * Mathf.Deg2Rad)));
        arrowVi = ((Mathf.Sqrt((Mathf.Abs(Physics2D.gravity.y)/(2 * ((stoneDX * (Mathf.Tan((generateAngle) * Mathf.Deg2Rad))) - stoneDY))))) * stoneDX) / (Mathf.Cos((generateAngle)* Mathf.Deg2Rad)) ;
        theShoot.speed = arrowVi;
        if(hit)
        {
            StartCoroutine(ropePull());
        }
    }
    public void generateProblem()
    {
        //arrowVi = Random.Range(10,15);
        generateAngle = Random.Range(30,25);
    }
    IEnumerator ropePull()
    {
        yield return new WaitForSeconds(3);
        thePlayer.moveSpeed = arrowVi;
       
    }
}
