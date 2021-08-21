using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectileHardTwo : MonoBehaviour
{
    public playerProjectile thePlayer;
    public ProjSimulationManager theSimulate;
    public QuestionControllerB theQuestion;
    public golem theGolem;
    public HeartManager theHeart;
    public CircularAnnotation[] theCircular;
    public Arrow[] theArrow;
    public DistanceMeter[] theMeter;
    public GameObject Mgear, stone, target, puller, arrow, projectArrow, projectArrowTrail, blastPrefab, deflector, trail, lineRenderer, lineDistance, boulder, angleArrow;
    public GameObject lineVertical, lineHorizontal, dimension, cam, golemInitial, playerInitial;
    public float Vo, generateVG, vG, generateVP, vP;
    public float angle, HRange, timer, generateTime, time, projectileTime,playerProjectileTime, golemTravelTime;
    public float stoneDY, correctAnswer, stoneDyR, generateAnswer;
    public float generateDistance, finalDistance, golemDistanceToTravel, playerDistanceToTravel, camDistance, playerSpeed;
    public bool timeStart, answerIsCorrect, shootReady, showProjectile, running, camFollow;
    string pronoun, pronoun2, gender;
     public TMP_Text golemVelo, golemAcc, VoTxt, playerVelo;
    void Start()
    {
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
        golemInitial.transform.position = theGolem.transform.position;
        playerInitial.transform.position = thePlayer.transform.position;
        if (running)
        {
            puller.GetComponent<Rigidbody2D>().velocity = new Vector2(playerSpeed, puller.GetComponent<Rigidbody2D>().velocity.y);
        }
        if (camFollow)
        {
            cam.transform.position = new Vector3(thePlayer.transform.position.x - camDistance, cam.transform.position.y, cam.transform.position.z);
        }
        if (!timeStart)
        {
            golemVelo.text = ("v = ") + vG.ToString("F2") + (" m/s");
            playerVelo.text = "v = " + vP.ToString("F2") + "m/s";
            golemAcc.text = ("a = none");
            VoTxt.text = "vi = ?";
            VoTxt.gameObject.transform.position = angleArrow.transform.position;
            trail.transform.position = this.transform.position;
            generateDistance = (float)System.Math.Round((this.transform.position.x - target.transform.position.x), 2);
            golemTravelTime = time + projectileTime;
            golemDistanceToTravel = vG * golemTravelTime;
            playerDistanceToTravel = vP * time;
            finalDistance = (generateDistance + playerDistanceToTravel) - golemDistanceToTravel;
            stoneDY = target.transform.position.y - this.transform.position.y;
            stoneDyR = (float)System.Math.Round(stoneDY, 2);
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, (180 - angle));
            angle = Mathf.Atan(((stoneDyR + ((9.81f / 2) * (projectileTime * projectileTime))) / finalDistance)) * Mathf.Rad2Deg;
            Vo = finalDistance / (Mathf.Cos((angle * Mathf.Deg2Rad)) * projectileTime);
            correctAnswer = (float)System.Math.Round(Vo, 2);
            playerProjectileTime = finalDistance / (Mathf.Cos((angle * Mathf.Deg2Rad)) * ProjSimulationManager.playerAnswer);
            angleArrow.transform.rotation = this.transform.rotation;
            angleArrow.transform.position = this.transform.position;
            theMeter[0].positionX = target.transform.position.x;
            theMeter[0].distance = generateDistance;
            theMeter[1].positionX = this.transform.position.x - .7f;
            theMeter[1].distance = this.transform.position.y + 0.25f;
            theMeter[2].positionX = target.transform.position.x + 1;
            theMeter[2].distance = target.transform.position.y + 0.25f;
        }
        if (ProjSimulationManager.simulate == true)
        {
            timeStart = true;
            running = true;
            ProjSimulationManager.simulate = false;
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
                    StartCoroutine(ropePull());
                    shootReady = false;
                   
                }

            }
            if (timer >= golemTravelTime)
            {
                theGolem.moveSpeed = 0;
            }
        }
    }
    public void generateProblem()
    {
        dimension.SetActive(true);
        puller.transform.position = new Vector2(45, thePlayer.transform.position.y);
        theGolem.transform.position = new Vector2(5, theGolem.transform.position.y);
        generateVG = Random.Range(2f, 2.5f);
        generateTime = Random.Range(3.5f, 4f);
        time = (float)System.Math.Round(generateTime, 2);
        projectileTime = (float)System.Math.Round(Random.Range(2.5f, 3f), 2);
        vG = (float)System.Math.Round(generateVG, 2);
        generateVP = Random.Range(1f, 1.7f);
        vP = (float)System.Math.Round(generateVP, 2);

    }
    IEnumerator ropePull()
    {
        yield return new WaitForSeconds(projectileTime);
        running = false;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 160);
        //theGolem.throwing = true;
        theGolem.moveSpeed = 0;
        yield return new WaitForSeconds(.55f);
        boulder.SetActive(false);
        //theGolem.throwing = false;
        //theBoulder.boulderThrow();
        puller.GetComponent<Rigidbody2D>().velocity = transform.right * 30;
        thePlayer.airdive = true;
        thePlayer.aim = false;
    }
    public void ShootArrow()
    {
        arrow.SetActive(true);
        arrow.transform.position = this.transform.position;
        theArrow[0].rb.bodyType = RigidbodyType2D.Dynamic;
        lineRenderer.GetComponent<LineRenderer>().enabled = true;
        theArrow[0].generateLine = true;
        trail.GetComponent<TrailRenderer>().time = 3000;
        if (ProjSimulationManager.playerAnswer < correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (ProjSimulationManager.playerAnswer-.5f);
            StartCoroutine(StuntResult());
        }
        if (ProjSimulationManager.playerAnswer > correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (ProjSimulationManager.playerAnswer + .5f);
            StartCoroutine(StuntResult());
        }
        if (ProjSimulationManager.playerAnswer == correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (ProjSimulationManager.playerAnswer+ .08f);
            StartCoroutine(StuntResult());
        }
        GameObject explosion = Instantiate(blastPrefab);
        explosion.transform.position = transform.position;

    }
    public IEnumerator positioning()
    {
        thePlayer.aim = false;
        thePlayer.slash = false;
        thePlayer.airdive = false;
        camFollow = true;
        running = true;
        thePlayer.running = true;
        yield return new WaitForSeconds(2);
        theGolem.standUp = true;
        StartCoroutine(theHeart.endBGgone());
        yield return new WaitForSeconds(2);
        generateProblem();
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

    }
    public void reShoot()
    {
        puller.GetComponent<Rigidbody2D>().velocity = transform.right * 10;
    }
     IEnumerator StuntResult()
    {
        trail.GetComponent<TrailRenderer>().time = 10;
        yield return new WaitForSeconds(projectileTime);
        StartCoroutine(theSimulate.DirectorsCall());
        theQuestion.ToggleModal();
        //theArrow[0].generateLine = false;
        //trail.SetActive(false);
        theArrow[0].getAngle = false;
        //theArrow[0].gameObject.SetActive(false);
        //thePlayer.sword.SetActive(false);
    }


}
