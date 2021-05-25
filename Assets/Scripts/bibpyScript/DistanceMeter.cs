using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DistanceMeter : MonoBehaviour
{
    public GameObject  d1, d2, edge;
    public TMP_Text distancetxt;
    public float positionX, positionY, distance;
    float d1Scale, d2Pos, d2Scale, edgePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        edgePos = positionX + distance;
        d1Scale = (distance/2) - 2.5f;
        d2Pos = positionX + ((distance/2) + 2);
        d2Scale = (distance/2) - 2.5f;
        gameObject.transform.position = new Vector2(positionX, positionY);
        edge.transform.position = new Vector2(edgePos, edge.transform.position.y);
        d1.transform.localScale = new Vector2(d1Scale, d1.transform.localScale.y);
        d2.transform.position = new Vector2(d2Pos, d2.transform.position.y);
        d2.transform.localScale = new Vector2(d2Scale, d2.transform.localScale.y);
        distancetxt.text = distance.ToString("F2") + ("m");
        distancetxt.gameObject.transform.position = new Vector2(positionX + (distance/2), distancetxt.gameObject.transform.position.y);
    }
}
