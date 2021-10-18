using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakSpotManager : MonoBehaviour
{
    public GameObject smallBlast;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.tag == ("weapon"))
        {  
            GetComponent<SpriteRenderer>().color = new Color32(0, 241, 10, 255);
            smallBlast.SetActive(true);
        }
    }
}
