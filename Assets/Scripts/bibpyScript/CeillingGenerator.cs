using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeillingGenerator : MonoBehaviour
{
    public int mapWitdh, stage;
    public float mapheight;
    //public GameObject tileprefab;
    //public GameObject ceilingprefab;
    public GameObject[] rubble;
    public float tileoffset = 1f;

    // Start is called before the first frame update
    void Start()
    {
        stage = SimulationManager.stage;
    }

    // Update is called once per frame
    void Update()
    {

        GameObject debris = rubble[Random.Range(0, 2)];
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
        for (int x = +0; x < mapWitdh + 3; x++)
        {
            GameObject debris = rubble[Random.Range(0, 2)];
            GameObject TempGo2 = Instantiate(debris);
            TempGo2.transform.position = new Vector3(x - 3 * tileoffset, 9 * tileoffset, 1);
        }
    }
    public void createQuadtilemap2()
    {
        for (int x = +0; x < mapWitdh + 3; x++)
        {
            GameObject debris = rubble[Random.Range(0, 2)];
            GameObject TempGo2 = Instantiate(debris);
            TempGo2.transform.position = new Vector3(x - 3 * tileoffset, 8.1f * tileoffset, 1);
        }
    }
    /*void SetTileInfo(GameObject GO, int x, int y)
    {
        GO.transform.parent = transform;
        GO.name = x.ToString() + ", " + y.ToString();

    }*/
}
