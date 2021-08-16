using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public static float throwVeloX, throwVeloY;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(throwVeloX, throwVeloY);
    }
    // void SetVelocity(float t, float x, float y)
    // {
    //     this.throwVeloX = x / t;
    //     this.throwVeloY = y / t;
    // }
}
