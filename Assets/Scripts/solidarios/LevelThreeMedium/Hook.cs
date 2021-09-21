using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public bool isCollided, isTrailing, getAngle;
    public bool isCollidedToFailCollider;
    public Rigidbody2D rb;
    Quaternion lastRot;
     float

            mylastrotX,
            mylastrotY,
            angle,
            newAngle;
    public GameObject target, hookLine,rope,trail;
    // Start is called before the first frame update
    void FixedUpdate() 
    {
         if (!getAngle)
        {
            angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            lastRot = transform.rotation;
        }
        if(isTrailing)
        {
            trail.transform.position = transform.position;
        }
    }
        
    
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
                isTrailing = false;
                getAngle = true;
               
            }

        }
    }
    public IEnumerator ropePull()
    {
        //yield return new WaitForSeconds(.7f);
        hookLine.SetActive(false);
        rope.SetActive(true);
        yield return new WaitForSeconds(.7f);
        //rope.SetActive(false);
       

    }
}
