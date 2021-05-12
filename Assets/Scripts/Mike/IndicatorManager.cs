using System.Collections;
using TMPro;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    public Vector2 spawnPoint;
    public float fontSize = 4, distance, distanceTraveled, playerVelocity, stuntTime;
    public Mode arrowMode;
    public Unit whatIsAsk;
    LineRenderer line1, line2, corretcAnswer, startLine;
    GameObject textDimension, answer, correctAnswer;
    public GameObject[] verticalArrows = new GameObject[2], horizontalArrows = new GameObject[2];

    int value;
    string playerAnswerIs;
    // Start is called before the first frame update
    void Start()
    {
        line1 = transform.Find("Line1").GetComponent<LineRenderer>();
        line2 = transform.Find("Line2").GetComponent<LineRenderer>();
        corretcAnswer = transform.Find("CorrectAnswerIndicator").GetComponent<LineRenderer>();
        startLine = transform.Find("StartLine").GetComponent<LineRenderer>();
        textDimension = transform.Find("Text").gameObject;
        answer = transform.Find("Answer").gameObject;
        correctAnswer = transform.Find("CorrectAnswer").gameObject;
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

    // Update is called once per frame
    void Update()
    {
        line1.startColor = Color.ColorMode();
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
            }
            horizontalArrows[0].transform.position = new Vector3(spawnPoint.x, (distanceTraveled + spawnPoint.y), 0);
            horizontalArrows[1].transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);
            line1.SetPosition(0, new Vector3(spawnPoint.x, ((distanceTraveled / 2) - 0.4f) + spawnPoint.y, 0));
            line1.SetPosition(1, new Vector3(spawnPoint.x, spawnPoint.y, 0));
            line2.SetPosition(0, new Vector3(spawnPoint.x, ((distanceTraveled / 2) + 0.4f) + spawnPoint.y, 0));
            line2.SetPosition(1, new Vector3(spawnPoint.x, (distanceTraveled + spawnPoint.y), 0));
            textDimension.transform.position = new Vector3(spawnPoint.x, (distanceTraveled / 2) + spawnPoint.y, 0);

        }
        else
        {
            //distanceTraveled = FindObjectOfType<Player>().GetComponent<Transform>().position.x;
            foreach (var item in horizontalArrows)
            {
                item.SetActive(false);
            }

            foreach (var item in verticalArrows)
            {
                item.SetActive(true);
            }
            verticalArrows[0].transform.position = new Vector3((distanceTraveled + spawnPoint.x), spawnPoint.y, 0);
            verticalArrows[1].transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);
            line1.SetPosition(0, new Vector3(((distanceTraveled / 2) - (0.2f * dimensionTextLength)) + spawnPoint.x, spawnPoint.y, 0));
            line1.SetPosition(1, new Vector3(spawnPoint.x, spawnPoint.y, 0));
            line2.SetPosition(0, new Vector3(((distanceTraveled / 2) + (0.2f * dimensionTextLength)) + spawnPoint.x, spawnPoint.y, 0));
            line2.SetPosition(1, new Vector3((distanceTraveled + spawnPoint.x), spawnPoint.y, 0));
            startLine.SetPosition(0, new Vector2(spawnPoint.x, spawnPoint.y - 0.25f));
            startLine.SetPosition(1, new Vector2(spawnPoint.x, spawnPoint.y + 0.5f));
            corretcAnswer.SetPosition(0, new Vector2(spawnPoint.x + distance, spawnPoint.y - 0.25f));
            corretcAnswer.SetPosition(1, new Vector2(spawnPoint.x + distance, spawnPoint.y + 1.5f));
            textDimension.transform.position = new Vector3((distanceTraveled / 2) + spawnPoint.x, spawnPoint.y, 0);

        }
        switch (whatIsAsk)
        {
            case Unit.distance:
                answer.GetComponent<TMP_Text>().text = System.Math.Round(distanceTraveled, 2) + "m";
                break;
            case Unit.time:
                answer.GetComponent<TMP_Text>().text = System.Math.Round(stuntTime) + "s";
                break;
            case Unit.velocity:
                answer.GetComponent<TMP_Text>().text = System.Math.Round(playerVelocity) + "m/s";
                break;
        }
        answer.transform.position = new Vector3((distanceTraveled) + spawnPoint.x, spawnPoint.y - 1, 0);
        correctAnswer.transform.position = new Vector3((distance) + spawnPoint.x, spawnPoint.y + 2, 0);
        textDimension.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(distanceTraveled, 2)}m");
    }

    //change value to string
    int ColorMode()
    {
        if (playerAnswerIs == "correct")
            value = 0xff0000;
        else if (playerAnswerIs == "wrong")
            value = 0xff0000;
        else
            value = 0x800080;
        return value;
    }
    public void AnswerIs(string answer){
        this.playerAnswerIs = answer;
    }
}

public enum Unit : byte
{
    distance = 0,
    time = 1,
    velocity = 2,
    acceleration = 3,
    angle = 4,
    angularVelocity = 5,
    force = 6,
    work = 7,
    energy = 8,
    power = 9,
    momentum = 10
}

