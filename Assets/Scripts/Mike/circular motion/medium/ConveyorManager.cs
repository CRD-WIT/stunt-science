using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    GameObject conveyorWheel1, conveyorWheel2;
    public GameObject conveyorTooth;
    Rigidbody2D conveyorWheel1RB, conveyorWheel2RB;
    float distance, time;
    public static float  conveyorSpeed, conveyorVelocity, angularVelocity;
    // Start is called before the first frame update
    void Start()
    {
        conveyorWheel1 = transform.Find("Wheel1").gameObject;
        conveyorWheel2 = transform.Find("Wheel2").gameObject;
        conveyorWheel1RB = conveyorWheel1.GetComponent<Rigidbody2D>();
        conveyorWheel2RB = conveyorWheel2.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        conveyorWheel1RB.angularVelocity = angularVelocity;
        conveyorWheel2RB.angularVelocity = angularVelocity;
        // if (toothCreated)
        //     StartCoroutine(ShowTooth());
    }
    public float SetConveyorSpeed(float aVelocity, float t)
    {
        float circumferenceOfWheel = (float)(Mathf.PI * 2.3f),
        arc = aVelocity * t,
        d = (circumferenceOfWheel-0.09f) * (arc / 360);
        distance = circumferenceOfWheel * (arc / 360);
        conveyorSpeed = d / t;
        conveyorVelocity = distance/t;
        time = distance / conveyorSpeed;
        angularVelocity = aVelocity;
        return conveyorVelocity;
    }
}
