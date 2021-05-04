using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellicopter : MonoBehaviour
{
    public float flySpeed;
    public AccMediumOne theManagerOne;
    public Rigidbody2D myRigidbody;
    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myRigidbody.velocity = new Vector2(flySpeed, myRigidbody.velocity.y);
        myAnimator.SetFloat("speed", myRigidbody.velocity.x);

    }
    void OnTriggerEnter2D(Collider2D other)
    {
         if (other.gameObject.tag == ("wall"))
        { 
            theManagerOne.chase = true;
        }
    }
}
