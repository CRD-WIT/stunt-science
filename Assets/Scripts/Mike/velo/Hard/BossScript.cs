using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    float Vx, Vy;
    void Start()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        rigidbody.velocity = GetVelocityOfTheHead();
    }
    public Vector2 GetVelocityOfTheHead()
    {
        return new Vector2(Vx, Vy);
    }
    public void SetVelocityOfTheHead(float x, float y, float velocity)
    {
        float dist = Mathf.Sqrt((x * x) + (y * y)),
            t = dist / velocity;
        Vx = x / t;
        Vy = y / t;
    }
}
