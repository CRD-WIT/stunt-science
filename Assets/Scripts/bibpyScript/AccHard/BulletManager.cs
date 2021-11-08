using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Rigidbody2D myRb;
    public GameObject blastprefab;
    public bool posCheck;
    private AccHardSimulation theSimulate;
    

    // Start is called before the first frame update
    void Start()
    {
        theSimulate = FindObjectOfType<AccHardSimulation>();
        myRb = FindObjectOfType<Rigidbody2D>();
        StartCoroutine(dissolve());

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -2)
        {
            //Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.tag == ("wall"))
        {
            if (theSimulate.stage == 1)
            {
                theSimulate.indicator.SetActive(true);
                theSimulate.indicator.transform.position = transform.position;
                theSimulate.posCheck = true;
                Destroy(gameObject);
            }

        }




        if (other.gameObject.tag == ("wall2"))
        {
            if (theSimulate.stage == 2)
            {
                theSimulate.indicator.SetActive(true);
                theSimulate.indicator.transform.position = transform.position;
                theSimulate.posCheck = true;
                Destroy(gameObject);
            }

        }
        if (other.gameObject.tag == ("wall3"))
        {
            if (theSimulate.stage == 3)
            {
                theSimulate.indicator.SetActive(true);
                theSimulate.indicator.transform.position = transform.position;
                theSimulate.posCheck = true;
                Destroy(gameObject);
            }

        }



        if (other.gameObject.tag == ("ground"))
        {
            theSimulate.indicator.SetActive(true);
            theSimulate.indicator.transform.position = transform.position;
            theSimulate.posCheck = true;
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
