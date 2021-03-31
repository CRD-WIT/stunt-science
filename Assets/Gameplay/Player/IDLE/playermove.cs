using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playermove : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D myRigidbody;
    public float jumpForce;
    public float jumpTime;
    public float jumpTimeCounter;
    public bool stoppedJumping;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundCheckRadius;
    public bool grounded;
    public float runtimecounter;
    public bool selectA;
    public bool selectB;
    public bool selectC;
    public bool selectD;
    public GameObject checkpointA;
    public GameObject checkpointB;
    public GameObject checkpointC;
    public GameObject checkpointD;

    public Text seconds;
    // Start is called before the first frame update
    void Start()
    {
        
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        jump();
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (selectA == true)
        {
            seconds.text = "time:" + runtimecounter + " sec";
            checkpointA.SetActive(true);
            runtimecounter -= Time.deltaTime;
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
            if (runtimecounter <= 0)
            {
                runtimecounter = 0;
            }
         
            


        }
        if (selectB == true)
        {
            seconds.text = "time:" + runtimecounter + " sec";
            checkpointB.SetActive(true);
            runtimecounter -= Time.deltaTime;
                myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
            if (runtimecounter <= 0)
            {
                runtimecounter = 0;
            }
           

        }
        if (selectC == true)
        {
            seconds.text = "time:" + runtimecounter + " sec";
            checkpointC.SetActive(true);
            runtimecounter -= Time.deltaTime;
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
            if (runtimecounter <= 0)
            {
                runtimecounter = 0;
            }
          

        }
        if (selectD == true)
        {
            seconds.text = "time:" + runtimecounter + " sec";
            checkpointD.SetActive(true);
            runtimecounter -= Time.deltaTime;
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
            if (runtimecounter <= 0)
            {
                runtimecounter = 0;
            }
           

        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("checkpoints"))
        {
            moveSpeed = 0;

        }
    }

    void jump()
    {
        if (grounded)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
           
            
        }
    }
    public void A()
    {
        selectA = true;
        runtimecounter = 5.67f;
    }
    public void B()
    {
        selectB = true;
        runtimecounter = 6.67f;
    }
    public void C()
    {
        selectC = true;
        runtimecounter = 7.67f;
    }
    public void D()
    {
       selectD = true;
        runtimecounter = 8.67f;
    }

    void choiceA()
    {
        runtimecounter = 16;
    }
    void choiceB()
    {
        runtimecounter = 8;
    }
    void choicec()
    {
        runtimecounter = 10;
    }
    void choiceD()
    {
        runtimecounter = 12;
    }
    void choiceE()
    {
        runtimecounter = 14;
    }
}
