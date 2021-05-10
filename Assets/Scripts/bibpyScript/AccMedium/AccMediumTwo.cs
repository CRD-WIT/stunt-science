using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccMediumTwo : MonoBehaviour
{
    public GameObject hangingRagdoll1, hangingRagdoll2, ropeTip1, ropeTip2;
    public Player thePlayer;
    public Hellicopter theChopper;
    public Suv theVan;
    float A, B, C;
    float generateViH, Vih, generateAccH, accH, generateViV, Viv, generateAccV, accV, generateDistance, distance;
    float chopperCurrentPos, vanCurrentPos, chopperAccPoint, vanAccPoint, kickpointTime, timer,kickDistance;
    bool reposition = true;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer.ropeHang = true;
        generateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        chopperCurrentPos = theChopper.transform.position.x;
        vanCurrentPos = theVan.transform.position.x;
        hangingRagdoll1.transform.position = ropeTip1.transform.position;
        hangingRagdoll2.transform.position = ropeTip2.transform.position;
        chopperAccPoint = -5f;
        vanAccPoint = distance - 5f;
        
        
        if (AccMidSimulation.simulate == true)
        {
            if (reposition)
            {
                theChopper.flySpeed = 5;
                theVan.moveSpeed = -5;
                if(chopperCurrentPos >= 30-(distance/2) & vanCurrentPos <= 30+(distance/2))
                {
                    theChopper.flySpeed = Vih;
                    theVan.moveSpeed = -Viv;
                    theChopper.accelaration = accH;
                    theVan.accelaration = accV;
                    theChopper.accelarating = true;
                    theVan.accelarating = true;
                    reposition = false;
                    
                    
                    
                }
            }
            if(reposition == false)
            {
                timer += Time.fixedDeltaTime;
                if(timer >= kickpointTime)
                {
                    Time.timeScale = 0;
                    AccMidSimulation.simulate = false;
                    /*AccMidSimulation.simulate = false;
                    theChopper.accelarating = false;
                    theVan.accelarating = false;
                    theVan.moveSpeed = 0;
                    theChopper.flySpeed = 0;*/
                }

            }

        }
    }
    public void generateProblem()
    {
        generateViH = Random.Range(5f, 6f);
        Vih = (float)System.Math.Round(generateViH, 2);
        generateAccH = Random.Range(1f, 2f);
        accH = (float)System.Math.Round(generateAccH, 2);
        generateViV = Random.Range(7f, 8f);
        Viv = (float)System.Math.Round(generateViV, 2);
        generateAccV = Random.Range(3f, 4f);
        accV = (float)System.Math.Round(generateAccV, 2);
        generateDistance = Random.Range(38f, 40f);
        distance = (float)System.Math.Round(generateDistance, 2);
        theChopper.transform.position = new Vector2(30 - (distance/2 + 30), theChopper.transform.position.y);
        theVan.transform.position = new Vector2(30 + (distance/2 + 30), theVan.transform.position.y);
        B = ((accH + accV)/ 2);
        A = Vih + Viv;
        C = -distance;
        //kickpointTime = Mathf.Abs(((accH* (kickpointTime * kickpointTime))/(2*Vih)) - (distance/Vih)); 
        kickpointTime = (-((accH + accV)/2) + (Mathf.Sqrt((((accH + accV)/2)*((accH + accV)/2)) -(4 * (Vih + Viv))*(-distance)))) / (2*(Vih + Viv));
        kickpointTime =(-B + Mathf.Sqrt((B*B) - (4*A*C)))/ 2*A;
        //kickDistance = (Vih * kickpointTime) + (accH * ((kickpointTime*kickpointTime)/2));
        
        //kickpointTime = (((distance/2)/(accH+Vih)) + ((distance/2)/(accV+Viv))) / 2;
        //kickpointTime = distance/((accH+Vih) + (accV + Viv)); 


    }
}
