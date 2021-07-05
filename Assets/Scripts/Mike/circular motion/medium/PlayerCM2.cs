using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCM2 : MonoBehaviour
{
    public float moveSpeed, groundedRadius, jumpforce;
    private Rigidbody2D myRigidbody;
    public Animator myAnimator;
    public GameObject player, stickprefab, stickmanpoint;
    public bool lost, happy, ragdollblow, posready, grounded, standup, slide, isHanging, brake, isGrabbing, 
        climb, hangWalk, isFalling, toJump, jumpHang, isLanded;
    // public AudioSource footstep;
    float currentpos;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    private Collider2D myCollider, ragDollTrigger;
    HeartManager life;

    void Start()
    {
        myAnimator = this.GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>(); 
        life = FindObjectOfType<HeartManager>();
    }
    void Update()
    {
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
        myAnimator.SetBool("brake",brake);
        myAnimator.SetBool("isHanging",isHanging);
        myAnimator.SetBool("grab", isGrabbing);
        myAnimator.SetBool("hangWalk", hangWalk);
        myAnimator.SetBool("isFalling", isFalling);
        myAnimator.SetBool("toJump", toJump);
        myAnimator.SetBool("jumpHang", jumpHang);
        myAnimator.SetBool("landed", isLanded);
        myAnimator.SetBool("climb", climb);

        if (climb){
            myRigidbody.velocity = new Vector2  (0, 1);
        }
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
        stickprefab.SetActive(true);
        GameObject stick = stickprefab;
        stick.transform.position = stickmanpoint.transform.position;
        ragdollblow = false;
    }
    public void playfootstep()
    {
        // TODO: Fix sound
        // footstep.Play(0);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        life.ReduceLife();
        ragDollTrigger = other;
        other.enabled=false;
        if (other.gameObject.tag == ("jumper"))
        {
            jump();
        }
        if (other.gameObject.tag == ("stickmanspawn"))
        {
            ragdollspawn();
            RagdollV2.disableRagdoll = true;
            lost = false;
            standup = true;
        }
    }
    public void ToggleTrigger(){
        ragDollTrigger.enabled = true;
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
