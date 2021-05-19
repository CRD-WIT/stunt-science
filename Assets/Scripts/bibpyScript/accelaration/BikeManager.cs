using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeManager : MonoBehaviour
{
    private Player thePlayer;
    private AccManagerTwo theManagerTwo;
    private accSimulation theSimulate;
    private Animator myAnimator;
    public bool collided;
    public GameObject driverPrefab;

    public float moveSpeed;
    public Rigidbody2D myRigidbody;
    public Collider2D myCollider;
    public GameObject driverStickman;
    public GameObject stickprefab;
    public GameObject stickmanpoint;
    public bool stopBackward;
    public bool stopForward;
    public bool brake;
    public GameObject afterStuntMessage;
    private HeartManager theHeart;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        theSimulate = FindObjectOfType<accSimulation>();
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        theHeart = FindObjectOfType<HeartManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        theManagerTwo = FindObjectOfType<AccManagerTwo>(); 
        myAnimator.SetFloat("speed", myRigidbody.velocity.x);

        if (stopBackward || stopForward)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y);
        }
        else
        {
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("wall"))
        {
            if (collided == false)
            {

                if (theSimulate.stage < 3)
                {
                    StartCoroutine(StuntResult());
                    stopBackward = true;
                }
                if (theSimulate.stage == 3)
                {
                    stopForward = true;
                }
                driverspawn();
                thePlayer.standup = true;
                //accSimulation.playerDead = true;
                collided = true;
                thePlayer.standup = true;
                driverPrefab.SetActive(false);
                driverStickman.SetActive(false);
            }
        }
        if (other.gameObject.tag == ("braker"))
        {
            brake = true;
            if(theSimulate.stage == 2)
            {
                theManagerTwo.follow = true;
            }
        }
        if (other.gameObject.tag == ("water"))
        {
            StartCoroutine(disablecol());
        }


    }

    public void driverspawn()
    {

        GameObject stick = Instantiate(stickprefab);
        stick.transform.position = stickmanpoint.transform.position;
        stick.transform.rotation = stickmanpoint.transform.rotation;
    }
    IEnumerator StuntResult()
    {
        theHeart.losinglife();
        StartCoroutine(theSimulate.DirectorsCall());
        yield return new WaitForSeconds(2);
        afterStuntMessage.SetActive(true);
    }
    IEnumerator disablecol()
    {
        myCollider.enabled = false;
        yield return new WaitForSeconds(1);
        myCollider.enabled = true;
    }

}
