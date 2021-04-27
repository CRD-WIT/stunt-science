using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassManager : MonoBehaviour
{
    private ColliderManager theCollide;
    private ForceSimulation theSimulate;
    // Start is called before the first frame update
    void Start()
    {
        theCollide = FindObjectOfType<ColliderManager>();
        theSimulate = FindObjectOfType<ForceSimulation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (theSimulate.stage < 3)
        {
            if (theCollide.collide == true)
            {
                StartCoroutine(glassDestroy());
            }
        }
        if (theSimulate.stage == 3)
        {
            if (theCollide.collide == true)
            {
                StartCoroutine(glassDestroy2());
            }
        }
    }
    IEnumerator glassDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);



    }
    IEnumerator glassDestroy2()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);



    }
}
