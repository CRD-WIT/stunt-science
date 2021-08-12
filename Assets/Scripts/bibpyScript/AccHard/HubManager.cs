using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    private HingeJoint2D myHinge;
    private Rigidbody2D myRb;
    public GameObject blastprefab;
    public TruckManager theTruck;
    public float moveSpeed;
    bool hit;
     public AccHardSimulation theSimulate;

    // Start is called before the first frame update
    void Start()
    {
        myHinge = GetComponent<HingeJoint2D>();
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
       
    }
    void OnTriggerEnter2D(Collider2D other)
    {
         if (theSimulate.stage == 1)
        {
            if (other.gameObject.tag == ("bullet"))
            {
                myHinge.enabled = false;
                GameObject explosion = Instantiate(blastprefab);
                explosion.transform.position = transform.position;
                
                
                //hit = true;
               StartCoroutine(bounce());
                theTruck.accelerating = false;
                //theTruck.moveSpeed = 10;

            }
        }
    }
    IEnumerator bounce()
    {
        myRb.velocity = transform.up * 10;
        yield return new WaitForSeconds(.1f);
        myRb.velocity = transform.right * 2;
    }
}
