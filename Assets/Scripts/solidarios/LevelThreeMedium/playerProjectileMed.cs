using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectileMed : MonoBehaviour
{
     private Animator myAnimator;
    public bool shooting, airdive, slash, aim, running, backward,jump;
    public bool grounded;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundedRadius;
    public GameObject  puller,hookline;
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
        myAnimator.SetBool("aim", aim);
        myAnimator.SetBool("grounded", grounded);
        myAnimator.SetBool("running", running);
        myAnimator.SetBool("backward", backward);
        myAnimator.SetBool("jump", jump);
        if(running)
        {
            puller.GetComponent<Rigidbody2D>().velocity = new Vector2(6,0);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.tag == ("wall2"))
        {
            jump = true;
            puller.GetComponent<Rigidbody2D>().velocity = transform.right * (9);
            hookline.SetActive(false);
            StartCoroutine(slowMo());
        }
    }
    IEnumerator slowMo()
    {
        //Time.timeScale = .4f;
        yield return new WaitForSeconds(.7f);
        //Time.timeScale = 1;
        running = true;
        
    }
}
