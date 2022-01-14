using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollisionManager : MonoBehaviour
{
    public ForceHardManagerOne theManagerOne;
    public ForceHardSimulation theSimulate;
    public PlayerContForcesMed thePlayer;
    public bool breakReady;
    public GameObject glassPrision, glassPrefab;
    public GameObject[] glassLoc;
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
        if (collision.gameObject.tag == "wall")
        {
            if (breakReady && theManagerOne.answerIsCorrect)
            {
                glassPrision.SetActive(false);
                GameObject glass = Instantiate(glassPrefab);
                glass.transform.position = glassLoc[0].transform.position;
                theSimulate.simulate = false;
                thePlayer.push = false;
                breakReady = false;
                StartCoroutine(theManagerOne.StuntResult());

            }
            if (breakReady && theManagerOne.answerIsMorethan)
            {
                glassPrision.SetActive(false);
                GameObject glass = Instantiate(glassPrefab);
                glass.transform.position = glassLoc[0].transform.position;
                breakReady = false;
                StartCoroutine(theManagerOne.overForce());
                StartCoroutine(theManagerOne.StuntResult());

            }
            if (breakReady && theManagerOne.answerIsLessthan)
            {
                theSimulate.simulate = false;
                thePlayer.push = false;
                breakReady = false;
                StartCoroutine(theManagerOne.StuntResult());

            }

        }
    }
}
