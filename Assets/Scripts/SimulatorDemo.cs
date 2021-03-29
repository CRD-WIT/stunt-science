using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatorDemo : MonoBehaviour
{
    public Animator debriStopper;

    // Update is called once per frame
    public void StartSimulation()
    {
        debriStopper.SetBool("isRunning", true);
    }
}
