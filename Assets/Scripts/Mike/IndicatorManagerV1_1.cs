using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;
using TMPro;

public class IndicatorManagerV1_1 : MonoBehaviour
{
    TextColorMode colorMode;
    [SerializeField] float fontSize;
    public Mode correctlengthOrientation;

    [Header("Lines Needed")]
    [Space(3)]
    [SerializeField] bool showDistance;
    [SerializeField] bool showTime;
    [SerializeField] bool showHeight;

    [Header("Line Position")]
    [Space(3)]
    [SerializeField] public Vector2 distanceSpawnPnt;
    [SerializeField] public Vector2 timeSpawnPnt;
    [SerializeField] public Vector2 heightSpawnPnt;
    [Header("Line Legnth")]
    [Space(3)]
    [SerializeField] float distance;
    [SerializeField] float timerLength;
    [SerializeField] float height;
    float velocity, timer;
    Vector2 velocitySpawnPnt;
    LineRenderer[] distanceLines = new LineRenderer[4];
    LineRenderer[] timeLines = new LineRenderer[4];
    LineRenderer[] heightlines = new LineRenderer[4];
    LineRenderer[] correctAnswerLines = new LineRenderer[4];
    [SerializeField] GameObject arrow;
    GameObject[] arrows, labelTxt;
    bool arrowPresent = false, answered, revealTime, revealDistance, revealVelocity, revealHeight, showCorrectAnswer;
    char requiredAnswer;
    QuestionControllerVThree qc;
    private void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();

        distanceLines[0] = transform.Find("Distance").Find("line1").GetComponent<LineRenderer>();
        distanceLines[1] = transform.Find("Distance").Find("line2").GetComponent<LineRenderer>();
        distanceLines[2] = transform.Find("Distance").Find("line3").GetComponent<LineRenderer>();
        distanceLines[3] = transform.Find("Distance").Find("line4").GetComponent<LineRenderer>();

        timeLines[0] = transform.Find("Time").Find("line1").GetComponent<LineRenderer>();
        timeLines[1] = transform.Find("Time").Find("line2").GetComponent<LineRenderer>();
        timeLines[2] = transform.Find("Time").Find("line3").GetComponent<LineRenderer>();
        timeLines[3] = transform.Find("Time").Find("line4").GetComponent<LineRenderer>();

        heightlines[0] = transform.Find("Height").Find("line1").GetComponent<LineRenderer>();
        heightlines[1] = transform.Find("Height").Find("line2").GetComponent<LineRenderer>();
        heightlines[2] = transform.Find("Height").Find("line3").GetComponent<LineRenderer>();
        heightlines[3] = transform.Find("Height").Find("line4").GetComponent<LineRenderer>();

        correctAnswerLines[0] = transform.Find("Correct").Find("line1").GetComponent<LineRenderer>();
        correctAnswerLines[1] = transform.Find("Correct").Find("line2").GetComponent<LineRenderer>();
        correctAnswerLines[2] = transform.Find("Correct").Find("line3").GetComponent<LineRenderer>();
        correctAnswerLines[3] = transform.Find("Correct").Find("line4").GetComponent<LineRenderer>();

