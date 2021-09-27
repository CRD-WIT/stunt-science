using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRockCheck : MonoBehaviour
{
    HardManager hm;
    // Start is called before the first frame update
    void Start()
    {  
        hm =FindObjectOfType<HardManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.tag == "boss")&&(hm.readyToCheck))
        {
            other.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Debug.Log("dropped");
        }

    }
}
