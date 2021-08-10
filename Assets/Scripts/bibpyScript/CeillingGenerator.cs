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
    public void createQuadtilemap(int s)
    {
        switch (s)
        {
            case 1:
                mapWitdh = 25;
                break;
            case 2:
                mapWitdh = 30;
                break;
            default:
                mapWitdh = 43;
                break;
        }
        for (float x = 0f; x < mapWitdh + 3f; x++)
        {
            int i = 1;
            while (i < 3)
            {
                GameObject TempGo2 = Instantiate(rubble[Random.Range(0, 2)]);
                TempGo2.transform.localScale = new Vector2(TempGo2.transform.localScale.x * (Random.Range(0.7f, 0.9f)), TempGo2.transform.localScale.y * (Random.Range(0.5f, 0.7f)));
                TempGo2.transform.position = new Vector3(x - 3 * tileoffset, (9 * tileoffset) + i, 1);
                i++;
            }
        }
        ceillingPresent = true;
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
