using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectileHardThree : MonoBehaviour
{
    public playerProjectile thePlayer;
    public ProjSimulationManager theSimulate;
    public QuestionControllerB theQuestion;
    public golem theGolem;
    public HeartManager theHeart;
    public CircularAnnotation theCircular;
    public Arrow[] theArrow;
    public DistanceMeter[] theMeter;
    public GameObject Mgear, target, puller, arrow, projectArrow, projectArrowTrail, blastPrefab, deflector, trail, lineRenderer, boulder, angleArrow;
    public GameObject dimension, cam, golemInitial, playerInitial, angleDimension,angleLine;
    public float Vo, generateVG, vG, generateVP, vP, accG;
    public float angle, HRange, timer, generateTime, time, projectileTime, playerProjectileTime, golemTravelTime;
    public float stoneDY, correctAnswer, stoneDyR, generateAnswer;
    public float hypSide, oppSide, adjSide, oppSide2, angleGround, finalHeight;
    public float initialDistance, finalDistance, golemDistanceToTravel, playerDistanceToTravel, camDistance, playerSpeed;
    public bool timeStart, answerIsCorrect, shootReady, showProjectile, running, camFollow;
    string pronoun, pronoun2, gender;
    public TMP_Text golemVelo, golemAcc, VoTxt, playerVelo, actionTxt, angleTxt;
    // Start is called before the first frame update
    void Start()
    {
        theSimulate.stage = 3;
        camDistance = thePlayer.transform.position.x - cam.transform.position.x;
        golemAcc.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angleLine.transform.position = this.transform.position;
        theCircular._origin = new Vector2(this.transform.position.x + .5f, this.transform.position.y);
        angleArrow.transform.rotation = this.transform.rotation;
        angleArrow.transform.position = new Vector2(transform.position.x +.1f, transform.position.y);
        golemInitial.transform.position = theGolem.transform.position;
        playerInitial.transform.position = thePlayer.transform.position;
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
            golemVelo.text = ("v = ") + vG.ToString("F2") + (" m/s");
            playerVelo.text = "v = " + vP.ToString("F2") + "m/s";
            VoTxt.text = "vi = ?";
            golemAcc.text = "a = " + accG.ToString("F2") + "m/s²";

            VoTxt.gameObject.transform.position = angleArrow.transform.position;

            initialDistance = initialDistance = (float)System.Math.Round((target.transform.position.x - this.transform.position.x), 2);
            playerDistanceToTravel = vP * time;
            golemTravelTime = time + projectileTime;
            hypSide = playerDistanceToTravel;
            golemDistanceToTravel = (vG * golemTravelTime) + ((accG * (golemTravelTime * golemTravelTime)) / 2);
            stoneDyR = (float)System.Math.Round(stoneDY, 2);
            oppSide = Mathf.Sin(angleGround * Mathf.Deg2Rad) * hypSide;
            adjSide = Mathf.Cos(angleGround * Mathf.Deg2Rad) * hypSide;
            oppSide2 = Mathf.Sqrt((hypSide * hypSide) - (adjSide * adjSide));
            finalDistance = initialDistance + adjSide - golemDistanceToTravel;
            stoneDY = 1.62f + oppSide;
            finalHeight = 5.97f - stoneDY;
            angle = Mathf.Atan(((finalHeight + ((9.81f / 2) * (projectileTime * projectileTime))) / finalDistance)) * Mathf.Rad2Deg;
            Vo = finalDistance / (Mathf.Cos((angle * Mathf.Deg2Rad)) * projectileTime);
            correctAnswer = (float)System.Math.Round(angle, 2);
            angleArrow.transform.rotation = this.transform.rotation;
            angleArrow.transform.position = this.transform.position;
            theMeter[0].distance = target.transform.position.y + 0.25f;
            theMeter[1].positionX = this.transform.position.x + 1;
            theMeter[1].distance = this.transform.position.y + 0.25f;
            theMeter[2].positionX = this.transform.position.x;
            theMeter[2].positionY = this.transform.position.y - 2.5f;
            theMeter[2].distance = -this.transform.position.x + target.transform.position.x;
            theCircular._degrees = angle;
            theCircular.initialAngle = 85 - angle;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, (angle));
        }
        if (ProjSimulationManager.simulate == true)
        {
            theCircular._degrees =ProjSimulationManager.playerAnswer;
            theCircular.initialAngle = 85 - ProjSimulationManager.playerAnswer;
            angleTxt.text = "Θ = " + ProjSimulationManager.playerAnswer.ToString("F2") + "°";
            trail.SetActive(true);
            timeStart = true;
            running = true;
            if (ProjSimulationManager.playerAnswer > correctAnswer)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, (ProjSimulationManager.playerAnswer));
                actionTxt.text = "retry";
                Vo -= .2f;

            }
            if (ProjSimulationManager.playerAnswer < correctAnswer)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, (ProjSimulationManager.playerAnswer));
                actionTxt.text = "retry";
                Vo += .2f;
            }
            if (ProjSimulationManager.playerAnswer == correctAnswer)
            {
                deflector.GetComponent<Collider2D>().isTrigger = true;
                theQuestion.answerIsCorrect = true;
                playerProjectileTime += 4;
                actionTxt.text = "done";

            }
            ProjSimulationManager.simulate = false;

        }
        if (timeStart)
        {
            playerProjectileTime = finalDistance / (Mathf.Cos((ProjSimulationManager.playerAnswer * Mathf.Deg2Rad)) * Vo);
            if (timer == 0)
            {
                dimension.SetActive(false);
                theGolem.accelarating = true;
                theGolem.accelaration = accG;
                theGolem.moveSpeed = vG;
                playerSpeed = -vP;

            }
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
                theGolem.accelarating = false;
                theGolem.moveSpeed = 0;
            }

        }


    }
    public void generateProblem()
    {
        angleTxt.text = "Θ = ?";
        angleDimension.SetActive(true);
        dimension.SetActive(true);
        timer = 0;
        arrow.SetActive(false);
        timeStart = false;
        shootReady = true;
        running = false;
        theArrow[0].getAngle = false;
        //dimension.SetActive(true);
        puller.transform.position = new Vector2(-15.23f, 1.5f);
        theGolem.transform.position = new Vector2(30, theGolem.transform.position.y);
        time = (float)System.Math.Round(Random.Range(2f, 2.5f), 2);
        projectileTime = (float)System.Math.Round(Random.Range(2.9f, 3.3f), 2);
        accG = (float)System.Math.Round(Random.Range(.3f, 0.6f), 2);
        vG = (float)System.Math.Round(Random.Range(2f, 2.5f), 2);
        vP = (float)System.Math.Round(Random.Range(1f, 1.7f), 2);
        angle = (float)System.Math.Round(Random.Range(50f, 58f), 2);
        trail.SetActive(false);
        theQuestion.SetQuestion("Simulation Test"); 
        //Vo = (float)System.Math.Round((Random.Range(50f, 57f)), 2);
    }
    public void ShootArrow()
    {
        angleDimension.SetActive(false);
        arrow.SetActive(true);
        arrow.transform.position = this.transform.position;
        theArrow[0].rb.bodyType = RigidbodyType2D.Dynamic;
        lineRenderer.GetComponent<LineRenderer>().enabled = true;
        theArrow[0].generateLine = true;
        trail.GetComponent<TrailRenderer>().time = 3000;
        arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (Vo + .1f);
        if (ProjSimulationManager.playerAnswer == correctAnswer)
        {
            StartCoroutine(ropePull());
        }
        StartCoroutine(StuntResult());
        GameObject explosion = Instantiate(blastPrefab);
        explosion.transform.position = transform.position;
    }
    IEnumerator ropePull()
    {
        yield return new WaitForSeconds(projectileTime);
        playerInitial.SetActive(false);
        running = false;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 17);
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
    public void reShoot()
    {
        puller.GetComponent<Rigidbody2D>().velocity = transform.right * 10;
    }
    public IEnumerator positioning()
    {
        arrow.SetActive(false);
        theQuestion.answerIsCorrect = false;
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
        arrow.GetComponent<Collider2D>().isTrigger = false;
        theArrow[0].rb.bodyType = RigidbodyType2D.Dynamic;
        deflector.GetComponent<Collider2D>().isTrigger = false;
        generateProblem();
        playerInitial.SetActive(true);
        playerInitial.SetActive(true);
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
    IEnumerator StuntResult()
    {
        running = false;
        yield return new WaitForSeconds(playerProjectileTime + 4);
        StartCoroutine(theSimulate.DirectorsCall());
        theQuestion.ToggleModal();
        theArrow[0].generateLine = false;
        thePlayer.sword.SetActive(false);
    }
}
