using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccHardOne : MonoBehaviour
{
    public float dX, dY, viT, aT, timer, vB, angleB, angleA, sideB, sideC, totalDistance, correctAnswer, answer, playerAnswer;
    //public Quaternion angleB;
    public GameObject gunBarrel, gun, target, targetWheel, projectileLine, dimensions;
    public GameObject verticalOne, horizontal;
    public GameObject bulletPos, wheelPos, bulletHere, targetHere;
    public TruckManager theTruck;
    public Hellicopter theChopper;
    public ShootManager theShoot;
    public DistanceMeter[] theMeter;
    public CircularAnnotation theCurve;
    public QuestionController theQuestion;
    private BulletManager theBullet;
    public AccHardSimulation theSimulate;
    float generateAngleB, generateViT, generateAT, generateVB, generateDX, generateDY;
    float ChopperY, chopperX, truckTime, bulletTime;
    bool shoot, shootReady, gas;
    public bool posCheck;
     public TMP_Text timertxt, timertxtTruck;
    // Start is called before the first frame update
    void Start()
    {

        generateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        theBullet= FindObjectOfType<BulletManager>();
        theShoot.speed = vB;
        
        sideB = (Mathf.Tan(angleA * Mathf.Deg2Rad)) * dY;
        sideC = Mathf.Sqrt((dY * dY) + (sideB * sideB));
        totalDistance = sideB + dX;
        truckTime = (-viT + Mathf.Sqrt((viT * viT) + (4 * ((aT / 2) * totalDistance)))) / aT;
        bulletTime = sideC / vB;
        correctAnswer = truckTime - bulletTime;
        answer = (float)System.Math.Round(correctAnswer, 2);
        playerAnswer = AccHardSimulation.playerAnswer;


        if (AccHardSimulation.simulate == true)
        {
            target.transform.position = targetWheel.transform.position;
            dimensions.SetActive(false);
            if (timer == 0)
            {
                theTruck.moveSpeed = viT;
                theTruck.accelaration = aT;
                theTruck.accelerating = true;
            }
            timer += Time.fixedDeltaTime;
            timertxt.text = timer.ToString("F2") + ("s");
            if (playerAnswer == answer)
            {
                if (timer >= correctAnswer + .05f)
                {

                    shoot = true;

                }
            }
            if (playerAnswer > answer)
            {
                if (timer >= playerAnswer + .1f)
                {
                    shoot = true;
                }
            }
            if (playerAnswer < answer)
            {
                if (timer >= playerAnswer - .1f)
                {
                    shoot = true;
                }
            }
        }

        if (shoot)
        {
            AccHardSimulation.simulate = false;
            timertxt.text = playerAnswer.ToString("F2");
            if (shootReady)
            {
                theShoot.Shoot();
                shootReady = false;
               
            }
            if (posCheck)
            {
                if (playerAnswer != answer)
                {
                    bulletPos.SetActive(true);
                    bulletPos.transform.position = theBullet.gameObject.transform.position;
                    bulletPos.transform.rotation = theBullet.gameObject.transform.rotation;
                    bulletHere.SetActive(true);
                    bulletHere.transform.position = bulletPos.transform.position;
                    wheelPos.SetActive(true);
                    wheelPos.transform.position = targetWheel.transform.position;
                    target.transform.position = wheelPos.transform.position;
                    targetHere.SetActive(true);
                    targetHere.transform.position = wheelPos.transform.position;
                }
                theTruck.accelerating = false;
                theTruck.deaccelerating = true;
                StartCoroutine(StuntResult());
                shoot = false;
            }

        }
    }
    public void generateProblem()
    {
        dimensions.SetActive(true);
        projectileLine.SetActive(true);
        shootReady = true;
        shoot = false;
        dX = Random.Range(9, 12);
        dY = Random.Range(10, 12);
        generateAngleB = Random.Range(20f, 30f);
        angleB = (float)System.Math.Round(generateAngleB, 2);
        generateViT = Random.Range(3f, 5f);
        viT = (float)System.Math.Round(generateViT, 2);
        generateAT = Random.Range(3f, 5f);
        aT = (float)System.Math.Round(generateAT, 2);
        generateVB = Random.Range(30f, 40f);
        vB = (float)System.Math.Round(generateVB, 2);
        gun.transform.rotation = Quaternion.Euler(gun.transform.rotation.x, gun.transform.rotation.y, -angleB);
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
        theMeter[0].positionY = dY - 5;
        theCurve._origin = new Vector2(gunBarrel.transform.position.x + .3f, targetWheel.transform.position.y + dY - .2f);
        theCurve._degrees = angleB;
        angleA = 90 - angleB;
        theQuestion.SetQuestion((("<b>") + PlayerPrefs.GetString("Name") + ("</b> is now instructed to shoot the hub or the center of the moving truck's wheel from a non moving helicopter. If at time = Φ, the hub is <b>") + dX.ToString("F2") + ("</b> meters horizontally behind and <b>") + dY.ToString("F2") + ("</b> meters vertically above the tip of the gun barrel that <b>") + PlayerPrefs.GetString("Name") + ("</b> holding, at how many seconds after should  <b>") + PlayerPrefs.GetString("Name") + ("</b> shoots if the truck has an initial velocity of <b>") + viT.ToString("F2") + ("</b> m/s and accelerating at <b>") + aT.ToString("F2") + ("</b> m/s² while the gun is aimed <b>") + angleB.ToString("F2") + ("</b> degrees below the horizon and its bullet travels at a constant velocity of <b>") + vB.ToString("F2") + ("</b> m/s?")));
        target.transform.position = targetWheel.transform.position;

    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(.5f);
        StartCoroutine(theSimulate.DirectorsCall());
        yield return new WaitForSeconds(.5f);
        theQuestion.ToggleModal();
        theTruck.deaccelerating = false;
        if (playerAnswer == answer)
        {
            Time.timeScale = 0;
        }
    }
}
