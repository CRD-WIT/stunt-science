using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMedium : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D myRigidbody;
    private Animator myAnimator;
    public bool push,zombieRun;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        myAnimator.SetFloat("zombieSpeed", moveSpeed);
        myAnimator.SetBool("push", push);
        myAnimator.SetBool("zombieRun", zombieRun);
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("moveLeft"))
        {
            moveSpeed = 0;
            zombieRun = false;
            Debug.Log("collide");
        }
        
    }
}
