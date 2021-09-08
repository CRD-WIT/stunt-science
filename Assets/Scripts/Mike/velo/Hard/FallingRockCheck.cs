using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRockCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "boss")
        {
            other.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Debug.Log("dropped");
        }

    }
}
