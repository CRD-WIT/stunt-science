using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassManager : MonoBehaviour
{
    private ColliderManager theCollide;
    // Start is called before the first frame update
    void Start()
    {
        theCollide = FindObjectOfType<ColliderManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(theCollide.collide == true)
        {
            StartCoroutine(glassDestroy());
        }
    }
    IEnumerator glassDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);



    }
}
