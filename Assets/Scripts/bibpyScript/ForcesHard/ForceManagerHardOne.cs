using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceManagerHardOne : MonoBehaviour
{
    public ForceHardSimulation theSimulate;
    public float accBoxOne, accBoxTwo, massBox, frictionForceOne, frictionForceTwo, normalForce, muOne, muTwo, distanceOne, distanceTwo, appliedForce, ViOne, VfOne, VfTwo, finalForceOne, finalForceTwo;
    public float timeOne, timeTwo, timeTotal, boxStartPos, boxCurrentPos, boxDistanceTravel;
    public float boxSpeed,angle;
    public GameObject box;
    public BoxManager theBox;
    public PlayerContForcesMed thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        normalForce = (massBox * 9.81f) * (Mathf.Cos(angle * Mathf.Deg2Rad));
        boxStartPos = box.transform.position.x;
        //showProblem();
    }

    // Update is called once per frame
    void Update()
    {
        boxCurrentPos = box.transform.position.x;
        boxDistanceTravel = boxStartPos - boxCurrentPos;
        if (theSimulate.simulate == true)
        {
            thePlayer.push = true;
            box.GetComponent<Rigidbody2D>().velocity = new Vector2(boxSpeed, box.GetComponent<Rigidbody2D>().velocity.y);
            if (boxDistanceTravel <= 5)
            {
                boxSpeed -= accBoxOne * Time.fixedDeltaTime;
                thePlayer.moveSpeed -= accBoxOne * Time.fixedDeltaTime;
            }
            else
            {
                boxSpeed -= accBoxTwo * Time.fixedDeltaTime;
                thePlayer.moveSpeed -= accBoxTwo * Time.fixedDeltaTime;
            }

        }
    }
    public void showProblem()
    {
        massBox = (float)System.Math.Round((Random.Range(45, 50f)), 2);
        muOne = (float)System.Math.Round((Random.Range(.6f, 0.65f)), 2);
        muTwo = (float)System.Math.Round((Random.Range(0.66f, 0.7f)), 2);
        appliedForce = Random.Range(350, 360);
        normalForce = massBox * 9.81f;
        frictionForceOne = normalForce * muOne;
        frictionForceTwo = normalForce * muTwo;
        finalForceOne = appliedForce - frictionForceOne;
        finalForceTwo = appliedForce - frictionForceTwo;
        accBoxOne = finalForceOne / massBox;
        accBoxTwo = finalForceTwo / massBox;
        VfOne = Mathf.Sqrt(ViOne * ViOne + 2 * accBoxOne * distanceOne);
        VfTwo = Mathf.Sqrt(VfOne * VfOne + 2 * accBoxTwo * distanceTwo);
        timeOne = (VfOne - ViOne) / accBoxOne;
        timeTwo = (VfTwo - VfOne) / accBoxTwo;
        timeTotal = timeOne + timeTwo;

    }
}
