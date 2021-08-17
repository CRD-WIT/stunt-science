using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    float throwVeloX, throwVeloY;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(throwVeloX, throwVeloY);
    }
    public void SetVelocity(float t, float x, float y)
    {
        this.throwVeloX = x / t;
        this.throwVeloY = y / t;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        this.GetComponent<Rigidbody2D>().gravityScale = 2;
        this.throwVeloX = this.GetComponent<Rigidbody2D>().velocity.x;
        this.throwVeloY = this.GetComponent<Rigidbody2D>().velocity.y;
    }
}
