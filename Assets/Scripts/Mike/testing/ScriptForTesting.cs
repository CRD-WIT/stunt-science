using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptForTesting : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        if (this.gameObject.name == "Circle1")
            this.rb.velocity = new Vector2(-Random.Range(8f, 10f), 0);
        else
            this.rb.velocity = new Vector2(Random.Range(8f, 10f), 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Boulder")
            this.gameObject.GetComponent<CapsuleCollider2D>().sharedMaterial.bounciness = 0.5f;
        else
            this.gameObject.GetComponent<CapsuleCollider2D>().sharedMaterial.bounciness = 0;


    }
}
