using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suv : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float moveSpeed, accelaration;
    public bool accelarating;
    public bool deaccelarating;
    // Start is called before the first frame update
    void Start()
    {
         myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        if(accelarating)
        {
            moveSpeed -= accelaration * Time.fixedDeltaTime;
        }
       
    }
}
