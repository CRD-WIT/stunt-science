using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    private Animator myAnimator;
    public float moveSpeedX;
    public float moveSpeedY;
    public bool grounded, goDown, happy, move, goSafe;
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
        grounded = true;
        myRigidbody.velocity = new Vector2(moveSpeedX, moveSpeedY);
        myAnimator.SetFloat("speed", myRigidbody.velocity.x);
        myAnimator.SetBool("grounded", grounded);
        myAnimator.SetBool("godown", goDown);
        myAnimator.SetBool("happy", happy);
        if (theManagerThree.crowdExit)
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
        }
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

}
