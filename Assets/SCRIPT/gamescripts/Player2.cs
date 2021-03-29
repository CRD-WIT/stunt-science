using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    public Objectpooler stickman;
    public GameObject player;
    public GameObject stickmanpoint;
    public bool lost;
    public bool happy;
   
     public bool ragdollblow;
   
   

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        
        stickman = FindObjectOfType<Objectpooler>();
    }

    // Update is called once per frame
    void Update()
    {
       
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        myAnimator.SetFloat("speed", myRigidbody.velocity.x);
        myAnimator.SetBool("lost", lost);
        myAnimator.SetBool("happy", happy);
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
       GameObject stick = stickman.GetPooledObject();
        stick.transform.position = stickmanpoint.transform.position;
     
       stick.SetActive(true);
       ragdollblow = false;
       
           
        
            
    }


   
}
