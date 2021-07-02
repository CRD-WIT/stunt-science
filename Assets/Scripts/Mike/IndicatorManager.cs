using System.Collections.Generic;
using TMPro;
using UnityEngine;
using GameConfig;

public class IndicatorManager : MonoBehaviour
{
    public Vector2 spawnPoint;
    public float fontSize = 6, distance, distanceTraveled, playerVelocity, stuntTime, T;
    public UnitOf whatIsAsk;
    public TextColorMode valueIs;
    public LineRenderer line1, line2, startLine, endLine;
    LineRenderer[] linesObj = new LineRenderer[4];
    GameObject textDimension, answer, correctAnswer;
    public GameObject[] arrows = new GameObject[2];
    // string playerAnswerIs;
    Color32 color;
    private QuestionControllerVThree qc;
    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        line1 = transform.Find("Line1").GetComponent<LineRenderer>();
        line2 = transform.Find("Line2").GetComponent<LineRenderer>();
        startLine = transform.Find("StartLine").GetComponent<LineRenderer>();
        endLine = transform.Find("EndLine").GetComponent<LineRenderer>();
        textDimension = transform.Find("length").gameObject;
        linesObj[0] = line1;
        linesObj[1] = line2;
        linesObj[2] = startLine;
        linesObj[3] = endLine;
    }

    // Update is called once per frame
    public void Update()
    {
        float dimensionTextLength = textDimension.GetComponent<TextMeshPro>().text.Length;
        foreach (var item in arrows)
        {
            item.SetActive(true);
            item.GetComponent<SpriteRenderer>().color = color;
        }
        arrows[0].transform.position = new Vector2(spawnPoint.x, spawnPoint.y);
        arrows[1].transform.position = new Vector2((distance + spawnPoint.x), spawnPoint.y);
        line1.SetPosition(0, new Vector2(((distance / 2) - (0.2f * dimensionTextLength)) + spawnPoint.x, spawnPoint.y));
        line1.SetPosition(1, new Vector2((distance + spawnPoint.x), spawnPoint.y));
        line2.SetPosition(0, new Vector2(((distance / 2) + (0.2f * dimensionTextLength)) + spawnPoint.x, spawnPoint.y));
        line2.SetPosition(1, new Vector2(spawnPoint.x, spawnPoint.y));
        startLine.SetPosition(0, new Vector2(spawnPoint.x, spawnPoint.y - 0.25f));
        startLine.SetPosition(1, new Vector2(spawnPoint.x, spawnPoint.y + 0.25f));
        endLine.SetPosition(0, new Vector2((distance + spawnPoint.x), spawnPoint.y - 0.25f));
        endLine.SetPosition(1, new Vector2((distance + spawnPoint.x), spawnPoint.y + 0.25f));
        textDimension.transform.position = new Vector2((distance / 2) + spawnPoint.x, spawnPoint.y);
        textDimension.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(Mathf.Abs(distance), 2)}{qc.Unit(UnitOf.distance)}");

        color = qc.getHexColor(TextColorMode.Given);
        for (int i = 0; linesObj.Length > i; i++)
        {
            linesObj[i].startColor = color;
            linesObj[i].endColor = color;
        }
        textDimension.GetComponent<TMP_Text>().color = color;
        textDimension.GetComponent<TMP_Text>().fontSize = fontSize;
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }

    public Vector2 SpawnPointValue()
    {
        return spawnPoint;
    }

    public void SetSpawningPoint(Vector2 point)
    {
        this.spawnPoint = point;
    }

    public void SetDistance(float value)
    {
        this.distanceTraveled = Mathf.Abs(value);
    }

}