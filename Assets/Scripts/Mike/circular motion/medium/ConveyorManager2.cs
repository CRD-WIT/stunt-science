using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManager2 : MonoBehaviour
{
    GameObject conveyorWheel1, conveyorWheel2;
    public GameObject conveyorTooth;
    Rigidbody2D conveyorWheel1RB, conveyorWheel2RB;
    float distance, time;
    public static float  conveyorSpeed, conveyorVelocity, angularVelocity;
    // Start is called before the first frame update
    void Start()
    {
        conveyorWheel1 = transform.Find("LargeGear").gameObject;
        conveyorWheel2 = transform.Find("SmallGear").gameObject;
        conveyorWheel1RB = conveyorWheel1.GetComponent<Rigidbody2D>();
        conveyorWheel2RB = conveyorWheel2.GetComponent<Rigidbody2D>();
        SetConveyorSpeed(50,2);
    }
    // Update is called once per frame
    void Update()
    {
        conveyorWheel1RB.angularVelocity = angularVelocity;
        conveyorWheel2RB.angularVelocity = angularVelocity*3.75f;//3.2666666666666666666666666666667
        // if (toothCreated)
        //     StartCoroutine(ShowTooth());
    }
    public float SetConveyorSpeed(float aVelocity, float t)
    {
        float circumferenceOfWheel1 = (float)(Mathf.PI * 4.9f)*2,
        // circuferenceOfWheel2 = (float)(Mathf.PI * 1.5f),
        arc = aVelocity * t,
        d = (circumferenceOfWheel1) * (arc / 360);
        distance = circumferenceOfWheel1 * (arc / 360);
        conveyorSpeed = (d / t);
        conveyorVelocity = distance/t;
        time = distance / conveyorSpeed;
        angularVelocity = -aVelocity;
        return conveyorVelocity;
    }
}
