using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum Mode : byte
{
    Vertical = 0,
    Horizontal = 1,

    sdfsdfsdfsdf = 2
}
public class Annotation : MonoBehaviour
{
    public Vector2 spawnPoint;
    public float distance;
    public Mode arrowMode;
    public float fontSize = 4;
    LineRenderer line1;
    LineRenderer line2;
    GameObject textDimension;
    public bool revealValue = true;
    public GameObject[] verticalArrows = new GameObject[2];
    public GameObject[] horizontalArrows = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        line1 = transform.Find("Line1").GetComponent<LineRenderer>();
        line2 = transform.Find("Line2").GetComponent<LineRenderer>();
        textDimension = transform.Find("Text").gameObject;
    }

    public void Hide(){
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
        this.distance = Mathf.Abs(value);
    }

    // Update is called once per frame
    void Update()
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
            }

            horizontalArrows[0].transform.position = new Vector3(spawnPoint.x, (distance + spawnPoint.y), 0);
            horizontalArrows[1].transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);
            line1.SetPosition(0, new Vector3(spawnPoint.x, ((distance / 2) - 0.4f) + spawnPoint.y, 0));
            line1.SetPosition(1, new Vector3(spawnPoint.x, spawnPoint.y, 0));
            line2.SetPosition(0, new Vector3(spawnPoint.x, ((distance / 2) + 0.4f) + spawnPoint.y, 0));
            line2.SetPosition(1, new Vector3(spawnPoint.x, (distance + spawnPoint.y), 0));
            textDimension.transform.position = new Vector3(spawnPoint.x, (distance / 2) + spawnPoint.y, 0);

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
            }
            verticalArrows[0].transform.position = new Vector3((distance + spawnPoint.x), spawnPoint.y, 0);
            verticalArrows[1].transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);
            line1.SetPosition(0, new Vector3(((distance / 2) - (0.2f * dimensionTextLength)) + spawnPoint.x, spawnPoint.y, 0));
            line1.SetPosition(1, new Vector3(spawnPoint.x, spawnPoint.y, 0));
            line2.SetPosition(0, new Vector3(((distance / 2) + (0.2f * dimensionTextLength)) + spawnPoint.x, spawnPoint.y, 0));
            line2.SetPosition(1, new Vector3((distance + spawnPoint.x), spawnPoint.y, 0));
            textDimension.transform.position = new Vector3((distance / 2) + spawnPoint.x, spawnPoint.y, 0);

        }

        if (revealValue)
        {
            textDimension.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(distance, 2)}m");
            textDimension.GetComponent<TextMeshPro>().fontSize = fontSize;
        }
        else
        {
            textDimension.GetComponent<TextMeshPro>().SetText("?");
            textDimension.GetComponent<TextMeshPro>().fontSize = fontSize;
        }


    }
}
