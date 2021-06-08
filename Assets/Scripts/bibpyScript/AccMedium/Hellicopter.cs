using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellicopter : MonoBehaviour
{
    public float flySpeed, flyUp;
    public AccMediumOne theManagerOne;
    public SubHellicopter theSubChopper;
    public Rigidbody2D myRigidbody;
    private Animator myAnimator;
    public bool accelarating, deaccelaration;
    public float accelaration;



    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myRigidbody.velocity = new Vector2(flySpeed, flyUp);
        myAnimator.SetFloat("speed", myRigidbody.velocity.x);

        if (accelarating)
        {
            flySpeed += accelaration * Time.fixedDeltaTime;
        }
        if (deaccelaration)
        {
            flySpeed -= accelaration * Time.fixedDeltaTime;
        }
       


    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("wall"))
        {
            theManagerOne.chase = true;
            theManagerOne.timeOn = true;
            theSubChopper.fade = true;
        }
    }
   
}
