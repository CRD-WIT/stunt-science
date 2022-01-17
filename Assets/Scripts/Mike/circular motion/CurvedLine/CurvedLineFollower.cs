using UnityEngine;
using TMPro;
using GameConfig;

public class CurvedLineFollower : MonoBehaviour
{
    TextColorMode lineColor;
    [SerializeField] TMP_Text angleDegree;
    [SerializeField] LineRenderer line, straightLine;
    public Rigidbody2D gear2;
    float angle, x, y, xSL, ySL;
    public static float arc, stage;
    public static bool? answerIs;
    QuestionController2_0_1 qc;
    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionController2_0_1>();
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
        if (arc != 0)
        {
            angleDegree.text = arc.ToString("f2") + "°";
            if (stage == 1)
            {
                angleDegree.transform.position = new Vector3(line.GetPosition(25).x + 4, line.GetPosition(25).y + 1, 0);
            }
            else if (stage == 2)
            {
                angleDegree.transform.position = new Vector3(line.GetPosition(25).x - 3.8f, line.GetPosition(25).y + 1f, 0);
            }
            else
            {
                angleDegree.transform.position = new Vector3(line.GetPosition(25).x + 7f, line.GetPosition(25).y + 7f, 0);
            }
            straightLine.SetPosition(1, new Vector3(0, 0, 0));
            CreatePoints();
            if (answerIs == true)
                lineColor = TextColorMode.Correct;
            else if (answerIs == false)
                lineColor = TextColorMode.Wrong;
            else
                lineColor = TextColorMode.Given;
        }
        else
        {
            if (stage == 1)
                angleDegree.text = "210°";
            else if (stage == 2)
            {
                angleDegree.rectTransform.position = new Vector2(-6.7f, 2.3f);
                angleDegree.text = "118°";
            }
            else
            {
                if (Level5EasyManager.playerAnswer == 0)
                    angleDegree.rectTransform.position = new Vector2(7.25f, 3.5f);
                else
                    angleDegree.rectTransform.position = new Vector3(5f, 4f, 0);
                angleDegree.text = Level5EasyManager.playerAnswer + "°";
            }
            line.enabled = false;
            straightLine.enabled = false;
            lineColor = TextColorMode.Given;
        }
        qc.SetColor(angleDegree, lineColor);
    }
    void CreatePoints()
    {
        line.enabled = true;
        straightLine.enabled = true;
        switch (stage)
        {
            case 1:
                angle = -120;
                straightLine.SetPosition(0, new Vector3(-5.196152f, -3, 0));
                ForwardArc();
                break;
            case 2:
                angle = -118;
                straightLine.SetPosition(0, new Vector3(-5.297686f, -2.816829f, 0));
                ForwardArc();
                break;
            case 3:
                angle = -180 + Level5EasyManager.playerAnswer;
                ReverseArc();
                straightLine.SetPosition(0, new Vector3(xSL, ySL, 0));
                break;
        }
        line.startColor = qc.getHexColor(lineColor);
        line.endColor = qc.getHexColor(lineColor);
        straightLine.startColor = qc.getHexColor(lineColor);
        straightLine.endColor = qc.getHexColor(lineColor);
    }
    void ReverseArc()
    {
        for (int i = 50; i >= 0; i--)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * 3.5f;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * 3.5f;
            xSL = Mathf.Sin(Mathf.Deg2Rad * angle) * 7f;
            ySL = Mathf.Cos(Mathf.Deg2Rad * angle) * 7f;

            line.SetPosition(i, new Vector3(x, y, 0));
            straightLine.SetPosition(2, new Vector3(xSL, ySL, 0));


            angle -= ((float)System.Math.Round(arc, 2) / 50);
        }
    }
    void ForwardArc()
    {
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
