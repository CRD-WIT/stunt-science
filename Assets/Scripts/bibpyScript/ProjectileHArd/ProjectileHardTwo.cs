using System.Collections;
using UnityEngine;
using TMPro;

public class ProjectileHardTwo : MonoBehaviour
{
    public TMP_Text debugAnswer;
    public playerProjectile thePlayer;
    public ProjHardSimulation theSimulate;
    public QuestionContProJHard  theQuestion;
    public golem theGolem;
    public HeartManager theHeart;
    public CircularAnnotation[] theCircular;
    public IndicatorManager theIndicator;
    public Arrow2[] theArrow;
    public DistanceMeter[] theMeter;
    public GameObject Mgear, stone, target, puller, arrow, projectArrow, projectArrowTrail, blastPrefab, deflector, trail, lineRenderer, boulder, angleArrow;
    public GameObject dimension, cam, golemInitial, playerInitials,hit,miss,indicator;
    public float Vo, generateVG, vG, generateVP, vP;
    public float angle, HRange, timer, generateTime, time, projectileTime, playerProjectileTime, golemTravelTime;
    public float stoneDY, correctAnswer, stoneDyR, generateAnswer,projectileDiff;
    public float initialDistance, finalDistance, golemDistanceToTravel, playerDistanceToTravel, camDistance, playerSpeed;
    public bool timeStart, answerIsCorrect, shootReady, showProjectile, running, camFollow,indicatorReady;
    string pronoun, pronoun2, gender;
    public TMP_Text golemVelo, VoTxt, playerVelo, actionTxt, vPtxt;
    public AudioSource gunShot, maneuverGear, oxygenSfx;
    void Start()
    {
        theQuestion.stage = 2;
        theSimulate.stage = 2;
        gender = PlayerPrefs.GetString("Gender");
        camDistance = thePlayer.transform.position.x - cam.transform.position.x;
        //generateProblem();
        if (gender == "Male")
        {
            pronoun = ("he");
            pronoun2 = ("his");
        }
        if (gender == "Female")
        {
            pronoun = ("she");
            pronoun2 = ("her");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         debugAnswer.SetText($"Answer: {correctAnswer}");
        golemInitial.transform.position = theGolem.transform.position;
        playerInitials.transform.position = thePlayer.transform.position;
         if(theArrow[0].showIndicator & indicatorReady)
        {
            StartCoroutine(showHitMiss());
        }
        if (running)
        {
            trail.transform.position = this.transform.position;
            puller.GetComponent<Rigidbody2D>().velocity = new Vector2(playerSpeed, puller.GetComponent<Rigidbody2D>().velocity.y);
        }
        if (camFollow)
        {
            cam.transform.position = new Vector3(thePlayer.transform.position.x - camDistance, cam.transform.position.y, cam.transform.position.z);
        }
        if (!timeStart)
        {
            playerInitials.SetActive(true);
            playerInitials.transform.position = thePlayer.transform.position;
            vPtxt.text = "v = " + vP.ToString("F2") + "m/s";
            golemVelo.text = ("v = ") + vG.ToString("F2") + (" m/s");
            playerVelo.text = "v = " + vP.ToString("F2") + "m/s";
            VoTxt.text = "vi = ?";
            VoTxt.gameObject.transform.position = angleArrow.transform.position;

            initialDistance = (float)System.Math.Round((this.transform.position.x - target.transform.position.x), 2);
            golemTravelTime = time + projectileTime;
            golemDistanceToTravel = vG * golemTravelTime;
            playerDistanceToTravel = vP * time;
            finalDistance = (initialDistance + playerDistanceToTravel) - golemDistanceToTravel;
            stoneDY = target.transform.position.y - this.transform.position.y;
            stoneDyR = (float)System.Math.Round(stoneDY, 2);
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, (180 - angle));
            angle = Mathf.Atan(((stoneDyR + ((9.81f / 2) * (projectileTime * projectileTime))) / finalDistance)) * Mathf.Rad2Deg;
            Vo = finalDistance / (Mathf.Cos((angle * Mathf.Deg2Rad)) * projectileTime);
            correctAnswer = (float)System.Math.Round(Vo, 2);
            angleArrow.transform.rotation = this.transform.rotation;
            angleArrow.transform.position = this.transform.position;
            theMeter[0].positionX = target.transform.position.x;
            theMeter[0].distance = initialDistance;
            theMeter[1].positionX = this.transform.position.x - .7f;
            theMeter[1].distance = this.transform.position.y + 0.25f;
            theMeter[2].positionX = target.transform.position.x + 1;
            theMeter[2].distance = target.transform.position.y + 0.25f;
            
        }
        if (theQuestion.isSimulating == true)
        {
            playerProjectileTime = finalDistance / (Mathf.Cos((angle * Mathf.Deg2Rad)) * ((Vo - ProjSimulationManager.playerAnswer)+ Vo));
            trail.SetActive(true);
            timeStart = true;
            running = true;
            theQuestion.isSimulating = false;
        }
        if (timeStart)
        {

            dimension.SetActive(false);
            theGolem.moveSpeed = vG;
            playerSpeed = vP;

            timer += Time.fixedDeltaTime;
            if (timer < time)
            {
                thePlayer.backward = true;
            }
            if (timer >= time)
            {
                if (shootReady)
                {

                    ShootArrow();
                    thePlayer.backward = false;
                    vP = 0;
                    shootReady = false;

                }

            }
            if (timer >= golemTravelTime)
            {
                theGolem.moveSpeed = 0;
            }
             if (timer >= playerProjectileTime + time)
            {
               //StartCoroutine(theIndicator.showIndicator());
            }
        }

    }
    public void generateProblem()
    {
        theArrow[0].showIndicatorReady = true;
        theHeart.losslife = false;
        indicatorReady = true;
        timer = 0;
        arrow.SetActive(false);
        timeStart = false;
        shootReady = true;
        running = false;
        theArrow[0].getAngle = false;
        dimension.SetActive(true);
        puller.transform.position = new Vector2(45, thePlayer.transform.position.y);
        theGolem.transform.position = new Vector2(5, theGolem.transform.position.y);
        generateVG = Random.Range(2f, 2.5f);
        generateTime = Random.Range(3.5f, 4f);
        time = (float)System.Math.Round(generateTime, 2);
        projectileTime = (float)System.Math.Round(Random.Range(3f, 3.5f), 2);
        vG = (float)System.Math.Round(generateVG, 2);
        generateVP = Random.Range(1f, 1.7f);
        vP = (float)System.Math.Round(generateVP, 2);
        trail.SetActive(false);
        theQuestion.SetQuestion((("<b>") + PlayerPrefs.GetString("Name") + ("</b> is now instructed to fire a gun hitting a Golem in its weakest spot. ") + PlayerPrefs.GetString("Name") + (" and the Golem is <b>") + initialDistance.ToString("F2") + ("</b> meters apart when the Golem starts to chase ") + PlayerPrefs.GetString("Name") + (" at the velocity of <b>") + vG.ToString("F2") + ("</b> m/s while ") + PlayerPrefs.GetString("Name") + (" is moving away at the velocity of <b>") + vP.ToString("F2") + ("</b> m/s. After <b>") + time.ToString("F2") + ("</b> seconds, ") + PlayerPrefs.GetString("Name") + (" stops running and fire its gun at a certain angle. What should be the initial velocity of the bullet if its hits the weakspot at exactly <b>") + projectileTime.ToString("F2") + ("</b> seconds projectile time?")));

    }
    IEnumerator showHitMiss()
    {
        theArrow[0].showIndicator = false;
        indicatorReady = false;
        if (ProjHardSimulation.playerAnswer == correctAnswer)
        {
            hit.SetActive(true);
            indicator.transform.position = arrow.transform.position;
             yield return new WaitForSeconds(1.5f);
             hit.SetActive(false);

        }
        if (ProjHardSimulation.playerAnswer != correctAnswer)
        {
            miss.SetActive(true);
            indicator.transform.position = arrow.transform.position;
             yield return new WaitForSeconds(1.5f);
             miss.SetActive(false);
        }
       

    }
    IEnumerator ropePull()
    {
        yield return new WaitForSeconds(projectileTime);
        running = false;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 161);
        //theGolem.throwing = true;
        theGolem.moveSpeed = 0;
        yield return new WaitForSeconds(.55f);
        playerInitials.SetActive(false);
        //theGolem.throwing = false;
        //theBoulder.boulderThrow();
        oxygenSfx.Play();
        maneuverGear.Play();
        puller.GetComponent<Rigidbody2D>().velocity = transform.right * 31;
        thePlayer.airdive = true;
        thePlayer.aim = false;
        StartCoroutine(StuntResult());
    }
    public void ShootArrow()
    {
        arrow.SetActive(true);
        arrow.transform.position = this.transform.position;
        theArrow[0].rb.bodyType = RigidbodyType2D.Dynamic;
        lineRenderer.GetComponent<LineRenderer>().enabled = true;
        theArrow[0].generateLine = true;
        trail.GetComponent<TrailRenderer>().time = 3000;
        gunShot.Play();
        maneuverGear.Play();
        if (ProjHardSimulation.playerAnswer < correctAnswer)
        {
            actionTxt.text = "retry";
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (ProjHardSimulation.playerAnswer - .2f);
            //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " fired the gun but the initial velocity of <b>" + ProjHardSimulation.playerAnswer.ToString("F2")+ "</b> m/s is too slow. The correct answer is  <b>" + correctAnswer.ToString("F2") + "</b> seconds.");
            StartCoroutine(StuntResult());
        }
        if (ProjHardSimulation.playerAnswer > correctAnswer)
        {
            actionTxt.text = "retry";
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (ProjHardSimulation.playerAnswer + .2f);
            StartCoroutine(StuntResult());
            //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " fired the gun but the initial velocity of <b>" + ProjHardSimulation.playerAnswer.ToString("F2")+ "</b> m/s is too fast. The correct answer is  <b>" + correctAnswer.ToString("F2") + "</b> seconds.");
        }
        if (ProjHardSimulation.playerAnswer == correctAnswer)
        {
            deflector.GetComponent<Collider2D>().isTrigger = true;
            theQuestion.answerIsCorrect = true;
            actionTxt.text = "next";
            StartCoroutine(ropePull());
            theQuestion.answerIsCorrect = true;
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (ProjHardSimulation.playerAnswer + .08f);
            //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " fired the gun with the exact initial velocity. The correct answer is  <b>" + correctAnswer.ToString("F2") + "</b> seconds.");

        }
        GameObject explosion = Instantiate(blastPrefab);
        explosion.transform.position = transform.position;

    }
    public IEnumerator positioning()
    {
        theQuestion.answerIsCorrect = false;
        thePlayer.aim = false;
        thePlayer.slash = false;
        thePlayer.airdive = false;
        camFollow = true;
        running = true;
        thePlayer.running = true;
        yield return new WaitForSeconds(2);
        theGolem.standUp = true;
        StartCoroutine(theSimulate.ExitTrans());
        yield return new WaitForSeconds(2);
        arrow.GetComponent<Collider2D>().isTrigger = false;
        theArrow[0].rb.bodyType = RigidbodyType2D.Dynamic;
        deflector.GetComponent<Collider2D>().isTrigger = false;
        generateProblem();
        playerInitials.SetActive(true);
        running = false;
        camFollow = false;
        thePlayer.running = false;
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        puller.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        theGolem.transform.localScale = new Vector2(-theGolem.transform.localScale.x, theGolem.transform.localScale.y);
        yield return new WaitForSeconds(1.2f);
        theGolem.standUp = false;
        theGolem.damage = false;
        thePlayer.aim = true;
        camFollow = false;
        theQuestion.answerIsCorrect = false;

    }
    public void reShoot()
    {
        puller.GetComponent<Rigidbody2D>().velocity = transform.right * 10;
    }
    IEnumerator StuntResult()
    {
          yield return new WaitForSeconds(projectileTime + 2);
        if ( ProjHardSimulation.playerAnswer == correctAnswer)
        {
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and hit the target"),true, false);
        }
         if ( ProjHardSimulation.playerAnswer != correctAnswer)
        {
           
            theHeart.ReduceLife();
            yield return new WaitForSeconds(2);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has unable to hit the target"),false, false);
        }
        StartCoroutine(theSimulate.DirectorsCall());
        //theQuestion.ToggleModal();
        theArrow[0].generateLine = false;
        //trail.SetActive(false);
        //theArrow[0].rb.bodyType = RigidbodyType2D.Dynamic;
        theArrow[0].getAngle = true;
        //theArrow[0].gameObject.SetActive(false);
        thePlayer.sword.SetActive(false);
        running = false;
    }


}
