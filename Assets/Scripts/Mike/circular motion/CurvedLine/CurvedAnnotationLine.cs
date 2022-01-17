using UnityEngine;
using System.Collections.Generic;
using TMPro;
using GameConfig;

[RequireComponent(typeof(LineRenderer))]
public class CurvedAnnotationLine : MonoBehaviour
{
    TextColorMode lineColor;
    [Range(0, 50)]
    public int segments = 50;
    [SerializeField] LineRenderer line, endLine;
    [SerializeField] float arc, angle;
    [SerializeField] EdgeCollider2D hangerTrigger;
    [SerializeField] RectTransform grabPoint;
    float x, y, EX, EY, ARadiusX, LRadiusX, ARadiusY, LRadiusY;
    QuestionController2_0_1 qc;

    void Start()
    {
        qc = FindObjectOfType<QuestionController2_0_1>();
        line = this.gameObject.GetComponent<LineRenderer>();
        //endLine = this.gameObject.GetComponent<LineRenderer>();
        endLine.positionCount = 3;
        endLine.useWorldSpace = false;
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
    }
    void Update()
    {
        if (arc == 0)
            grabPoint.gameObject.SetActive(false);
        else
            grabPoint.gameObject.SetActive(true);
        CreatePoints();
        lineColor = TextColorMode.Given;
        line.startColor = qc.getHexColor(lineColor);
        line.endColor = qc.getHexColor(lineColor);
        endLine.startColor = qc.getHexColor(lineColor);
        endLine.endColor = qc.getHexColor(lineColor);
    }

    void CreatePoints()
    {
        switch (CurvedLineFollower.stage)
        {
            case 1:
                ARadiusX = 3;
                LRadiusX = 6;
                ARadiusY = 3;
                LRadiusY = 6;
                angle = -120;
                arc = 210;
                endLine.SetPosition(0, new Vector3(-5.196152f, -3, 0));
                break;
            case 2:
                ARadiusX = 3;
                LRadiusX = 6;
                ARadiusY = 3;
                LRadiusY = 6;
                angle = -118;
                arc = 118;
                endLine.SetPosition(0, new Vector3(-5.297686f, -2.816829f, 0));
                break;
            case 3:
                ARadiusX = 3.5f;
                LRadiusX = 7;
                ARadiusY = 3.5f;
                LRadiusY = 7;
                angle = -180;
                arc = Level5EasyManager.playerAnswer;
                endLine.SetPosition(0, new Vector3(0, -7, 0));
                Vector2[] triggerEdges;
                triggerEdges = hangerTrigger.points;
                triggerEdges[0] = new Vector2(0, 0);
                triggerEdges[1] = new Vector2(endLine.GetPosition(2).x, endLine.GetPosition(2).y);
                hangerTrigger.points = triggerEdges;
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
            grabPoint.position = new Vector2(EX + 7, EY + 7);
            angle += (arc / segments);
        }
        endLine.SetPosition(1, new Vector3(0, 0, 0));
    }
}