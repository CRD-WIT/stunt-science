using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;
using TMPro;

public class SlantingIndicator : MonoBehaviour
{
    TextColorMode colorMode, distanceColor, velocityColor;
    [SerializeField] float fontSize;

    // [Header("Lines Needed")]
    // [Space(2)]
    // [SerializeField] bool showDistance;
    // [SerializeField] bool showTime;

    [Header("Line Position")]
    [Space(2)]
    [SerializeField] public Vector2 distanceSpawnPnt;
    // [SerializeField] public Vector2 timeSpawnPnt;
    [Header("Line Legnth")]
    [Space(2)]
    [SerializeField] float distance, angle, x, y;
    // [SerializeField] float timerLength;
    float velocity, timer, dimensionTxtLength, timeTxtLength, correctDistanceTxtLength, velocityTxtLength, annotationDistanceEnds, annotationTimeEnds;
    Vector2 velocitySpawnPnt;
    [SerializeField] LineRenderer[] distanceLines = new LineRenderer[4], correctDistanceLines = new LineRenderer[4];
    [SerializeField] GameObject arrow;
    GameObject[] arrows = new GameObject[3];
    [SerializeField] GameObject[] labelTxt = new GameObject[2];
    public bool showCorrectTime = false, showCorrectDistance = false;
    bool arrowPresent = false, answered, revealDistance, revealVelocity;
    char requiredAnswer;
    QuestionControllerVThree qc;
    private void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
    }

    // private Vector2 CorrectIndicatorSpawnPos()
    // {
    //     arrows[5] = Instantiate(arrow);
    //     arrows[6] = Instantiate(arrow);
    //     arrows[5].transform.SetParent(transform.Find("Correct"));
    //     arrows[6].transform.SetParent(transform.Find("Correct"));

    //     arrows[6].transform.Rotate(0, 0, 180);
    //     return new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y - 3);
    // }

    // IEnumerator 
    public void SetDistance(float distance, float angle, float x, float y)
    {
        if (arrowPresent)
        {
            foreach (var item in arrows)
                Destroy(item);
            arrowPresent = false;
            if (!arrowPresent)
            {
                for (int i = 0; i <= 3; i++)
                {
                    arrows[i] = Instantiate(arrow);
                    arrows[i].transform.SetParent(transform.Find("Arrows"));
                }
                arrows[0].transform.Rotate(0, 0, angle);
            }
        }
    }

    // public void showLines(float? dLength, float? tLength, float v, float t)
    // {
    //     if (arrowPresent)
    //         foreach (var item in arrows)
    //             Destroy(item);
    //     arrowPresent = false;
    //     if (!arrowPresent)
    //     {
    //         for (int i = 0; i <= 8; i++)
    //         {
    //             arrows[i] = Instantiate(arrow);
    //             arrows[i].transform.SetParent(transform.Find("UnusedArrows"));
    //         }
    //         if (dLength != null)
    //         {
    //             distance = (float)dLength;

    //             arrows[0].transform.Rotate(0, 0, 180);
    //             arrows[0].transform.SetParent(transform.Find("Distance"));
    //             arrows[1].transform.SetParent(transform.Find("Distance"));

    //             this.showDistance = true;
    //         }
    //         else this.showDistance = false;
    //         if (tLength != null)
    //         {
    //             timerLength = (float)tLength;
    //             timer = t;

    //             arrows[2].transform.Rotate(0, 0, 180);
    //             arrows[2].transform.SetParent(transform.Find("Time"));
    //             // annotationTimeEnds = 0.25f;

    //             this.showTime = true;
    //         }
    //         else
    //         {
    //             this.showTime = false;
    //         }

    //         velocity = v;
    //         arrows[7].transform.SetParent(transform.Find("Velocity"));
    //         arrows[7].transform.Rotate(0, 0, 180);

    //         showCorrectDistance = false;
    //         showCorrectTime = false;

    //         arrows[5].transform.Rotate(0, 0, 180);
    //         arrows[5].transform.SetParent(transform.Find("CorrectDistance"));
    //         arrows[6].transform.SetParent(transform.Find("CorrectDistance"));

    //         arrows[8].transform.Rotate(0, 0, 180);
    //         arrows[8].transform.SetParent(transform.Find("CorrectTime"));

    //         showCorrectTime = false;
    //         showCorrectDistance = false;

    //         annotationTimeEnds = 0.75f;
    //         arrowPresent = true;
    //         distanceColor = TextColorMode.Given;
    //         timeColor = TextColorMode.Given;
    //     }
    // }
    // public void UnknownIs(char? whatIsAsk)
    // {
    //     showCorrectDistance = false;
    //     colorMode = TextColorMode.Given;
    //     answered = false;
    //     if (whatIsAsk != null)
    //     {
    //         if (whatIsAsk == 'v')
    //             revealVelocity = false;
    //         else
    //             revealVelocity = true;
    //         if (whatIsAsk == 'd')
    //             revealDistance = false;
    //         else
    //             revealDistance = true;
    //         if (whatIsAsk == 't')
    //             revealTime = false;
    //         else
    //             revealTime = true;
    //     }
    //     else
    //     {
    //         revealVelocity = true;
    //         revealTime = true;
    //         revealDistance = true;
    //     }
    //     requiredAnswer = (char)whatIsAsk;
    //     distanceColor = colorMode;
    //     timeColor = colorMode;
    //     velocityColor = colorMode;
    // }
    // public void SetPlayerPosition(Vector2 Pnt)
    // {
    //     velocitySpawnPnt = new Vector2(Pnt.x, Pnt.y + 2.5f);
    //     velocityTxtLength = labelTxt[4].GetComponent<TextMeshPro>().text.Length;
    // }
    // public void AnswerIs(bool isCorrect, bool lineRecolor)
    // {
    //     switch (requiredAnswer)
    //     {
    //         case 'v':
    //             if (isCorrect)
    //             {
    //                 velocityColor = TextColorMode.Correct;
    //             }
    //             else
    //             {
    //                 velocityColor = TextColorMode.Wrong;
    //             }
    //             if (lineRecolor)
    //             {
    //                 distanceColor = velocityColor;
    //             }
    //             break;
    //         case 'd':
    //             if (isCorrect)
    //             {
    //                 distanceColor = TextColorMode.Correct;
    //             }
    //             else
    //             {
    //                 distanceColor = TextColorMode.Wrong;
    //             }
    //             if (lineRecolor)
    //             {
    //                 timeColor = distanceColor;
    //             }
    //             break;
    //         case 't':
    //             if (isCorrect)
    //             {
    //                 timeColor = TextColorMode.Correct;
    //                 // distanceColor = TextColorMode.Correct;
    //             }
    //             else
    //             {
    //                 timeColor = TextColorMode.Wrong;
    //                 // distanceColor = TextColorMode.Wrong;
    //             }
    //             if (lineRecolor)
    //             {
    //                 distanceColor = timeColor;
    //             }
    //             break;
    //     }
    // }
    // public void IsRunning(float answer, float distanceCovered, float runTime, float? timeAnnotaionEndPnt)
    // {
    //     distance = distanceCovered;
    //     if (timeAnnotaionEndPnt != null)
    //     {
    //         timerLength = (float)timeAnnotaionEndPnt;
    //     }
    //     else
    //     {
    //         timerLength = distanceCovered;
    //     }
    //     switch (requiredAnswer)
    //     {
    //         case 'v':
    //             velocity = answer;
    //             revealVelocity = true;
    //             break;
    //         case 'd':
    //             distance = answer;
    //             revealDistance = true;
    //             break;
    //         case 't':
    //             timer = answer;
    //             revealTime = true;
    //             break;
    //         default:
    //             Debug.Log("AnswerIs(answer)");
    //             break;
    //     }
    //     answered = true;
    //     timer = runTime;
    //     annotationTimeEnds = -0.75f;
    // }

    // public void ShowVelocityLabel(bool show)
    // {
    //     labelTxt[4].SetActive(show);
    //     if (!show)
    //         arrows[7].transform.SetParent(transform.Find("UnusedArrows"));
    //     else arrows[7].transform.SetParent(transform.Find("Velocity"));

    // }
    private void Update()
    {
        x = Mathf.Sin(Mathf.Deg2Rad * angle) * distance;
        y = Mathf.Cos(Mathf.Deg2Rad * angle) * distance;

        // foreach (var item in distanceLines)
        // {
        //     item.enabled = showDistance;
        // }
        // foreach (var item in timeLines)
        // {
        //     item.enabled = showTime;
        // }

        // foreach (var item in correctTimeLines)
        // {
        //     item.enabled = showCorrectTime;
        // }
        // foreach (var item in correctDistanceLines)
        // {
        //     item.enabled = showCorrectDistance;
        // }
        // labelTxt[0].SetActive(showDistance);/

        // arrows[5].SetActive(showCorrectDistance);
        // arrows[6].SetActive(showCorrectDistance);

        dimensionTxtLength = labelTxt[0].GetComponent<TextMeshPro>().text.Length;
        // timeTxtLength = labelTxt[1].GetComponent<TextMeshPro>().text.Length;
        // velocityTxtLength = labelTxt[4].GetComponent<TextMeshPro>().text.Length;

        arrows[0].transform.position = new Vector2(x, distanceSpawnPnt.y);
        arrows[1].transform.position = new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y);
        distanceLines[0].SetPosition(0, new Vector2(((x / 2) - (0.18f * dimensionTxtLength)) + distanceSpawnPnt.x, ((y / 2) - (0.18f * dimensionTxtLength)) + distanceSpawnPnt.y));
        distanceLines[0].SetPosition(1, new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y));
        distanceLines[1].SetPosition(0, new Vector2(((x / 2) + (0.18f * dimensionTxtLength)) + distanceSpawnPnt.x, ((x / 2) + (0.18f * dimensionTxtLength)) + distanceSpawnPnt.y));
        distanceLines[1].SetPosition(1, new Vector2((x + distanceSpawnPnt.x), distanceSpawnPnt.y));
        distanceLines[2].SetPosition(0, new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y - 1f));
        distanceLines[2].SetPosition(1, new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y + 0.5f));
        distanceLines[3].SetPosition(0, new Vector2(distanceSpawnPnt.x + x, distanceSpawnPnt.y - 1f));
        distanceLines[3].SetPosition(1, new Vector2(distanceSpawnPnt.x + x, distanceSpawnPnt.y + 0.5f));
        labelTxt[0].transform.position = new Vector2((x / 2) + distanceSpawnPnt.x, distanceSpawnPnt.y);

        // arrows[2].transform.position = new Vector2(timerLength + timeSpawnPnt.x, timeSpawnPnt.y + 1);
        // timeLines[0].SetPosition(0, new Vector2(((timerLength / 2) - (0.18f * timeTxtLength)) + timeSpawnPnt.x, timeSpawnPnt.y + 1));//timerLength = distance
        // timeLines[0].SetPosition(1, new Vector2(timeSpawnPnt.x, timeSpawnPnt.y + 1));
        // timeLines[1].SetPosition(0, new Vector2(((timerLength / 2) + (0.18f * timeTxtLength)) + timeSpawnPnt.x, timeSpawnPnt.y + 1));
        // timeLines[1].SetPosition(1, new Vector2((timerLength + timeSpawnPnt.x), timeSpawnPnt.y + 1));
        // timeLines[2].SetPosition(0, new Vector2(timeSpawnPnt.x, timeSpawnPnt.y + annotationTimeEnds));
        // timeLines[2].SetPosition(1, new Vector2(timeSpawnPnt.x, timeSpawnPnt.y + 1.25f));
        // timeLines[3].SetPosition(0, new Vector2(timeSpawnPnt.x + timerLength, timeSpawnPnt.y + annotationTimeEnds));
        // timeLines[3].SetPosition(1, new Vector2(timeSpawnPnt.x + timerLength, timeSpawnPnt.y + 1.25f));
        // labelTxt[1].transform.position = new Vector2((timerLength / 2) + timeSpawnPnt.x, timeSpawnPnt.y + 1);

        labelTxt[1].transform.position = velocitySpawnPnt;
        arrows[2].transform.position = new Vector2(velocitySpawnPnt.x + (0.22f * velocityTxtLength), velocitySpawnPnt.y);

        // if (!revealDistance)
        //     labelTxt[0].GetComponent<TextMeshPro>().SetText($"distance = ?{qc.Unit(UnitOf.distance)}");
        // else
        //     labelTxt[0].GetComponent<TextMeshPro>().SetText($"{System.Math.Round(distance, 2)}{qc.Unit(UnitOf.distance)}");
        // if (!revealTime)
        //     labelTxt[1].GetComponent<TextMeshPro>().SetText($"time = ?{qc.Unit(UnitOf.time)}");
        // else
        //     labelTxt[1].GetComponent<TextMeshPro>().SetText($"{System.Math.Round(timer, 2)}{qc.Unit(UnitOf.time)}");
        // if (!revealVelocity)
        //     labelTxt[4].GetComponent<TextMeshPro>().SetText($"velocity = ?{qc.Unit(UnitOf.velocity)}");
        // else
        //     labelTxt[4].GetComponent<TextMeshPro>().SetText($"{System.Math.Round(velocity, 2)}{qc.Unit(UnitOf.velocity)}");

        // switch (requiredAnswer)
        // {
        //     case 'v':

        //         break;
        // }
        qc.SetColor(labelTxt[1].GetComponent<TMP_Text>(), velocityColor);
        arrows[2].GetComponent<SpriteRenderer>().color = qc.getHexColor(velocityColor);

        qc.SetColor(labelTxt[0].GetComponent<TMP_Text>(), distanceColor);
        distanceLines[0].startColor = qc.getHexColor(distanceColor);
        distanceLines[0].endColor = qc.getHexColor(distanceColor);
        distanceLines[1].startColor = qc.getHexColor(distanceColor);
        distanceLines[1].endColor = qc.getHexColor(distanceColor);
        distanceLines[2].startColor = qc.getHexColor(distanceColor);
        distanceLines[2].endColor = qc.getHexColor(distanceColor);
        distanceLines[3].startColor = qc.getHexColor(distanceColor);
        distanceLines[3].endColor = qc.getHexColor(distanceColor);
        arrows[0].GetComponent<SpriteRenderer>().color = qc.getHexColor(distanceColor);
        arrows[1].GetComponent<SpriteRenderer>().color = qc.getHexColor(distanceColor);

        // qc.SetColor(labelTxt[1].GetComponent<TMP_Text>(), timeColor);
        // timeLines[0].startColor = qc.getHexColor(timeColor);
        // timeLines[0].endColor = qc.getHexColor(timeColor);
        // timeLines[1].startColor = qc.getHexColor(timeColor);
        // timeLines[1].endColor = qc.getHexColor(timeColor);
        // timeLines[2].startColor = qc.getHexColor(timeColor);
        // timeLines[2].endColor = qc.getHexColor(timeColor);
        // timeLines[3].startColor = qc.getHexColor(timeColor);
        // timeLines[3].endColor = qc.getHexColor(timeColor);
        // arrows[2].GetComponent<SpriteRenderer>().color = qc.getHexColor(timeColor);

        foreach (var item in labelTxt)
        {
            item.GetComponent<TextMeshPro>().fontSize = fontSize;
        }
    }
    // public void ShowCorrectDistance(float correctD, bool showCorrect, Vector2 pos)
    // {
    //     showCorrectDistance = showCorrect;
    //     float posY = distanceSpawnPnt.y - pos.y;
    //     foreach (var item in correctDistanceLines)
    //     {
    //         item.enabled = showCorrectDistance;
    //         item.startColor = qc.getHexColor(TextColorMode.Correct);
    //         item.endColor = qc.getHexColor(TextColorMode.Correct);
    //     }
    //     arrows[5].GetComponent<SpriteRenderer>().color = qc.getHexColor(TextColorMode.Correct);
    //     arrows[6].GetComponent<SpriteRenderer>().color = qc.getHexColor(TextColorMode.Correct);
    //     arrows[5].SetActive(showCorrectDistance);
    //     arrows[6].SetActive(showCorrectDistance);
    //     arrows[5].transform.position = new Vector2((correctD + pos.x), posY);
    //     arrows[6].transform.position = new Vector2(pos.x, posY);

    //     labelTxt[3].GetComponent<TextMeshPro>().color = qc.getHexColor(TextColorMode.Correct);
    //     labelTxt[3].SetActive(showCorrectDistance);
    //     labelTxt[3].GetComponent<TextMeshPro>().SetText($"{System.Math.Round(correctD, 2)}{qc.Unit(UnitOf.distance)}");
    //     labelTxt[3].transform.position = new Vector2((correctD / 2) + pos.x, posY);
    //     correctDistanceTxtLength = labelTxt[3].GetComponent<TextMeshPro>().text.Length;

    //     correctDistanceLines[0].SetPosition(0, new Vector2(((correctD / 2) - (0.18f * correctDistanceTxtLength)) + pos.x, posY));
    //     correctDistanceLines[0].SetPosition(1, new Vector2(pos.x, posY));
    //     correctDistanceLines[1].SetPosition(0, new Vector2(((correctD / 2) + (0.18f * correctDistanceTxtLength)) + pos.x, posY));
    //     correctDistanceLines[1].SetPosition(1, new Vector2((correctD + pos.x), posY));
    //     correctDistanceLines[2].SetPosition(0, new Vector2(pos.x, posY - 0.25f));
    //     correctDistanceLines[2].SetPosition(1, new Vector2(pos.x, posY + 0.5f));
    //     correctDistanceLines[3].SetPosition(0, new Vector2(pos.x + correctD, posY - 0.25f));
    //     correctDistanceLines[3].SetPosition(1, new Vector2(pos.x + correctD, posY + 0.5f));
    // }
    // public void ShowCorrectTime(float timer, float correctT, bool showCorrect)
    // {
    //     showCorrectTime = showCorrect;
    //     float posY = timeSpawnPnt.y - 1.2f;
    //     foreach (var item in correctTimeLines)
    //     {
    //         item.enabled = showCorrectTime;
    //         item.startColor = qc.getHexColor(TextColorMode.Correct);
    //         item.endColor = qc.getHexColor(TextColorMode.Correct);
    //     }
    //     arrows[8].GetComponent<SpriteRenderer>().color = qc.getHexColor(TextColorMode.Correct);
    //     arrows[8].SetActive(showCorrectTime);
    //     arrows[8].transform.position = new Vector2((correctT + timeSpawnPnt.x), posY);

    //     labelTxt[5].GetComponent<TextMeshPro>().color = qc.getHexColor(TextColorMode.Correct);
    //     labelTxt[5].SetActive(showCorrectTime);
    //     labelTxt[5].GetComponent<TextMeshPro>().SetText($"{System.Math.Round(timer, 2)}{qc.Unit(UnitOf.time)}");
    //     labelTxt[5].transform.position = new Vector2((correctT / 2) + timeSpawnPnt.x, posY);
    //     float correctTimeTxtLength = labelTxt[3].GetComponent<TextMeshPro>().text.Length;

    //     correctTimeLines[0].SetPosition(0, new Vector2(((correctT / 2) - (0.18f * correctTimeTxtLength)) + timeSpawnPnt.x, posY));
    //     correctTimeLines[0].SetPosition(1, new Vector2(timeSpawnPnt.x, posY));
    //     correctTimeLines[1].SetPosition(0, new Vector2(((correctT / 2) + (0.18f * correctTimeTxtLength)) + timeSpawnPnt.x, posY));
    //     correctTimeLines[1].SetPosition(1, new Vector2((correctT + timeSpawnPnt.x), posY));
    //     correctTimeLines[2].SetPosition(0, new Vector2(timeSpawnPnt.x, posY - 0.25f));
    //     correctTimeLines[2].SetPosition(1, new Vector2(timeSpawnPnt.x, posY + 0.25f));
    //     correctTimeLines[3].SetPosition(0, new Vector2(timeSpawnPnt.x + correctT, posY - 0.25f));
    //     correctTimeLines[3].SetPosition(1, new Vector2(timeSpawnPnt.x + correctT, posY + 0.25f));
    // }
}