        labelTxt[0] = transform.Find("Distance").Find("Label").gameObject;
        labelTxt[1] = transform.Find("Time").Find("Label").gameObject;
        labelTxt[2] = transform.Find("Height").Find("Label").gameObject;
        labelTxt[3] = transform.Find("Correct").Find("Label").gameObject;
        labelTxt[4] = transform.Find("Velocity").Find("Label").gameObject;
    }

    private Vector2 CorrectIndicatorSpawnPos()
    {
        arrows[5] = Instantiate(arrow);
        arrows[6] = Instantiate(arrow);
        arrows[5].transform.SetParent(transform.Find("Correct"));
        arrows[6].transform.SetParent(transform.Find("Correct"));
        if (correctlengthOrientation == Mode.Vertical)
        {
            arrows[5].transform.Rotate(0, 0, 90);
            arrows[6].transform.Rotate(0, 0, -90);

            return new Vector2(heightSpawnPnt.x - 3, heightSpawnPnt.y);
        }
        else
        {
            arrows[6].transform.Rotate(0, 0, 180);
            return new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y - 3);
        }
    }

    public void showLines(Vector2? dLength, Vector2? tLength, Vector2? hLength)
    {
        if (arrowPresent)
            Destroy(arrow);
        else
        {
            if (dLength != null)
            {
                arrows[0] = Instantiate(arrow);
                arrows[1] = Instantiate(arrow);

                arrows[1].transform.Rotate(0, 0, 180);
                arrows[0].transform.SetParent(transform.Find("Distance"));
                arrows[1].transform.SetParent(transform.Find("Distance"));

                this.showDistance = true;
            }
            else this.showDistance = false;
            if (tLength != null)
            {
                arrows[2] = Instantiate(arrow);

                arrows[2].transform.Rotate(0, 0, 180);
                arrows[2].transform.SetParent(transform.Find("Time"));
                this.showTime = true;
            }
            else this.showTime = false;
            if (hLength != null)
            {
                arrows[3] = Instantiate(arrow);
                arrows[4] = Instantiate(arrow);

                arrows[3].transform.Rotate(0, 0, 90);
                arrows[4].transform.Rotate(0, 0, -90);
                arrows[3].transform.SetParent(transform.Find("Height"));
                arrows[4].transform.SetParent(transform.Find("Height"));

                this.showHeight = true;
            }
            else this.showHeight = false;
            arrows[7] = Instantiate(arrow);
            arrows[7].transform.SetParent(transform.Find("Velocity"));
            arrows[7].transform.Rotate(0, 0, 180);
            arrowPresent = true;
        }
    }
    public void Variables(float d, float t, float v, float h, char? whatIsAsk, float? endPntOfTime)
    {
        showCorrectAnswer = false;
        colorMode = TextColorMode.Given;
        answered = false;
        if (whatIsAsk != null)
        {
            if (whatIsAsk == 'v')
                revealVelocity = false;
            else
                revealVelocity = true;
            if (whatIsAsk == 'd')
                revealDistance = false;
            else
                revealDistance = true;
            if (whatIsAsk == 't')
                revealTime = false;
            else
                revealTime = true;
            if (whatIsAsk == 'h')
                revealHeight = false;
            else
                revealHeight = true;
        }
        else
        {
            revealVelocity = true;
            revealTime = true;
            revealDistance = true;
            revealHeight = true;
        }
        requiredAnswer = (char)whatIsAsk;
        distance = d;
        if (endPntOfTime != null)
            timerLength = (float)endPntOfTime;
        else
            timerLength = distance;
        height = h;
        timer = t;
        velocity = v;
    }
    public void SetPlayerPosition(Vector2 Pnt)
    {
        velocitySpawnPnt = new Vector2(Pnt.x, Pnt.y + 3);
    }

    private void Update()
    {
        foreach (var item in distanceLines)
        {
            item.enabled = showDistance;
        }
        foreach (var item in timeLines)
        {
            item.enabled = showTime;
        }
        foreach (var item in heightlines)
        {
            item.enabled = showHeight;
        }
        foreach (var item in correctAnswerLines)
        {
            item.enabled = showCorrectAnswer;
        }

        float dimensionTxtLength = labelTxt[0].GetComponent<TextMeshPro>().text.Length,
        timeTxtLength = labelTxt[1].GetComponent<TextMeshPro>().text.Length,
        heightTxtLength = labelTxt[2].GetComponent<TextMeshPro>().text.Length,
        correctTxtLength = labelTxt[3].GetComponent<TextMeshPro>().text.Length,
        velocityTxtLength = labelTxt[4].GetComponent<TextMeshPro>().text.Length;

        arrows[0].transform.position = new Vector2((distance + distanceSpawnPnt.x), distanceSpawnPnt.y);
        arrows[1].transform.position = new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y);
        distanceLines[0].SetPosition(0, new Vector2(((distance / 2) - (0.2f * dimensionTxtLength)) + distanceSpawnPnt.x, distanceSpawnPnt.y));
        distanceLines[0].SetPosition(1, new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y));
        distanceLines[1].SetPosition(0, new Vector2(((distance / 2) + (0.2f * dimensionTxtLength)) + distanceSpawnPnt.x, distanceSpawnPnt.y));
        distanceLines[1].SetPosition(1, new Vector2((distance + distanceSpawnPnt.x), distanceSpawnPnt.y));
        distanceLines[2].SetPosition(0, new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y - 1));
        distanceLines[2].SetPosition(1, new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y + 0.5f));
        distanceLines[3].SetPosition(0, new Vector2(distanceSpawnPnt.x + distance, distanceSpawnPnt.y - 1));
        distanceLines[3].SetPosition(1, new Vector2(distanceSpawnPnt.x + distance, distanceSpawnPnt.y + 0.5f));
        labelTxt[0].transform.position = new Vector2((distance / 2) + distanceSpawnPnt.x, distanceSpawnPnt.y);

        arrows[2].transform.position = new Vector2(timerLength + timeSpawnPnt.x, arrows[0].transform.position.y + 1);
        timeLines[0].SetPosition(0, new Vector2(((timerLength / 2) - (0.2f * timeTxtLength)) + timeSpawnPnt.x, timeSpawnPnt.y + 1));//timerLength = distance
        timeLines[0].SetPosition(1, new Vector2(timeSpawnPnt.x, timeSpawnPnt.y + 1));
        timeLines[1].SetPosition(0, new Vector2(((timerLength / 2) + (0.2f * timeTxtLength)) + timeSpawnPnt.x, timeSpawnPnt.y + 1));
        timeLines[1].SetPosition(1, new Vector2((timerLength + timeSpawnPnt.x), timeSpawnPnt.y + 1));
        timeLines[2].SetPosition(0, new Vector2(timeSpawnPnt.x, timeSpawnPnt.y + .75f));
        timeLines[2].SetPosition(1, new Vector2(timeSpawnPnt.x, timeSpawnPnt.y + 1.25f));
        timeLines[3].SetPosition(0, new Vector2(timeSpawnPnt.x + timerLength, timeSpawnPnt.y + .75f));
        timeLines[3].SetPosition(1, new Vector2(timeSpawnPnt.x + timerLength, timeSpawnPnt.y + 1.25f));
        labelTxt[1].transform.position = new Vector2((timerLength / 2) + timeSpawnPnt.x, timeSpawnPnt.y + 1);

        arrows[7].transform.position = new Vector2(velocitySpawnPnt.x + (0.2f * velocityTxtLength), velocitySpawnPnt.y + 3f);
        labelTxt[4].transform.position = velocitySpawnPnt;
        if (!revealDistance)
            labelTxt[0].GetComponent<TextMeshPro>().SetText($"distance = ?{qc.Unit(UnitOf.distance)}");
        else
            labelTxt[0].GetComponent<TextMeshPro>().SetText($"{System.Math.Round(distance, 2)}{qc.Unit(UnitOf.distance)}");
        if (!revealTime)
            labelTxt[1].GetComponent<TextMeshPro>().SetText($"time = ?{qc.Unit(UnitOf.time)}");
        else
            labelTxt[1].GetComponent<TextMeshPro>().SetText($"{System.Math.Round(timer, 2)}{qc.Unit(UnitOf.time)}");
        if (!revealVelocity)
            labelTxt[4].GetComponent<TextMeshPro>().SetText($"velocity = ?{qc.Unit(UnitOf.velocity)}");
        else
            labelTxt[4].GetComponent<TextMeshPro>().SetText($"{System.Math.Round(velocity, 2)}{qc.Unit(UnitOf.velocity)}");
        // }
        // else
        // {
        //     arrows[0].transform.position = new Vector2((distanceTraveled + spawnPnt.x), spawnPnt.y);
        //     arrows[1].transform.position = new Vector2(spawnPnt.x, spawnPnt.y);
        //     distanceLine1.SetPosition(0, new Vector2(((distanceTraveled / 2) - (0.2f * dimensionTextLength)) + spawnPnt.x, spawnPnt.y));
        //     distanceLine1.SetPosition(1, new Vector2(spawnPnt.x, spawnPnt.y));
        //     distanceLine2.SetPosition(0, new Vector2(((distanceTraveled / 2) + (0.2f * dimensionTextLength)) + spawnPnt.x, spawnPnt.y));
        //     distanceLine2.SetPosition(1, new Vector2((distanceTraveled + spawnPnt.x), spawnPnt.y));
        //     startlength.SetPosition(0, new Vector2(spawnPnt.x, spawnPnt.y - 1));
        //     startlength.SetPosition(1, new Vector2(spawnPnt.x, spawnPnt.y - 0.5f));
        //     endLength.SetPosition(0, new Vector2(spawnPnt.x + distanceTraveled, spawnPnt.y - 1));
        //     endLength.SetPosition(1, new Vector2(spawnPnt.x + distanceTraveled, spawnPnt.y - 0.5f));
        //     distanceIndicatorTxt.transform.position = new Vector2((distanceTraveled / 2) + spawnPnt.x, spawnPnt.y);

        //     arrows[2].transform.position = new Vector2(lengthOfTime + timeStartPnt, spawnPnt.y);
        //     timeLines[].SetPosition(0, new Vector2(((lengthOfTime / 2) - (0.2f * timeTextLength)) + timeStartPnt, spawnPnt.y));
        //     timeLines[].SetPosition(1, new Vector2(timeStartPnt, spawnPnt.y));
        //     timeLine2.SetPosition(0, new Vector2(((lengthOfTime / 2) + (0.2f * timeTextLength)) + timeStartPnt, spawnPnt.y));
        //     timeLine2.SetPosition(1, new Vector2((lengthOfTime + timeStartPnt), spawnPnt.y));
        //     endTime.SetPosition(0, new Vector2(timeStartPnt + lengthOfTime, spawnPnt.y + .75f));
        //     endTime.SetPosition(1, new Vector2(timeStartPnt + lengthOfTime, spawnPnt.y + 1.25f));
        //     labelTxt[1].transform.position = new Vector2((lengthOfTime / 2) + timeStartPnt, spawnPnt.y);

        //     labelTxt[1].GetComponent<TextMeshPro>().SetText($"{System.Math.Round(elapsedTime, 2)}{qc.Unit(UnitOf.time)}");
        //     velocityIndicatorTxt.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(velocity, 2)}{qc.Unit(UnitOf.velocity)}");
        //     distanceIndicatorTxt.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(distanceTraveled, 2)}{qc.Unit(UnitOf.distance)}");
        // }

        // arrows[3].transform.position = new Vector2(veloTxtPnt.x + 1.5f, veloTxtPnt.y + 3f);
        // velocityIndicatorTxt.transform.position = new Vector2(veloTxtPnt.x, veloTxtPnt.y + 3);


        qc.SetColor(labelTxt[4].GetComponent<TMP_Text>(), colorMode);

        qc.SetColor(labelTxt[0].GetComponent<TMP_Text>(), colorMode);
        distanceLines[0].startColor = qc.getHexColor(colorMode);
        distanceLines[0].endColor = qc.getHexColor(colorMode);
        distanceLines[1].startColor = qc.getHexColor(colorMode);
        distanceLines[1].endColor = qc.getHexColor(colorMode);
        distanceLines[2].startColor = qc.getHexColor(colorMode);
        distanceLines[2].endColor = qc.getHexColor(colorMode);
        distanceLines[3].startColor = qc.getHexColor(colorMode);
        distanceLines[3].endColor = qc.getHexColor(colorMode);
        arrows[0].GetComponent<SpriteRenderer>().color = qc.getHexColor(colorMode);
        arrows[1].GetComponent<SpriteRenderer>().color = qc.getHexColor(colorMode);

        qc.SetColor(labelTxt[1].GetComponent<TMP_Text>(), colorMode);
        timeLines[0].startColor = qc.getHexColor(colorMode);
        timeLines[0].endColor = qc.getHexColor(colorMode);
        timeLines[1].startColor = qc.getHexColor(colorMode);
        timeLines[1].endColor = qc.getHexColor(colorMode);
        timeLines[2].startColor = qc.getHexColor(colorMode);
        timeLines[2].endColor = qc.getHexColor(colorMode);
        timeLines[3].startColor = qc.getHexColor(colorMode);
        timeLines[3].endColor = qc.getHexColor(colorMode);
        arrows[2].GetComponent<SpriteRenderer>().color = qc.getHexColor(colorMode);

        // startlength.startColor = qc.getHexColor(colorMode);
        // startlength.endColor = qc.getHexColor(colorMode);
        // endLength.startColor = qc.getHexColor(colorMode);
        // endLength.endColor = qc.getHexColor(colorMode);

        foreach (var item in labelTxt)
        {
            item.GetComponent<TextMeshPro>().fontSize = fontSize;
        }
    }
    IEnumerator DestroyArrows()
    {
        Destroy(arrow);
        yield return new WaitForEndOfFrame();
        arrowPresent = false;
    }

    //     [SerializeField] float distance, time, velocity, distanceTraveled, lengthOfTime, elapsedTime;
    //     public float fontSize = 4;
    //     LineRenderer distanceLine1, distanceLine2, timeLine1, timeLine2, startlength, endLength, endTime;
    //     GameObject distanceIndicatorTxt, labelTxt[1], velocityIndicatorTxt;
    //     [SerializeField] bool revealDistance = true, revealTime = true, revealVelocity = true;
    //     public GameObject[] arrows = new GameObject[4];
    //     public float timeStartPnt, timeEndPnt;
    //     char required;
    //     bool answered, isAnswerCorrect;
    //     Vector2 veloTxtPnt;
    //     QuestionControllerVThree qc;

    //     // Start is called before the first frame update
    //     void Start()
    //     {
    //         qc = FindObjectOfType<QuestionControllerVThree>();
    //         distanceLine1 = transform.Find("DistanceLine1").GetComponent<LineRenderer>();
    //         distanceLine2 = transform.Find("DistanceLine2").GetComponent<LineRenderer>();
    //         timeLine1 = transform.Find("TimeLine1").GetComponent<LineRenderer>();
    //         timeLine2 = transform.Find("TimeLine2").GetComponent<LineRenderer>();
    //         startlength = transform.Find("StartAnnotationLine").GetComponent<LineRenderer>();
    //         endLength = transform.Find("EndAnnotationLine").GetComponent<LineRenderer>();
    //         endTime = transform.Find("EndAnnotationTime").GetComponent<LineRenderer>();
    //         distanceIndicatorTxt = transform.Find("Distance").gameObject;
    //         labelTxt[1] = transform.Find("Time").gameObject;
    //         velocityIndicatorTxt = transform.Find("Velocity").gameObject;
    //     }
    //     public void ShowTimeAnnotation(bool show)
    //     {
    //         timeLine1.gameObject.SetActive(show);
    //         timeLine2.gameObject.SetActive(show);
    //         labelTxt[1].gameObject.SetActive(show);
    //         endTime.gameObject.SetActive(show);
    //         arrows[2].gameObject.SetActive(show);
    //     }
    //     public void ShowDistanceAnnotation(bool show)
    //     {
    //         distanceLine1.gameObject.SetActive(show);
    //         distanceLine2.gameObject.SetActive(show);
    //         distanceIndicatorTxt.gameObject.SetActive(show);
    //         startlength.gameObject.SetActive(show);
    //         endLength.gameObject.SetActive(show);
    //         arrows[0].gameObject.SetActive(show);
    //         arrows[1].gameObject.SetActive(show);
    //     }
    //     public void Show(bool s)
    //     {
    //         foreach (var item in arrows)
    //         {
    //             item.SetActive(s);
    //         }
    //         distanceLine1.gameObject.SetActive(s);
    //         distanceLine2.gameObject.SetActive(s);
    //         distanceIndicatorTxt.SetActive(s);

    //         timeLine1.gameObject.SetActive(s);
    //         timeLine2.gameObject.SetActive(s);
    //         labelTxt[1].SetActive(s);

    //         velocityIndicatorTxt.SetActive(s);

    //         startlength.gameObject.SetActive(s);
    //         endLength.gameObject.SetActive(s);
    //         endTime.gameObject.SetActive(s);
    //     }
    //     public void SetPlayerPosition(Vector2 Pnt)
    //     {
    //         veloTxtPnt = Pnt;
    //     }
    //     public void Variables(float d, float t, float v, char whatIsAsk, float? EndPntOfTime)
    //     {
    //         colorMode = TextColorMode.Given;
    //         answered = false;
    //         switch (whatIsAsk)
    //         {
    //             case 'v':
    //                 revealVelocity = false;
    //                 revealTime = true;
    //                 revealDistance = true;
    //                 break;
    //             case 'd':
    //                 revealVelocity = true;
    //                 revealTime = true;
    //                 revealDistance = false;
    //                 break;
    //             case 't':
    //                 revealVelocity = true;
    //                 revealTime = false;
    //                 revealDistance = true;
    //                 break;
    //             case 'N':
    //                 revealVelocity = true;
    //                 revealTime = true;
    //                 revealDistance = true;
    //                 break;
    //             default:
    //                 Debug.Log("<color=yellow>Variables</color>(distance value, time value, velocity value, symbol{d, v, t} if all value is revealed put 'N')");
    //                 break;
    //         }
    //         required = whatIsAsk;
    //         distance = d;
    //         if (EndPntOfTime != null)
    //             timeEndPnt = (float)EndPntOfTime;
    //         else
    //             timeEndPnt = distance;
    //         time = t;
    //         velocity = v;
    //     }
    //     public void AnswerIs(bool isCorrect)
    //     {
    //         if (isCorrect)
    //         {
    //             colorMode = TextColorMode.Correct;
    //         }
    //         else
    //         {
    //             colorMode = TextColorMode.Wrong;
    //         }
    //     }
    //     public void IsRunning(float distanceCovered, float runTime, float? timeAnnotaionEndPnt)
    //     {
    //         answered = true;
    //         distanceTraveled = distanceCovered;
    //         elapsedTime = runTime;
    //         if (timeAnnotaionEndPnt != null)
    //         {
    //             lengthOfTime = (float)timeAnnotaionEndPnt;
    //         }
    //         else
    //         {
    //             lengthOfTime = distanceCovered;
    //         }
    //     }
    //     public void TimeHasDifferentPnts(bool yes)
    //     {

    //     }
    //     public void PlayerAnswerIs(float answer)
    //     {
    //         // answered = false;
    //         switch (required)
    //         {
    //             case 'v':
    //                 velocity = answer;
    //                 revealVelocity = true;
    //                 break;
    //             case 'd':
    //                 distance = answer;
    //                 revealDistance = true;
    //                 break;
    //             case 't':
    //                 time = answer;
    //                 revealTime = true;
    //                 break;
    //             default:
    //                 Debug.Log("AnswerIs(answer)");
    //                 break;
    //         }
    //     }
    //     // Update is called once per frame
    //     void Update()
    //     {
    //         float dimensionTextLength = distanceIndicatorTxt.GetComponent<TextMeshPro>().text.Length;
    //         float timeTextLength = labelTxt[1].GetComponent<TextMeshPro>().text.Length;
    //         if (!answered)
    //         {
    //             arrows[0].transform.position = new Vector2((distance + spawnPnt.x), spawnPnt.y);
    //             arrows[1].transform.position = new Vector2(spawnPnt.x, spawnPnt.y);
    //             distanceLine1.SetPosition(0, new Vector2(((distance / 2) - (0.2f * dimensionTextLength)) + spawnPnt.x, spawnPnt.y));
    //             distanceLine1.SetPosition(1, new Vector2(spawnPnt.x, spawnPnt.y));
    //             distanceLine2.SetPosition(0, new Vector2(((distance / 2) + (0.2f * dimensionTextLength)) + spawnPnt.x, spawnPnt.y));
    //             distanceLine2.SetPosition(1, new Vector2((distance + spawnPnt.x), spawnPnt.y));
    //             startlength.SetPosition(0, new Vector2(spawnPnt.x, spawnPnt.y - 1));
    //             startlength.SetPosition(1, new Vector2(spawnPnt.x, spawnPnt.y + 0.5f));
    //             endLength.SetPosition(0, new Vector2(spawnPnt.x + distance, spawnPnt.y - 1));
    //             endLength.SetPosition(1, new Vector2(spawnPnt.x + distance, spawnPnt.y + 0.5f));
    //             distanceIndicatorTxt.transform.position = new Vector2((distance / 2) + spawnPnt.x, spawnPnt.y);

    //             arrows[2].transform.position = new Vector2(timeEndPnt + timeStartPnt, arrows[0].transform.position.y + 1);
    //             timeLine1.SetPosition(0, new Vector2(((timeEndPnt / 2) - (0.2f * timeTextLength)) + timeStartPnt, spawnPnt.y + 1));//timeEndPnt = distance
    //             timeLine1.SetPosition(1, new Vector2(timeStartPnt, spawnPnt.y + 1));
    //             timeLine2.SetPosition(0, new Vector2(((timeEndPnt / 2) + (0.2f * timeTextLength)) + timeStartPnt, spawnPnt.y + 1));
    //             timeLine2.SetPosition(1, new Vector2((timeEndPnt + timeStartPnt), spawnPnt.y + 1));
    //             endTime.SetPosition(0, new Vector2(timeStartPnt + timeEndPnt, spawnPnt.y + .75f));
    //             endTime.SetPosition(1, new Vector2(timeStartPnt + timeEndPnt, spawnPnt.y + 1.25f));
    //             labelTxt[1].transform.position = new Vector2((timeEndPnt / 2) + timeStartPnt, spawnPnt.y + 1);

    //             arrows[3].transform.position = new Vector2(veloTxtPnt.x + 1.5f, veloTxtPnt.y + 3f);
    //             velocityIndicatorTxt.transform.position = new Vector2(veloTxtPnt.x, veloTxtPnt.y + 3);
    //             if (!revealDistance)
    //                 distanceIndicatorTxt.GetComponent<TextMeshPro>().SetText($"?{qc.Unit(UnitOf.distance)}");
    //             else
    //                 distanceIndicatorTxt.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(distance, 2)}{qc.Unit(UnitOf.distance)}");
    //             if (!revealTime)
    //                 labelTxt[1].GetComponent<TextMeshPro>().SetText($"?{qc.Unit(UnitOf.time)}");
    //             else
    //                 labelTxt[1].GetComponent<TextMeshPro>().SetText($"{System.Math.Round(time, 2)}{qc.Unit(UnitOf.time)}");
    //             if (!revealVelocity)
    //                 velocityIndicatorTxt.GetComponent<TextMeshPro>().SetText($"?{qc.Unit(UnitOf.velocity)}");
    //             else
    //                 velocityIndicatorTxt.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(velocity, 2)}{qc.Unit(UnitOf.velocity)}");
    //         }
    //         else
    //         {
    //             arrows[0].transform.position = new Vector2((distanceTraveled + spawnPnt.x), spawnPnt.y);
    //             arrows[1].transform.position = new Vector2(spawnPnt.x, spawnPnt.y);
    //             distanceLine1.SetPosition(0, new Vector2(((distanceTraveled / 2) - (0.2f * dimensionTextLength)) + spawnPnt.x, spawnPnt.y));
    //             distanceLine1.SetPosition(1, new Vector2(spawnPnt.x, spawnPnt.y));
    //             distanceLine2.SetPosition(0, new Vector2(((distanceTraveled / 2) + (0.2f * dimensionTextLength)) + spawnPnt.x, spawnPnt.y));
    //             distanceLine2.SetPosition(1, new Vector2((distanceTraveled + spawnPnt.x), spawnPnt.y));
    //             startlength.SetPosition(0, new Vector2(spawnPnt.x, spawnPnt.y - 1));
    //             startlength.SetPosition(1, new Vector2(spawnPnt.x, spawnPnt.y - 0.5f));
    //             endLength.SetPosition(0, new Vector2(spawnPnt.x + distanceTraveled, spawnPnt.y - 1));
    //             endLength.SetPosition(1, new Vector2(spawnPnt.x + distanceTraveled, spawnPnt.y - 0.5f));
    //             distanceIndicatorTxt.transform.position = new Vector2((distanceTraveled / 2) + spawnPnt.x, spawnPnt.y);

    //             arrows[2].transform.position = new Vector2(lengthOfTime + timeStartPnt, spawnPnt.y);
    //             timeLine1.SetPosition(0, new Vector2(((lengthOfTime / 2) - (0.2f * timeTextLength)) + timeStartPnt, spawnPnt.y));
    //             timeLine1.SetPosition(1, new Vector2(timeStartPnt, spawnPnt.y));
    //             timeLine2.SetPosition(0, new Vector2(((lengthOfTime / 2) + (0.2f * timeTextLength)) + timeStartPnt, spawnPnt.y));
    //             timeLine2.SetPosition(1, new Vector2((lengthOfTime + timeStartPnt), spawnPnt.y));
    //             endTime.SetPosition(0, new Vector2(timeStartPnt + lengthOfTime, spawnPnt.y + .75f));
    //             endTime.SetPosition(1, new Vector2(timeStartPnt + lengthOfTime, spawnPnt.y + 1.25f));
    //             labelTxt[1].transform.position = new Vector2((lengthOfTime / 2) + timeStartPnt, spawnPnt.y);

    //             labelTxt[1].GetComponent<TextMeshPro>().SetText($"{System.Math.Round(elapsedTime, 2)}{qc.Unit(UnitOf.time)}");
    //             velocityIndicatorTxt.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(velocity, 2)}{qc.Unit(UnitOf.velocity)}");
    //             distanceIndicatorTxt.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(distanceTraveled, 2)}{qc.Unit(UnitOf.distance)}");
    //         }
    //         distanceIndicatorTxt.GetComponent<TextMeshPro>().fontSize = fontSize;
    //         velocityIndicatorTxt.GetComponent<TextMeshPro>().fontSize = fontSize;
    //         labelTxt[1].GetComponent<TextMeshPro>().fontSize = fontSize;

    //         arrows[3].transform.position = new Vector2(veloTxtPnt.x + 1.5f, veloTxtPnt.y + 3f);
    //         velocityIndicatorTxt.transform.position = new Vector2(veloTxtPnt.x, veloTxtPnt.y + 3);

    //         qc.SetColor(labelTxt[1].GetComponent<TMP_Text>(), colorMode);
    //         qc.SetColor(distanceIndicatorTxt.GetComponent<TMP_Text>(), colorMode);
    //         qc.SetColor(velocityIndicatorTxt.GetComponent<TMP_Text>(), colorMode);
    //         distanceLine1.startColor = qc.getHexColor(colorMode);
    //         distanceLine1.endColor = qc.getHexColor(colorMode);
    //         distanceLine2.startColor = qc.getHexColor(colorMode);
    //         distanceLine2.endColor = qc.getHexColor(colorMode);
    //         timeLine1.startColor = qc.getHexColor(colorMode);
    //         timeLine1.endColor = qc.getHexColor(colorMode);
    //         timeLine2.startColor = qc.getHexColor(colorMode);
    //         timeLine2.endColor = qc.getHexColor(colorMode);
    //         endTime.startColor = qc.getHexColor(colorMode);
    //         endTime.endColor = qc.getHexColor(colorMode);
    //         startlength.startColor = qc.getHexColor(colorMode);
    //         startlength.endColor = qc.getHexColor(colorMode);
    //         endLength.startColor = qc.getHexColor(colorMode);
    //         endLength.endColor = qc.getHexColor(colorMode);
    //         foreach (var item in arrows)
    //         {
    //             item.GetComponent<SpriteRenderer>().color = qc.getHexColor(colorMode);
    //         }
    //     }
}
