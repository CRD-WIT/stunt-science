using UnityEngine;
using GameConfig;
using TMPro;

public class AngleAnnotaion : MonoBehaviour
{
    [SerializeField] TextColorMode lineColor;
    float angle, x, y, xSL, ySL, angle1, x1, y1, angle2, x2, y2;
    public float startingAngle, angleA, angleB, angleC, legA, legB, legC, hypotenuse, fontSize = 4, angleLabelOffset, MoE= 0.00001f;
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
        // foreach (var item in lines)
        // {
        //     item.useWorldSpace = false;
        //     // item.bounds.center;
        // }
        lines[1].positionCount = 3;
        lineColor = TextColorMode.Given;
    }
    public void SetSpawnPnt(Vector2 sp)
    {
        spawnPnt = sp;
    }
    void Update()
    {
        LineSetting();
        TextSetting();
    }
    void LineSetting()
    {
        foreach (var item in lines)
        {
            item.enabled = true;
            item.startColor = qc.getHexColor(lineColor);
            item.endColor = qc.getHexColor(lineColor);
        }
        lines[1].SetPosition(1, spawnPnt);
        lines[1].SetPosition(0, new Vector2(legA * Mathf.Sin(startingAngle * Mathf.Deg2Rad) + spawnPnt.x, legA * Mathf.Cos(startingAngle * Mathf.Deg2Rad) + spawnPnt.y));
        AngleA();
        if ((angleA / Mathf.Abs(angleA)) == 1)
        {
            angleC = Mathf.Acos(((legB * legB) - (legC * legC) - (legA * legA)) / (-2 * legC * legA)) * Mathf.Rad2Deg;
            if (((angleC - MoE) == 90) || ((angleC + MoE) == 90))
                angleC = 90;
            angleB = 180 - Mathf.Abs(angleC) - Mathf.Abs(angleA);
        }
        else
        {
            angleC = -Mathf.Acos(((legB * legB) - (legC * legC) - (legA * legA)) / (-2 * legC * legA)) * Mathf.Rad2Deg;
            if (((angleC + MoE) == -90) || ((angleC - MoE) == -90))
                angleC = -90;
            angleB = -(180 - Mathf.Abs(angleC) - Mathf.Abs(angleA));
        }
        lines[1].SetPosition(2, new Vector2(xSL + spawnPnt.x, ySL + spawnPnt.y));
        AngleB();
        AngleC();
    }
    void AngleA()
    {
        float actualAngleA = Mathf.Abs(angleA % 360);
        if (actualAngleA == 90)
        {
            lines[0].positionCount = 3;
            arcPntsA = 2;
            legC = Mathf.Sqrt((legA * legA) + (legB * legB));
            hypotenuse = legC;
        }
        else
        {
            lines[0].enabled = !hideAngleA;
            if (actualAngleA == 270)
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
        angle = startingAngle;
        for (int i = 0; i <= arcPntsA; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle);
            y = Mathf.Cos(Mathf.Deg2Rad * angle);
            xSL = Mathf.Sin(Mathf.Deg2Rad * angle) * legB;
            ySL = Mathf.Cos(Mathf.Deg2Rad * angle) * legB;

            lines[0].SetPosition(i, new Vector2(lines[1].GetPosition(1).x + x, lines[1].GetPosition(1).y + y));
            if (Mathf.Abs(angleA) == 90)
                lines[0].SetPosition(1, new Vector2(x + Mathf.Sin(startingAngle * Mathf.Deg2Rad),
                    y + Mathf.Cos(startingAngle * Mathf.Deg2Rad)));
            else
                lines[0].SetPosition(i, new Vector2(lines[1].GetPosition(1).x + (x*2.5f), lines[1].GetPosition(1).y + (y*2.5f)));
            angle += angleA / arcPntsA;
        }
    }
    void AngleB()
    {
        float actualAngleB = Mathf.Abs(angleB % 360);
        if (actualAngleB == 90)
        {
            lines[2].positionCount = 3;
            arcPntsB = 2;
        }
        else
        {
            lines[2].enabled = !hideAngleB;
            lines[2].positionCount = 61;
            arcPntsB = 60;
        }
        angle1 = startingAngle + angleA - 180;
        for (int i = 0; i <= arcPntsB; i++)
        {
            x1 = Mathf.Sin(Mathf.Deg2Rad * angle1);
            y1 = Mathf.Cos(Mathf.Deg2Rad * angle1);

            lines[2].SetPosition(i, new Vector2(lines[1].GetPosition(2).x + x1, lines[1].GetPosition(2).y + y1));
            if (Mathf.Abs(angleB) == 90)
                lines[2].SetPosition(1, new Vector2(lines[1].GetPosition(2).x + x1 + Mathf.Sin((startingAngle - 180 + angleA) * Mathf.Deg2Rad),
                    lines[1].GetPosition(2).y + y1 + Mathf.Cos((startingAngle - 180 + angleA) * Mathf.Deg2Rad)));
            else 
                lines[2].SetPosition(i, new Vector2(lines[1].GetPosition(2).x + (x1*2.5f), lines[1].GetPosition(2).y + (y1*2.5f)));

            angle1 += angleB / arcPntsB;
        }
    }
    void AngleC()
    {
        float actualAngleC = Mathf.Abs(angleC % 360);
        if (actualAngleC == 90)
        {
            lines[3].positionCount = 3;
            arcPntsC = 2;
        }
        else
        {
            lines[3].enabled = !hideAngleC;
            lines[3].positionCount = 61;
            arcPntsC = 60;
        }
        angle2 = startingAngle - 180 - angleC;
        for (int i = 0; i <= arcPntsC; i++)
        {
            x2 = Mathf.Sin(Mathf.Deg2Rad * angle2);
            y2 = Mathf.Cos(Mathf.Deg2Rad * angle2);

            lines[3].SetPosition(i, new Vector2(lines[1].GetPosition(0).x + x2, lines[1].GetPosition(0).y + y2));
            if (Mathf.Abs(angleC) == 90)
                lines[3].SetPosition(1, new Vector2(lines[1].GetPosition(0).x + x2 + Mathf.Sin((startingAngle + 180 - angleC) * Mathf.Deg2Rad),
                    lines[1].GetPosition(0).y + y2 + Mathf.Cos((startingAngle - 180 - angleC) * Mathf.Deg2Rad)));
            else
                lines[3].SetPosition(i, new Vector2(lines[1].GetPosition(0).x + (x2*2.5f), lines[1].GetPosition(0).y + (y2*2.5f)));
            angle2 += angleC / arcPntsC;
        }
    }
    void TextSetting()
    {
        foreach (var item in labelTxt)
        {
            item.fontSize = fontSize;
            item.color = Color.black;
        }
        labelTxt[0].text = Mathf.Abs(angleA).ToString("f2") + qc.Unit(UnitOf.angle);
        labelTxt[1].text = legA.ToString("f2") + qc.Unit(UnitOf.distance);
        labelTxt[2].text = legB.ToString("f2") + qc.Unit(UnitOf.distance);
        labelTxt[3].text = legC.ToString("f2") + qc.Unit(UnitOf.distance);
        labelTxt[4].text = Mathf.Abs(angleB).ToString("f2") + qc.Unit(UnitOf.angle);
        labelTxt[5].text = Mathf.Abs(angleC).ToString("f2") + qc.Unit(UnitOf.angle);

        labelTxt[0].transform.position = new Vector2(lines[0].GetPosition(arcPntsA / 2).x + angleLabelOffset,
            lines[0].GetPosition(arcPntsA / 2).y + angleLabelOffset);
        labelTxt[1].transform.position = new Vector2(spawnPnt.x + ((legA * Mathf.Sin(startingAngle * Mathf.Deg2Rad)) / 2),
            spawnPnt.y + ((legA * Mathf.Cos(startingAngle * Mathf.Deg2Rad)) / 2));
        labelTxt[2].transform.position = new Vector2(spawnPnt.x + (xSL / 2),
            spawnPnt.y + (ySL / 2));
        labelTxt[3].transform.position = new Vector2(spawnPnt.x + ((legA * Mathf.Sin(startingAngle * Mathf.Deg2Rad) + xSL) / 2),
            spawnPnt.y + ((legA * Mathf.Cos(startingAngle * Mathf.Deg2Rad) + ySL) / 2));
        labelTxt[4].transform.position = new Vector2(lines[2].GetPosition(arcPntsB / 2).x + angleLabelOffset,
            lines[2].GetPosition(arcPntsB / 2).y + angleLabelOffset);
        labelTxt[5].transform.position = new Vector2(lines[3].GetPosition(arcPntsC / 2).x + angleLabelOffset,
            lines[3].GetPosition(arcPntsC / 2).y + angleLabelOffset);

        labelTxt[0].gameObject.SetActive(!hideAngleA);
        labelTxt[1].gameObject.SetActive(!hideSideA);
        labelTxt[2].gameObject.SetActive(!hideSideB);
        labelTxt[3].gameObject.SetActive(!hideSideC);
        labelTxt[4].gameObject.SetActive(!hideAngleB);
        labelTxt[5].gameObject.SetActive(!hideAngleC);
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
