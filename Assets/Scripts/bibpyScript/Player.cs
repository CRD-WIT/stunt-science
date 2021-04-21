using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;


    public GameObject player;
    public GameObject stickmanpoint;
    public bool lost;
    public bool happy;

    public bool ragdollblow;
    public AudioSource footstep;
    public GameObject stickprefab;
    float currentpos;
    public bool posready;
    public bool grounded;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundedRadius;
    private Collider2D myCollider;
    public EdgeCollider2D slideCollider;
    public float jumpforce;
    public bool standup, slide, isHanging;








    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //grounded = Physics2D.IsTouchingLayers(myCollider, whatIsGround);
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
        currentpos = player.transform.position.x;


        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        myAnimator.SetFloat("speed", myRigidbody.velocity.x);
        myAnimator.SetBool("lost", lost);
        myAnimator.SetBool("happy", happy);
        myAnimator.SetBool("grounded", grounded);
        myAnimator.SetBool("standup", standup);
        myAnimator.SetBool("slide", slide);
        myAnimator.SetBool("cranking",Level5EasyManager.cranked);
        myAnimator.SetBool("isHanging",isHanging);
        if (posready == true)
        {
            if (currentpos >= 0)
            {
                moveSpeed = 0;
                player.transform.position = new Vector3(0f, player.transform.position.y, player.transform.position.z);
                posready = false;


            }
        }
        if (happy == true)
        {
            StartCoroutine(happyOn());
        }
        if (slide)
        {
            StartCoroutine(Sliding());
        }




    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("stickmanspawn"))
        {

            ragdollblow = true;
        }

    }
    public void ragdollspawn()
    {



        player.SetActive(false);
        //GameObject stick = stickman.GetPooledObject();
        // stick.transform.position = stickmanpoint.transform.position;

        //stick.SetActive(true);

        GameObject stick = Instantiate(stickprefab);
        stick.transform.position = stickmanpoint.transform.position;
        ragdollblow = false;
    }
    public void startstunt()
    {
        StartCoroutine(stunt());
    }
    IEnumerator stunt()
    {
        yield return new WaitForSeconds(1);
        //ragdollspawn();
    }
    public void playfootstep()
    {
        // TODO: Fix sound
        //footstep.Play(0);
    }
    public void positioning()
    {

        moveSpeed = 4;
        posready = true;


    }

    public void SetEmotion(string emotion)
    {
        switch (emotion)
        {
            case "happy":
                this.happy = true;
                break;
            default:
                this.happy = false;
                break;
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("jumper"))
        {
            jump();

        }
        if (other.gameObject.tag == ("stickmanspawn"))
        {
            SimulationManager.playerDead = true;
            ragdollspawn();
            lost = false;
            standup = true;
        }
    }

    public void jump()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, jumpforce);
    }
    IEnumerator happyOn()
    {
        yield return new WaitForSeconds(1);
        happy = false;
    }
    IEnumerator Sliding()
    {
        yield return new WaitForSeconds(1f);
        slideCollider.enabled = true;
        myCollider.enabled =false;
        myRigidbody.mass = 0.00001f;
        myRigidbody.gravityScale = 90;
        yield return new WaitForSeconds(3.5f);
        myRigidbody.mass = 10;
        myRigidbody.gravityScale = 1;
        slideCollider.enabled =false;
        myCollider.enabled =true;
        slide = false;
    }



}
