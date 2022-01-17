using UnityEngine;
using GameConfig;
using TMPro;

public class IndicatorManagerV1_1 : MonoBehaviour
{
    TextColorMode colorMode,
        distanceColor,
        timeColor,
        heightColor,
        velocityColor;

    [SerializeField]
    float fontSize;
    public Mode correctlengthOrientation;

    [Header("Lines Needed")]
    [Space(3)]
    [SerializeField]
    bool showDistance;

    [SerializeField]
    bool showHeight;

    [Header("Line Position")]
    [Space(3)]
    [SerializeField]
    public Vector2 distanceSpawnPnt;

    [SerializeField]
    public Vector2 heightSpawnPnt;

    [Header("Line Legnth")]
    [Space(3)]
    [SerializeField]
    float distance;

    [SerializeField]
    float height;
    float velocity,
        timer,
        dimensionTxtLength,
        heightTxtLength,
        correctDistanceTxtLength,
        velocityTxtLength,
        annotationDistanceEnds,
        annotationTimeEnds,
        upperDistanceEndLines = 0.5f,
        lowerDistanceEndLines = 1,
        leftHeightEndLines = 0.25f,
        rightHeightEndLines = 1.5f;

    Vector2 velocitySpawnPnt;

    [SerializeField]
    LineRenderer[] distanceLines = new LineRenderer[4],
        heightLines = new LineRenderer[4],
        correctDistanceLines = new LineRenderer[4];

    [SerializeField]
    LineRenderer timeLine;

    [SerializeField]
    GameObject arrow;
    GameObject[] arrows = new GameObject[8];

    [SerializeField]
    GameObject[] labelTxt;
    public bool showCorrectTime = false,
        showCorrectDistance = false;
    bool arrowPresent = false,
        answered,
        revealTime,
        revealDistance,
        revealVelocity,
        revealHeight,
        hideTime;
    char requiredAnswer;
    QuestionControllerVThree qc;

