using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class golem : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D myRigidbody;
    private Animator myAnimator;
    public ProjSimulationManager theSimulate;
    public bool throwing;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(theSimulate.stage == 1)
        {
            myRigidbody.velocity = new Vector2(-moveSpeed, myRigidbody.velocity.y);
        }
       
        myAnimator.SetFloat("speed", moveSpeed);
        myAnimator.SetBool("throw", throwing);
    }
}
