using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public float Vx, Vy, rotate=0;
    void Update()
    {
        transform.rotation = Quaternion.Inverse(Quaternion.Euler(0, 0, this.transform.rotation.z - rotate));
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
    public void SetRotation(float angle, float elapsed, float time){
        rotate = angle*(elapsed/time);
    }
}
