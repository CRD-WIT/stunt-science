using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D myRigidbody;
    private Animator myAnimator;

    
    public GameObject player;
    public GameObject stickmanpoint;
    public bool lost, brake;
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
     public float jumpforce;
     public bool standup;
     
     
    
     
     
    
  
   
   

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
        myAnimator.SetBool("brake", brake);
        if(brake == true)
        {
            brake = false;
        }
        
        if (posready == true)
        {
            if(currentpos >= 0)
            {
                moveSpeed = 0;
                player.transform.position = new Vector3(0f, player.transform.position.y, player.transform.position.z);
                posready = false;
                
                 
            }
        }
        if(happy == true)
        {
            StartCoroutine(happyOn());
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
    public void driverspawn()
    {
        
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

    public void SetEmotion(string emotion){
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
    


   
}
