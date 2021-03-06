using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AccHardOne : MonoBehaviour
{
    // Stunt Guide
    public GameObject[] stuntGuideObjectPrefabs;
    public Image stuntGuideImage;
    public Sprite stuntGuideImageSprite;
    // End of Stunt Guide
    public float dX, dY, viT, aT, timer, vB, angleB, angleA, sideB, sideC, totalDistance, correctAnswer, answer, playerAnswer;
    //public Quaternion angleB;
    public GameObject gunBarrel, gun, target, targetWheel, projectileLine, dimensions, truckInitials;
    public GameObject verticalOne, horizontal;
    public GameObject bulletPos, wheelPos, bulletHere, targetHere, cam, targetSignAge;
    public TruckManager theTruck;
    public Hellicopter theChopper;
    public ShootManager theShoot;
    public DistanceMeter[] theMeter;
    public CircularAnnotation theCurve;
    public QuestionControllerC theQuestion;
    private BulletManager theBullet;
    public MulticabManager theMulticab;
    public HeartManager theHeart;

    public AccHardSimulation theSimulate;
    float generateAngleB, generateViT, generateAT, generateVB, generateDX, generateDY;
    float ChopperY, chopperX, truckTime, bulletTime, truckCurrentPos, camPos;
    bool shoot, shootReady, gas, startTime;
    public TMP_Text timertxt, timertxtTruck, actiontxt, viTtxt, aTtxt;
    int tries, stopTruckPos, attemp;
    public Button play;

    public TMP_Text debugAnswer;
    public AudioSource chopperEngine, truckIdle,truckRunning,GunShot;
    // Start is called before the first frame update
    void Start()
    {
        tries = 1;
        attemp = 1;
        stopTruckPos = 50;
        camPos = cam.transform.position.x - theChopper.transform.position.x;
        generateProblem();
        PlayerPrefs.SetInt("Life", 3);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Stunt Guide        
        stuntGuideObjectPrefabs[0].SetActive(true);
        stuntGuideObjectPrefabs[1].SetActive(false);
        stuntGuideObjectPrefabs[2].SetActive(false);
        stuntGuideImage.sprite = stuntGuideImageSprite;

        targetSignAge.transform.position = new Vector3(targetWheel.transform.position.x-2, targetWheel.transform.position.y, -5);
        debugAnswer.SetText($"Answer: {System.Math.Round(correctAnswer, 2)}");
        truckInitials.transform.position = theTruck.transform.position;
        cam.transform.position = new Vector3(theChopper.transform.position.x + camPos, cam.transform.position.y, cam.transform.position.z);
        truckCurrentPos = theTruck.transform.position.x;
        theBullet = FindObjectOfType<BulletManager>();
        theShoot.speed = vB;

        sideB = (Mathf.Tan(angleA * Mathf.Deg2Rad)) * dY;
        sideC = Mathf.Sqrt((dY * dY) + (sideB * sideB));
        totalDistance = sideB + dX;
        truckTime = (-viT + Mathf.Sqrt((viT * viT) + (4 * ((aT / 2) * totalDistance)))) / aT;
        bulletTime = sideC / vB;
        correctAnswer = truckTime - bulletTime;
        answer = (float)System.Math.Round(correctAnswer, 2);
        playerAnswer = AccHardSimulation.playerAnswer;
        if (startTime)
        {
            timer += Time.fixedDeltaTime;
            timertxtTruck.text = timer.ToString("F2") + ("s");
        }


        if (AccHardSimulation.simulate == true)
        {
            play.interactable = false;
            startTime = true;
            target.transform.position = targetWheel.transform.position;
            dimensions.SetActive(false);
            viTtxt.text = ("v = ") + theTruck.moveSpeed.ToString("F2") + ("m/s");
            if (timer == 0)
            {
                theTruck.moveSpeed = viT;
                theTruck.accelaration = aT;
                theTruck.accelerating = true;
            }

            timertxt.text = timer.ToString("F2") + ("s");
            if (playerAnswer == answer)
            {
                theSimulate.answerIsCorrect = true;
                theQuestion.answerIsCorrect = true;
                actiontxt.text = ("next");
                //theQuestion.SetModalTitle("Stunt success");
                //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " successfully hit the target!");
                if (timer >= correctAnswer + .05f)
                {

                    shoot = true;
                    timertxtTruck.color = new Color32(13, 106, 0, 255);

                }
            }
            if (playerAnswer > answer)
            {
                //theQuestion.SetModalTitle("Stunt failed");
                //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " shot too late. The correct answer is </color>" + answer.ToString("F2") + "seconds.");
                if (timer >= playerAnswer + .1f)
                {
                    shoot = true;

                }
            }
            if (playerAnswer < answer)
            {
                //theQuestion.SetModalTitle("Stunt failed");
                //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " shot too soon. The correct answer is </color>" + answer.ToString("F2") + "seconds.");

                if (timer >= playerAnswer - .1f)
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
            timertxt.text = playerAnswer.ToString("F2")+ ("s");

            if (shootReady)
            {
                theShoot.Shoot();
                shootReady = false;

            }
            if (theSimulate.posCheck)
            {
                if (playerAnswer != answer)
                {
                    theHeart.ReduceLife();
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
                startTime = false;
                timertxtTruck.text = (playerAnswer + bulletTime).ToString("F2")+ ("s");
            }

        }
        if (truckCurrentPos >= 70)
        {
            theTruck.moveSpeed = 0;
        }
    }
    public void generateProblem()
    {
        truckRunning.Stop();
        chopperEngine.Play();
        truckIdle.Play();
        play.interactable = true;
        theQuestion.isSimulating = false;
        StartCoroutine(theHeart.startBGgone());
        truckInitials.SetActive(true);
        target.SetActive(true);
        startTime = false;
        dimensions.SetActive(true);
        projectileLine.SetActive(true);
        shootReady = true;
        shoot = false;
        dX = Random.Range(8, 10);
        dY = Random.Range(10, 12);
        generateAngleB = Random.Range(20f, 30f);
        angleB = (float)System.Math.Round(generateAngleB, 2);
        generateViT = Random.Range(2f, 3f);
        viT = (float)System.Math.Round(generateViT, 2);
        generateAT = Random.Range(2f, 4f);
        aT = (float)System.Math.Round(generateAT, 2);
        generateVB = Random.Range(30f, 40f);
        vB = (float)System.Math.Round(generateVB, 2);
        gun.transform.rotation = Quaternion.Euler(gun.transform.rotation.x, gun.transform.rotation.y, -angleB);
        ChopperY = theChopper.transform.position.y - gunBarrel.transform.position.y;
        chopperX = gunBarrel.transform.position.x - theChopper.transform.position.x;
        theChopper.transform.position = new Vector2(targetWheel.transform.position.x + dX - chopperX, targetWheel.transform.position.y + dY + ChopperY);
        horizontal.transform.position = gunBarrel.transform.position;
        verticalOne.transform.position = new Vector2(gunBarrel.transform.position.x, verticalOne.transform.position.y);
        theMeter[1].distance = dY;
        theMeter[1].positionX = targetWheel.transform.position.x;
        theMeter[1].positionY = targetWheel.transform.position.y;
        theMeter[0].distance = dX;
        theMeter[0].positionX = targetWheel.transform.position.x;
        theMeter[0].positionY = dY - 5;
        theCurve._origin = new Vector2(gunBarrel.transform.position.x, gunBarrel.transform.position.y);
        theCurve._degrees = angleB;
        angleA = 90 - angleB;
        theQuestion.SetQuestion((("<b>") + PlayerPrefs.GetString("Name") + ("</b> is now instructed to shoot the hub or the center of the moving truck's wheel from a non-moving helicopter. If at time = ??, the hub is <b>") + dX.ToString("F2") + ("</b> meters horizontally behind and <b>") + dY.ToString("F2") + ("</b> meters vertically below the tip of the gun barrel that <b>") + PlayerPrefs.GetString("Name") + ("</b> holding, at how many seconds after should  <b>") + PlayerPrefs.GetString("Name") + ("</b> shoots if the truck has an initial velocity of <b>") + viT.ToString("F2") + ("</b> m/s and accelerates at <b>") + aT.ToString("F2") + ("</b> m/s?? while the gun is aimed <b>") + angleB.ToString("F2") + ("</b> degrees below the horizon and its bullet travels at a constant velocity of <b>") + vB.ToString("F2") + ("</b> m/s?")));
        target.transform.position = targetWheel.transform.position;
        timer = 0;
        bulletPos.SetActive(false);
        bulletHere.SetActive(false);
        wheelPos.SetActive(false);
        targetHere.SetActive(false);
        theSimulate.posCheck = false;
        viTtxt.text = ("vi = ") + viT.ToString("F2") + ("m/s");
        aTtxt.text = ("a = ") + aT.ToString("F2") + ("m/s??");
        timertxtTruck.text = timer.ToString("F2") + ("s");
        theMulticab.gameObject.transform.position = new Vector2(theChopper.transform.position.x - 13, theMulticab.transform.position.y);

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
            theQuestion.ActivateResult(PlayerPrefs.GetString("Name") + " successfully hit the target!",true, false);
            //Time.timeScale = 0;
        }
        if (playerAnswer != answer)
        {
            theHeart.ReduceLife();
            theQuestion.ActivateResult(PlayerPrefs.GetString("Name") + " unable to performed the stunt and missed the target",false, false);
        }
        /*if (playerAnswer < answer)
        {
            theQuestion.ActivateResult(PlayerPrefs.GetString("Name") + " shot too soon. The correct answer is </color>" + answer.ToString("F2") + "seconds.",false, false);
        }*/


    }
    public IEnumerator positioning()
    {
        target.SetActive(false);
        tries += 1;
        attemp += 1;
        stopTruckPos += 90;
        bulletPos.SetActive(false);
        bulletHere.SetActive(false);
        wheelPos.SetActive(false);
        targetHere.SetActive(false);
        theTruck.moveSpeed = 3;
        theChopper.flySpeed = 16;
        yield return new WaitForSeconds(1);
        theMulticab.moveSpeed = 20;
        yield return new WaitForSeconds(4);
        //theHeart.startBGgone();
        theTruck.moveSpeed = 0;
        theChopper.flySpeed = 0;
        theMulticab.moveSpeed = 0;
        yield return new WaitForSeconds(0.1f);
        generateProblem();


    }
    public IEnumerator positioningTwo()
    {
        truckInitials.SetActive(false);
        theSimulate.takeNumber += 1;
        timer = 0;
        target.SetActive(false);
        tries += 1;
        attemp += 1;
        bulletPos.SetActive(false);
        bulletHere.SetActive(false);
        wheelPos.SetActive(false);
        targetHere.SetActive(false);
        theTruck.transform.position = new Vector2(theTruck.transform.position.x - 1, theTruck.transform.position.y);
        theTruck.moveSpeed = -13;
        yield return new WaitForSeconds(4);
        //theHeart.startbgentrance();
        yield return new WaitForSeconds(1f);
        theTruck.moveSpeed = 0;
        yield return new WaitForSeconds(1.8f);
        generateProblem();
        theSimulate.playButton.interactable = true;

    }
}
