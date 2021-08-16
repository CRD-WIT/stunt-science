using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHardTwo : MonoBehaviour
{
    public playerProjectile thePlayer;
    public ProjSimulationManager theSimulate;
    public QuestionControllerB theQuestion;
    public golem theGolem;
    public CircularAnnotation[] theCircular;
    public Arrow[] theArrow;
    public GameObject Mgear, stone, target, puller, arrow, projectArrow, projectArrowTrail, blastPrefab, deflector, trail, lineAngle, lineDistance, boulder, angleArrow;
    public GameObject lineVertical, lineHorizontal, dimension;
    public float vi, generateVG, vG;
    public float generateAngle, generateStoneAngle, stoneAngle, stoneOpp, HRange, timer, projectileTime, golemTravelTime;
    public float stoneDY, correctAnswer, stoneDyR, generateAnswer;
    public float generateDistance, totalDistance, golemDistanceToTravel;
    public bool timeStart, answerIsCorrect, shootReady, showProjectile;
    string pronoun, pronoun2, gender;
    void Start()
    {
        theGolem.transform.localScale = new Vector2(-theGolem.transform.localScale.x, theGolem.transform.localScale.y);
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        gender = PlayerPrefs.GetString("Gender");
        thePlayer.aim = true;
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
    void Update()
    {
        
    }
    public void generateProblem()
    {
        thePlayer.transform.position = new Vector2(20, thePlayer.transform.position.y);
        theGolem.transform.position = new Vector2(-10, theGolem.transform.position.y);
        generateAngle = Random.Range(30,50);
        generateVG = Random.Range(2f,4f);
        vG = (float)System.Math.Round(generateStoneAngle, 2);
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
}
