using UnityEngine;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class CurvedAnnotationLine : MonoBehaviour
{
    [Range(0, 50)]
    public int segments = 50;
    [SerializeField] LineRenderer line, endLine;
    public float arc, angle;
     public float x, y, EX, EY, ARadiusX, LRadiusX, ARadiusY, LRadiusY;

    void Start()
    {
        line = this.gameObject.GetComponent<LineRenderer>();
        endLine = this.gameObject.GetComponent<LineRenderer>();
        endLine.positionCount = 3;
        endLine.useWorldSpace = false;
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
         CreatePoints();
    }
    void Update()
    {
       
    }

    void CreatePoints()
    {




       
        endLine.SetPosition(0, new Vector3(-5.196152f, -3, 0));
              
          
        
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * ARadiusX;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * ARadiusY;
            EX = Mathf.Sin(Mathf.Deg2Rad * angle) * LRadiusX;
            EY = Mathf.Cos(Mathf.Deg2Rad * angle) * LRadiusY;

            line.SetPosition(i, new Vector3(x, y, 0));
            endLine.SetPosition(2, new Vector3(EX, EY, 0));
            angle += (arc/segments);
        }
        endLine.SetPosition(1, new Vector3(0, 0, 0));
    }
}