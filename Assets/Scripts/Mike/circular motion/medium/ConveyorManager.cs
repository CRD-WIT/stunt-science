using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    GameObject conveyorWheel1, conveyorWheel2;
    public GameObject conveyorTooth;
    Rigidbody2D conveyorWheel1RB, conveyorWheel2RB;
    float distance, time, conveyorVelocity, speed;
    public static float conveyorSpeed, angularVelocity;
    public static bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        conveyorWheel1 = transform.Find("Wheel1").gameObject;
        conveyorWheel2 = transform.Find("Wheel2").gameObject;
        conveyorWheel1RB = conveyorWheel1.GetComponent<Rigidbody2D>();
        conveyorWheel2RB = conveyorWheel2.GetComponent<Rigidbody2D>();
        isActive =true;
    }
    // Update is called once per frame
    void Update()
    {
        conveyorWheel1RB.angularVelocity = angularVelocity;
        conveyorWheel2RB.angularVelocity = angularVelocity;
        if(!isActive){
            StartCoroutine(ConveyorDestroyer());
        }
    }
    IEnumerator ConveyorDestroyer(){
        Destroy(this.gameObject);
        yield return new WaitForEndOfFrame();
    }
    public void SetConveyorSpeed(float aVelocity, float t)
    {
        conveyorSpeed = 0;
        float circumferenceOfWheel = (float)(Mathf.PI * 2.3f),
        arc = aVelocity * t,
        d = (circumferenceOfWheel - 0.09f) * (arc / 360);
        distance = circumferenceOfWheel * (arc / 360);
        conveyorSpeed = d / t;
        conveyorVelocity = distance / t;
        time = distance / conveyorSpeed;
        angularVelocity = aVelocity;
    }
    public float GetConveyorVelocity()
    {
        return this.conveyorVelocity;
    }
}
