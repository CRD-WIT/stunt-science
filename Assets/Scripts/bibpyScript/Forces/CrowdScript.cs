using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    private Animator myAnimator;
    public float moveSpeedX;
    public float moveSpeedY;
    public bool grounded, goDown;
    private ForceManagerThree theManagerThree;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        theManagerThree = FindObjectOfType<ForceManagerThree>();
        
    }

    // Update is called once per frame
    void Update()
    {
        grounded =true;
        myRigidbody.velocity = new Vector2(moveSpeedX, moveSpeedY);
        myAnimator.SetFloat("speed", myRigidbody.velocity.x);
        myAnimator.SetBool("grounded", grounded);
        myAnimator.SetBool("godown", goDown);
        
        if (transform.position.x >= 22.6)
        {
            moveSpeedX = 0;
            goDown = true;
            theManagerThree.crowdExit = false;
            transform.position = new Vector2(22.8f, transform.position.y);
        }
        if(theManagerThree.crowdExit == true)
        {
            moveSpeedX = 4;
        }
        if(goDown)
        {
            moveSpeedY = -8;
        }
    }
    public void playfootstep()
    {
        // TODO: Fix sound
        //footstep.Play(0);
    }
}
