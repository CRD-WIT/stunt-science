using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDestroyer : MonoBehaviour
{
    public GameObject debriPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SimulationManager.destroyPrefab == true)
        {
            Destroy(debriPrefab.gameObject);
        }
        
    }
    IEnumerator resetPrefab()
    {
        Destroy(debriPrefab.gameObject);
        yield return new WaitForSeconds(0);
        SimulationManager.destroyPrefab = false;
    }
}
