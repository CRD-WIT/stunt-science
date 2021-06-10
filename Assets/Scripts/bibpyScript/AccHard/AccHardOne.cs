using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccHardOne : MonoBehaviour
{
    public float dX, dY, viT, aT, timer, vB, angleB, angleA, sideB, sideC, totalDistance, correctAnswer;
    //public Quaternion angleB;
    public GameObject gunBarrel,gun, target, targetWheel;
    public GameObject verticalOne, horizontal;
    public TruckManager theTruck;
    public Hellicopter theChopper;
    public ShootManager theShoot;
    public DistanceMeter[] theMeter;
    public CircularAnnotation theCurve;
    float generateAngleB, generateViT, generateAT, generateVB, generateDX, generateDY;
    float ChopperY, chopperX, truckTime,bulletTime;
    bool shoot;
    // Start is called before the first frame update
    void Start()
    {
        generateProblem();
    }

    // Update is called once per frame
    void Update()
    { 
        theShoot.speed = vB;
        target.transform.position = targetWheel.transform.position;
        sideB = (Mathf.Tan(angleA * Mathf.Deg2Rad)) *dY;
        sideC = Mathf.Sqrt((dY*dY)+(sideB * sideB));
        totalDistance = sideB + dX;
        truckTime = (-viT + Mathf.Sqrt((viT*viT) + (4*((aT/2)* totalDistance)))) / aT;
        bulletTime = sideC/ vB;
        correctAnswer = truckTime-bulletTime;
        if(AccHardSimulation.simulate == true)
        {
            timer += Time.fixedDeltaTime;
            theTruck.accelerating = true;
            theTruck.moveSpeed = viT;
            theTruck.accelaration = aT;
            if(timer >= correctAnswer)
            {
                shoot = true;
            }
        }
        if(shoot)
        {
            theShoot.Shoot();
        }
    }
    public void generateProblem()
    {
        shoot = false;
        dX = Random.Range(9,12);
        dY = Random.Range(10,12);
        generateAngleB = Random.Range(-20f, -30f);
        angleB =(float)System.Math.Round(generateAngleB, 2);
        generateViT = Random.Range(3f,5f);
        viT = (float)System.Math.Round(generateViT, 2);
        generateAT = Random.Range(3f, 5f);
        aT = (float)System.Math.Round(generateAT, 2);
        generateVB = Random.Range(20f, 30f);
        vB = (float)System.Math.Round(generateVB, 2);
        gun.transform.rotation = Quaternion.Euler(gun.transform.rotation.x,gun.transform.rotation.y,angleB);
        ChopperY = theChopper.transform.position.y - gunBarrel.transform.position.y;
        chopperX = gunBarrel.transform.position.x - theChopper.transform.position.x;
        theChopper.transform.position = new Vector2(targetWheel.transform.position.x + dX - chopperX, targetWheel.transform.position.y + dY + ChopperY);
        horizontal.transform.position = new Vector2(horizontal.transform.position.x, gunBarrel.transform.position.y);
        verticalOne.transform.position = new Vector2(gunBarrel.transform.position.x, verticalOne.transform.position.y);
        theMeter[1].distance = dY;
        theMeter[1].positionX = targetWheel.transform.position.x;
        theMeter[1].positionY = targetWheel.transform.position.y;
        theMeter[0].distance = dX;
        theMeter[0].positionX = targetWheel.transform.position.x;
        theMeter[0].positionY =dY - 5;
        theCurve._origin = new Vector2 (gunBarrel.transform.position.x +.3f, targetWheel.transform.position.y + dY -.2f);
        theCurve._degrees = -angleB;
        angleA = 90+angleB;
        

    }
}
