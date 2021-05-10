using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccMediumThree : MonoBehaviour
{
    public GameObject edge;
    public SubSuv theSubVan;
    public SubHellicopter theSubChopper;
    public Suv theSuv;
    public Player thePlayer;
    public Hellicopter theChopper;
    float accH, accV, velocity, dv, dx, dh = 40;
    float suvPos, generateDv, generateVelocity, generateAccH, generateAccV;
    bool repos;
    // Start is called before the first frame update
    void Start()
    {
        generateProblem();
        

    }

    // Update is called once per frame
    void Update()
    {
        thePlayer.myRigidbody.mass = 0;
        suvPos = theSuv.transform.position.x;
        if (AccMidSimulation.simulate == true)
        {
            if (repos)
            {
                theChopper.flySpeed = -velocity;
                theSuv.moveSpeed = -velocity;
                if (suvPos <= theSubVan.transform.position.x)
                {
                    theChopper.deaccelaration = true;
                    theSuv.accelarating = true;
                    theChopper.accelaration = accH;
                    theSuv.accelaration = accV;
                    repos = false;
                }
            }
            if(suvPos <= 18)
            {
                
            }
        }

    }
    public void generateProblem()
    {
        repos =true;
        generateDv = Random.Range(25f, 28f);
        generateVelocity = Random.Range(5f, 7f);
        generateAccH = Random.Range(6f, 9f);
        generateAccV = Random.Range(1f, 3f);
        accH = (float)System.Math.Round(generateAccH, 2);
        accV = (float)System.Math.Round(generateAccV, 2);
        dv = (float)System.Math.Round(generateDv, 2);
        velocity = (float)System.Math.Round(generateVelocity, 2);
        dx = dh - dv;
        theSubVan.transform.position = new Vector2(edge.transform.position.x + dv, theSubVan.transform.position.y);
        theSubChopper.transform.position = new Vector2(theSubVan.transform.position.x + dx, theSubChopper.transform.position.y);
        theChopper.transform.position = new Vector2(theSuv.transform.position.x + dx, theChopper.transform.position.y);
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);


    }
}
