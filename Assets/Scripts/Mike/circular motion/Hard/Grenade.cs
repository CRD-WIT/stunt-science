 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    Rigidbody2D rb;
    float flightTime = 0;
    Vector2 initState;
    public bool isThrown = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        initState = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if(isThrown){
            rb.bodyType = RigidbodyType2D.Dynamic;
            flightTime += Time.deltaTime;
            rb.velocity = new Vector2(-5.5f, (15f)*(1f-flightTime));
        }
        else
        {
            // rb.bodyType = RigidbodyType2D.Kenimatic;
            rb.isKinematic = true;
            flightTime = 0;
            rb.velocity = initState;
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "head")
            isThrown = false;
    }
}