    private void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
    }

    private Vector2 CorrectIndicatorSpawnPos()
    {
        arrows[4] = Instantiate(arrow);
        arrows[5] = Instantiate(arrow);
        arrows[4].transform.SetParent(transform.Find("Correct"));
        arrows[5].transform.SetParent(transform.Find("Correct"));
        if (correctlengthOrientation == Mode.Vertical)
        {
            arrows[4].transform.Rotate(0, 0, 90);
            arrows[5].transform.Rotate(0, 0, -90);

            return new Vector2(heightSpawnPnt.x - 3, heightSpawnPnt.y);
        }
        else
        {
            arrows[5].transform.Rotate(0, 0, 180);
            return new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y - 3);
        }
    }

    public void ResizeEndLines(float? upD, float? lowD, float? lH, float? rH)
    {
        if (upD != null)
            upperDistanceEndLines = (float)upD;
        if (lowD != null)
            lowerDistanceEndLines = (float)lowD;

        if (lH != null)
            leftHeightEndLines = (float)lH;
        if (rH != null)
            rightHeightEndLines = (float)rH;
    }

    public void showLines(float? dLength, float? hLength, float v, float? t)
    {
        if (arrowPresent)
            foreach (var item in arrows)
                Destroy(item);
        arrowPresent = false;
        if (!arrowPresent)
        {
            for (int i = 0; i <= 7; i++)
            {
                arrows[i] = Instantiate(arrow);
                arrows[i].transform.SetParent(transform.Find("UnusedArrows"));
            }
            if (dLength != null)
            {
                distance = (float)dLength;

                arrows[0].transform.Rotate(0, 0, 180);
                arrows[0].transform.SetParent(transform.Find("Distance"));
                arrows[1].transform.SetParent(transform.Find("Distance"));

                this.showDistance = true;
            }
            else
                this.showDistance = false;
            if (t != null)
            {
                hideTime = false;
                timer = (float)t;
            }
            else
                hideTime = true;
            if (hLength != null)
            {
                height = (float)hLength;

                arrows[2].transform.Rotate(0, 0, 90);
                arrows[3].transform.Rotate(0, 0, -90);
                arrows[2].transform.SetParent(transform.Find("Height"));
                arrows[3].transform.SetParent(transform.Find("Height"));

                this.showHeight = true;
            }
            else
                this.showHeight = false;

            velocity = v;
            arrows[6].transform.SetParent(transform.Find("Velocity"));
            arrows[6].transform.Rotate(0, 0, 180);

            showCorrectDistance = false;
            showCorrectTime = false;

            arrows[4].transform.Rotate(0, 0, 180);
            arrows[4].transform.SetParent(transform.Find("CorrectDistance"));
            arrows[5].transform.SetParent(transform.Find("CorrectDistance"));

            showCorrectTime = false;
            showCorrectDistance = false;

            annotationTimeEnds = 0.75f;
            arrowPresent = true;
            distanceColor = TextColorMode.Given;
            timeColor = TextColorMode.Given;
            heightColor = TextColorMode.Given;
        }
    }

    public void UnknownIs(char? whatIsAsk)
    {
        showCorrectDistance = false;
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
        distanceColor = colorMode;
        timeColor = colorMode;
        velocityColor = colorMode;
        heightColor = colorMode;
    }

    public void SetPlayerPosition(Vector2 Pnt)
    {
        velocitySpawnPnt = new Vector2(Pnt.x, Pnt.y + 2.5f);
        velocityTxtLength = labelTxt[3].GetComponent<TextMeshPro>().text.Length;
    }

    public void AnswerIs(bool isCorrect, bool lineRecolor)
    {
        switch (requiredAnswer)
        {
            case 'v':
                if (isCorrect)
                {
                    velocityColor = TextColorMode.Correct;
                }
                else
                {
                    velocityColor = TextColorMode.Wrong;
                }
                if (lineRecolor)
                {
                    distanceColor = velocityColor;
                }
                break;
            case 'd':
                if (isCorrect)
                {
                    distanceColor = TextColorMode.Correct;
                }
                else
                {
                    distanceColor = TextColorMode.Wrong;
                }
                if (lineRecolor)
                {
                    timeColor = distanceColor;
                }
                break;
            case 't':
                if (isCorrect)
                {
                    timeColor = TextColorMode.Correct;
                }
                else
                {
                    timeColor = TextColorMode.Wrong;
                }
                if (lineRecolor)
                {
                    distanceColor = timeColor;
                }
                break;
            default:
                if (isCorrect)
                {
                    heightColor = TextColorMode.Correct;
                }
                else
                {
                    heightColor = TextColorMode.Wrong;
                }
                break;
        }
    }

    public void IsRunning(float answer, float distanceCovered)
    {
        distance = distanceCovered;
        switch (requiredAnswer)
        {
            case 'v':
                velocity = answer;
                revealVelocity = true;
                break;
            case 'd':
                distance = answer;
                revealDistance = true;
                break;
            case 't':
                timer = answer;
                revealTime = true;
                break;
            default:
                Debug.Log("AnswerIs(answer)");
                break;
        }
        answered = true;
        annotationTimeEnds = -0.75f;
    }

    public void ShowVelocityLabel(bool show)
    {
        if (!show)
            arrows[6].transform.SetParent(transform.Find("UnusedArrows"));
        else
            arrows[6].transform.SetParent(transform.Find("Velocity"));
    }

    private void Update()
    {
        foreach (var item in distanceLines)
        {
            item.enabled = showDistance;
        }

        foreach (var item in heightLines)
        {
            item.enabled = showHeight;
        }

        foreach (var item in correctDistanceLines)
        {
            item.enabled = showCorrectDistance;
        }
        labelTxt[0].SetActive(showDistance);
        labelTxt[1].SetActive(showHeight);
        labelTxt[2].SetActive(showCorrectDistance);

        arrows[2].SetActive(showHeight);
        arrows[3].SetActive(showHeight);
        arrows[4].SetActive(showCorrectDistance);
        arrows[5].SetActive(showCorrectDistance);

        dimensionTxtLength = labelTxt[0].GetComponent<TextMeshPro>().text.Length;
        heightTxtLength = labelTxt[1].GetComponent<TextMeshPro>().text.Length;
        velocityTxtLength = labelTxt[3].GetComponent<TextMeshPro>().text.Length;

        arrows[0].transform.position = new Vector2(
            (distance + distanceSpawnPnt.x),
            distanceSpawnPnt.y
        );
        arrows[1].transform.position = new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y);
        distanceLines[0].SetPosition(
            0,
            new Vector2(
                ((distance / 2) - (0.18f * dimensionTxtLength)) + distanceSpawnPnt.x,
                distanceSpawnPnt.y
            )
        );
        distanceLines[0].SetPosition(1, new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y));
        distanceLines[1].SetPosition(
            0,
            new Vector2(
                ((distance / 2) + (0.18f * dimensionTxtLength)) + distanceSpawnPnt.x,
                distanceSpawnPnt.y
            )
        );
        distanceLines[1].SetPosition(
            1,
            new Vector2((distance + distanceSpawnPnt.x), distanceSpawnPnt.y)
        );
        distanceLines[2].SetPosition(
            0,
            new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y - lowerDistanceEndLines)
        );
        distanceLines[2].SetPosition(
            1,
            new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y + upperDistanceEndLines)
        );
        distanceLines[3].SetPosition(
            0,
            new Vector2(distanceSpawnPnt.x + distance, distanceSpawnPnt.y - lowerDistanceEndLines)
        );
        distanceLines[3].SetPosition(
            1,
            new Vector2(distanceSpawnPnt.x + distance, distanceSpawnPnt.y + upperDistanceEndLines)
        );
        labelTxt[0].transform.position = new Vector2(
            (distance / 2) + distanceSpawnPnt.x,
            distanceSpawnPnt.y
        );

        arrows[3].transform.position = new Vector2(heightSpawnPnt.x, (height + heightSpawnPnt.y));
        arrows[2].transform.position = new Vector2(heightSpawnPnt.x, heightSpawnPnt.y);
        heightLines[0].SetPosition(
            0,
            new Vector2(heightSpawnPnt.x, ((height / 2) - (0.18f * 1.5f)) + heightSpawnPnt.y)
        );
        heightLines[0].SetPosition(1, new Vector2(heightSpawnPnt.x, heightSpawnPnt.y));
        heightLines[1].SetPosition(
            0,
            new Vector2(heightSpawnPnt.x, ((height / 2) + (0.18f * 1.5f)) + heightSpawnPnt.y)
        );
        heightLines[1].SetPosition(1, new Vector2((heightSpawnPnt.x), height + heightSpawnPnt.y));
        heightLines[2].SetPosition(
            0,
            new Vector2(heightSpawnPnt.x + rightHeightEndLines, heightSpawnPnt.y)
        );
        heightLines[2].SetPosition(
            1,
            new Vector2(heightSpawnPnt.x - leftHeightEndLines, heightSpawnPnt.y)
        );
        heightLines[3].SetPosition(
            0,
            new Vector2(heightSpawnPnt.x + rightHeightEndLines, heightSpawnPnt.y + height)
        );
        heightLines[3].SetPosition(
            1,
            new Vector2(heightSpawnPnt.x - leftHeightEndLines, heightSpawnPnt.y + height)
        );
        labelTxt[1].transform.position = new Vector2(
            heightSpawnPnt.x,
            (height / 2) + heightSpawnPnt.y
        );

        labelTxt[3].transform.position = velocitySpawnPnt;
        arrows[6].transform.position = new Vector2(
            velocitySpawnPnt.x + (0.18f * velocityTxtLength),
            velocitySpawnPnt.y - 0.35f
        );
        timeLine.SetPosition(
            0,
            new Vector2(
                velocitySpawnPnt.x + (0.18f * velocityTxtLength),
                velocitySpawnPnt.y - 0.35f
            )
        );
        timeLine.SetPosition(
            1,
            new Vector2(
                velocitySpawnPnt.x - (0.18f * velocityTxtLength),
                velocitySpawnPnt.y - 0.35f
            )
        );

        if (!revealDistance)
            labelTxt[0].GetComponent<TextMeshPro>().SetText($"d = ?{qc.Unit(UnitOf.distance)}");
        else
            labelTxt[0]
                .GetComponent<TextMeshPro>()
                .SetText($"{System.Math.Round(distance, 2)}{qc.Unit(UnitOf.distance)}");

        if (!revealTime && !revealVelocity)
            labelTxt[3]
                .GetComponent<TextMeshPro>()
                .SetText($"?{qc.Unit(UnitOf.velocity)} at t = ?{qc.Unit(UnitOf.time)}");
        else if (revealTime && revealVelocity)
        {
            if (hideTime)
                labelTxt[3]
                    .GetComponent<TextMeshPro>()
                    .SetText($"{velocity.ToString("f2")}{qc.Unit(UnitOf.velocity)}");
            else
                labelTxt[3]
                    .GetComponent<TextMeshPro>()
                    .SetText(
                        $"{velocity.ToString("f2")}{qc.Unit(UnitOf.velocity)} at {System.Math.Round(timer, 2)}{qc.Unit(UnitOf.time)}"
                    );
        }
        else if (revealTime && !revealVelocity)
            labelTxt[3]
                .GetComponent<TextMeshPro>()
                .SetText(
                    $"v = ?{qc.Unit(UnitOf.velocity)} at {timer.ToString("f2")}{qc.Unit(UnitOf.time)}"
                );
        else
            labelTxt[3]
                .GetComponent<TextMeshPro>()
                .SetText(
                    $"{System.Math.Round(velocity, 2)}{qc.Unit(UnitOf.velocity)} at t = ?{qc.Unit(UnitOf.time)}"
                );

        labelTxt[1]
            .GetComponent<TextMeshPro>()
            .SetText($"{System.Math.Round(height, 2)}{qc.Unit(UnitOf.distance)}");

        switch (requiredAnswer)
        {
            case 'v':

                break;
        }
        qc.SetColor(labelTxt[3].GetComponent<TMP_Text>(), velocityColor);
        arrows[6].GetComponent<SpriteRenderer>().color = qc.getHexColor(velocityColor);

        qc.SetColor(labelTxt[0].GetComponent<TMP_Text>(), distanceColor);
        timeLine.startColor = qc.getHexColor(velocityColor);
        timeLine.endColor = qc.getHexColor(velocityColor);

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

        qc.SetColor(labelTxt[1].GetComponent<TMP_Text>(), heightColor);
        heightLines[0].startColor = qc.getHexColor(heightColor);
        heightLines[0].endColor = qc.getHexColor(heightColor);
        heightLines[1].startColor = qc.getHexColor(heightColor);
        heightLines[1].endColor = qc.getHexColor(heightColor);
        heightLines[2].startColor = qc.getHexColor(heightColor);
        heightLines[2].endColor = qc.getHexColor(heightColor);
        heightLines[3].startColor = qc.getHexColor(heightColor);
        heightLines[3].endColor = qc.getHexColor(heightColor);
        arrows[2].GetComponent<SpriteRenderer>().color = qc.getHexColor(heightColor);
        arrows[3].GetComponent<SpriteRenderer>().color = qc.getHexColor(heightColor);

        foreach (var item in labelTxt)
        {
            item.GetComponent<TextMeshPro>().fontSize = fontSize;
        }
    }

    public void ShowCorrectDistance(float correctD, bool showCorrect, Vector2 pos)
    {
        showCorrectDistance = showCorrect;
        float posY = distanceSpawnPnt.y - pos.y;
        foreach (var item in correctDistanceLines)
        {
            item.enabled = showCorrectDistance;
            item.startColor = qc.getHexColor(TextColorMode.Correct);
            item.endColor = qc.getHexColor(TextColorMode.Correct);
        }
        arrows[4].GetComponent<SpriteRenderer>().color = qc.getHexColor(TextColorMode.Correct);
        arrows[5].GetComponent<SpriteRenderer>().color = qc.getHexColor(TextColorMode.Correct);
        arrows[4].SetActive(showCorrectDistance);
        arrows[5].SetActive(showCorrectDistance);
        arrows[4].transform.position = new Vector2((correctD + pos.x), posY);
        arrows[5].transform.position = new Vector2(pos.x, posY);

        labelTxt[2].GetComponent<TextMeshPro>().color = qc.getHexColor(TextColorMode.Correct);
        labelTxt[2].SetActive(showCorrectDistance);
        labelTxt[2]
            .GetComponent<TextMeshPro>()
            .SetText($"{System.Math.Round(correctD, 2)}{qc.Unit(UnitOf.distance)}");
        labelTxt[2].transform.position = new Vector2((correctD / 2) + pos.x, posY);
        correctDistanceTxtLength = labelTxt[2].GetComponent<TextMeshPro>().text.Length;

        correctDistanceLines[0].SetPosition(
            0,
            new Vector2(((correctD / 2) - (0.18f * correctDistanceTxtLength)) + pos.x, posY)
        );
        correctDistanceLines[0].SetPosition(1, new Vector2(pos.x, posY));
        correctDistanceLines[1].SetPosition(
            0,
            new Vector2(((correctD / 2) + (0.18f * correctDistanceTxtLength)) + pos.x, posY)
        );
        correctDistanceLines[1].SetPosition(1, new Vector2((correctD + pos.x), posY));
        correctDistanceLines[2].SetPosition(0, new Vector2(pos.x, posY - 0.25f));
        correctDistanceLines[2].SetPosition(1, new Vector2(pos.x, posY + 0.5f));
        correctDistanceLines[3].SetPosition(0, new Vector2(pos.x + correctD, posY - 0.25f));
        correctDistanceLines[3].SetPosition(1, new Vector2(pos.x + correctD, posY + 0.5f));
    }
}
