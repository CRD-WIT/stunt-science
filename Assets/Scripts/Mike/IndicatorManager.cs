using System.Collections.Generic;
using TMPro;
using UnityEngine;
using GameConfig;

public class IndicatorManager : MonoBehaviour
{
    public Vector2 spawnPoint;
    public float fontSize = 4, distance, distanceTraveled, playerVelocity, stuntTime, T;
    public Mode arrowMode;
    public UnitOf whatIsAsk;
    public TextColorMode valueIs;
    public LineRenderer line1, line2, startLine, endLine, correctAnswerIndicator;
    LineRenderer[] linesObj = new LineRenderer[4];
    GameObject textDimension, answer, correctAnswer;
    public GameObject[] verticalArrows = new GameObject[2], horizontalArrows = new GameObject[2];
    // string playerAnswerIs;
    Color32 color;
    private QuestionControllerVThree qc;
    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        line1 = transform.Find("Line1").GetComponent<LineRenderer>();
        line2 = transform.Find("Line2").GetComponent<LineRenderer>();
        correctAnswerIndicator = transform.Find("CorrectAnswerIndicator").GetComponent<LineRenderer>();
        startLine = transform.Find("StartLine").GetComponent<LineRenderer>();
        endLine = transform.Find("EndLine").GetComponent<LineRenderer>();
        textDimension = transform.Find("length").gameObject;
        answer = transform.Find("Answer").gameObject;
        correctAnswer = transform.Find("CorrectAnswer").gameObject;
        linesObj[0] = line1;
        linesObj[1] = line2;
        linesObj[2] = startLine;
        linesObj[3] = endLine;
    }

    // Update is called once per frame
    public void Update()
    {
        float dimensionTextLength = textDimension.GetComponent<TextMeshPro>().text.Length;
        if (arrowMode == Mode.Vertical)
        {
            foreach (var item in verticalArrows)
            {
                item.SetActive(false);
            }

            foreach (var item in horizontalArrows)
            {
                item.SetActive(true);
                item.GetComponent<SpriteRenderer>().color = color;
            }
            horizontalArrows[0].transform.position = new Vector3(spawnPoint.x, (distanceTraveled + spawnPoint.y), 0);
            horizontalArrows[1].transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);
            line1.SetPosition(0, new Vector3(spawnPoint.x, ((distanceTraveled / 2) - 0.4f) + spawnPoint.y, 0));
            line1.SetPosition(1, new Vector3(spawnPoint.x, spawnPoint.y, 0));
            line2.SetPosition(0, new Vector3(spawnPoint.x, ((distanceTraveled / 2) + 0.4f) + spawnPoint.y, 0));
            line2.SetPosition(1, new Vector3(spawnPoint.x, (distanceTraveled + spawnPoint.y), 0));
            startLine.SetPosition(0, new Vector2(spawnPoint.x - 0.25f, spawnPoint.y));
            startLine.SetPosition(1, new Vector2(spawnPoint.x + 0.25f, spawnPoint.y));
            endLine.SetPosition(0, new Vector2(spawnPoint.x - 0.25f, (distanceTraveled + spawnPoint.y)));
            endLine.SetPosition(1, new Vector2(spawnPoint.x + 0.25f, (distanceTraveled + spawnPoint.y)));
            correctAnswerIndicator.SetPosition(0, new Vector2(spawnPoint.x - 0.25f, spawnPoint.y + distance));
            correctAnswerIndicator.SetPosition(1, new Vector2(spawnPoint.x + 1.5f, spawnPoint.y + distance));
            textDimension.transform.position = new Vector3(spawnPoint.x, (distanceTraveled / 2) + spawnPoint.y, 0);
            answer.transform.position = new Vector2(spawnPoint.x - 1, spawnPoint.y + (distanceTraveled));
            correctAnswer.transform.position = new Vector2(spawnPoint.x + 2, (distance) + spawnPoint.y);
        }
        else
        {
            foreach (var item in horizontalArrows)
            {
                item.SetActive(false);
            }
            foreach (var item in verticalArrows)
            {
                item.SetActive(true);
                item.GetComponent<SpriteRenderer>().color = color;
            }
            verticalArrows[0].transform.position = new Vector3((distanceTraveled + spawnPoint.x), spawnPoint.y, 0);
            verticalArrows[1].transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);
            line1.SetPosition(0, new Vector3(((distanceTraveled / 2) - (0.2f * dimensionTextLength)) + spawnPoint.x, spawnPoint.y, 0));
            line1.SetPosition(1, new Vector3(spawnPoint.x, spawnPoint.y, 0));
            line2.SetPosition(0, new Vector3(((distanceTraveled / 2) + (0.2f * dimensionTextLength)) + spawnPoint.x, spawnPoint.y, 0));
            line2.SetPosition(1, new Vector3((distanceTraveled + spawnPoint.x), spawnPoint.y, 0));
            startLine.SetPosition(0, new Vector2(spawnPoint.x, spawnPoint.y - 0.25f));
            startLine.SetPosition(1, new Vector2(spawnPoint.x, spawnPoint.y + 0.25f));
            endLine.SetPosition(0, new Vector2((distanceTraveled + spawnPoint.x), spawnPoint.y - 0.25f));
            endLine.SetPosition(1, new Vector2((distanceTraveled + spawnPoint.x), spawnPoint.y + 0.25f));
            correctAnswerIndicator.SetPosition(0, new Vector2(spawnPoint.x + distance, spawnPoint.y - 0.25f));
            correctAnswerIndicator.SetPosition(1, new Vector2(spawnPoint.x + distance, spawnPoint.y + 1.5f));
            textDimension.transform.position = new Vector3((distanceTraveled / 2) + spawnPoint.x, spawnPoint.y, 0);
            answer.transform.position = new Vector3((distanceTraveled) + spawnPoint.x, spawnPoint.y - 1, 0);
            correctAnswer.transform.position = new Vector3((distance) + spawnPoint.x, spawnPoint.y + 2, 0);
        }
        switch (whatIsAsk)
        {
            case UnitOf.distance:
                answer.GetComponent<TMP_Text>().text = System.Math.Round(distanceTraveled, 2) + qc.Unit(whatIsAsk);
                break;
            case UnitOf.time:
                answer.GetComponent<TMP_Text>().text = System.Math.Round(stuntTime, 2) + qc.Unit(whatIsAsk);
                break;
            case UnitOf.velocity:
                answer.GetComponent<TMP_Text>().text = System.Math.Round(playerVelocity, 2) + qc.Unit(whatIsAsk);
                break;
        }
        answer.GetComponent<TMP_Text>().color = color;
        answer.GetComponent<TMP_Text>().fontSize = fontSize;
        textDimension.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(distanceTraveled, 2)}{qc.Unit(UnitOf.distance)}");
        textDimension.GetComponent<TextMeshPro>().fontSize = fontSize;

        switch (valueIs)
        {
            case TextColorMode.Wrong:
                color = new Color32(255, 0, 0, 255);
                break;
            case TextColorMode.Correct:
                color = new Color32(0, 255, 0, 255);
                break;
            case TextColorMode.Given:
                color = new Color32(128, 0, 128, 255);
                break;
        }
        for (int i = 0; linesObj.Length > i; i++)
        {
            linesObj[i].startColor = color;
            linesObj[i].endColor = color;
        }
        textDimension.GetComponent<TMP_Text>().color = color;
        textDimension.GetComponent<TMP_Text>().fontSize =fontSize;
        correctAnswerIndicator.startColor = new Color32(0, 255, 0, 255);
        correctAnswerIndicator.endColor = new Color32(0, 255, 0, 255);
        correctAnswer.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(distance, 2)}{qc.Unit(UnitOf.distance)}");
        correctAnswer.GetComponent<TextMeshPro>().fontSize = fontSize;

        string longprob = $"<color={qc.getHexColor(TextColorMode.Given)}>sample</color>";
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