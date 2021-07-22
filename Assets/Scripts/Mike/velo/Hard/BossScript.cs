using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public float Vx, Vy, dist, t;
    void Start()
    {
    }
    void Update()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = GetVelocityOfTheHead();
    }
    public Vector2 GetVelocityOfTheHead()
    {
        return new Vector2(Vx, Vy);
    }
    public void SetVelocityOfTheHead(float x, float y, float velocity)
    {
        dist = Mathf.Sqrt((x * x) + (y * y));
            t = dist / velocity;
        this.Vx = x / t;
        this.Vy = y / t;
    }
}
