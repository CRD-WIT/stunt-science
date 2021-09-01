using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;
using TMPro;

public class AngleAnnotaion : MonoBehaviour
{
    TextColorMode lineColor;
    float angle, x, y, xSL, ySL, angle1, x1, y1, xSL1, ySL1, angle2, x2, y2, xSL2, ySL2;
    public float startingAngle, angleA, angleB, angleC, legA, legB, legC, hypotenuse, fontSize = 4, angleLabelOffset;
    int arcPntsA, arcPntsB, arcPntsC;
    public bool hideAngleA, hideAngleB, hideAngleC, hideSideA, hideSideB, hideSideC;
    public Vector2 spawnPnt;
    [SerializeField] TMP_Text[] labelTxt;
    [SerializeField] LineRenderer[] lines;
    QuestionControllerVThree qc;
    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        foreach (var item in lines)
        {
            item.useWorldSpace = false;
        }
        lines[1].positionCount = 3;
        lineColor = TextColorMode.Given;
    }
    public void SetSpawnPnt(Vector2 sp)
    {
        spawnPnt = sp;
    }

    // Update is called once per frame
    void AngleA()
    {
        float actualAngle = Mathf.Abs(angleA % 360);
        if (actualAngle == 90)
        {
            lines[0].positionCount = 3;
            arcPntsA = 2;
            legC = Mathf.Sqrt((legA * legA) + (legB * legB));
            hypotenuse = legC;
        }
        else
        {
            if (actualAngle == 270)
            {
                legC = Mathf.Sqrt((legA * legA) + (legB * legB));
                hypotenuse = legC;
            }
            else
            {
                legC = Mathf.Sqrt((legA * legA) + (legB * legB) - (2 * legA * legB * Mathf.Cos(angleA * Mathf.Deg2Rad)));
                if (legA > legB)
                {
                    if (legA > legC)
                        hypotenuse = legA;
                    else
                        hypotenuse = legC;
                }
                else
                {
                    if (legB > legC)
                        hypotenuse = legB;
                    else
                        hypotenuse = legC;
                }
            }
            lines[0].positionCount = 61;
            arcPntsA = 60;
        }
    }
    void AngleB()
    {
        float actualAngle = Mathf.Abs(angleB % 360);
        if (actualAngle == 90)
        {
            lines[2].positionCount = 3;
            arcPntsB = 2;
        }
        else
        {
            lines[2].positionCount = 61;
            arcPntsB = 60;
        }
    }
    void AngleC()
    {
        float actualAngle = Mathf.Abs(angleC % 360);
        if (actualAngle == 90)
        {
            lines[3].positionCount = 3;
            arcPntsC = 2;
        }
        else
        {
            lines[3].positionCount = 61;
            arcPntsC = 60;
        }
    }
    void Update()
    {
        this.gameObject.transform.position = spawnPnt;
        angleC = Mathf.Acos(((legB * legB) - (legC * legC) - (legA * legA)) / (-2 * legC * legA)) * Mathf.Rad2Deg;
        angleB = 180 - Mathf.Abs(angleC) - Mathf.Abs(angleA);
        AngleA();
        AngleB();
        AngleC();
        lines[1].SetPosition(1, new Vector2(0, 0));
        lines[2].transform.position = new Vector2(lines[1].GetPosition(2).x, lines[1].GetPosition(2).y);
        lines[3].transform.position = new Vector2(lines[1].GetPosition(0).x, lines[1].GetPosition(0).y);
        CreatePoints();
        foreach (var item in labelTxt)
        {
            item.fontSize = fontSize;
            item.color = Color.black;
        }
        labelTxt[0].gameObject.SetActive(!hideAngleA);
        labelTxt[1].gameObject.SetActive(!hideSideA);
        labelTxt[2].gameObject.SetActive(!hideSideB);
        labelTxt[3].gameObject.SetActive(!hideSideC);
        labelTxt[4].gameObject.SetActive(!hideAngleB);
        labelTxt[5].gameObject.SetActive(!hideAngleC);
    }
    void CreatePoints()
    {
        foreach (var item in lines)
        {
            item.enabled = true;
            // item.startColor = qc.getHexColor(lineColor);
            // item.endColor = qc.getHexColor(lineColor);
        }
        angle = startingAngle;
        angle1 = startingAngle - 90 + (angleA - 90);
        angle2 = startingAngle - 90 - (angleC + 90);
        lines[1].SetPosition(0, new Vector2(legA * Mathf.Sin(startingAngle * Mathf.Deg2Rad), legA * Mathf.Cos(startingAngle * Mathf.Deg2Rad)));
        for (int i = 0; i <= arcPntsA; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle);
            y = Mathf.Cos(Mathf.Deg2Rad * angle);
            xSL = Mathf.Sin(Mathf.Deg2Rad * angle) * legB;
            ySL = Mathf.Cos(Mathf.Deg2Rad * angle) * legB;

            lines[1].SetPosition(2, new Vector2(xSL, ySL));

            lines[0].SetPosition(i, new Vector2(x, y));
            if (Mathf.Abs(angleA) == 90)
                lines[0].SetPosition(1, new Vector2(x + Mathf.Sin(startingAngle * Mathf.Deg2Rad), y + Mathf.Cos(startingAngle * Mathf.Deg2Rad)));
            angle += angleA / arcPntsA;
        }
        for (int i = 0; i <= arcPntsB; i++)
        {
            x1 = (Mathf.Sin(Mathf.Deg2Rad * angle1) * 1f);
            y1 = (Mathf.Cos(Mathf.Deg2Rad * angle1) * 1f);
            xSL1 = Mathf.Sin(Mathf.Deg2Rad * angle1) * legC;
            ySL1 = Mathf.Cos(Mathf.Deg2Rad * angle1) * legC;

            lines[2].SetPosition(i, new Vector2(x1, y1));
            if (Mathf.Abs(angleB) == 90)
                lines[2].SetPosition(1, new Vector2(x1 + Mathf.Sin((startingAngle - 90 + (angleA - 90)) * Mathf.Deg2Rad),y1 + Mathf.Cos((startingAngle - 90 + (angleA - 90)) * Mathf.Deg2Rad)));

            angle1 += angleB / arcPntsB;
        }
        for (int i = 0; i <= arcPntsC; i++)
        {
            x2 = (Mathf.Sin(Mathf.Deg2Rad * angle2) * 1f);
            y2 = (Mathf.Cos(Mathf.Deg2Rad * angle2) * 1f);
            xSL2 = Mathf.Sin(Mathf.Deg2Rad * angle2) * legA;
            ySL2 = Mathf.Cos(Mathf.Deg2Rad * angle2) * legA;

            lines[3].SetPosition(i, new Vector2(x2, y2));
            if (Mathf.Abs(angleC) == 90)
                lines[3].SetPosition(1, new Vector2(x2 + Mathf.Sin((startingAngle - 90 - (angleC + 90)) * Mathf.Deg2Rad),y2 + Mathf.Cos((startingAngle - 90 - (angleC + 90)) * Mathf.Deg2Rad)));
            angle2 += angleC / arcPntsC;
        }
        lines[0].startColor = qc.getHexColor(lineColor);
        lines[0].endColor = qc.getHexColor(lineColor);
        lines[1].startColor = qc.getHexColor(lineColor);
        lines[1].endColor = qc.getHexColor(lineColor);
        lines[2].startColor = qc.getHexColor(lineColor);
        lines[2].endColor = qc.getHexColor(lineColor);
        lines[3].startColor = qc.getHexColor(lineColor);
        lines[3].endColor = qc.getHexColor(lineColor);
        
        // labelTxt[0].text = Mathf.Abs(angleA).ToString("f2") + qc.Unit(UnitOf.angle);
        // labelTxt[1].transform.position = new Vector2(this.transform.position.x + ((legA * Mathf.Sin(startingAngle * Mathf.Deg2Rad)) / 2), this.transform.position.y + ((legA * Mathf.Cos(startingAngle * Mathf.Deg2Rad)) / 2));
        // labelTxt[1].text = legA.ToString("f2") + qc.Unit(UnitOf.distance);
        // labelTxt[2].transform.position = new Vector2(this.transform.position.x + (xSL / 2), this.transform.position.y + (ySL / 2));
        // labelTxt[2].text = legB.ToString("f2") + qc.Unit(UnitOf.distance);
        // labelTxt[3].transform.position = new Vector2(this.transform.position.x + (((legA * Mathf.Sin(startingAngle * Mathf.Deg2Rad)) + xSL) / 2), this.transform.position.y + (((legA * Mathf.Cos(startingAngle * Mathf.Deg2Rad)) + ySL) / 2));
        // labelTxt[3].text = legC.ToString("f2") + qc.Unit(UnitOf.distance);
        // labelTxt[4].text = Mathf.Abs(angleB).ToString("f2") + qc.Unit(UnitOf.angle);
        // labelTxt[5].text = Mathf.Abs(angleC).ToString("f2") + qc.Unit(UnitOf.angle);

        // labelTxt[0].transform.position = new Vector2(this.transform.position.x + lines[0].GetPosition(arcPntsA / 2).x + angleLabelOffset,
        //     this.transform.position.y + lines[0].GetPosition(arcPntsA / 2).y + angleLabelOffset);
        // labelTxt[4].transform.position = new Vector2(lines[1].GetPosition(2).x + lines[2].GetPosition(arcPntsB / 2).x + angleLabelOffset,
        //     lines[1].GetPosition(2).y + lines[2].GetPosition(arcPntsB / 2).y + angleLabelOffset);
        // labelTxt[5].transform.position = new Vector2(lines[1].GetPosition(0).x + lines[3].GetPosition(arcPntsC / 2).x + angleLabelOffset,
        //     lines[1].GetPosition(0).y + lines[3].GetPosition(arcPntsC / 2).y + angleLabelOffset);
    }
    public void HideValuesOf(bool angleA, bool angleB, bool angleC, bool sideA, bool sideB, bool sideC)
    {
        hideAngleA = angleA;
        hideAngleB = angleB;
        hideAngleC = angleC;
        hideSideA = sideA;
        hideSideB = sideB;
        hideSideC = sideC;
    }
}
