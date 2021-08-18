using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;
using TMPro;

public class AngleAnnotaion : MonoBehaviour
{
    TextColorMode lineColor;
    float angle, x, y, xSL, ySL, actualAngle;
    public float startingAngle, arc, legA, legB, legC, hypotenuse, fontSize = 4, angleLabelOffset;
    int arcPnts;
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

    // Update is called once per frame
    void Update()
    {
        actualAngle = Mathf.Abs(arc % 360);
        if ((arc == 90) || (arc == -90))
        {
            lines[0].positionCount = 3;
            arcPnts = 2;
            labelTxt[0].transform.position = new Vector2(this.transform.position.x + lines[0].GetPosition(1).x + angleLabelOffset, this.transform.position.y + lines[0].GetPosition(1).y + angleLabelOffset);
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
                legC = Mathf.Sqrt((legA * legA) + (legB * legB) - (2 * legA * legB * Mathf.Cos(arc * Mathf.Deg2Rad)));
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
            arcPnts = 60;
            labelTxt[0].transform.position = new Vector2(this.transform.position.x + lines[0].GetPosition(30).x + angleLabelOffset, this.transform.position.y + lines[0].GetPosition(30).y + angleLabelOffset);
        }

        lines[1].SetPosition(1, new Vector2(0, 0));
        CreatePoints();

        foreach (var item in labelTxt)
        {
            item.fontSize = fontSize;
            item.color = Color.black;
        }
        labelTxt[0].text = arc.ToString("f2") + qc.Unit(UnitOf.angle);
        labelTxt[1].transform.position = new Vector2(this.transform.position.x + ((legA * Mathf.Sin(startingAngle * Mathf.Deg2Rad)) / 2), this.transform.position.y + ((legA * Mathf.Cos(startingAngle * Mathf.Deg2Rad)) / 2));
        labelTxt[1].text = legA.ToString("f2") + qc.Unit(UnitOf.distance);
        labelTxt[2].transform.position = new Vector2(this.transform.position.x + (xSL / 2), this.transform.position.y + (ySL / 2));
        labelTxt[2].text = legB.ToString("f2") + qc.Unit(UnitOf.distance);
        labelTxt[3].transform.position = new Vector2(this.transform.position.x + (((legA * Mathf.Sin(startingAngle * Mathf.Deg2Rad))+xSL)/2), this.transform.position.y + (((legA * Mathf.Cos(startingAngle * Mathf.Deg2Rad))+ySL)/2));
        labelTxt[3].text = legC.ToString("f2") + qc.Unit(UnitOf.distance);
    }
    void CreatePoints()
    {
        lines[0].enabled = true;
        lines[1].enabled = true;
        angle = startingAngle;
        lines[1].SetPosition(0, new Vector2(legA * Mathf.Sin(startingAngle * Mathf.Deg2Rad), legA * Mathf.Cos(startingAngle * Mathf.Deg2Rad)));
        for (int i = 0; i <= arcPnts; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * 1f;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * 1f;
            xSL = Mathf.Sin(Mathf.Deg2Rad * angle) * legB;
            ySL = Mathf.Cos(Mathf.Deg2Rad * angle) * legB;

            lines[1].SetPosition(2, new Vector2(xSL, ySL));

            if ((arc == 90) || (arc == -90))
            {
                lines[0].SetPosition(0, new Vector2(1 * Mathf.Sin(startingAngle * Mathf.Deg2Rad), 1 * Mathf.Cos(startingAngle * Mathf.Deg2Rad)));
                lines[0].SetPosition(1, new Vector2(x + Mathf.Sin(startingAngle * Mathf.Deg2Rad), y + Mathf.Cos(startingAngle * Mathf.Deg2Rad)));
                lines[0].SetPosition(2, new Vector2(x, y));
            }
            else
            {
                lines[0].SetPosition(i, new Vector3(x, y, 0));
            }
            angle += ((float)System.Math.Round(arc, 2) / arcPnts);
        }

        lines[0].startColor = qc.getHexColor(lineColor);
        lines[0].endColor = qc.getHexColor(lineColor);
        lines[1].startColor = qc.getHexColor(lineColor);
        lines[1].endColor = qc.getHexColor(lineColor);
    }
}
