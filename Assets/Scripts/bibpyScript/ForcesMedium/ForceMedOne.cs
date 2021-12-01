using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMedOne : MonoBehaviour
{
    public ForceMedSimulation theSimulate;
    public PlayerContForcesMed thePlayer;
    public float velocity,velocityFinal, accelerationPlayer,accelerationFinal,time, timer,totalDistance,playerDistance,massBox, force, massPlayer,momentum;
    public float weightBox,finalForce,timeInitial,accelerationBox, boxVf,boxTime,timeMomentum,momentumNeeded,forceNeeded,VfPlayer;
    public bool preset,startRunning;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(preset)
        {
            // accelerationFinal = totalDistance / (time*time);
            // force = massPlayer * accelerationPlayer;
            // accelerationPlayer = force/massPlayer;
            // velocityFinal = Mathf.Sqrt((velocity*velocity) + 2*accelerationPlayer*playerDistance );
            // momentum = massPlayer * velocityFinal;
            // VfPlayer = Mathf.Sqrt( (2*accelerationPlayer)*playerDistance);
            // momentum = force*timeInitial;
            // weightBox = massBox * 9.81f;
            // timeInitial = (VfPlayer - velocity)/ accelerationPlayer;
            // force = massPlayer * accelerationPlayer;
            // finalForce = momentum / timeInitial;
            // accelerationBox = force / massBox;
            // boxVf = Mathf.Sqrt( (2*accelerationBox)*totalDistance);
            // boxTime = boxVf/accelerationBox;
            // momentumNeeded = massBox/accelerationPlayer;
            // timeMomentum = momentumNeeded/force;
            // forceNeeded = massBox/accelerationFinal;
            weightBox = massBox * 9.81f;
            accelerationBox = ((2*totalDistance)/(timeInitial*timeInitial));
            force = massBox * accelerationBox;
            accelerationPlayer = force/massPlayer;
            
        }
        if(startRunning)
        {
            //thePlayer.moveSpeed += accelerationBox * Time.fixedDeltaTime;
        }
        if(theSimulate.simulate == true)
        {
             momentum = force*timer;
            preset = false;
            if(timer <= 0)
            {
                thePlayer.moveSpeed = velocity;
            }
            timer += Time.fixedDeltaTime;
            //  thePlayer.moveSpeed = (time*force) / massPlayer;
            //  velocity = thePlayer.moveSpeed;
            thePlayer.moveSpeed += accelerationPlayer * Time.fixedDeltaTime;
            if(timer >= timeInitial)
            {
                Time.timeScale = 0;
            }
            
        }
    }
}
