using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpChar : MonoBehaviour
{
    public GameObject ragdoll,
        ragdollSpantPnt;
    public float velocity;
    public bool hanging = false;
    bool hangWalk = true;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hanging)
        {
            if (hangWalk)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                StartCoroutine(ToStatic());
            }
            else
                rb.velocity = new Vector2(0.7f, 0);
        }
        else
            rb.velocity = new Vector2(velocity, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("stickmanspawn"))
        {
            ragdollspawn();
            RagdollV2.disableRagdoll = true;
        }
    }

    public void ragdollspawn()
    {
        this.gameObject.SetActive(false);
        ragdoll.SetActive(true);
        GameObject stick = ragdoll;
        stick.transform.position = ragdollSpantPnt.transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("stickmanspawn"))
        {
            ragdollspawn();
            RagdollV2.disableRagdoll = true;
        }
    }

    IEnumerator ToStatic()
    {
        yield return new WaitForSeconds(0.025f);
        hangWalk = false;
        // rb.bodyType = RigidbodyType2D.Static;
        rb.mass = 0;
        rb.gravityScale = 0;
    }
}
