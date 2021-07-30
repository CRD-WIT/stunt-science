using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;
using TMPro;

public class TestingScript : MonoBehaviour
{
    TextColorMode distanceColor, velocityColor;
    [SerializeField] float fontSize;
    [Header("Line Position")]
    [Space(2)]
    [SerializeField] public Vector2 distanceSpawnPnt = new Vector2(0, 0);
    [Header("Line Legnth")]
    [Space(2)]
    public float distance = 20, angle, x, y;
    float dimensionTxtLength, velocityTxtLength;
    Vector2 velocitySpawnPnt;
    [SerializeField] LineRenderer[] distanceLines = new LineRenderer[4];
    [SerializeField] GameObject arrow;
    GameObject[] arrows = new GameObject[3];
    [SerializeField] GameObject[] labelTxt = new GameObject[2];
    bool arrowPresent = false;
    QuestionControllerVThree qc;
    private void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
    }
    // public void SetDistance(float distance, float angle, float x, float y)
    // {
    //     if (arrowPresent)
    //     {
    //         foreach (var item in arrows)
    //             Destroy(item);
    //         arrowPresent = false;
    //         if (!arrowPresent)
    //         {
    //             for (int i = 0; i <= 3; i++)
    //             {
    //                 arrows[i] = Instantiate(arrow);
    //                 arrows[i].transform.SetParent(transform.Find("Arrows"));
    //             }
    //             arrows[0].transform.Rotate(0, 0, angle);
    //         }
    //         distanceColor = TextColorMode.Given;
    //     }
    // }
    // public void SetSpawnPnt(Vector2 pos)
    // {
    //     this.distanceSpawnPnt = pos;
    // }
    private void Update()
    {
        x = Mathf.Sin(Mathf.Deg2Rad * angle) * distance;
        y = Mathf.Cos(Mathf.Deg2Rad * angle) * distance;

        // if (arrowPresent)
        // {
        //     foreach (var item in arrows)
        //         Destroy(item);
        //     arrowPresent = false;
        // }
        if (!arrowPresent)
        {
            arrowPresent =true;
            for (int i = 0; i <= 3; i++)
            {
                arrows[i] = Instantiate(arrow);
                arrows[i].transform.SetParent(transform.Find("Arrows"));
            }
            arrows[0].transform.Rotate(0, 0, angle);
        }
        distanceColor = TextColorMode.Given;

        dimensionTxtLength = labelTxt[0].GetComponent<TextMeshPro>().text.Length;

        arrows[0].transform.position = new Vector2(x, distanceSpawnPnt.y);
        // arrows[1].transform.position = new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y);
        distanceLines[0].SetPosition(0, new Vector2(((x / 2) - (0.18f * dimensionTxtLength)) + distanceSpawnPnt.x, ((y / 2) - (0.18f * dimensionTxtLength)) + distanceSpawnPnt.y));
        distanceLines[0].SetPosition(1, new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y));
        // distanceLines[1].SetPosition(0, new Vector2(((x / 2) + (0.18f * dimensionTxtLength)) + distanceSpawnPnt.x, ((y / 2) + (0.18f * dimensionTxtLength)) + distanceSpawnPnt.y));
        // distanceLines[1].SetPosition(1, new Vector2((x + distanceSpawnPnt.x), distanceSpawnPnt.y));
        // distanceLines[2].SetPosition(0, new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y - 1f));
        // distanceLines[2].SetPosition(1, new Vector2(distanceSpawnPnt.x, distanceSpawnPnt.y + 0.5f));
        // distanceLines[3].SetPosition(0, new Vector2(distanceSpawnPnt.x + x, distanceSpawnPnt.y - 1f));
        // distanceLines[3].SetPosition(1, new Vector2(distanceSpawnPnt.x + x, distanceSpawnPnt.y + 0.5f));
        labelTxt[0].transform.position = new Vector2((x / 2) + distanceSpawnPnt.x, distanceSpawnPnt.y);

        labelTxt[1].transform.position = velocitySpawnPnt;
        arrows[2].transform.position = new Vector2(velocitySpawnPnt.x + (0.22f * velocityTxtLength), velocitySpawnPnt.y);

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
        foreach (var item in labelTxt)
        {
            item.GetComponent<TextMeshPro>().fontSize = fontSize;
        }
    }
}
