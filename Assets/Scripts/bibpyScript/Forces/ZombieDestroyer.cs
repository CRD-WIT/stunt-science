using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDestroyer : MonoBehaviour
{
    private ForceSimulation theSimulate;
    // Start is called before the first frame update
    void Start()
    {
        theSimulate = FindObjectOfType<ForceSimulation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(theSimulate.destroyZombies)
        {
            Destroy(gameObject);
        }
           
       
    }
}
