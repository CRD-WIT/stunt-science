using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public bool isCollided;
    public bool isCollidedToFailCollider;
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("HookTarget"))
        {
            isCollided = true;
            transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            transform.GetComponent<Rigidbody2D>().Sleep();
        }

        if (collision.gameObject.tag == ("HookFailedCollider"))
        {
            isCollidedToFailCollider = true;
        }
    }
}
