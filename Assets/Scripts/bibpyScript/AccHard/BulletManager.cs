using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Rigidbody2D myRb;
     public GameObject blastprefab;
     public bool posCheck;
     private AccHardOne theManagerOne;

    // Start is called before the first frame update
    void Start()
    {
        theManagerOne = FindObjectOfType<AccHardOne>();
        myRb =FindObjectOfType<Rigidbody2D>();
        StartCoroutine(dissolve());
       
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= -2)
        {
            //Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("wall"))
        {
            theManagerOne.posCheck = true;
            Destroy(gameObject);
        }
        if (other.gameObject.tag == ("ground"))
        {
            theManagerOne.posCheck = true;
            GameObject explosion = Instantiate(blastprefab);
            explosion.transform.position = transform.position;
           
        }
    }
   
    IEnumerator stop()
    {
            yield return new WaitForSeconds(0.05f);
            myRb.velocity = new Vector2(0, 0);
    }
    IEnumerator dissolve()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
