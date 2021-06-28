using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDestroyer : MonoBehaviour
{
    public GameObject debriPrefab;
    private SimulationManager theSimulate;
    public static bool end;
    // Start is called before the first frame update
    void Start()
    {
        theSimulate = FindObjectOfType<SimulationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(theSimulate.destroyPrefab == true)
        {
            Destroy(debriPrefab.gameObject);
        }
        
    }
    
}
