using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForceManagerTwo : MonoBehaviour
{
    public Player thePlayer;
    public QuestionController theQuestion;
    private ForceSimulation theSimulate;
    private BombManager theBomb;
    public BombScript theBombScript;
    private ColliderManager theCollider;
    private HeartManager theHeart;
    float generateForce, Force, playerAccelaration, generateMass, mass, generateCorrectAnswer, currentPos;
    public float correctAnswer,playerAnswer,playerForce;
    public GameObject glassHolder, stickPrefab, stickmanpoint, bombHinge, glassDebri, bomb, director, speechBubble, playerInitials, navigator;
    public GameObject[] glassDebriLoc;
    public bool tooWeak, tooStrong, ragdollReady;
    public bool throwBomb;
    public TMP_Text  masstxt, acctxt, breakingforcetxt, forcetxt, actiontxt;
    string gender, pronoun;


    // Start is called before the first frame update
    void Start()
    {
        if (gender == "Male")
        {
            pronoun = ("he");
        }
        if (gender == "Female")
        {
            pronoun = ("she");
        }
        theQuestion.stageNumber = 2;
        theCollider = FindObjectOfType<ColliderManager>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        theBomb = FindObjectOfType<BombManager>();
        theHeart = FindObjectOfType<HeartManager>();
        GenerateProblem();
        director.transform.position = new Vector2(-5.2f, 2.4f);
        director.transform.localScale = new Vector2(-director.transform.localScale.x, director.transform.localScale.y);
        speechBubble.transform.localScale = new Vector2(-speechBubble.transform.localScale.x, speechBubble.transform.localScale.y);
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        navigator.transform.position = new Vector2(25.6f, navigator.transform.position.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPos = thePlayer.transform.position.x;
        playerAnswer = ForceSimulation.playerAnswer;
        generateCorrectAnswer = Force/mass;
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);
        playerForce = playerAnswer * mass;
        

        
        if (ForceSimulation.simulate == true)
        {
            if(playerAnswer == correctAnswer)
            {
                forcetxt.text = ("f = ")+ Force.ToString("F2")+("N");
            }
            if(playerAnswer != correctAnswer) 
            {
                forcetxt.text = ("f = ")+ playerForce.ToString("F2")+("N");
            }
            forcetxt.gameObject.SetActive(true);    
            forcetxt.gameObject.transform.position = new Vector2(thePlayer.transform.position.x + 4,thePlayer.transform.position.y);  
            playerInitials.SetActive(false);
            thePlayer.moveSpeed -= playerAnswer * Time.fixedDeltaTime;
            if (theCollider.collide == true)
            {
                if(playerAnswer == correctAnswer)
                {
                    actiontxt.text = "Next";
                    theQuestion.answerIsCorrect = true;
                    glassHolder.SetActive(false);
                    theQuestion.SetModalTitle("Stunt Success");
                    theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " has broken the glass</color>");
                    if(currentPos <= 5.7f)
                    {
                        StartCoroutine(braking());
                        thePlayer.moveSpeed = 0; 
                        thePlayer.transform.position = new Vector2(5.8f, -0.63f);
                        ForceSimulation.simulate = false;
                        StartCoroutine(collision());
                        StartCoroutine(StuntResult());
                        
                        
                    }
                }
                if(playerAnswer < correctAnswer)
                {
                    actiontxt.text = "retry";
                    theQuestion.answerIsCorrect = false;
                     if(ragdollReady)
                    {
                        ragdollSpawn();
                        thePlayer.standup = true;
                    }
                    theBombScript.inPlayer = false;
                    theQuestion.SetModalTitle("Stunt Failed");
                    theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " accelerated too slow and unable to break the glass. The correct answer is "+ correctAnswer.ToString("F1") +"m/s².");
                    tooWeak = true;
                    thePlayer.gameObject.SetActive(false);
                   
                    thePlayer.moveSpeed = 0;
                    StartCoroutine(collision());
                    StartCoroutine(StuntResult());
                    theSimulate.playerDead = true;
                    theHeart.losinglife();
                    ForceSimulation.simulate = false;
                }
                if(playerAnswer > correctAnswer)
                {
                    actiontxt.text = "retry";
                    theQuestion.answerIsCorrect = false;
                    if(ragdollReady)
                    {
                        ragdollSpawn();
                    }
                    theQuestion.SetModalTitle("Stunt Failed");
                    theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " accelerated too fast and able to break the glass but also went through it. The correct answer is "+ correctAnswer.ToString("F1") +"m/s².");
                    tooStrong = true;
                    thePlayer.gameObject.SetActive(false);
                    glassHolder.SetActive(false);
                    throwBomb = true;
                    
                   StartCoroutine(collision());
                   StartCoroutine(StuntResult());
                   ForceSimulation.simulate = false;
                   theHeart.losinglife();
                }
            }
        }
    }
    public void GenerateProblem()
    {
        theBombScript.inPlayer = true;
        generateForce = Random.Range(500, 700);
        Force = (float)System.Math.Round(generateForce, 2);
        generateMass = Random.Range(70f, 75f);
        mass = (float)System.Math.Round(generateMass, 2);
        glassRespawn();
        ragdollReady = true;
        theBomb.bomb.SetActive(true);
        theBomb.glassHolder[1].SetActive(true);
        theBomb.otherGlassHolder[1].SetActive(true);
        thePlayer.transform.position = new Vector2(31,-0.6f);
        theBomb.bomb.transform.position = thePlayer.transform.position;
        theQuestion.SetQuestion((PlayerPrefs.GetString("Name") + ("</b> must break another glass wall to throw out the 2nd bomb. If  <b>") + PlayerPrefs.GetString("Name") + ("</b> has a mass of  <b>") + mass.ToString("F2") + ("</b> kg and the glass wall has an impact force breaking point of <b>") + Force.ToString("F2") + ("</b> Newtons, how fast should ") + pronoun + (" accelerate towards the glass wall just enough to break it and not go through it?")));
        masstxt.text = ("m = ")+mass.ToString("F2") + ("kg");
        acctxt.text = "a = ?";
        breakingforcetxt.text = ("Breaking Impact Force = ")+ Force.ToString("F2") + ("N");
        playerInitials.SetActive(true);
        forcetxt.gameObject.SetActive(false);



    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(stickPrefab);
        stick.transform.position = stickmanpoint.transform.position;
        stick.transform.localScale = new Vector2(-stick.transform.localScale.x, stick.transform.localScale.y);
        stick.transform.rotation = Quaternion.Inverse(stick.transform.rotation);
        ragdollReady = false;
    }
    IEnumerator braking()
    {
        thePlayer.brake = true;
        thePlayer.throwing = true;
        yield return new WaitForSeconds(.7f);
        theBombScript.inPlayer = false;
        throwBomb = true;
        yield return new WaitForSeconds(.3f);
        bombHinge.SetActive(false);
        thePlayer.brake = false;
        thePlayer.throwing = false;
      
    }
    IEnumerator collision()
    {
        yield return new WaitForEndOfFrame();
        theCollider.collide = false;

    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(1);
        ForceSimulation.simulate = false;
        StartCoroutine(theSimulate.DirectorsCall());
        yield return new WaitForSeconds(5);
         theQuestion.ToggleModal();
        
    }
    public void glassRespawn()
    {
        GameObject glass1 = Instantiate(glassDebri);
        glass1.transform.position = glassDebriLoc[0].transform.position;
        GameObject glass2 = Instantiate(glassDebri);
        glass2.transform.position = glassDebriLoc[1].transform.position;
        GameObject glass3 = Instantiate(glassDebri);
        glass3.transform.position = glassDebriLoc[2].transform.position;

        

    }

}
