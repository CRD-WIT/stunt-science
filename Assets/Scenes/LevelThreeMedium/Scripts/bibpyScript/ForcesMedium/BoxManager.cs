using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float boxMass, boxSpeed1,boxSpeed2,boxSpeed3;
    public GameObject Box1,box2,box3;
    public Rigidbody2D box1Rigidbody,box2Rigidbody,box3Rigidbody;
    void Start()
    {
        //pushTheBox();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
       box1Rigidbody.velocity = new Vector2(boxSpeed1,box1Rigidbody.velocity.y);
       box2Rigidbody.velocity = new Vector2(boxSpeed2,box2Rigidbody.velocity.y);
       box3Rigidbody.velocity = new Vector2(boxSpeed3,box2Rigidbody.velocity.y);
    }
    public void pushTheBox()
    {
        //Box1.GetComponent<Rigidbody2D>().velocity = transform.right * boxSpeed;
    }
}
