using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GyroParallax : MonoBehaviour
{
    public TMP_Text debugText;
    public Transform[] parallaxLayers;
    public float[] paralaxScales;
    public Transform cameraObject;
    private Vector3 previousCamPos;
    // Start is called before the first frame update
    void Awake(){
        cameraObject = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cameraObject.position;
        if(!Input.gyro.enabled){
            Input.gyro.enabled = true;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = "input.gyro: " + Input.gyro.attitude;   
        // for (int i = 0; i < parallaxLayers.Length; i++)
        // {
        //     parallaxLayers[i].transform.position.x += Input.gyro.attitude.x;
        // }
    }

     private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
