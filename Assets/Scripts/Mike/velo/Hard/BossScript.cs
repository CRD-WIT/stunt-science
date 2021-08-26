using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public float Vx, Vy;
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
    public void SetVelocityOfTheHead(float t, float x, float y)
    {
        this.Vx = x / t;
        this.Vy = y / t;
    }
}
