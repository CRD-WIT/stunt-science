using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaManager : MonoBehaviour
{
    Vector2 velocity;
    float time;
    Rigidbody2D[] gearsRB = new Rigidbody2D[3];
    Rigidbody2D bodyRB;
    [SerializeField] GameObject[] gears = new GameObject[3];
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i <= 2; i++)
        {
            gearsRB[i] = gears[i].GetComponent<Rigidbody2D>();
        }
        bodyRB = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bodyRB.velocity = velocity;
        if(velocity.x != 0) {
        gearsRB[0].angularVelocity = GearSpeed(-0.575f);
        gearsRB[1].angularVelocity = GearSpeed(-1.05f);
        gearsRB[2].angularVelocity = GearSpeed(-0.775f);}
    }
    public void SetMechaVelocity(float angularVelocity, float time){
        float circumferenceOfWheel = 2*(float)(Mathf.PI * (1.05f)),
        arc = angularVelocity * time,
        d = circumferenceOfWheel * (arc / 360);
        velocity = new Vector2 (d/time,0);
    }
    float GearSpeed(float radius){
        return (360*velocity.x/(2*(float)(Mathf.PI*radius)));
    }
}
