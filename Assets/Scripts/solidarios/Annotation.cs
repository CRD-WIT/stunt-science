using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum Mode : byte
{
    Vertical = 0,
    Horizontal = 1
}
public class Annotation : MonoBehaviour
{
    public Vector2 spawnPoint;
    public float distance;
    public Mode arrowMode;
    LineRenderer line1;
    LineRenderer line2;
    GameObject textDimension;
    public GameObject[] verticalArrows = new GameObject[2];
    public GameObject[] horizontalArrows = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        line1 = transform.Find("Line1").GetComponent<LineRenderer>();
        line2 = transform.Find("Line2").GetComponent<LineRenderer>();
        textDimension = transform.Find("Text").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
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
            line1.SetPosition(0, new Vector3(spawnPoint.x, ((distance / 2) - 0.2f) + spawnPoint.y, 0));
            line1.SetPosition(1, new Vector3(spawnPoint.x, spawnPoint.y, 0));
            line2.SetPosition(0, new Vector3(spawnPoint.x, ((distance / 2) + 0.2f) + spawnPoint.y, 0));
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
            line1.SetPosition(0, new Vector3(((distance / 2) - 0.2f) + spawnPoint.x, spawnPoint.y, 0));
            line1.SetPosition(1, new Vector3(spawnPoint.x, spawnPoint.y, 0));
            line2.SetPosition(0, new Vector3(((distance / 2) + 0.2f) + spawnPoint.y, spawnPoint.y, 0));
            line2.SetPosition(1, new Vector3((distance + spawnPoint.x), spawnPoint.y, 0));
            textDimension.transform.position = new Vector3((distance / 2) + spawnPoint.x, spawnPoint.y, 0);

        }

        textDimension.GetComponent<TextMeshPro>().SetText($"{distance}m");

    }
}
