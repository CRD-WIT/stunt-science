using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectileHardOne : MonoBehaviour
{
    public playerProjectile thePlayer;
    public ProjSimulationManager theSimulate;
    public QuestionControllerB theQuestion;
    public golem theGolem;
    public CircularAnnotation[] theCircular;
    public BoulderProjectile theBoulder;
    public DistanceMeter[] theMeter;
    public Arrow theArrow;
    public GameObject Mgear, stone, target, puller, arrow,projectArrow, blastPrefab, deflector, trail, lineAngle, lineDistance, boulder, angleArrow;
    public GameObject lineVertical, lineHorizontal, dimension;
    public float vi, generateVG, vG;
    public float generateAngle, generateStoneAngle, stoneAngle, stoneOpp, HRange, timer, projectileTime, golemTravelTime;
    public float stoneDY, correctAnswer, stoneDyR, generateAnswer;
    public float generateDistance, totalDistance, golemDistanceToTravel;
    public bool timeStart, answerIsCorrect, shootReady, showProjectile;
    public TMP_Text golemVelo, golemAcc, projViTxt;
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

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lineAngle.transform.position = stone.transform.position;
        lineDistance.transform.position = new Vector2(stone.transform.position.x, this.transform.position.y);
        if (!timeStart)
        {
            golemVelo.text= ("v = ") + vG.ToString("F2") + (" m/s");
            golemAcc.text = ("a = none");
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
            theCircular[0]._origin = new Vector2(this.transform.position.x + .5f, this.transform.position.y - .1f);
            theCircular[1]._degrees = generateStoneAngle;
            theCircular[1]._origin = new Vector2(stone.transform.position.x-.2f , stone.transform.position.y-.2f);
            projViTxt.text = ("vi = ")+ vi.ToString("F2") + (" m/s");
             theQuestion.SetQuestion(( PlayerPrefs.GetString("Name") + (" is now instructed to fire a gun aiming at an angle of <b>") + generateAngle.ToString("F2") + ("</b> degrees horizontally and must hit a moving Golem in its weakspot that measures <b>") + (stoneDyR + 1.62).ToString("F2") + ("</b> meters from the ground and moves <b>") + vG.ToString("F2") + ("</b> m/s directly towards ") + PlayerPrefs.GetString("Name") + (" wherein <b>") + ((-this.transform.position.x + target.transform.position.x)).ToString("F2") + ("</b> meters away from the Golem. In order to perfect this stunt, ") +pronoun+ ("will use a special gun with a muscle initial velocity of <b>") +vi.ToString("F2") + ("</b> m/s, firing at a height of <b>1.62</b> meters from the ground to the tip of its gun barrel. How long will ") +PlayerPrefs.GetString("Name")+ (" wait before pulling the trigger?")));
            if(showProjectile)
            {
                StartCoroutine(project());
                showProjectile = false;
            }

        }
        if (timeStart)
        {
            timer += Time.fixedDeltaTime;
            if(timer >= projectileTime + ProjSimulationManager.playerAnswer)
            {
                theGolem.moveSpeed = 0;
            }

        }

        if (ProjSimulationManager.simulate == true)
        {
            timeStart = true;
            theGolem.moveSpeed = vG;
            trail.SetActive(true);
            dimension.SetActive(false);
            if (shootReady)
            {
                if (timer >= ProjSimulationManager.playerAnswer)
                {
                    
                    ProjSimulationManager.simulate = false;
                    if (ProjSimulationManager.playerAnswer == correctAnswer)
                    {
                         vi += .08f;
                        theQuestion.answerIsCorrect = true;
                        Shoot();
                        answerIsCorrect = true;
                        deflector.GetComponent<Collider2D>().isTrigger = true;
                    }
                    if (ProjSimulationManager.playerAnswer > correctAnswer)
                    {
                        vi += .3f;
                    }
                     if (ProjSimulationManager.playerAnswer < correctAnswer)
                    {
                        vi -= .3f;
                    }
                    ShootArrow();

                    shootReady = false;
                }

            }



            if (ProjSimulationManager.playerAnswer != correctAnswer)
            {
                //StartCoroutine(golemThrow());
                StartCoroutine(StuntResult());
            }




        }


    }
    public void generateProblem()
    {
        dimension.SetActive(true);
        generateVG = Random.Range(2f, 3f);
        vG = (float)System.Math.Round(generateVG, 2);
        generateDistance = Random.Range(25f, 30);
        theGolem.transform.position = new Vector2(20, theGolem.transform.position.y);
        timeStart = false;
        arrow.SetActive(false);
        trail.SetActive(false);
        arrow.transform.position = this.transform.position;
        theArrow.line.SetActive(true);
        theArrow.getAngle = false;
        theArrow.generateLine = true;
        generateAngle = Random.Range(50, 60);
       
        //Mgear.transform.rotation = Quaternion.Euler(Mgear.transform.rotation.x, Mgear.transform.rotation.y, generateAngle);





    }
    IEnumerator ropePull()
    {
        

        yield return new WaitForSeconds(projectileTime);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 20);
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
    IEnumerator golemThrow()
    {
        yield return new WaitForSeconds(1.5f);
        theGolem.throwing = true;
        yield return new WaitForSeconds(.55f);
        boulder.SetActive(false);
        theGolem.throwing = false;
        theBoulder.boulderThrow();
    }
    public void Shoot()
    {
        StartCoroutine(ropePull());
    }
    public void reShoot()
    {
        puller.GetComponent<Rigidbody2D>().velocity = transform.right * 10;
    }
    public void ShootArrow()
    {
        arrow.SetActive(true);
        arrow.transform.position = transform.position;
        if (ProjSimulationManager.playerAnswer < correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (vi);
        }
        if (ProjSimulationManager.playerAnswer > correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (vi);
        }
        if (ProjSimulationManager.playerAnswer == correctAnswer)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = transform.right * (vi);
        }
        GameObject explosion = Instantiate(blastPrefab);
        explosion.transform.position = transform.position;

    }
    IEnumerator project()
    {
        yield return new WaitForSeconds(1);
        projectArrow.SetActive(true);
        projectArrow.transform.position = transform.position;
        projectArrow.GetComponent<Rigidbody2D>().velocity = transform.right * (vi);
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(theSimulate.DirectorsCall());
        theQuestion.ToggleModal();
    }

}