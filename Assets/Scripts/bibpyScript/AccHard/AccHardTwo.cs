using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccHardTwo : MonoBehaviour
{
    public float dX, dY, viT, aT, timer, vB, angleB, angleA, sideB, sideC, overlapDistance, correctAnswer, answer, playerAnswer, truckDistance, targetDistance;
    //public Quaternion angleB;
    public GameObject gunBarrel, gun, target, targetWheel, projectileLine, dimensions, cam;
    public GameObject verticalOne, horizontal;
    public GameObject bulletPos, wheelPos, bulletHere, targetHere;
    public TruckManager theTruck;
    public MulticabManager theMulticab;
    public Hellicopter theChopper;
    public ShootManager theShoot;
    public DistanceMeter[] theMeter;
    public CircularAnnotation theCurve;
    public QuestionController2 theQuestion;
    private BulletManager theBullet;
    public AccHardSimulation theSimulate;
    float generateAngleB, generateViT, generateAT, generateVB, generateDX, generateDY, generateTime, time;
    float generateAH, aH, viH;
    float ChopperY, chopperX, truckTime, bulletTime, truckCurrentPos, chopperTimeToTravel;
    bool shoot, shootReady, gas, startTime;
    public bool posCheck;
    public TMP_Text timertxt, timertxtTruck, actiontxt, viTtxt, aTtxt;
    float camPos, distanceCheck;
    int tries, stopTruckPos, attemp;
    // Start is called before the first frame update
    void Start()
    {
        theSimulate.stage = 2;
        StartCoroutine(positioning());
        camPos = cam.transform.position.x - theChopper.transform.position.x;
        stopTruckPos = 210;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cam.transform.position = new Vector3(theChopper.transform.position.x + camPos, cam.transform.position.y, cam.transform.position.z);
        truckDistance = (viT * time) + (aT * (time * time)) / 2;
        overlapDistance = sideB - dX;
        targetDistance = truckDistance - overlapDistance;
        truckCurrentPos = theTruck.transform.position.x;
        theBullet = FindObjectOfType<BulletManager>();
        theShoot.speed = vB;
        sideB = dY / (Mathf.Tan(angleB * Mathf.Deg2Rad));
        sideC = Mathf.Sqrt((dY * dY) + (sideB * sideB));
        bulletTime = sideC / vB;
        chopperTimeToTravel = time - bulletTime;
        viH = AccHardSimulation.playerAnswer;
        correctAnswer = (targetDistance / chopperTimeToTravel) - ((aH * chopperTimeToTravel) / 2);
        answer = (float)System.Math.Round(correctAnswer, 2);
        distanceCheck = (correctAnswer * chopperTimeToTravel) + (((chopperTimeToTravel * chopperTimeToTravel) * aH) / 2);
        playerAnswer = AccHardSimulation.playerAnswer;
        if (startTime)
        {
            timer += Time.fixedDeltaTime;
            timertxtTruck.text = timer.ToString("F2") + ("s");
        }
        if (AccHardSimulation.simulate == true)
        {
            
            startTime = true;
            target.transform.position = targetWheel.transform.position;
            dimensions.SetActive(false);
            viTtxt.text = ("v = ") + theTruck.moveSpeed.ToString("F2") + ("m/s");
            if (timer == 0)
            {
                theChopper.flySpeed = viH;
                theChopper.accelaration = aH;
                theChopper.accelarating = true;

                theTruck.moveSpeed = viT;
                theTruck.accelaration = aT;
                theTruck.accelerating = true;
            }

            timertxt.text = timer.ToString("F2") + ("s");
            if (playerAnswer == answer)
            {
                theQuestion.answerIsCorrect = true;
                actiontxt.text = ("next");
                if (timer >= chopperTimeToTravel + .1f)
                {
                    theChopper.flySpeed = 0;
                    theChopper.accelaration = 0;
                    theChopper.accelarating = false;
                    shoot = true;
                }
            }
            if (playerAnswer > answer)
            {
                if (timer >= chopperTimeToTravel)
                {
                    shoot = true;

                }
            }
            if (playerAnswer < answer)
            {

                if (timer >= chopperTimeToTravel)
                {
                    shoot = true;
                }
            }
            if (shoot == false & truckCurrentPos >= stopTruckPos)
            {
                AccHardSimulation.simulate = false;
                startTime = false;
                StartCoroutine(StuntResult());

            }
        }

        if (shoot)
        {
            AccHardSimulation.simulate = false;
            target.SetActive(false);
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
                    actiontxt.text = ("Retry");
                    bulletPos.SetActive(true);
                    bulletPos.transform.position = theBullet.gameObject.transform.position;
                    bulletPos.transform.rotation = theBullet.gameObject.transform.rotation;
                    bulletHere.SetActive(true);
                    bulletHere.transform.position = bulletPos.transform.position;
                    wheelPos.SetActive(true);
                    wheelPos.transform.position = targetWheel.transform.position;
                    target.SetActive(true);
                    target.transform.position = wheelPos.transform.position;
                    targetHere.SetActive(true);
                    targetHere.transform.position = wheelPos.transform.position;
                }
                theTruck.accelerating = false;
                StartCoroutine(StuntResult());
                shoot = false;
            }

        }
        if (tries == attemp)
        {
            if (truckCurrentPos >= stopTruckPos)
            {
                theTruck.moveSpeed = 0;
            }
        }


    }
    public void generateProblem()
    {
        target.SetActive(true);
        dimensions.SetActive(true);
        shootReady = true;
        dX = Random.Range(11, 13);
        generateAH = Random.Range(3, 5);
        aH = (float)System.Math.Round(generateAH, 2);
        generateTime = Random.Range(2f, 2.5f);
        time = (float)System.Math.Round(generateTime, 2);
        generateAngleB = Random.Range(26f, 35f);
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
        dX = targetWheel.transform.position.x - gunBarrel.transform.position.x;
        dY = gunBarrel.transform.position.y - targetWheel.transform.position.y;
        //theChopper.transform.position = new Vector2(targetWheel.transform.position.x + dX - chopperX, targetWheel.transform.position.y + dY + ChopperY);
        horizontal.transform.position = new Vector2(gunBarrel.transform.position.x, gunBarrel.transform.position.y);
        verticalOne.transform.position = new Vector2(gunBarrel.transform.position.x, verticalOne.transform.position.y);
        theMeter[1].distance = dY;
        theMeter[1].positionX = targetWheel.transform.position.x;
        theMeter[1].positionY = targetWheel.transform.position.y;
        theMeter[0].distance = dX;
        theMeter[0].positionX = gunBarrel.transform.position.x;
        theMeter[0].positionY = dY - 5;
        theCurve._origin = new Vector2(gunBarrel.transform.position.x, gunBarrel.transform.position.y);
        theCurve._degrees = angleB;
        angleA = 90 - angleB;
        theQuestion.SetQuestion((("<b>") + PlayerPrefs.GetString("Name") + ("</b> is now instructed to shoot the hub or the center of the moving truck's wheel from a non moving helicopter. If at time = Φ, the hub is <b>") + dX.ToString("F2") + ("</b> meters horizontally behind and <b>") + dY.ToString("F2") + ("</b> meters vertically above the tip of the gun barrel that <b>") + PlayerPrefs.GetString("Name") + ("</b> holding, at how many seconds after should  <b>") + PlayerPrefs.GetString("Name") + ("</b> shoots if the truck has an initial velocity of <b>") + viT.ToString("F2") + ("</b> m/s and accelerating at <b>") + aT.ToString("F2") + ("</b> m/s² while the gun is aimed <b>") + angleB.ToString("F2") + ("</b> degrees below the horizon and its bullet travels at a constant velocity of <b>") + vB.ToString("F2") + ("</b> m/s?")));
        target.transform.position = targetWheel.transform.position;
        timer = 0;
        bulletPos.SetActive(false);
        bulletHere.SetActive(false);
        wheelPos.SetActive(false);
        targetHere.SetActive(false);
        posCheck = false;
        viTtxt.text = ("vi = ") + viT.ToString("F2") + ("m/s");
        aTtxt.text = ("a = ") + aT.ToString("F2") + ("m/s²");
        timertxtTruck.text = timer.ToString("F2") + ("s");
    }
    IEnumerator positioning()
    {
        Time.timeScale = 1;
        theChopper.moving = true;
        theTruck.moveSpeed = 3;
        theChopper.flySpeed = 15;
        yield return new WaitForSeconds(1);
        theMulticab.moveSpeed = 20;
        yield return new WaitForSeconds(3);
        theChopper.flySpeed = 0;
        theMulticab.moveSpeed = 0;
        theTruck.moveSpeed = 0;
        generateProblem();


    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(theSimulate.DirectorsCall());
        yield return new WaitForSeconds(1f);
        theQuestion.ToggleModal();
        theTruck.deaccelerating = false;
        if (playerAnswer == answer)
        {
            targetWheel.SetActive(false);
            Time.timeScale = 0;
        }


    }
}
