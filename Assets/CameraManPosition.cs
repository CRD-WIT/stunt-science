using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManPosition : MonoBehaviour
{ 
    public Camera myCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var v3Pos = new  Vector3(.95f, 0.90f, 0.25f);
        transform.position = myCamera.ViewportToWorldPoint(v3Pos);
    }
}
