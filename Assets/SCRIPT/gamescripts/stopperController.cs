using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopperController : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D myRigidbody;
    private Vector2 startpoint;
    public GameObject StopperCont;
    // Start is called before the first frame update
    void Start()
    {
         myRigidbody = GetComponent<Rigidbody2D>();
         startpoint = StopperCont.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
         //myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);

    }
    public void stopperReset()
    {
        StopperCont.transform.position = startpoint;
        moveSpeed = 0;
    }
}
