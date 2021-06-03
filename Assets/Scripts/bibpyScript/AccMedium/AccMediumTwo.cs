using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccMediumTwo : MonoBehaviour
{
    public GameObject hangingRagdoll1, hangingRagdoll2, ropeTip1, ropeTip2;
    public Player thePlayer;
    public Hellicopter theChopper;
    public SubHellicopter theSubChopper;
    public SubSuv theSubSuv;
    public Suv theVan;
    float A, B, C, D;
    float generateViH, Vih, generateAccH, accH, generateViV, Viv, generateAccV, accV, generateDistance, distance,checkDistance;
    float chopperCurrentPos, vanCurrentPos, kickpointTimeA, kickpointTimeB, timer,kickDistance, kickpointTimeC, playerKickDistance;
    float chopperAccPos, vanAccPos,vanTime, chopperDistance;
    bool reposition = true;
    public QuestionController theQuestion;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
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
        checkDistance = ((Vih*kickpointTimeA)+ ((accH*(kickpointTimeA * kickpointTimeA))/2)) + ((Viv*kickpointTimeA)+ ((accV*(kickpointTimeA * kickpointTimeA))/2));
        kickDistance = ((Vih*kickpointTimeA)+((accH*(kickpointTimeA * kickpointTimeA))/2)) + chopperAccPos;
        playerKickDistance = AccMidSimulation.playerAnswer + chopperAccPos;

        
        if (AccMidSimulation.simulate == true)
        {
            if (reposition)
            {
                theChopper.flySpeed = Vih;
                theVan.moveSpeed = -Viv;
                if(chopperCurrentPos >= chopperAccPos & vanCurrentPos <= vanAccPos)
                {
                    theChopper.accelaration = accH;
                    theVan.accelaration = accV;
                    theChopper.accelarating = true;
                    theVan.accelarating = true;
                    reposition = false;
                    theSubChopper.fade = true;
                    theSubSuv.fade = true;
                    
                    
                    
                }
            }
            if(reposition == false)
            {
                timer += Time.fixedDeltaTime;
                if(chopperCurrentPos >= playerKickDistance)
                {
                   
                   
                }

            }

        }
    }
    public void generateProblem()
    {
        generateViH = Random.Range(5, 6);
        Vih = (float)System.Math.Round(generateViH, 2);
        generateAccH = Random.Range(3, 5);
        accH = (float)System.Math.Round(generateAccH, 2);
        generateViV = Random.Range(7, 8);
        Viv = (float)System.Math.Round(generateViV, 2);
        generateAccV = Random.Range(3, 4);
        accV = (float)System.Math.Round(generateAccV, 2);
        generateDistance = Random.Range(38, 40);
        distance = (float)System.Math.Round(generateDistance, 2);
        B = ((accH + accV)/2);
        A = -(Vih + Viv);
        C = -distance;
        D = Mathf.Sqrt((B*B) - (4*A*C));
        //kickpointTime = Mathf.Abs(((accH* (kickpointTime * kickpointTime))/(2*Vih)) - (distance/Vih)); 
        //kickpointTime = (-((accH + accV)/2) + (Mathf.Sqrt((((accH + accV)/2)*((accH + accV)/2)) -(4 * (Vih + Viv))*(-distance)))) / (2*(Vih + Viv));
        kickpointTimeA =(A + Mathf.Sqrt((A*A) - (4*B*C)))/ (2*B);
        kickpointTimeB =(B - Mathf.Sqrt((B*B) - (4*A*C)))/ (2*A);
        //kickpointTimeC = (kickpointTimeA - kickpointTimeB)/ 2;
        //kickpointTime = (((distance/2)/(accH+Vih)) + ((distance/2)/(accV+Viv))) / 2;
        //kickpointTime = distance/((accH+Vih) + (accV + Viv));
        vanTime = 30/Viv;
        chopperDistance = Vih * vanTime;
        chopperAccPos = 20-(distance/2);
        vanAccPos = 20+(distance/2);
        theChopper.transform.position = new Vector2(chopperAccPos-chopperDistance, theChopper.transform.position.y);
        theVan.transform.position = new Vector2(20 + (distance/2 + 30), theVan.transform.position.y); 
        theSubChopper.transform.position = new Vector2(chopperAccPos,  theSubChopper.transform.position.y);
        theSubSuv.transform.position = new Vector2(vanAccPos,theSubSuv.transform.position.y);



    }
     public IEnumerator errorMesage()
    {
        theQuestion.popupVisible = true;
        yield return new WaitForSeconds(3);
        theQuestion.popupVisible = false;
    }
}
