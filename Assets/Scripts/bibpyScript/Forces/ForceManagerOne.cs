using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceManagerOne : MonoBehaviour
{
    private Player thePlayer;
    private ColliderManager theCollider;
    float generateAccelaration;
    float accelaration;
    float playerAccelaration;
    float generateMass;
    float mass;
    float generateCorrectAnswer;
    float correctAnswer;
    float playerAnswer;
    float currentPos;
    public GameObject glassHolder, stickPrefab, stickmanpoint;
    public bool tooWeak, tooStrong, ragdollReady;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
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
            thePlayer.moveSpeed += accelaration * Time.fixedDeltaTime;
            if (theCollider.collide == true)
            {
                if(playerAnswer == correctAnswer)
                {
                    glassHolder.SetActive(false);
                   
                    if(currentPos >= 22)
                    {
                        thePlayer.moveSpeed = 0;
                        thePlayer.brake = true;
                        thePlayer.transform.position = new Vector2(22, -0.63f);
                        ForceSimulation.simulate = false;
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
                }
            }
        }
    }
    public void GenerateProblem()
    {
        generateAccelaration = Random.Range(4f, 6f);
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
}
