using System.Collections;
using UnityEngine;

public class GlassManager : MonoBehaviour
{
    private ColliderManager theCollide;
    private ForceSimulation theSimulate;
    private ForceManagerThree theManagerThree;
    void Start()
    {
        theCollide = FindObjectOfType<ColliderManager>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        theManagerThree = FindObjectOfType<ForceManagerThree>();
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
        if (theManagerThree.playerAnswer != theManagerThree.correctAnswer)
        {
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
        if (theManagerThree.playerAnswer == theManagerThree.correctAnswer)
        {
            yield return new WaitForSeconds(20);
            Destroy(gameObject);
        }



    }
}
