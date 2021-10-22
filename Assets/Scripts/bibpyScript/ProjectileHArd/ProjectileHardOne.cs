using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectileHardOne : MonoBehaviour
{
    public playerProjectile thePlayer;
    public ProjHardSimulation theSimulate;
    public QuestionContProJHard theQuestion;
    //public IndicatorManager theIndicator;
    public golem theGolem;
    public CircularAnnotation[] theCircular;
    public BoulderProjectile theBoulder;
    public DistanceMeter[] theMeter;
    public HeartManager theHeart;
    public Arrow2[] theArrow;
    public GameObject Mgear, stone, target, puller, arrow, projectArrow, projectArrowTrail, blastPrefab, deflector, trail, lineAngle, lineDistance, boulder, angleArrow;
    public GameObject lineVertical, lineHorizontal, dimension, golemInitial;
    public float vi, generateVG, vG;
    public float generateAngle, generateStoneAngle, stoneAngle, stoneOpp, HRange, timer, projectileTime, golemTravelTime;
    public float stoneDY, correctAnswer, stoneDyR, generateAnswer;
    public float generateDistance, totalDistance, golemDistanceToTravel;
    public bool timeStart, answerIsCorrect, shootReady, showProjectile, indicatorReady;
    public TMP_Text golemVelo, VoTxt, timerTxt, actiontxt;
    string pronoun, pronoun2, gender;

    // Start is called before the first frame update
    void Start()
    {
        gender = PlayerPrefs.GetString("Gender");
        thePlayer.aim = true;
        generateProblem();
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
        PlayerPrefs.SetInt("Life", 3);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        golemInitial.transform.position = theGolem.transform.position;
        lineAngle.transform.position = stone.transform.position;
        lineDistance.transform.position = new Vector2(stone.transform.position.x, this.transform.position.y);
        if (!timeStart)
        {
            golemVelo.text = ("v = ") + vG.ToString("F2") + (" m/s");
            stone.transform.position = new Vector2(this.transform.position.x + generateDistance, target.transform.position.y);
            angleArrow.transform.position = this.transform.position;
            angleArrow.transform.rotation = this.transform.rotation;
            golemTravelTime = golemDistanceToTravel / vG;

            golemDistanceToTravel = totalDistance - stoneOpp;
            //generateDistance = stone.transform.position.x - this.transform.position.x;
            //distance = (float)System.Math.Round(generateDistance, 2);
            stoneDY = stone.transform.position.y - this.transform.position.y;
            stoneDyR = (float)System.Math.Round(stoneDY, 2);
            generateStoneAngle = Mathf.Atan((generateDistance / stoneDyR)) * Mathf.Rad2Deg;
            stoneAngle = (float)System.Math.Round(generateStoneAngle, 2);
            stoneOpp = (Mathf.Tan(stoneAngle * Mathf.Deg2Rad)) * stoneDyR;
            stone.transform.position = new Vector2(stone.transform.position.x, target.transform.position.y);
            trail.transform.position = this.transform.position;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, generateAngle);
            totalDistance = (float)System.Math.Round((-this.transform.position.x + target.transform.position.x), 2);
            //HRange = ((arrowVi*arrowVi) * (Mathf.Sin((generateAngle*2) * Mathf.Deg2Rad)))/ Mathf.Abs(Physics2D.gravity.y);
            //rangePoint.transform.position = new Vector2(theShoot.transform.position.x +stoneDH, theShoot.transform.position.y);
            //theShoot.speed = Mathf.Sqrt((stoneDH*Mathf.Abs(Physics2D.gravity.y)) / (Mathf.Sin((generateAngle*2) * Mathf.Deg2Rad)));
            vi = ((Mathf.Sqrt((Mathf.Abs(Physics2D.gravity.y) / (2 * ((stoneOpp * (Mathf.Tan((generateAngle) * Mathf.Deg2Rad))) - stoneDyR))))) * stoneOpp) / (Mathf.Cos((generateAngle) * Mathf.Deg2Rad));
            generateAnswer = golemTravelTime - projectileTime;
            correctAnswer = (float)System.Math.Round(generateAnswer, 2);
            projectileTime = stoneOpp / (vi * (Mathf.Cos(generateAngle * Mathf.Deg2Rad)));
            lineVertical.transform.position = stone.transform.position;
            lineHorizontal.transform.position = stone.transform.position;
            theMeter[0].positionX = lineVertical.transform.position.x + 1;
            //theMeter[0].positionY = this.transform.position.y;
            theMeter[0].distance = stone.transform.position.y + 0.25f;
            theMeter[1].positionX = this.transform.position.x + 1;
            theMeter[1].distance = this.transform.position.y + 0.25f;
            theMeter[2].positionX = this.transform.position.x;
            theMeter[2].positionY = this.transform.position.y - 2.5f;
            theMeter[2].distance = -this.transform.position.x + target.transform.position.x;

            theCircular[0]._degrees = generateAngle;
            theCircular[0]._origin = new Vector2(this.transform.position.x + .5f, this.transform.position.y);
            theCircular[1]._degrees = generateStoneAngle;
            theCircular[1]._origin = new Vector2(stone.transform.position.x - .2f, stone.transform.position.y - .2f);
            VoTxt.text = ("vi = ") + vi.ToString("F2") + (" m/s");
            VoTxt.gameObject.transform.position = angleArrow.transform.position;
            theQuestion.SetQuestion((PlayerPrefs.GetString("Name") + (" is now instructed to fire a gun aiming at an angle of <b>") + generateAngle.ToString("F2") + ("</b> degrees horizontally and must hit a moving Golem in its weakspot that measures <b>") + (stoneDyR + 1.62).ToString("F2") + ("</b> meters from the ground and moves <b>") + vG.ToString("F2") + ("</b> m/s directly towards ") + PlayerPrefs.GetString("Name") + (" wherein <b>") + ((-this.transform.position.x + target.transform.position.x)).ToString("F2") + ("</b> meters away from the Golem. In order to perfect this stunt, ") + pronoun + (" will use a special gun with a muscle initial velocity of <b>") + vi.ToString("F2") + ("</b> m/s, firing at a height of <b>1.62</b> meters from the ground to the tip of its gun barrel. How long will ") + PlayerPrefs.GetString("Name") + (" wait before pulling the trigger?")));
            if (showProjectile)
            {
                StartCoroutine(project());
                showProjectile = false;
            }

        }
        if (timeStart)
        {
            timer += Time.fixedDeltaTime;
            if (timer <  ProjHardSimulation.playerAnswer)
            {
                timerTxt.text = timer.ToString("F2");
            }
            if (timer >=  ProjHardSimulation.playerAnswer)
            {
                timerTxt.text =  ProjHardSimulation.playerAnswer.ToString("F2");
            }
            if (timer >= projectileTime +  ProjHardSimulation.playerAnswer)
            {
                theGolem.moveSpeed = 0;
            }
            if (timer >= projectileTime +  ProjHardSimulation.playerAnswer)
            {
                //StartCoroutine(theIndicator.showIndicator());
            }

        }

        if (theQuestion.isSimulating == true)
        {
            timeStart = true;
            theGolem.moveSpeed = vG;
            trail.SetActive(true);
            dimension.SetActive(false);
            if (shootReady)
            {
                if (timer >= ProjHardSimulation.playerAnswer)
                {

                    theQuestion.isSimulating = false;
                    if ( ProjHardSimulation.playerAnswer == correctAnswer)
                    {
                        vi += .08f;
                        theQuestion.answerIsCorrect = true;
                        actiontxt.text = ("next");
                        StartCoroutine(ropePull());
                        answerIsCorrect = true;
                        deflector.GetComponent<Collider2D>().isTrigger = true;

                        //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " pulled the trigger at the exact timed. The correct answer is  <b>" + correctAnswer.ToString("F2") + "</b> seconds.");

                    }
                    if ( ProjHardSimulation.playerAnswer > correctAnswer)
                    {
                        //theQuestion.SetModalTitle("Stunt failed");
                        //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " pulled the trigger after <b>" +  ProjHardSimulation.playerAnswer.ToString("F2") + "</b> seconds and too late to hit the target. The correct answer is  <b>" + correctAnswer.ToString("F2") + "</b> seconds.");
                        vi += .3f;
                        StartCoroutine(StuntResult());
                    }
                    if ( ProjHardSimulation.playerAnswer < correctAnswer)
                    {
                        //theQuestion.SetModalTitle("Stunt failed");
                        //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " pulled the trigger after <b>" +  ProjHardSimulation.playerAnswer.ToString("F2") + "</b> seconds and too soon to hit the target. The correct answer is  <b>" + correctAnswer.ToString("F2") + "</b> seconds.");
                        vi -= .3f;
                        StartCoroutine(StuntResult());
                    }
                    ShootArrow();

                    shootReady = false;
                }

            }



            if ( ProjHardSimulation.playerAnswer != correctAnswer)
            {
                //StartCoroutine(golemThrow());

            }




        }


    }
    public void generateProblem()
    {
        theHeart.losslife = false;
        //theIndicator.showReady = true;
        indicatorReady = true;
        generateAngle = Random.Range(50, 60);
        projectArrowTrail.SetActive(false);
        showProjectile = true;
        theArrow[1].generateLine = true;
        theArrow[1].rb.bodyType = RigidbodyType2D.Dynamic;
        projectArrow.SetActive(false);
        timer = 0;
        shootReady = true;
        dimension.SetActive(true);
        generateVG = Random.Range(2f, 3f);
        vG = (float)System.Math.Round(generateVG, 2);
        generateDistance = Random.Range(25f, 30);
        theGolem.transform.position = new Vector2(30, theGolem.transform.position.y);
        timeStart = false;
        arrow.SetActive(false);
        trail.SetActive(false);
        projectArrow.transform.position = this.transform.position;
        arrow.transform.position = this.transform.position;
        theArrow[0].line.SetActive(true);
        theArrow[0].getAngle = false;
        theArrow[0].generateLine = true;


        //Mgear.transform.rotation = Quaternion.Euler(Mgear.transform.rotation.x, Mgear.transform.rotation.y, generateAngle);





    }
    IEnumerator ropePull()
    {


        yield return new WaitForSeconds(projectileTime);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 19);
        //theGolem.throwing = true;
        theGolem.moveSpeed = 0;
        yield return new WaitForSeconds(.55f);
        boulder.SetActive(false);
        //theGolem.throwing = false;
        //theBoulder.boulderThrow();
        puller.GetComponent<Rigidbody2D>().velocity = transform.right * 32;
        thePlayer.airdive = true;
        thePlayer.aim = false;
        StartCoroutine(StuntResult());

    }
    IEnumerator golemThrow()
    {
        yield return new WaitForSeconds(1.5f);
        theGolem.throwing = true;
        yield return new WaitForSeconds(.55f);
        boulder.SetActive(false);
        theGolem.throwing = false;
        theBoulder.boulderThrow();
    }

    public void reShoot()
    {
        puller.GetComponent<Rigidbody2D>().velocity = transform.right * 10;
    }
    public void ShootArrow()
    {
        arrow.SetActive(true);
        trail.GetComponent<TrailRenderer>().time = 3000;
        arrow.transform.position = transform.position;
        if ( ProjHardSimulation.playerAnswer < correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (vi);
        }
        if ( ProjHardSimulation.playerAnswer > correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (vi);
        }
        if ( ProjHardSimulation.playerAnswer == correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (vi);
        }
        GameObject explosion = Instantiate(blastPrefab);
        explosion.transform.position = transform.position;

    }
    IEnumerator project()
    {

        yield return new WaitForSeconds(1);
        projectArrowTrail.GetComponent<TrailRenderer>().time = 3000;
        projectArrow.SetActive(true);
        projectArrow.transform.position = transform.position;
        projectArrow.GetComponent<Rigidbody2D>().velocity = transform.right * (vi);
    }
     public IEnumerator errorMesage()
    {
        theQuestion.popupVisible = true;
        yield return new WaitForSeconds(3);
        theQuestion.popupVisible = false;
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(projectileTime + 2);
        if ( ProjHardSimulation.playerAnswer != correctAnswer)
        {
            //TODO: reduceLife
        }
        if ( ProjHardSimulation.playerAnswer == correctAnswer)
        {
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and hit the target"),true, false);
        }
         if ( ProjHardSimulation.playerAnswer != correctAnswer)
        {
            theHeart.ReduceLife();
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has unable to hit the target"),false, false);
        }
         /*if ( ProjHardSimulation.playerAnswer > correctAnswer)
        {
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " pulled the trigger after <b>" +  ProjHardSimulation.playerAnswer.ToString("F2") + "</b> seconds and too soon to hit the target. The correct answer is  <b>" + correctAnswer.ToString("F2") + "</b> seconds."),false, false);
        }*/
        //trail.GetComponent<TrailRenderer>().time = 3;
        
        StartCoroutine(theSimulate.DirectorsCall());
        //theQuestion.ToggleModal();
        theArrow[0].generateLine = false;
        //trail.SetActive(false);
        //theArrow[0].rb.bodyType = RigidbodyType2D.Dynamic;
        theArrow[0].getAngle = false;
        theArrow[0].gameObject.SetActive(false);
        thePlayer.sword.SetActive(false);
    }

}