using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class CurvedAnnotationLine : MonoBehaviour
{
    [Range(0, 50)]
    public int segments = 50;
    [SerializeField] LineRenderer line, endLine;
    [SerializeField] float arc, angle;
    float x, y, EX, EY, ARadiusX, LRadiusX, ARadiusY, LRadiusY;

    void Start()
    {
        line = this.gameObject.GetComponent<LineRenderer>();
        //endLine = this.gameObject.GetComponent<LineRenderer>();
        endLine.positionCount = 3;
        endLine.useWorldSpace = false;
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
    }
    void Update()
    {
        CreatePoints();
    }

    void CreatePoints()
    {
        
        switch (CurvedLineFollower.stage)
        {
            case 1:
                ARadiusX = 3;
                LRadiusX = 6;
                ARadiusY= 3;
                LRadiusY = 6;
                angle = -120;
                arc = 210;
                endLine.SetPosition(0, new Vector3(-5.196152f, -3, 0));
                break;
            case 2:
                ARadiusX = 3;
                LRadiusX = 6;
                ARadiusY= 3;
                LRadiusY = 6;
                angle = -360;
                arc = 118;
                endLine.SetPosition(0, new Vector3(-5.297686f, -2.816829f, 0));
                break;
            case 3:
                ARadiusX = 3.5f;
                LRadiusX = 7;
                ARadiusY= 3.5f;
                LRadiusY = 7;
                //line = GOline.GetComponent<LineRenderer>();
                // endLine = GOendLine.GetComponent<LineRenderer>();
                angle = -180;
                arc = 75; // playerAnswer
                endLine.SetPosition(0, new Vector3(0, -7, 0));
                break;
        }
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * ARadiusX;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * ARadiusY;
            EX = Mathf.Sin(Mathf.Deg2Rad * angle) * LRadiusX;
            EY = Mathf.Cos(Mathf.Deg2Rad * angle) * LRadiusY;

            line.SetPosition(i, new Vector3(x, y, 0));
            endLine.SetPosition(2, new Vector3(EX, EY, 0));
            angle += (arc / segments);
        }
        endLine.SetPosition(1, new Vector3(0, 0, 0));
    }
}