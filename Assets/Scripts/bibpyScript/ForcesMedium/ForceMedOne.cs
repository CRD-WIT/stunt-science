using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMedOne : MonoBehaviour
{
    public ForceMedSimulation theSimulate;
    public PlayerContForcesMed thePlayer;
    public float velocity,velocityFinal, accelerationPlayer,time, timer,totalDistance,massBox, forcePlayer, massPlayer,momentum;
    public float weightBox,timeInitial,accelerationBox;
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
            // forcePlayer = massPlayer * accelerationPlayer;
            // accelerationPlayer = forcePlayer/massPlayer;
            // velocityFinal = Mathf.Sqrt((velocity*velocity) + 2*accelerationPlayer*playerDistance );
            // momentum = massPlayer * velocityFinal;
            // VfPlayer = Mathf.Sqrt( (2*accelerationPlayer)*playerDistance);
            // momentum = forcePlayer*timeInitial;
            // weightBox = massBox * 9.81f;
            // timeInitial = (VfPlayer - velocity)/ accelerationPlayer;
            // forcePlayer = massPlayer * accelerationPlayer;
            // finalForce = momentum / timeInitial;
            // accelerationBox = forcePlayer / massBox;
            // boxVf = Mathf.Sqrt( (2*accelerationBox)*totalDistance);
            // boxTime = boxVf/accelerationBox;
            // momentumNeeded = massBox/accelerationPlayer;
            // timeMomentum = momentumNeeded/forcePlayer;
            // forceNeeded = massBox/accelerationFinal;
            weightBox = massBox * 9.81f;
            accelerationBox = ((2*totalDistance)/(timeInitial*timeInitial));
            forcePlayer = massPlayer * accelerationBox;
            accelerationPlayer = accelerationBox;
            
        }
        if(startRunning)
        {
            //thePlayer.moveSpeed += accelerationBox * Time.fixedDeltaTime;
        }
        if(theSimulate.simulate == true)
        {
             momentum = forcePlayer*timer;
            preset = false;
            if(timer <= 0)
            {
                thePlayer.moveSpeed = velocity;
            }
            timer += Time.fixedDeltaTime;
            //  thePlayer.moveSpeed = (time*forcePlayer) / massPlayer;
            //  velocity = thePlayer.moveSpeed;
            thePlayer.moveSpeed += accelerationBox * Time.fixedDeltaTime;
            if(timer >= timeInitial)
            {
                Time.timeScale = 0;
            }
            
        }
    }
}
