using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeillingGenerator : MonoBehaviour
{
    public int stage;
    public GameObject[] rubble;
    public float mapheight, mapWitdh, tileoffset = 1f, startPoint = 0, endpoint;
    bool ceillingPresent;
    QuestionControllerVThree qc;

    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        endpoint = mapWitdh - startPoint;
        ceillingPresent = false;
    }

    // Update is called once per frame
    void Update()
    {
        stage = qc.stage;
        //GameObject debris = rubble[Random.Range(0, 2)];
    }

    public void createQuadtilemap()
    {
        if (stage == 1)
        {
            mapWitdh = 25;
        }
        else if (stage == 2)
        {
            mapWitdh = 30;
        }
        else
        {
            mapWitdh = 42;
        }
        for (int x = 0; x < mapWitdh + 3; x++)
        {
            GameObject TempGo2 = Instantiate(rubble[Random.Range(0, 2)]);
            TempGo2.transform.position = new Vector3(x - 3 * tileoffset, 9 * tileoffset, 1);
            ceillingPresent = true;
        }
    }
    public void createQuadtilemap2()
    {
        for (float x = startPoint; x < endpoint + 3; x++)
        {
            GameObject TempGo2 = Instantiate(rubble[Random.Range(0, 2)]);
            TempGo2.transform.position = new Vector3(x - 3 * tileoffset, mapheight * tileoffset, 1);
            ceillingPresent = true;
        }
    }
    /*void SetTileInfo(GameObject GO, int x, int y)
    {
        GO.transform.parent = transform;
        GO.name = x.ToString() + ", " + y.ToString();

    }*/
}
