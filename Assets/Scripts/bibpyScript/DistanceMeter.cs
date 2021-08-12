using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DistanceMeter : MonoBehaviour
{
    public GameObject d1, d2, edge, mother;
    public TMP_Text distancetxt;
    public float positionX, positionY, distance;
    float d1Scale, d2Pos, d2Scale, edgePos;
    public bool horizontal, vertical;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (horizontal)
        {
            edgePos = positionX + distance;
            d1Scale = (distance / 2) - 2.5f;
            d2Pos = positionX + ((distance / 2) + 2);
            d2Scale = (distance / 2) - 2.5f;
            if (d1Scale < 0)
            {
                d1Scale = 0;
            }
            if (d2Scale < 0)
            {
                d2Scale = 0;
            }
            mother.transform.position = new Vector2(positionX, positionY);
            edge.transform.position = new Vector2(edgePos, edge.transform.position.y);
            d1.transform.localScale = new Vector2(d1Scale, d1.transform.localScale.y);
            d1.transform.position = new Vector2(positionX + .5f, d1.transform.position.y);
            d2.transform.position = new Vector2(d2Pos, d2.transform.position.y);
            d2.transform.localScale = new Vector2(d2Scale, d2.transform.localScale.y);
            distancetxt.text = distance.ToString("F2") + ("m");
            distancetxt.gameObject.transform.position = new Vector2(positionX + (distance / 2), distancetxt.gameObject.transform.position.y);
        }
        if(vertical)
        {   mother.transform.rotation = Quaternion.Euler(mother.transform.rotation.x,mother.transform.rotation.y,90);
            edgePos = positionY+ distance;
            d1Scale = (distance / 2) - 1f;
            d2Pos = positionY + ((distance / 2) + 1);
            d2Scale = (distance / 2) - 1.5f;
            if (d1Scale < 0)
            {
                d1Scale = 0;
            }
            if (d2Scale < 0)
            {
                d2Scale = 0;
            }
            mother.transform.position = new Vector2(positionX, positionY);
            edge.transform.position = new Vector2(edge.transform.position.x, edgePos);
            d1.transform.localScale = new Vector2(d1Scale, d1.transform.localScale.y);
            d1.transform.position = new Vector2( d1.transform.position.x, positionY + .5f);
            d2.transform.position = new Vector2(d2.transform.position.x, d2Pos);
            d2.transform.localScale = new Vector2(d2Scale, d2.transform.localScale.y);
            distancetxt.text = distance.ToString("F2") + ("m");
            distancetxt.gameObject.transform.rotation =  Quaternion.Euler(distancetxt.gameObject.transform.rotation.x,distancetxt.gameObject.transform.rotation.y,0);
            distancetxt.gameObject.transform.position = new Vector2(distancetxt.gameObject.transform.position.x, positionY + (distance / 2));
        }

    }
}
