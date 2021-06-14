using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    SurfaceEffector2D conveyorEffect;
    GameObject conveyorWheel1, conveyorWheel2;
    public GameObject conveyorTooth;
    Rigidbody2D conveyorWheel1RB, conveyorWheel2RB;
    float distance, time, angularVelocity;
    public static float  conveyorSpeed;
    bool toothCreated;
    // Start is called before the first frame update
    void Start()
    {
        conveyorWheel1 = transform.Find("Wheel1").gameObject;
        conveyorWheel2 = transform.Find("Wheel2").gameObject;
        conveyorWheel1RB = conveyorWheel1.GetComponent<Rigidbody2D>();
        conveyorWheel2RB = conveyorWheel2.GetComponent<Rigidbody2D>();
        conveyorEffect = this.GetComponent<SurfaceEffector2D>();
        toothCreated = true;
    }
    // Update is called once per frame
    void Update()
    {
        conveyorWheel1RB.angularVelocity = angularVelocity;
        conveyorWheel2RB.angularVelocity = angularVelocity;
        conveyorEffect.speed = -conveyorSpeed;
        // if (toothCreated)
        //     StartCoroutine(ShowTooth());
    }
    public void SetConveyorSpeed(float aVelocity, float t)
    {
        float circumferenceOfWheel = 2 * (float)(Mathf.PI * 1.15),
        arc = aVelocity * t,
        d = circumferenceOfWheel / 8;
        distance = (2 * (float)(Mathf.PI) * 1.15f) * (arc / 360);
        conveyorSpeed = distance / t;
        time = distance / conveyorSpeed;
        angularVelocity = aVelocity;
    }
    // IEnumerator ShowTooth()
    // {
    //     toothCreated = false;
    //     for (int i = 0; i < 100; i++)
    //     {
    //         yield return new WaitForSeconds(time);
    //         GameObject tooth = Instantiate(conveyorTooth);
    //         tooth.transform.position = new Vector2(11.3f, 1.18f);
    //     }

    // }
}
