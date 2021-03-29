using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tilesprefab : MonoBehaviour
{
    public GameManager thegamemnager;
    public level2Manager thelevel2;
    public level3Manager thelevel3;
    public actionCount theAct;
     public GameObject tiles;

    // Start is called before the first frame update
    void Start()
    {
        thegamemnager = FindObjectOfType<GameManager>();
        thelevel2 = FindObjectOfType<level2Manager>();
        thelevel3 = FindObjectOfType<level3Manager>();
        theAct = FindObjectOfType<actionCount>();
    }

    // Update is called once per frame
    void Update()
    {
        if (thegamemnager.destroytiles == true)
        {
            Destroy(tiles.gameObject);
        }
        if (thelevel2.destroytiles == true)
        {
            Destroy(tiles.gameObject);
        }
         if (thelevel3.destroytiles == true)
        {
            Destroy(tiles.gameObject);
        }
         if (theAct.destroytiles == true)
        {
            Destroy(tiles.gameObject);
        }
    }
   
        
}
