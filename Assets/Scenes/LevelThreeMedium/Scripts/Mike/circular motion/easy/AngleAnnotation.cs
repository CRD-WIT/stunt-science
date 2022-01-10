using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleAnnotation : MonoBehaviour
{
    float startAngle = 0, endAngle = 210,radius = 2, segments = 50;
    public void DrawArc()
    {
        List<Vector2> arcPoints = new List<Vector2>();
        float angle = startAngle;
        float arcLength = endAngle - startAngle;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
   
            arcPoints.Add(new Vector2(x, y));

            angle += (arcLength / segments);
        }
    }
}
