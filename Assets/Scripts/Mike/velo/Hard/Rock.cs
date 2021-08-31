using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    public bool? hit;
    float throwVeloX, throwVeloY;
    public bool destroyer;
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
        // Debug.Log("Destroyed "+hardManager.destroyStone);
    }
    public void SetVelocity(float t, float x, float y)
    {
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.throwVeloX = x / t;
        this.throwVeloY = y / t;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        // hardManager.GetStonePos();
        if (other.gameObject.tag == "boss")
        {
            hit = false;
            this.GetComponent<Rigidbody2D>().gravityScale = 1;
            this.throwVeloX = this.GetComponent<Rigidbody2D>().velocity.x;
            this.throwVeloY = this.GetComponent<Rigidbody2D>().velocity.y;
        }
        else if (other.gameObject.tag == "gem")
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
