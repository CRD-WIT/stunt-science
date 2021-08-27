using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;
using TMPro;

public class AngleAnnotaion : MonoBehaviour
{
    TextColorMode lineColor;
    float angle, x, y, xSL, ySL;
    public float startingAngle, angleA, angleB, angleC, legA, legB, legC, hypotenuse, fontSize = 4, angleLabelOffset;
    int arcPntsA, arcPntsB, arcPntsC;
    bool hideAngle = false, hideSideA = false, hideSideB = false, hideSideC = false;
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
    public void SetSpawnPnt(Vector2 sp){
        spawnPnt = sp;
    }

    // Update is called once per frame
    void AngleA()
    {
        float actualAngle = Mathf.Abs(angleA % 360);
        if ((angleA == 90) || (angleA == -90))
        {
            lines[0].positionCount = 3;
            arcPntsA = 2;
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
            labelTxt[0].transform.position = new Vector2(this.transform.position.x + lines[0].GetPosition(30).x + angleLabelOffset, this.transform.position.y + lines[0].GetPosition(30).y + angleLabelOffset);
        }

    }
    void AngleB(){
        float actualAngle = Mathf.Abs(angleB % 360);
        if ((angleB == 90) || (angleB == -90))
        {
            lines[0].positionCount = 3;
            arcPntsB = 2;
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
                legC = Mathf.Sqrt((legA * legA) + (legB * legB) - (2 * legA * legB * Mathf.Cos(angleB * Mathf.Deg2Rad)));
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
            arcPntsB = 60;
            labelTxt[0].transform.position = new Vector2(this.transform.position.x + lines[0].GetPosition(30).x + angleLabelOffset, this.transform.position.y + lines[0].GetPosition(30).y + angleLabelOffset);
        }
    }
    void AngleC(){

    }
    void Update() {
        this.gameObject.transform.position = spawnPnt;
        AngleA();
        // AngleB();
        
        lines[1].SetPosition(1, new Vector2(0, 0));
        CreatePoints();
        foreach (var item in labelTxt)
        {
            item.fontSize = fontSize;
            item.color = Color.black;
        }
        labelTxt[0].text = Mathf.Abs(angleA).ToString("f2") + qc.Unit(UnitOf.angle);
        labelTxt[1].transform.position = new Vector2(this.transform.position.x + ((legA * Mathf.Sin(startingAngle * Mathf.Deg2Rad)) / 2), this.transform.position.y + ((legA * Mathf.Cos(startingAngle * Mathf.Deg2Rad)) / 2));
        labelTxt[1].text = legA.ToString("f2") + qc.Unit(UnitOf.distance);
        labelTxt[2].transform.position = new Vector2(this.transform.position.x + (xSL / 2), this.transform.position.y + (ySL / 2));
        labelTxt[2].text = legB.ToString("f2") + qc.Unit(UnitOf.distance);
        labelTxt[3].transform.position = new Vector2(this.transform.position.x + (((legA * Mathf.Sin(startingAngle * Mathf.Deg2Rad)) + xSL) / 2), this.transform.position.y + (((legA * Mathf.Cos(startingAngle * Mathf.Deg2Rad)) + ySL) / 2));
        labelTxt[3].text = legC.ToString("f2") + qc.Unit(UnitOf.distance);
    }
    void CreatePoints()
    {
        lines[0].enabled = true;
        lines[1].enabled = true;
        angle = startingAngle;
        lines[1].SetPosition(0, new Vector2(legA * Mathf.Sin(startingAngle * Mathf.Deg2Rad), legA * Mathf.Cos(startingAngle * Mathf.Deg2Rad)));
        for (int i = 0; i <= arcPntsA; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * 1f;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * 1f;
            xSL = Mathf.Sin(Mathf.Deg2Rad * angle) * legB;
            ySL = Mathf.Cos(Mathf.Deg2Rad * angle) * legB;

            lines[1].SetPosition(2, new Vector2(xSL, ySL));

            if ((angleA == 90) || (angleA == -90))
            {
                lines[0].SetPosition(0, new Vector2(1 * Mathf.Sin(startingAngle * Mathf.Deg2Rad), 1 * Mathf.Cos(startingAngle * Mathf.Deg2Rad)));
                lines[0].SetPosition(1, new Vector2(x + Mathf.Sin(startingAngle * Mathf.Deg2Rad), y + Mathf.Cos(startingAngle * Mathf.Deg2Rad)));
                lines[0].SetPosition(2, new Vector2(x, y));
            }
            else
            {
                lines[0].SetPosition(i, new Vector2(x, y));
            }
            angle += ((float)System.Math.Round(angleA, 2) / arcPntsA);
        }

        // for (int i = 0; i <= arcPntsB; i++)
        // {
        //     x = Mathf.Sin(Mathf.Deg2Rad * angle) * (legB - 1f);
        //     y = Mathf.Cos(Mathf.Deg2Rad * angle) * (legB + 1f);
        //     xSL = Mathf.Sin(Mathf.Deg2Rad * angle) * legC;
        //     ySL = Mathf.Cos(Mathf.Deg2Rad * angle) * legC;

        //     lines[1].SetPosition(2, new Vector2(xSL, ySL));

        //     if ((angleB == 90) || (angleB == -90))
        //     {
        //         lines[2].SetPosition(0, new Vector2(1 * Mathf.Sin(startingAngle * Mathf.Deg2Rad), 1 * Mathf.Cos(startingAngle * Mathf.Deg2Rad)));
        //         lines[2].SetPosition(1, new Vector2(x + Mathf.Sin(startingAngle * Mathf.Deg2Rad), y + Mathf.Cos(startingAngle * Mathf.Deg2Rad)));
        //         lines[2].SetPosition(2, new Vector2(x, y));
        //     }
        //     else
        //     {
        //         lines[2].SetPosition(i, new Vector2(x, y));
        //     }
        //     angle += ((float)System.Math.Round(angleB, 2) / arcPntsB);
        // }

        lines[0].startColor = qc.getHexColor(lineColor);
        lines[0].endColor = qc.getHexColor(lineColor);
        lines[1].startColor = qc.getHexColor(lineColor);
        lines[1].endColor = qc.getHexColor(lineColor);
        lines[2].startColor = qc.getHexColor(lineColor);
        lines[2].endColor = qc.getHexColor(lineColor);
        lines[3].startColor = qc.getHexColor(lineColor);
        lines[3].endColor = qc.getHexColor(lineColor);

        labelTxt[0].gameObject.SetActive(!hideAngle);
        labelTxt[1].gameObject.SetActive(!hideSideA);
        labelTxt[2].gameObject.SetActive(!hideSideB);
        labelTxt[3].gameObject.SetActive(!hideSideC);
    }
    public void HideValuesOf(bool angle, bool sideA, bool sideB, bool sideC)
    {
        hideAngle = angle;
        hideSideA = sideA;
        hideSideB = sideB;
        hideSideC = sideC;
    }
}
