using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerManager : MonoBehaviour
{
    public ForceHardManagerOne theManagerOne;
    public bool ragdollReady;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "box")
        {
            if(ragdollReady)
            {
                theManagerOne.ragdollSpawn();
                ragdollReady = false;
            }
            
        }
    }
}
