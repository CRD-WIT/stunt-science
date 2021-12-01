using System.Collections;
using UnityEngine;

public class CrowdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    private Animator myAnimator;
    public int moveSpeedX;
    private ForceSimulation theSimulate;
    public float moveSpeedY;
    public bool grounded, goDown, happy, move, goSafe, devour;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundedRadius;
    private ForceManagerThree theManagerThree;
    private Collider2D myCollider;
    private PlayerB thePlayer;
    bool addSpeed = true, ragdollReady = true;
    public GameObject zombieRagdoll;
    public Transform stickmanpoint;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        theManagerThree = FindObjectOfType<ForceManagerThree>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        myCollider = GetComponent<Collider2D>();
        thePlayer = FindObjectOfType<PlayerB>();

    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
        myRigidbody.velocity = new Vector2(moveSpeedX, myRigidbody.velocity.y);
        myAnimator.SetInteger("zombieSpeed", moveSpeedX);
        myAnimator.SetBool("grounded", grounded);
        myAnimator.SetBool("godown", goDown);
        myAnimator.SetBool("happy", happy);
        myAnimator.SetBool("devour", devour);
        if (theSimulate.zombieChase)
        {
            if (addSpeed)
            {
                StartCoroutine(adddingSpeed());
                addSpeed = false;
            }
        }
        if (theSimulate.zombieChase == false)
        {
            moveSpeedX = 0;
            addSpeed = true;
        }
        if (theSimulate.destroyZombies)
        {
            Destroy(gameObject);
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
            moveSpeedX = 0;
            if (ragdollReady)
            {
                ragdollSpawn();
                Destroy(gameObject);
            }

        }

    }
    IEnumerator adddingSpeed()
    {
        if (theSimulate.stage == 1)
        {
            yield return new WaitForSeconds(1);
            moveSpeedX = Random.Range(5, 7);
        }
        if (theSimulate.stage == 2)
        {
            yield return new WaitForSeconds(1);
            if (ForceSimulation.playerAnswer < 2)
            {
                moveSpeedX = Random.Range(-2, -4);
            }
            if (ForceSimulation.playerAnswer >= 2)
            {
                moveSpeedX = Random.Range(-5, -7);
            }
        }
        if (theSimulate.stage == 3)
        {
            yield return new WaitForSeconds(1);
            moveSpeedX = Random.Range(5, 7);
        }

    }
    public void ragdollSpawn()
    {
        if (theSimulate.stage == 1 || theSimulate.stage == 3)
        {
            GameObject stick = Instantiate(zombieRagdoll);
            stick.transform.position = stickmanpoint.transform.position;
            ragdollReady = false;
        }
         if (theSimulate.stage == 2)
        {
            GameObject stick = Instantiate(zombieRagdoll);
            stick.transform.position = stickmanpoint.transform.position;
            stick.transform.localScale = new Vector2(-stick.transform.localScale.x, stick.transform.localScale.y);
            ragdollReady = false;
        }
    }


}
