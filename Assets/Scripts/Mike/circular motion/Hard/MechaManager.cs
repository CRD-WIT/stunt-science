using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaManager : MonoBehaviour
{
    public Vector2 velocity;
    public float armRotation;
    float time;
    public bool off;
    Rigidbody2D[] gearsRB = new Rigidbody2D[3];
    Rigidbody2D bodyRB;

    [SerializeField]
    Transform[] joints = new Transform[3];

    [SerializeField]
    Rigidbody2D armRB;

    [SerializeField]
    GameObject[] gears = new GameObject[3];
    BoxCollider2D rdMarker;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 2; i++)
        {
            gearsRB[i] = gears[i].GetComponent<Rigidbody2D>();
        }
        bodyRB = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LvlFiveHardManager.stage == 1)
            bodyRB.velocity = velocity;
        else
            bodyRB.velocity = new Vector2(0, 0);
        if (velocity.x != 0)
        {
            gearsRB[0].angularVelocity = GearSpeed(0.575f);
            gearsRB[1].angularVelocity = GearSpeed(1.05f);
            gearsRB[2].angularVelocity = GearSpeed(0.775f);
        }
        else
        {
            gearsRB[0].angularVelocity = 0;
            gearsRB[1].angularVelocity = 0;
            gearsRB[2].angularVelocity = 0;
        }
        armRB.angularVelocity = armRotation;
        if (off)
        {
            StartCoroutine(MechaDown());
        }
    }

    public void SetMechaVelocity(float angularVelocity, float time, float radius)
    {
        float circumferenceOfWheel = 2 * (float)(Mathf.PI * (radius)),
            arc = angularVelocity * time,
            d = circumferenceOfWheel * (arc / 360);
        velocity = new Vector2(d / time, 0);
    }

    float GearSpeed(float radius)
    {
        return -(360 * velocity.x / (2 * (float)(Mathf.PI * radius)));
    }

    public float MechaVelocity(float av, float t, float r)
    {
        float circumferenceOfWheel = 2 * (float)(Mathf.PI * (r)),
            arc = av * t,
            d = circumferenceOfWheel * (arc / 360);
        return d / t;
    }

    public IEnumerator MechaDown()
    {
        // joints[0].localEulerAngles = new Vector3(0,0,60);
        // joints[3].localEulerAngles = new Vector3(0,0, -25);
        // yield return new WaitForSeconds(0.25f);
        // joints[1].localEulerAngles = new Vector3(0,0,30);
        // yield return new WaitForSeconds(0.25f);
        // joints[2].localEulerAngles = new Vector3(0,0, 0);
        off = false;
        for (float i = 0; i <= 45; i++)
        {
            joints[0].localEulerAngles = new Vector3(0, 0, i);
            joints[1].localEulerAngles = new Vector3(0, 0, i);
            if (i < 25)
                joints[3].localEulerAngles = new Vector3(0, 0, -i);
            yield return new WaitForEndOfFrame();
        }
        // float k = joints[2].localEulerAngles.z;
        // while (k != -329.295f)
        // {
        //     joints[2].localEulerAngles=new Vector3(0, 0, k);
        //     yield return new WaitForEndOfFrame();
        //     if (k > 0)
        //         k++;
        //     else
        //         k--;
        // }
    }
}
