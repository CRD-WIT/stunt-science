using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AccHardTwo : MonoBehaviour
{
    // Stunt Guide
    public GameObject[] stuntGuideObjectPrefabs;
    public Image stuntGuideImage;
    public Sprite stuntGuideImageSprite;
    // End of Stunt Guide
    public float dX, dY, viT, aT, timer, vB, angleB, angleA, sideB, sideC, overlapDistance, correctAnswer, answer, playerAnswer, truckDistance, targetDistance;
    //public Quaternion angleB;
    public GameObject gunBarrel, gun, target, targetWheel, projectileLine, dimensions, cam, directorPlatform;
    public GameObject verticalOne, horizontal;
    public GameObject bulletPos, wheelPos, bulletHere, targetHere, truckInitials, chopperInitials,targetSignAge;
    public TruckManager theTruck;
    public MulticabManager theMulticab;
    public Hellicopter theChopper;
    public ShootManager theShoot;
    public DistanceMeter[] theMeter;
    public CircularAnnotation theCurve;
    public QuestionControllerC theQuestion;
    private BulletManager theBullet;
    public AccHardSimulation theSimulate;
    public HeartManager theHeart;
    float generateAngleB, generateViT, generateAT, generateVB, generateDX, generateDY, generateTime, time;
    float generateAH, aH, viH;
    float ChopperY, chopperX, truckTime, bulletTime, truckCurrentPos, chopperTimeToTravel, distanceBetween, truckTimeToTravel;
    bool shoot, shootReady, gas, startTime;
    public bool posCheck, toRetry, camFollow;
    public TMP_Text timertxt, timertxtTruck, actiontxt, viTtxt, aTtxt, viHtxt, accHtxt;
    float camPos, distanceCheck;
    int tries, attemp;
    public TMP_Text debugAnswer;
    public Button play;
    public AudioSource chopperEngine, truckIdle,truckRunning,GunShot;
    // Start is called before the first frame update
    void Start()
    {
        theQuestion.stage = 2;
        targetSignAge.SetActive(true);
        //theQuestion.stageNumber = 2;
        theSimulate.stage = 2;
        StartCoroutine(positioning());
        camPos = cam.transform.position.x - theChopper.transform.position.x;
        theSimulate.takeNumber = 1;
        camFollow = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
                //Stunt Guide        
        stuntGuideObjectPrefabs[0].SetActive(false);
        stuntGuideObjectPrefabs[1].SetActive(true);
        stuntGuideObjectPrefabs[2].SetActive(false);
        stuntGuideImage.sprite = stuntGuideImageSprite;
        
        targetSignAge.transform.position = new Vector3(targetWheel.transform.position.x-2, targetWheel.transform.position.y, -5);
        debugAnswer.SetText($"Answer: {System.Math.Round(correctAnswer,2)}");
        truckInitials.transform.position = theTruck.transform.position;
        accHtxt.text = ("a = 0 m/s??");
        chopperInitials.transform.position = theChopper.transform.position;
        truckDistance = (viT * truckTimeToTravel) + (aT * (truckTimeToTravel * truckTimeToTravel)) / 2;
        overlapDistance = sideB - dX;
        targetDistance = truckDistance + overlapDistance;
        truckCurrentPos = theTruck.transform.position.x;
        theBullet = FindObjectOfType<BulletManager>();
        theShoot.speed = vB;
        viH = AccHardSimulation.playerAnswer;
        //correctAnswer = (targetDistance / chopperTimeToTravel) - ((aH * chopperTimeToTravel) / 2);
        correctAnswer = targetDistance / time;
        answer = (float)System.Math.Round(correctAnswer, 2);
        distanceCheck = (correctAnswer * chopperTimeToTravel) + (((chopperTimeToTravel * chopperTimeToTravel) * aH) / 2);
        playerAnswer = AccHardSimulation.playerAnswer;
        distanceBetween = truckCurrentPos - theChopper.transform.position.x;
        if (camFollow == true)
        {
            cam.transform.position = new Vector3(theChopper.transform.position.x + camPos, cam.transform.position.y, cam.transform.position.z);
        }
        if (startTime)
        {
            timer += Time.fixedDeltaTime;
            timertxtTruck.text = timer.ToString("F2") + ("s");
        }
        if (distanceBetween >= 75f)
        {
            theTruck.accelerating = false;
            theTruck.moveSpeed = 0;
            startTime = false;

        }
        if (AccHardSimulation.simulate == true)
        {
            play.interactable = false;
            viHtxt.text = ("v = " + viH.ToString("F2")) + ("m/s");
            camFollow = true;
            startTime = true;
            target.transform.position = targetWheel.transform.position;
            dimensions.SetActive(false);
            viTtxt.text = ("v = ") + theTruck.moveSpeed.ToString("F2") + ("m/s");
            if (timer == 0)
            {
                theChopper.flySpeed = viH;
                //theChopper.accelaration = aH;
                //theChopper.accelarating = true;

                theTruck.moveSpeed = viT;
                theTruck.accelaration = aT;
                theTruck.accelerating = true;
                theMulticab.moveSpeed = viT + 5;
                theChopper.moving = true;
            }

            timertxt.text = timer.ToString("F2") + ("s");
            if (playerAnswer == answer)
            {
                theSimulate.answerIsCorrect = true;
                theQuestion.answerIsCorrect = true;
                actiontxt.text = ("next");
                //theQuestion.SetModalTitle("Stunt success");
                //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " successfully hit the 2nd target!");
                if (timer >= time + .3f)
                {
                    shoot = true;
                    theChopper.flySpeed = 0;
                    theChopper.accelaration = 0;
                    theChopper.accelarating = false;
                    viHtxt.color = new Color32(13, 106, 0, 255);


                }
            }
            if (playerAnswer > answer)
            {
                theChopper.flySpeed = viH + 1f;
                //theQuestion.SetModalTitle("Stunt failed");
                //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " flies the helicopter too fast. The correct answer is </color>" + answer.ToString("F2") + "m/s.");
                if (timer >= time)
                {
                    theChopper.flySpeed = 0;
                    theChopper.accelaration = 0;
                    theChopper.accelarating = false;
                    shoot = true;


                }
            }
            if (playerAnswer < answer)
            {
                theChopper.flySpeed = viH - 1.5f;
                //theQuestion.SetModalTitle("Stunt failed");
                //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " flies the helicopter too slow. The correct answer is </color>" + answer.ToString("F2") + "m/s.");

                if (timer >= time)
                {
                    theChopper.flySpeed = 0;
                    theChopper.accelaration = 0;
                    theChopper.accelarating = false;
                    shoot = true;
                }
            }

        }

        if (shoot)
        {
            projectileLine.SetActive(false);
            AccHardSimulation.simulate = false;
            target.SetActive(false);
            timertxt.text = time.ToString("F2") + ("s");

            if (shootReady)
            {
                theShoot.Shoot();
                shootReady = false;

            }
            if (theSimulate.posCheck)
            {
                if (playerAnswer == answer)
                {
                    theTruck.accelerating = false;
                    theTruck.deaccelerating = true;

                }
                if (playerAnswer != answer)
                {
                    theQuestion.answerIsCorrect = false;
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
                StartCoroutine(StuntResult());
                shoot = false;
                theMulticab.moveSpeed = 0;
                startTime = false;
                timertxtTruck.text = (time + bulletTime).ToString("F2") + ("s");

            }

        }


    }
    public void generateProblem()
    {
        truckRunning.Stop();
        chopperEngine.Play();
        truckIdle.Play();
        StartCoroutine(theHeart.startBGgone());
         play.interactable = true;
        theQuestion.isSimulating = false;
        startTime = false;
        theTruck.accelerating = false;
        theTruck.moveSpeed = 0;
        chopperInitials.SetActive(true);
        projectileLine.SetActive(true);
        theChopper.transform.position = new Vector2(cam.transform.position.x + 5, theChopper.transform.position.y);
        //directorPlatform.transform.localScale = new Vector2(-directorPlatform.transform.localScale.x, directorPlatform.transform.localScale.y);
        toRetry = true;
        target.SetActive(true);
        dimensions.SetActive(true);
        shootReady = true;
        dX = Random.Range(15, 18);
        //generateAH = Random.Range(2, 3);
        //aH = (float)System.Math.Round(generateAH, 2);
        generateTime = Random.Range(2f, 2.5f);
        time = (float)System.Math.Round(generateTime, 2);
        generateAngleB = Random.Range(35f, 40f);
        angleB = (float)System.Math.Round(generateAngleB, 2);
        generateViT = Random.Range(3f, 5f);
        viT = (float)System.Math.Round(generateViT, 2);
        generateAT = Random.Range(3f, 5f);
        aT = (float)System.Math.Round(generateAT, 2);
        generateVB = Random.Range(20f, 30f);
        vB = (float)System.Math.Round(generateVB, 2);
        gun.transform.rotation = Quaternion.Euler(gun.transform.rotation.x, gun.transform.rotation.y, -(180 - angleB));
        ChopperY = theChopper.transform.position.y - gunBarrel.transform.position.y;
        chopperX = gunBarrel.transform.position.x - theChopper.transform.position.x;
        //dX = targetWheel.transform.position.x - gunBarrel.transform.position.x;
        dY = gunBarrel.transform.position.y - targetWheel.transform.position.y;
        //theChopper.transform.position = new Vector2(targetWheel.transform.position.x + dX - chopperX, targetWheel.transform.position.y + dY + ChopperY);
        horizontal.transform.position = new Vector2(gunBarrel.transform.position.x, gunBarrel.transform.position.y);
        verticalOne.transform.position = new Vector2(gunBarrel.transform.position.x, verticalOne.transform.position.y);

        theCurve._origin = new Vector2(gunBarrel.transform.position.x, gunBarrel.transform.position.y);
        theCurve._degrees = angleB;
        theCurve._horizRadius = -2.5f;
        theCurve.textOffset = new Vector2(-5, -.5f);
        angleA = 90 - angleB;
        timer = 0;
        bulletPos.SetActive(false);
        bulletHere.SetActive(false);
        wheelPos.SetActive(false);
        targetHere.SetActive(false);
        theSimulate.posCheck = false;
        viTtxt.text = ("vi = ") + viT.ToString("F2") + ("m/s");
        aTtxt.text = ("a = ") + aT.ToString("F2") + ("m/s??");
        timertxtTruck.text = timer.ToString("F2") + ("s");
        timertxtTruck.color = new Color32(87, 0, 255, 255);
        theMulticab.transform.position = new Vector2(theChopper.transform.position.x - 30, theMulticab.transform.position.y);
        theTruck.transform.position = new Vector2(gunBarrel.transform.position.x - (dX - 6), theTruck.transform.position.y);
        theMeter[0].positionX = targetWheel.transform.position.x;
        theMeter[1].distance = dY;
        theMeter[1].positionX = targetWheel.transform.position.x;
        theMeter[1].positionY = targetWheel.transform.position.y;
        theMeter[0].distance = dX;
        theMeter[0].positionY = dY - 5;
        target.transform.position = targetWheel.transform.position;
        camPos = cam.transform.position.x - theChopper.transform.position.x;
        sideB = dY / (Mathf.Tan(angleB * Mathf.Deg2Rad));
        sideC = Mathf.Sqrt((dY * dY) + (sideB * sideB));
        bulletTime = sideC / vB;
        truckTimeToTravel = time + bulletTime;
        viHtxt.text = ("v = ?");
        theQuestion.SetQuestion((("<b>") + PlayerPrefs.GetString("Name") + ("</b> is now instructed to shoot the hub or the center of the moving truck's 2nd wheel from a moving helicopter. If at time = ??, the hub is <b>") + dX.ToString("F2") + ("</b> meters horizontally behind and <b>") + dY.ToString("F2") + ("</b> meters vertically below the tip of the gun barrel that <b>") + PlayerPrefs.GetString("Name") + ("</b> holding, if <b>") + PlayerPrefs.GetString("Name") + ("</b> shoots exactly after <b>") + time.ToString("F2") + ("</b> seconds, if the truck has an initial velocity of <b>") + viT.ToString("F2") + ("</b> m/s and accelerating at <b>") + aT.ToString("F2") + ("</b> m/s??, what should be the velocity of helicopter, while the gun is aimed <b>") + angleB.ToString("F2") + ("</b> degrees below the horizon and its bullet travels at a constant velocity of <b>") + vB.ToString("F2") + ("</b> m/s?")));
    }

    IEnumerator positioning()
    {
        Time.timeScale = 1;
        projectileLine.SetActive(false);
        theChopper.moving = true;
        theTruck.moveSpeed = 2;
        theChopper.flySpeed = 20;
        yield return new WaitForSeconds(.5f);
        theMulticab.moveSpeed = 20;
        yield return new WaitForSeconds(1.5f);
        //StartCoroutine(theHeart.endBGgone());
        yield return new WaitForSeconds(1f);
        camFollow = false;
        theTruck.moveSpeed = 0;
        yield return new WaitForSeconds(.8f);
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
        //theQuestion.ToggleModal();
        theTruck.deaccelerating = false;
        if (playerAnswer == answer)
        {
            targetWheel.SetActive(false);
            targetSignAge.SetActive(false);
            theQuestion.ActivateResult(PlayerPrefs.GetString("Name") + " successfully hit the 2nd target!",true, false);
            //Time.timeScale = 0;
        }
       if (playerAnswer != answer)
        {
            theHeart.ReduceLife();
            theQuestion.ActivateResult(PlayerPrefs.GetString("Name") + " unable to performed the stunt and missed the target",false, false);
        }
        /*if (playerAnswer < answer)
        {
            theQuestion.ActivateResult(PlayerPrefs.GetString("Name") + " flies the helicopter too slow. The correct answer is </color>" + answer.ToString("F2") + "m/s.",false, false);
        }*/
    }
    public IEnumerator positioningTwo()
    {
        theChopper.flySpeed = 5;
        theMulticab.moveSpeed = 4;
        truckInitials.SetActive(false);
        theSimulate.takeNumber += 1;
        timer = 0;
        target.SetActive(false);
        bulletPos.SetActive(false);
        bulletHere.SetActive(false);
        wheelPos.SetActive(false);
        targetHere.SetActive(false);
        theTruck.transform.position = new Vector2(theTruck.transform.position.x - 1, theTruck.transform.position.y);
        theTruck.moveSpeed = -10;
        yield return new WaitForSeconds(3);
        //StartCoroutine(theHeart.endBGgone());
        yield return new WaitForSeconds(1f);
        theChopper.flySpeed = 0;
        theMulticab.moveSpeed = 0;
        yield return new WaitForSeconds(1.8f);
        theTruck.moveSpeed = 0;
        generateProblem();
        theSimulate.playButton.interactable = true;
    }
}
