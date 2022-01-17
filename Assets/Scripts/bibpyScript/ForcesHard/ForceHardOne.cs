using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceHardOne : MonoBehaviour
{
     public ForceHardSimulation theSimulate;
      public PlayerContForcesMed thePlayer;
      public float breakingForce, downhillForce,frictionForce, downhillResultantForce,normalForce,appliedForce,boxMass,mu,angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showProblem()
    {
        normalForce = (boxMass * 9.81f) * (Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
