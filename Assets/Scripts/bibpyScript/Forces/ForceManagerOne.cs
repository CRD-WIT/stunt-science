using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForceManagerOne : MonoBehaviour
{
    public PlayerB thePlayer;
    public BombScript theBombScript;
    private ForceSimulation theSimulate;
    private ColliderManager theCollider;
    public QuestionContForces theQuestion;
    float generateAccelaration, accelaration, playerAccelaration, generateMass, mass, generateCorrectAnswer, currentPos;
    public float correctAnswer,playerAnswer;
    public GameObject glassHolder, stickPrefab, stickmanpoint, glassDebri, playerInitials,triggerDevour;
    public GameObject[] glassDebriLoc;
    public bool tooWeak, tooStrong, ragdollReady;
    public bool throwBomb;
    public TMP_Text masstxt,  acctxt, breakingforcetxt, forcetxt, actiontxt;
    private HeartManager theHeart;


    // Start is called before the first frame update
    void Start()
    {
        //theQuestion.stageNumber = 1;
        //thePlayer = FindObjectOfType<Player>();
        theCollider = FindObjectOfType<ColliderManager>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        theHeart = FindObjectOfType<HeartManager>();
        GenerateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPos = thePlayer.transform.position.x;
        playerAnswer = ForceSimulation.playerAnswer;
        generateCorrectAnswer = mass * accelaration;
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);
        forcetxt.text = ("f = ")+ correctAnswer.ToString("F2")+("N");
        forcetxt.color = new Color32(107, 0, 176, 255);
        
        if (ForceSimulation.simulate == true)
        {
             
            forcetxt.gameObject.SetActive(true); 
            forcetxt.gameObject.transform.position = new Vector2(thePlayer.transform.position.x - 4,thePlayer.transform.position.y);  
            playerInitials.SetActive(false);
            thePlayer.moveSpeed += accelaration * Time.fixedDeltaTime;
            theSimulate.zombieChase = true;
            if (theCollider.collide == true)
            {
                if(playerAnswer == correctAnswer)
                {
                    
                    actiontxt.text = "Next";
                    theQuestion.answerIsCorrect = true;
                    //theQuestion.SetModalTitle("Stunt Success");
                    //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " has broken the glass and succesfully escaped from zombies</color>");
                    glassHolder.SetActive(false);
                    
                    if(currentPos >= 22)
                    {
                        
                        braking();
                        thePlayer.moveSpeed = 0; 
                        thePlayer.transform.position = new Vector2(22, 3.86f);
                        ForceSimulation.simulate = false;
                        StartCoroutine(collision());
                        StartCoroutine(StuntResult());
                        
                        
                    }
                }
                if(playerAnswer > correctAnswer)
                {
                    //theQuestion.SetModalTitle("Stunt Failed");
                    //theQuestion.SetModalText(" the glass was too tough for </color>" + PlayerPrefs.GetString("Name") + ", and unable to break the glass. The correct answer is "+ correctAnswer.ToString("F2") +"Newtons.");
                    tooWeak = true;
                    thePlayer.gameObject.SetActive(false);
                    if(ragdollReady)
                    {
                        ragdollSpawn();
                        thePlayer.standup = true;
                    }
                    thePlayer.moveSpeed = 0;
                    StartCoroutine(collision());
                    StartCoroutine(StuntResult());
                    theSimulate.playerDead = true;
                    theHeart.losinglife();
                    ForceSimulation.simulate = false;
                }
                if(playerAnswer < correctAnswer)
                {
                   //triggerDevour.SetActive(true);
                    //theQuestion.SetModalTitle("Stunt Failed");
                    //theQuestion.SetModalText(" the glass was too weak for </color>" + PlayerPrefs.GetString("Name") + ", able to break the glass but also went through it. The correct answer is "+ correctAnswer.ToString("F2") +"Newtons.");
                    tooStrong = true;
                    thePlayer.gameObject.SetActive(false);
                    glassHolder.SetActive(false);
                    throwBomb = true;
                    if(ragdollReady)
                    {
                        ragdollSpawn();
                    }
                   StartCoroutine(collision());
                   StartCoroutine(StuntResult());
                   theSimulate.playerDead = true;
                   theHeart.losinglife();
                   thePlayer.moveSpeed = 0;
                   ForceSimulation.simulate = false;
                   
                }
            }
        }
    }
    public void GenerateProblem()
    {
        generateAccelaration = Random.Range(5f, 7f);
        accelaration = (float)System.Math.Round(generateAccelaration, 2);
        generateMass = Random.Range(70f, 75f);
        mass = (float)System.Math.Round(generateMass, 2);
        theSimulate.glassHolder[0].SetActive(true);
        theSimulate.otherGlassHolder[0].SetActive(true);
        ragdollReady = true;
        //theBomb.bomb.SetActive(true);
        //theBomb.bomb.transform.position = thePlayer.transform.position;
        //bombHinge.transform.position = thePlayer.transform.position;
        glassRespawn();
        theQuestion.SetQuestion((PlayerPrefs.GetString("Name") + ("</b> is instructed to break the glass wall by running into it using his own body mass. If  <b>") + PlayerPrefs.GetString("Name") + ("</b> has a mass of  <b>") + mass.ToString("F2") + ("</b> kg and runs with an accelaration of <b>") + accelaration.ToString("F2") + ("</b> m/s², what should impact force breaking point of the glass wall? If the glass is too tough , it will not break. If the glass is too weak, ") + PlayerPrefs.GetString("Name") + (" will overshoot beyond the glass after breaking.")));
        masstxt.text = ("m = ")+mass.ToString("F2") + ("kg");
        acctxt.text = ("a = ") + accelaration.ToString("F2") + ("m/s²");
        breakingforcetxt.text = "Breaking Impact Force = ?";
        playerInitials.SetActive(true);
        forcetxt.gameObject.SetActive(false);
        //theBombScript.inPlayer = true;
        

        
        


    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(stickPrefab);
        stick.transform.position = stickmanpoint.transform.position;
        ragdollReady = false;
    }
    void braking()
    {
        thePlayer.brake = true;
        thePlayer.exitDown = true;
        thePlayer.myRigidbody.bodyType = RigidbodyType2D.Static;
        thePlayer.myCollider.isTrigger = true;

       
    }
    IEnumerator collision()
    {
        yield return new WaitForEndOfFrame();
        theCollider.collide = false;
        
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(2f);
        ForceSimulation.simulate = false;
         if(playerAnswer == correctAnswer)
       {
            yield return new WaitForSeconds(4);
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and hit the target"), true, false);
       }
        StartCoroutine(theSimulate.DirectorsCall());
       if(playerAnswer != correctAnswer)
       {
            theSimulate.zombieChase = false;
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " has succesfully performed the stunt and hit the target"), false, false);
            yield return new WaitForSeconds(3);
       }
       
       //theQuestion.ToggleModal();
        
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
    public IEnumerator createZombies()
    {
        yield return new WaitForEndOfFrame();
        theSimulate.destroyZombies = false;
        GameObject zombie1 = Instantiate(theSimulate.zombiePrefab);
        zombie1.transform.position = new Vector2(-12, 3.86f);
        GameObject zombie2 = Instantiate(theSimulate.zombiePrefab);
        zombie2.transform.position = new Vector2(-8.5f, 3.86f);
        GameObject zombie3 = Instantiate(theSimulate.zombiePrefab);
        zombie3.transform.position = new Vector2(-6.6f, 3.86f);
        GameObject zombie4 = Instantiate(theSimulate.zombiePrefab);
        zombie4.transform.position = new Vector2(-10, 3.86f);
    }

}
