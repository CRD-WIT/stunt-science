using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    public bool? hit;
    float throwVeloX, throwVeloY;
    public bool destroyer, triggered;
    HardManager hardManager;
    // Start is called before the first frame update
    void Start()
    {
        hardManager = FindObjectOfType<HardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(throwVeloX, throwVeloY);
        if (destroyer)
        {
            StartCoroutine(DestroyObj());
        }
        if(hit != null)
            this.throwVeloY = -1;
    }
    public void SetVelocity(float t, float x, float y)
    {
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.throwVeloX = x / t;
        this.throwVeloY = y / t;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        this.GetComponent<Rigidbody2D>().gravityScale = 1;
        this.throwVeloX = this.GetComponent<Rigidbody2D>().velocity.x;
        this.throwVeloY = this.GetComponent<Rigidbody2D>().velocity.y;
        hit = false;
        if (other.gameObject.tag == "gem")
        {
            destroyer =false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "gem")
        {
            hit = true;
            destroyer = true;
        }
    }
    IEnumerator DestroyObj()
    {
        Debug.Log("destroyed");
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
        yield return new WaitForEndOfFrame();
        destroyer = false;
    }
}
