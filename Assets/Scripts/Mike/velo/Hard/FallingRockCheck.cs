using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRockCheck : MonoBehaviour
{
    HardManager hm;
    AudioSource fallingSound;
    bool ready;
    // Start is called before the first frame update
    void Start()
    {
        hm = FindObjectOfType<HardManager>();
        fallingSound = this.GetComponent<AudioSource>();
        ready =true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        // this.fallingSound.Stop();
        if (ready)
        {
            ready =false;
            this.fallingSound.Play();
        }
        if ((other.gameObject.tag == "boss") && (hm.readyToCheck))
        {
            if (other.gameObject.name == "head")
                other.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            else
                other.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}
