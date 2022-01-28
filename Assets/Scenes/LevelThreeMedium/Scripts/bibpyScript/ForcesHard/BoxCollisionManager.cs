using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollisionManager : MonoBehaviour
{
    public ForceHardManagerOne theManagerOne;
    public ForceHardManagerTwo theManagerTwo;
    public ForceHardManagerThree theManagerThree;
    public ForceHardSimulation theSimulate;
    public PlayerContForcesMed thePlayer;
    public bool breakReady;
    public GameObject glassPrision, glassPrefab;
    public GameObject[] glassLoc;
    public AudioSource glassBreak,dragSfx,thud;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (theSimulate.stage == 1)
        {
            if (collision.gameObject.tag == "wall")
            {
                dragSfx.Stop();
                if (breakReady && theManagerOne.answerIsCorrect)
                {
                    glassBreak.Play();
                    glassPrision.SetActive(false);
                    GameObject glass = Instantiate(glassPrefab);
                    glass.transform.position = glassLoc[0].transform.position;
                    theSimulate.simulate = false;
                    thePlayer.push = false;
                   
                    StartCoroutine(theManagerOne.StuntResult());
                     breakReady = false;

                }
                if (breakReady && theManagerOne.answerIsMorethan)
                {
                    glassBreak.Play();
                    glassPrision.SetActive(false);
                    GameObject glass = Instantiate(glassPrefab);
                    glass.transform.position = glassLoc[0].transform.position;
                    
                    StartCoroutine(theManagerOne.overForce());
                    StartCoroutine(theManagerOne.StuntResult());
                    breakReady = false;

                }
                if (breakReady && theManagerOne.answerIsLessthan)
                {
                    thud.Play();
                    theSimulate.simulate = false;
                    thePlayer.push = false;
                    StartCoroutine(theManagerOne.StuntResult());
                    breakReady = false;

                }

            }
        }
        //  if (theSimulate.stage == 2)
        // {
        //     if (collision.gameObject.tag == "wall")
        //     {
        //         if (breakReady && theManagerTwo.answerIsCorrect == false)
        //         {
        //             theManagerTwo.wallGlass.SetActive(false);
        //             GameObject glass = Instantiate(glassPrefab);
        //             glass.transform.position = glassLoc[0].transform.position;
        //             theSimulate.simulate = false;
        //             theManagerTwo.boxSpeed = 0;
        //             thePlayer.pull = false;
        //             thePlayer.moveSpeed = 0;
        //             breakReady = false;
        //             StartCoroutine(theManagerTwo.StuntResult());

        //         }

        //     }
        // }
        if (theSimulate.stage == 3)
        {
            if (collision.gameObject.tag == "wall")
            {
                dragSfx.Stop();
                if (breakReady && theManagerThree.answerIsCorrect)
                {
                    glassBreak.Play();
                    glassPrision.SetActive(false);
                    GameObject glass = Instantiate(glassPrefab);
                    glass.transform.position = glassLoc[0].transform.position;
                    theSimulate.simulate = false;
                    thePlayer.push = false;
                    breakReady = false;
                    StartCoroutine(theManagerThree.StuntResult());

                }
                if (breakReady && theManagerThree.answerIsLessthan)
                {
                    glassBreak.Play();
                    glassPrision.SetActive(false);
                    GameObject glass = Instantiate(glassPrefab);
                    glass.transform.position = glassLoc[0].transform.position;
                    breakReady = false;
                    StartCoroutine(theManagerThree.overForce());
                    StartCoroutine(theManagerThree.StuntResult());

                }
                if (breakReady && theManagerThree.answerIsMorethan)
                {
                    thud.Play();
                    theSimulate.simulate = false;
                    thePlayer.push = false;
                    breakReady = false;
                    StartCoroutine(theManagerThree.StuntResult());

                }

            }
        }
        
    }
}
