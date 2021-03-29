using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class springtrap : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    public float verticalpush; 
    bool pushready;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       if (pushready = false)
       {
           StartCoroutine(pushreadycyc());
       }
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, verticalpush);
    }
    IEnumerator pump()
    {
        if (pushready == true)
        {
             verticalpush = 10;
             yield return new WaitForSeconds(1);
             verticalpush = 0;
             pushready = false;
        }
       
        
    }
    IEnumerator pushreadycyc()
    {
        pushready = true;
        yield return new WaitForSeconds(1);
        pushready = false;
    }
}
