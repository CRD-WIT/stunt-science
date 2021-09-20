using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public bool isCollided;
    public bool isCollidedToFailCollider;
    public Rigidbody2D rb;
    public GameObject target, hookLine;
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("HookTarget"))
        {
            isCollided = true;
            //rb.bodyType = RigidbodyType2D.Static;
            transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            transform.GetComponent<Rigidbody2D>().Sleep();
        }

        if (collision.gameObject.tag == ("HookFailedCollider"))
        {
            if (isCollidedToFailCollider == false)
            {
                target.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(ropePull());
                isCollidedToFailCollider = true;
            }

        }
    }
    public IEnumerator ropePull()
    {
        yield return new WaitForSeconds(2);
        rb.velocity = new Vector2(-15, rb.velocity.y);
        yield return new WaitForSeconds(1);
        hookLine.SetActive(false);

    }
}
