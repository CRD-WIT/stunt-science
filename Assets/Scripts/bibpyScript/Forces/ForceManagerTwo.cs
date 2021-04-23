using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForceManagerTwo : MonoBehaviour
{
    public Player thePlayer;
    private ForceSimulation theSimulate;
    private BombManager theBomb;
    public BombScript theBombScript;
    private ColliderManager theCollider;
    float generateForce, Force, playerAccelaration, generateMass, mass, generateCorrectAnswer, currentPos;
    public float correctAnswer,playerAnswer;
    public GameObject glassHolder, stickPrefab, stickmanpoint, bombHinge, afterStuntMessage, retry, next, glassDebri, bomb, wordedBoard, director, speechBubble;
    public GameObject[] glassDebriLoc;
    public bool tooWeak, tooStrong, ragdollReady;
    public bool throwBomb;
    public TMP_Text stuntMessageTxt;
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
        theCollider = FindObjectOfType<ColliderManager>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        theBomb = FindObjectOfType<BombManager>();
        wordedBoard.transform.position = new Vector2(20, wordedBoard.transform.position.y);
        GenerateProblem();
        director.transform.position = new Vector2(-5.2f, 2.4f);
        director.transform.localScale = new Vector2(-director.transform.localScale.x, director.transform.localScale.y);
        speechBubble.transform.localScale = new Vector2(-speechBubble.transform.localScale.x, speechBubble.transform.localScale.y);
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPos = thePlayer.transform.position.x;
        playerAnswer = ForceSimulation.playerAnswer;
        generateCorrectAnswer = Force/mass;
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);
        
        if (ForceSimulation.simulate == true)
        {
             
                
             
            thePlayer.moveSpeed -= playerAnswer * Time.fixedDeltaTime;
            if (theCollider.collide == true)
            {
                if(playerAnswer == correctAnswer)
                {
                    glassHolder.SetActive(false);
                    stuntMessageTxt.text = "<b><color=green>Your Answer is Correct!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " has broken the glass</color>";
                    if(currentPos <= 5.7f)
                    {
                        StartCoroutine(braking());
                        thePlayer.moveSpeed = 0; 
                        thePlayer.transform.position = new Vector2(5.8f, -0.63f);
                        ForceSimulation.simulate = false;
                        StartCoroutine(collision());
                        
                        
                    }
                }
                if(playerAnswer < correctAnswer)
                {
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " accelerated too slow and unable to break the glass. The correct answer is "+ correctAnswer.ToString("F1") +"m/s².";
                    tooWeak = true;
                    thePlayer.gameObject.SetActive(false);
                    if(ragdollReady)
                    {
                        ragdollSpawn();
                        thePlayer.standup = true;
                    }
                    thePlayer.moveSpeed = 0;
                    retry.SetActive(true);
                    StartCoroutine(collision());
                    StartCoroutine(StuntResult());
                    theSimulate.playerDead = true;
                }
                if(playerAnswer > correctAnswer)
                {
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " accelerated too fast and able to break the glass but also went through it. The correct answer is "+ correctAnswer.ToString("F1") +"m/s².";
                    tooStrong = true;
                    thePlayer.gameObject.SetActive(false);
                    glassHolder.SetActive(false);
                    throwBomb = true;
                    if(ragdollReady)
                    {
                        ragdollSpawn();
                    }
                   StartCoroutine(collision());
                   retry.SetActive(true);
                   StartCoroutine(StuntResult());
                }
            }
        }
    }
    public void GenerateProblem()
    {
        generateForce = Random.Range(500, 700);
        Force = (float)System.Math.Round(generateForce, 2);
        generateMass = Random.Range(70f, 75f);
        mass = (float)System.Math.Round(generateMass, 2);
        glassRespawn();
        ragdollReady = true;
        theBomb.bomb.SetActive(true);
        theBomb.bomb.transform.position = thePlayer.transform.position;
        theBomb.followRagdoll = false;
        theBomb.glassHolder[1].SetActive(true);
        theBomb.otherGlassHolder[1].SetActive(true);
        thePlayer.transform.position = new Vector2(31,-0.6f);
        ForceSimulation.question = ((PlayerPrefs.GetString("Name") + ("</b> must break another glass wall to throw out the 2nd bomb. If  <b>") + PlayerPrefs.GetString("Name") + ("</b> has a mass of  <b>") + mass.ToString("F2") + ("</b> kg and the glass wall has an impact force breaking point of <b>") + Force.ToString("F2") + ("</b> Newtons, how fast should ") + pronoun + (" accelerate towards the glass wall just enough to break it and not go through it?")));
        


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
        yield return new WaitForSeconds(1);
        bombHinge.SetActive(false);
        throwBomb = true;
        thePlayer.brake = false;
        theBombScript.inPlayer = false;
    }
    IEnumerator collision()
    {
        yield return new WaitForEndOfFrame();
        theCollider.collide = false;

    }
    IEnumerator StuntResult()
    {
        ForceSimulation.simulate = false;
        StartCoroutine(theSimulate.DirectorsCall());
        yield return new WaitForSeconds(5);
        afterStuntMessage.SetActive(true);
        
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
