using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeillingGenerator : MonoBehaviour
{
    public int mapwitdh ;
    public float mapheight ;
    //public GameObject tileprefab;
    //public GameObject ceilingprefab;
    public GameObject[] rubble;

    public float tileoffset = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject debris = rubble[Random.Range(0,2)];
    }
    
    public void createQuadtilemap ()
    {
        /*for(int x =+ 0; x < mapwitdh; x++)
        {
            for(int y = 0; y <= mapheight; y++)
            {
                GameObject TempGo = Instantiate(tileprefab);
                TempGo.transform.position = new Vector3(x * tileoffset, -1 * tileoffset,1);
                
                TempGo.transform.parent = transform;
                TempGo.name = x.ToString() + ", " + y.ToString();
                 SetTileInfo(TempGo, x, y);

            }
        }*/
        
        for(int x =+ 0; x < mapwitdh + 3; x++)
        {
             GameObject debris = rubble[Random.Range(0,2)];
             GameObject TempGo2 = Instantiate(debris);
             TempGo2.transform.position = new Vector3(x-3 * tileoffset, mapheight * tileoffset,1);
        }
    }
    public void createQuadtilemap2 ()
    {
       
        
        for(int x =+ 0; x < mapwitdh + 3; x++)
        {
             GameObject debris = rubble[Random.Range(0,2)];
             GameObject TempGo2 = Instantiate(debris);
            TempGo2.transform.position = new Vector3(x-3 * tileoffset, 5.5f * tileoffset,1);
        }
    }
    /*void SetTileInfo(GameObject GO, int x, int y)
    {
        GO.transform.parent = transform;
        GO.name = x.ToString() + ", " + y.ToString();

    }*/
}
