using System.Collections;
using UnityEngine;

public class HubManagerThree : MonoBehaviour
{
     private HingeJoint2D myHinge;
    private Rigidbody2D myRb;
    public GameObject blastprefab;
    public TruckManager theTruck;
    public float moveSpeed;
    bool hit;
    //public GameObject hinge;
    public AccHardSimulation theSimulate;
    public AudioSource explosionSfx;


    // Start is called before the first frame update
    void Start()
    {
        myHinge = GetComponent<HingeJoint2D>();
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            myRb.velocity = new Vector2(moveSpeed, myRb.velocity.y);
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (theSimulate.stage == 3)
        {
            if (other.gameObject.tag == ("bullet"))
            {
                explosionSfx.Play();
                myHinge.enabled = false;
                GameObject explosion = Instantiate(blastprefab);
                explosion.transform.position = transform.position;
               
                //hinge.SetActive(false);
                //hit = true;
                StartCoroutine(bounce());
                theTruck.accelerating = false;
                //theTruck.moveSpeed = 10;


            }
        }

    }
    IEnumerator bounce()
    {
        myRb.velocity = transform.up * 10;
        yield return new WaitForSeconds(.1f);
        myRb.velocity = transform.right * 5;

    }
}
