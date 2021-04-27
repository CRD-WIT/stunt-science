using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurvedLineFollower : MonoBehaviour
{
    [SerializeField] TMP_Text angleDegree;
    [SerializeField] LineRenderer line, straightLine;
    public Rigidbody2D gear2;
    float arc;
    public static float stage;
    // Start is called before the first frame update
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = 51;
        line.useWorldSpace = false;

        straightLine.positionCount = 3;
        straightLine.useWorldSpace = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Level5EasyManager.gear2Speed == 0)
            gear2.rotation = 0;
        gear2.angularVelocity = Level5EasyManager.gear2Speed;
        if (Level5EasyManager.isOnStunt)
        {
            arc = Level5EasyManager.playerAnswer * Level5EasyManager.gameTime;
        }
        else
        {
            arc = Level5EasyManager.playerAnswer * GearHangers.hangTime;//Level5EasyManager.elapsed;
        }
        if (arc != 0)
        {
            angleDegree.text = "<color=#A15D04>" + System.Math.Round(arc, 2) + "</color>";
            angleDegree.transform.position = new Vector3(line.GetPosition(25).x + 4, line.GetPosition(25).y + 1, 0);
            straightLine.SetPosition(0, new Vector3(-5.196152f, -3, 0));
            straightLine.SetPosition(1, new Vector3(0, 0, 0));
            CreatePoints();
        }
        else
        {
            angleDegree.text = "<color=#9A0000>210°</color>";
            line.enabled = false;
            straightLine.enabled = false;
        }
    }
    void CreatePoints()
    {
        line.enabled = true;
        straightLine.enabled = true;
        float x;
        float y;
        float xSL;
        float ySL;
        float angle = -120;

        for (int i = 0; i <= 50; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * 3f;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * 3f;
            xSL = Mathf.Sin(Mathf.Deg2Rad * angle) * 6f;
            ySL = Mathf.Cos(Mathf.Deg2Rad * angle) * 6f;

            line.SetPosition(i, new Vector3(x, y, 0));
            straightLine.SetPosition(2, new Vector3(xSL, ySL, 0));

            angle += ((float)System.Math.Round(arc, 2) / 50);
        }


    }
}
