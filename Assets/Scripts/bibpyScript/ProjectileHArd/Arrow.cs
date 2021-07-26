using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private ProjectileHardOne theManagerOne;
    Rigidbody2D rb;
    float mylastrotX, mylastrotY, angle, newAngle;
    bool getAngle;
    Quaternion lastRot;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        theManagerOne = FindObjectOfType<ProjectileHardOne>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!getAngle)
        {
            angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            lastRot = transform.rotation;
        }
        
        
    }
     void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.tag == ("wall"))
        {  
            getAngle = true;        
            rb.bodyType = RigidbodyType2D.Static;
            transform.rotation = lastRot;
        }
    }
}
