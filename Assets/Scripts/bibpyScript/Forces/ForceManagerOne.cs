using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForceManagerOne : MonoBehaviour
{
    public Player thePlayer;
    public BombScript theBombScript;
    private ForceSimulation theSimulate;
    private ColliderManager theCollider;
    public QuestionController theQuestion;
    private BombManager theBomb;
    float generateAccelaration, accelaration, playerAccelaration, generateMass, mass, generateCorrectAnswer, currentPos;
    public float correctAnswer,playerAnswer;
    public GameObject glassHolder, stickPrefab, stickmanpoint, bombHinge, glassDebri, playerInitials;
    public GameObject[] glassDebriLoc;
    public bool tooWeak, tooStrong, ragdollReady;
    public bool throwBomb;
    public TMP_Text masstxt,  acctxt, breakingforcetxt, forcetxt;
    private HeartManager theHeart;


    // Start is called before the first frame update
    void Start()
    {
        //thePlayer = FindObjectOfType<Player>();
        theCollider = FindObjectOfType<ColliderManager>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        theBomb = FindObjectOfType<BombManager>();
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
        
        if (ForceSimulation.simulate == true)
        {
             
            forcetxt.gameObject.SetActive(true); 
            forcetxt.gameObject.transform.position = new Vector2(thePlayer.transform.position.x - 4,thePlayer.transform.position.y);  
            playerInitials.SetActive(false);
            thePlayer.moveSpeed += accelaration * Time.fixedDeltaTime;
            if (theCollider.collide == true)
            {
                if(playerAnswer == correctAnswer)
                {
                     theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " has broken the glass</color>");
                    glassHolder.SetActive(false);
                    
                    if(currentPos >= 22)
                    {
                        
                        StartCoroutine(braking());
                        thePlayer.moveSpeed = 0; 
                        thePlayer.transform.position = new Vector2(22, -0.63f);
                        ForceSimulation.simulate = false;
                        StartCoroutine(collision());
                        StartCoroutine(StuntResult());
                        
                        
                    }
                }
                if(playerAnswer > correctAnswer)
                {
                    theQuestion.SetModalText(" the glass was too tough for </color>" + PlayerPrefs.GetString("Name") + ", and unable to break the glass. The correct answer is "+ correctAnswer.ToString("F2") +"Newtons.");
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
                    theQuestion.SetModalText(" the glass was too weak for </color>" + PlayerPrefs.GetString("Name") + ", able to break the glass but also went through it. The correct answer is "+ correctAnswer.ToString("F2") +"Newtons.");
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
        
        generateAccelaration = Random.Range(7f, 9f);
        accelaration = (float)System.Math.Round(generateAccelaration, 2);
        generateMass = Random.Range(70f, 75f);
        mass = (float)System.Math.Round(generateMass, 2);
        theBomb.glassHolder[0].SetActive(true);
        theBomb.otherGlassHolder[0].SetActive(true);
        ragdollReady = true;
        theBomb.bomb.SetActive(true);
        theBomb.bomb.transform.position = thePlayer.transform.position;
        //bombHinge.transform.position = thePlayer.transform.position;
        glassRespawn();
        theQuestion.SetQuestion((PlayerPrefs.GetString("Name") + ("</b> is instructed to break the glass wall by running into it using his own body mass. If  <b>") + PlayerPrefs.GetString("Name") + ("</b> has a mass of  <b>") + mass.ToString("F2") + ("</b> kg and runs with an accelaration of <b>") + accelaration.ToString("F2") + ("</b> m/s², what should impact force breaking point of the glass wall? If the glass is too tough , it will not break. If the glass is too weak, ") + PlayerPrefs.GetString("Name") + (" will overshoot beyond the glass after breaking.")));
        masstxt.text = mass.ToString("F2") + ("kg");
        acctxt.text = ("a = ") + accelaration.ToString("F2") + ("m/s²");
        breakingforcetxt.text = "Breaking Impact Force = ?";
        playerInitials.SetActive(true);
        forcetxt.gameObject.SetActive(false);

        
        


    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(stickPrefab);
        stick.transform.position = stickmanpoint.transform.position;
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
        //bombHinge.SetActive(false);
       
        thePlayer.brake = false;
       
    }
    IEnumerator collision()
    {
        yield return new WaitForEndOfFrame();
        theCollider.collide = false;
        
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(.5f);
        ForceSimulation.simulate = false;
        StartCoroutine(theSimulate.DirectorsCall());
       if(playerAnswer == correctAnswer)
       {
            yield return new WaitForSeconds(3);
       }
       if(playerAnswer != correctAnswer)
       {
            yield return new WaitForSeconds(6);
       }
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
