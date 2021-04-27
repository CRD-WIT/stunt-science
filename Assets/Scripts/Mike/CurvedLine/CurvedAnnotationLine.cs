using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class CurvedAnnotationLine : MonoBehaviour
{
    [Range(0, 50)]
    public int segments = 50;
    [SerializeField] LineRenderer line, endLine;
    [SerializeField] float arc;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();

        endLine.positionCount = 3;
        endLine.useWorldSpace = false;
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        CreatePoints();
    }

    void CreatePoints()
    {
        float x, y, angle, EX, EY;
        switch (CurvedLineFollower.stage)
        {
            case 1:
                angle = -120;
                arc = 210;
                for (int i = 0; i < (segments + 1); i++)
                {
                    x = Mathf.Sin(Mathf.Deg2Rad * angle) * 3;
                    y = Mathf.Cos(Mathf.Deg2Rad * angle) * 3;
                    EX = Mathf.Sin(Mathf.Deg2Rad * angle) * 6;
                    EY = Mathf.Cos(Mathf.Deg2Rad * angle) * 6;

                    line.SetPosition(i, new Vector3(x, y, 0));
                    endLine.SetPosition(2, new Vector3(EX, EY, 0));
                    angle += (arc / segments);
                }
                endLine.SetPosition(0, new Vector3(-5.196152f, -3, 0));
                break;
            case 2:
                angle = -118;
                arc = 118;
                for (int i = 0; i < (segments + 1); i++)
                {
                    x = Mathf.Sin(Mathf.Deg2Rad * angle) * 3;
                    y = Mathf.Cos(Mathf.Deg2Rad * angle) * 3;
                    EX = Mathf.Sin(Mathf.Deg2Rad * angle) * 6;
                    EY = Mathf.Cos(Mathf.Deg2Rad * angle) * 6;

                    line.SetPosition(i, new Vector3(x, y, 0));
                    endLine.SetPosition(2, new Vector3(EX, EY, 0));
                    angle += (arc / segments);
                }
                endLine.SetPosition(0, new Vector3(-5.297686f, -2.816829f, 0));
                break;
        }
        endLine.SetPosition(1, new Vector3(0, 0, 0));
    }
}