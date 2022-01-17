using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prisonGlassManager : MonoBehaviour
{
    // Start is called before the first frame update
    private ForceHardSimulation theSimulate;
    void Start()
    {
        theSimulate = FindObjectOfType<ForceHardSimulation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(theSimulate.destroyGlass == true)
        {
            Destroy(gameObject);
        }
    }
}
