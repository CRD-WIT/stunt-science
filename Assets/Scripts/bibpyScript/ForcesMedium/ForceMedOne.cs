using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMedOne : MonoBehaviour
{
    public ForceMedSimulation theSimulate;
    public PlayerContForcesMed thePlayer;
    public float velocity,velocityFinal, accelerationPlayer,accelerationFinal,time, timer,totalDistance,playerDistance,massBox, force, massPlayer,momentum;
    public float weightBox,finalForce,timeInitial;
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
            // force = massBox * accelerationFinal;
            // accelerationPlayer = force/massPlayer;
            velocityFinal = Mathf.Sqrt((velocity*velocity) + 2*accelerationPlayer*playerDistance );
            momentum = massPlayer * velocityFinal;
            //momentum = force*timeInitial;
            weightBox = massBox * 9.81f;
            timeInitial = (velocityFinal - velocity)/ accelerationPlayer;
            force = massPlayer * accelerationPlayer;
            finalForce = momentum / timeInitial;
        }
        if(startRunning)
        {
            thePlayer.moveSpeed += accelerationPlayer * Time.fixedDeltaTime;
        }
        if(theSimulate.simulate == true)
        {
            preset = false;
            if(timer <= 0)
            {
                thePlayer.moveSpeed = velocity;
            }
            timer += Time.fixedDeltaTime;
            //  thePlayer.moveSpeed = (time*force) / massPlayer;
            //  velocity = thePlayer.moveSpeed;
            thePlayer.moveSpeed += accelerationPlayer * Time.fixedDeltaTime;
            if(timer >= time)
            {
                //Time.timeScale = 0;
            }
            
        }
    }
}
