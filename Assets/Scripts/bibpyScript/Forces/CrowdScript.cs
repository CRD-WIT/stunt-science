using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    private Animator myAnimator;
    public float moveSpeedX;
    private ForceSimulation theSimulate;
    public float moveSpeedY;
    public bool grounded, goDown, happy, move, goSafe, devour;
    public LayerMask whatIsGround;
     public Transform groundCheck;
     public float groundedRadius;
    private ForceManagerThree theManagerThree;
    private Collider2D myCollider;
    bool addSpeed = true;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        theManagerThree = FindObjectOfType<ForceManagerThree>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        myCollider = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
        myRigidbody.velocity = new Vector2(moveSpeedX, myRigidbody.velocity.y);
        myAnimator.SetFloat("speed", myRigidbody.velocity.x);
        myAnimator.SetBool("grounded", grounded);
        myAnimator.SetBool("godown", goDown);
        myAnimator.SetBool("happy", happy);
        myAnimator.SetBool("devour", devour);
        if(theSimulate.zombieChase)
        {
            if(addSpeed)
            {
                moveSpeedX = Random.Range(4, 5);
                addSpeed = false; 
            }            
        }
        if(theSimulate.zombieChase == false)
        {
            moveSpeedX = 0;
            addSpeed = true;
        }
        
        /*if (theManagerThree.crowdExit)
        {
            moveSpeedX = Random.Range(4f, 5f);
            if (transform.position.x >= 22.6)
            {
                moveSpeedX = 0;
                goDown = true;
                transform.position = new Vector2(22.8f, transform.position.y);
            }
        }
        if (goDown)
        {
            moveSpeedY = -8;
            if (transform.position.y <= -7.4f)
            {
                goDown = false;
                theManagerThree.crowdExit = false;
                moveSpeedY = 0;
                goSafe = true;
            }
        }
        if (goSafe)
        {
            moveSpeedX = Random.Range(3f, 4f);
            StartCoroutine(inSafe());
            goSafe = false;
        }*/
    }
    public void playfootstep()
    {
        // TODO: Fix sound
        //footstep.Play(0);
    }
    IEnumerator inSafe()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 2f));
        moveSpeedX = 0;
        yield return new WaitForSeconds(2);
        happy = true;
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("wall"))
        {
            devour = true;
            moveSpeedX = 0;
           
        }

    }

}
