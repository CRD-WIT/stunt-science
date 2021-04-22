using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceManagerTwo : MonoBehaviour
{
    public Player thePlayer;
    private ForceSimulation theSimulate;
    private ColliderManager theCollider;
    float generateForce, Force, playerAccelaration, generateMass, mass, generateCorrectAnswer, currentPos;
    public float correctAnswer,playerAnswer;
    public GameObject glassHolder, stickPrefab, stickmanpoint, bombHinge, afterStuntMessage, retry, next, glassDebri, bomb, wordedBoard, director;
    public GameObject[] glassDebriLoc;
    public bool tooWeak, tooStrong, ragdollReady;
    public bool throwBomb;


    // Start is called before the first frame update
    void Start()
    {
        //thePlayer = FindObjectOfType<Player>();
        theCollider = FindObjectOfType<ColliderManager>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        wordedBoard.transform.position = new Vector2(20, wordedBoard.transform.position.y);
        GenerateProblem();
        director.transform.position = new Vector2(-5.2f, 2.4f);
        director.transform.localScale = new Vector2(-director.transform.localScale.x, director.transform.localScale.y);
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
        bomb.SetActive(true);
        bomb.transform.position = new Vector2(32, -0.6f);
        thePlayer.transform.position = new Vector2(31,-0.6f);
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        //ForceSimulation.question = ((PlayerPrefs.GetString("Name") + ("</b> is   <b>") + pronoun + ("</b>is in, <b>") + pronoun + ("</b> must drive his motorcycle from a complete standstill to a speed of <b>") + Vf.ToString("F1") + ("</b> m/s, after ") + time.ToString("F1") + ("seconds. What should be ") + pronoun + (" Force in order to achieve the final velocity?"));
        
        


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
        yield return new WaitForSeconds(3);
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
