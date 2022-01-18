using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerManager : MonoBehaviour
{
    public ForceHardManagerOne theManagerOne;
    public float moveSpeed;
    public bool ragdollReady, runLeft, runRight;
    public Rigidbody2D rb;
    public Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        myAnimator.SetFloat("speed", moveSpeed);

        if (runLeft)
        {
            moveSpeed = 4;
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }

        if (runRight)
        {
            moveSpeed = 4;
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "box")
        {
            if (ragdollReady)
            {
                theManagerOne.ragdollSpawn();
                ragdollReady = false;
            }

        }
    }
    public IEnumerator startRun()
    {
        runLeft = true;
        yield return new WaitForSeconds(5);
        runLeft = false;
        moveSpeed = 0;
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }
}
