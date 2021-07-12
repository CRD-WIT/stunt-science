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
        isActive = true;
    }
    // Update is called once per frame
    void Update()
    {
        conveyorWheel1RB.angularVelocity = angularVelocity;
        if (MediumManager.stage == 1)
            conveyorWheel2RB.angularVelocity = angularVelocity;
        else if (MediumManager.stage == 2)
            conveyorWheel2RB.angularVelocity = angularVelocity * (3.85f / 1.22f);
        if (!isActive)
        {
            StartCoroutine(ConveyorDestroyer());
        }
    }
    IEnumerator ConveyorDestroyer()
    {
        Destroy(this.gameObject);
        yield return new WaitForEndOfFrame();
    }
    public void SetConveyorSpeed(float aVelocity, float t, float radius)
    {
        conveyorSpeed = 0;
        float circumferenceOfWheel = 2*(float)(Mathf.PI * radius),
        arc = aVelocity * t,
        d = (circumferenceOfWheel- 0.09f) * (arc / 360);
        distance = circumferenceOfWheel * (arc / 360);
        conveyorSpeed = d/t;
        conveyorVelocity = distance / t;
        time = distance / conveyorSpeed;
        angularVelocity = aVelocity;
    }
    public float GetConveyorVelocity()
    {
        return this.conveyorVelocity;
    }
}
