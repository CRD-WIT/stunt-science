using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    private ForceSimulation theSimulate;
    // Start is called before the first frame update
    void Start()
    {
        theSimulate = FindObjectOfType<ForceSimulation>();
        if(theSimulate.stage == 1 || theSimulate.stage == 3)
        {
            GetComponent<Rigidbody2D>().velocity = transform.right * Random.Range(15, 25);
        }
         if(theSimulate.stage == 2)
        {
            GetComponent<Rigidbody2D>().velocity = transform.right *  Random.Range(-15, -25);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
