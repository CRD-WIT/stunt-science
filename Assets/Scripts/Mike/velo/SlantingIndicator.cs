using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;
using TMPro;

public class SlantingIndicator : MonoBehaviour
{
    TextColorMode distanceColor, velocityColor;
    [SerializeField] float fontSize;
    [Header("Line Position")]
    [Space(2)]
    [SerializeField] public Vector2 distanceSpawnPnt;
    [Header("Line Legnth")]
    [Space(2)]
    public float distance, angle, x, y;
    float dimensionTxtLength, velocityTxtLength;
    Vector2 velocitySpawnPnt;
    [SerializeField] LineRenderer[] distanceLines;
    [SerializeField] GameObject arrow;
    GameObject[] arrows = new GameObject[3];
    [SerializeField] GameObject[] labelTxt = new GameObject[2];
    bool arrowPresent = true;
    QuestionControllerVThree qc;
    private void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
    }
    public void SetDistance(float d, float a, float dx, float dy)
    {
        if (arrowPresent)
            foreach (var item in arrows)
                Destroy(item);
        arrowPresent = false;
        if (!arrowPresent)
        {
            for (int i = 0; i <= 2; i++)
            {
                arrows[i] = Instantiate(arrow);
                arrows[i].transform.SetParent(transform.Find("Arrows"));
            }
            arrows[0].transform.Rotate(0, 0, (90 + angle));
            arrows[1].transform.Rotate(0, 0, (90 - angle));
        }
        distance = d;
        angle =a;
        x =dx;
        y =dy;
        distanceColor = TextColorMode.Given;
    }
    public void SetSpawnPnt(Vector2 pos)
    {
        this.distanceSpawnPnt = pos;
    }
    private void Update()
    {
        x = Mathf.Sin(Mathf.Deg2Rad * angle) * distance;
        y = Mathf.Cos(Mathf.Deg2Rad * angle) * distance;

        dimensionTxtLength = labelTxt[0].GetComponent<TextMeshPro>().text.Length;

        arrows[0].transform.position = new Vector2(x, y);
        arrows[1].transform.position = new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y);
        distanceLines[0].SetPosition(0, new Vector2(x + distanceSpawnPnt.x, y + distanceSpawnPnt.y));
        distanceLines[0].SetPosition(1, new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y));
        labelTxt[0].transform.position = new Vector2((x / 2) + distanceSpawnPnt.x, distanceSpawnPnt.y);

        labelTxt[1].transform.position = velocitySpawnPnt;
        arrows[2].transform.position = new Vector2(velocitySpawnPnt.x + (0.22f * velocityTxtLength), velocitySpawnPnt.y);

        qc.SetColor(labelTxt[1].GetComponent<TMP_Text>(), velocityColor);
        arrows[2].GetComponent<SpriteRenderer>().color = qc.getHexColor(velocityColor);

        qc.SetColor(labelTxt[0].GetComponent<TMP_Text>(), distanceColor);
        distanceLines[0].startColor = qc.getHexColor(distanceColor);
        distanceLines[0].endColor = qc.getHexColor(distanceColor);
        arrows[0].GetComponent<SpriteRenderer>().color = qc.getHexColor(distanceColor);
        arrows[1].GetComponent<SpriteRenderer>().color = qc.getHexColor(distanceColor);
        foreach (var item in labelTxt)
        {
            item.GetComponent<TextMeshPro>().fontSize = fontSize;
        }
    }
}