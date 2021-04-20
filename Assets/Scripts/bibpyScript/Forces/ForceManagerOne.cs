using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceManagerOne : MonoBehaviour
{
    public Player thePlayer;
    private ColliderManager theCollider;
    float generateAccelaration, accelaration, playerAccelaration, generateMass, mass, generateCorrectAnswer, currentPos;
    public float correctAnswer,playerAnswer;
    public GameObject glassHolder, stickPrefab, stickmanpoint, bombHinge;
    public bool tooWeak, tooStrong, ragdollReady;
    public bool throwBomb;


    // Start is called before the first frame update
    void Start()
    {
        //thePlayer = FindObjectOfType<Player>();
        theCollider = FindObjectOfType<ColliderManager>();
        GenerateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPos = thePlayer.transform.position.x;
        playerAnswer = ForceSimulation.playerAnswer;
        generateCorrectAnswer = mass * accelaration;
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);
        
        if (ForceSimulation.simulate == true)
        {
             
                thePlayer.brake = true;
             
            thePlayer.moveSpeed += accelaration * Time.fixedDeltaTime;
            if (theCollider.collide == true)
            {
                if(playerAnswer == correctAnswer)
                {
                    glassHolder.SetActive(false);
                    
                    if(currentPos >= 22)
                    {
                        StartCoroutine(braking());
                        thePlayer.moveSpeed = 0; 
                        thePlayer.transform.position = new Vector2(22, -0.63f);
                        ForceSimulation.simulate = false;
                        throwBomb = true;
                        bombHinge.SetActive(false);
                        theCollider.collide = false;
                        
                        
                    }
                }
                if(playerAnswer < correctAnswer)
                {
                    tooWeak = true;
                    thePlayer.gameObject.SetActive(false);
                    if(ragdollReady)
                    {
                        ragdollSpawn();
                    }
                    StartCoroutine(collision());
                }
                if(playerAnswer > correctAnswer)
                {
                    tooStrong = true;
                    thePlayer.gameObject.SetActive(false);
                    glassHolder.SetActive(false);
                    if(ragdollReady)
                    {
                        ragdollSpawn();
                    }
                    theCollider.collide = false;
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
        yield return new WaitForSeconds(.5f);
        thePlayer.brake = false;
    }
    IEnumerator collision()
    {
        yield return new WaitForEndOfFrame();
        theCollider.collide = false;

    }

}
