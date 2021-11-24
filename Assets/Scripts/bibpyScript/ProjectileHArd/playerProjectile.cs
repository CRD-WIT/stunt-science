using System.Collections;
using UnityEngine;

public class playerProjectile : MonoBehaviour
{
    private Animator myAnimator;
    public bool shooting, airdive, slash, aim, running, backward;
    public GameObject puller, mGear, sword, arrow;
    public ProjectileHardOne theManagerOne;
    public ProjectileHardTwo theManagerTwo;
    public ProjectileHardThree theManagerThree;
    public bool grounded;
    public ProjHardSimulation theSimulate;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundedRadius;
    public AudioSource maneuverGear, swordSlash;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
        myAnimator.SetBool("shooting", shooting);
        myAnimator.SetBool("airdive", airdive);
        myAnimator.SetBool("slash", slash);
        myAnimator.SetBool("aim", aim);
        myAnimator.SetBool("grounded", grounded);
        myAnimator.SetBool("running", running);
        myAnimator.SetBool("backward", backward);
    }
    void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.tag == ("wall2"))
        {
            maneuverGear.Stop();
            StartCoroutine(slowMo());
            sword.SetActive(true);
            slash = true;
            if (theSimulate.stage == 1)
            {
                //theManagerOne.generateAngle += 3;
                theManagerOne.reShoot();
            }
            if (theSimulate.stage == 2)
            {
                theManagerTwo.gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 160);
                theManagerTwo.reShoot();
            }
            if (theSimulate.stage == 3)
            {
                theManagerThree.gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 25);
                theManagerThree.reShoot();
            }

           



        }
    }
    IEnumerator slowMo()
    {
        Time.timeScale = .4f;
        yield return new WaitForSeconds(.7f);
        Time.timeScale = 1;
        arrow.GetComponent<LineRenderer>().enabled = false;
    }
    public void playfootstep()
    {
        // TODO: Fix sound
        //footstep.Play(0);
    }
    public void reShoot()
    {
        puller.GetComponent<Rigidbody2D>().velocity = transform.right * 10;
    }
}
