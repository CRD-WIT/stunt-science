using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{
    Vector3 direction;
    Vector3 target;
     private Rigidbody2D myRigidbody;
     float speed = 3;
    // Start is called before the first frame update
    void Start()
    {
         myRigidbody = GetComponent<Rigidbody2D>();
        target = new Vector3(3, 17, 0);
       
    }

    // Update is called once per frame
    void Update()
    {
         myRigidbody.velocity = new Vector2(speed, myRigidbody.velocity.y);
        direction = target - transform.position;
        
        transform.Translate(direction * (myRigidbody.velocity));
    }
}
