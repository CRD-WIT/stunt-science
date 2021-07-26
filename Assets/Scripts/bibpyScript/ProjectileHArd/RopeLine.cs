using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeLine : MonoBehaviour
{
    public GameObject guntip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        GetComponent<LineRenderer>().SetPosition(0, guntip.transform.position);
        GetComponent<LineRenderer>().SetPosition(1, transform.position);

    }
}
