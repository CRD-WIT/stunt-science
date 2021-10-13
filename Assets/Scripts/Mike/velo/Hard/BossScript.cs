using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    float Vx, Vy, rotate = 0, time;

    void Update()
    {
        this.transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
        this.gameObject.GetComponent<Rigidbody2D>().velocity = GetVelocityOfTheHead();
    }
    public Vector2 GetVelocityOfTheHead()
    {
        if (time == 0)
            return this.GetComponent<Rigidbody2D>().velocity;
        else
            return new Vector2(Vx, Vy);
    }
    public void SetVelocityOfTheHead(float t, float x, float y)
    {
        time = t;
        this.Vx = x / t;
        this.Vy = y / t;
    }
    public void SetRotation(float angle, float elapsed, float time)
    {
        rotate = angle * (elapsed / time);
    }
}
