using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera camMain, stage2Cam, stage3Cam;
    public Canvas myCanvas;
    QuestionControllerVThree qc;
    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        myCanvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (qc.stage == 1)
        {
            myCanvas.worldCamera = camMain;
            stage2Cam.enabled = false;
            stage3Cam.enabled = false;
        }
        else if (qc.stage == 2)
        {
            stage2Cam.enabled = true;
            myCanvas.worldCamera = stage2Cam;
            camMain.enabled = false;
            stage3Cam.enabled = false;
        }
        else if (qc.stage == 3)
        {
            stage3Cam.enabled = true;
            myCanvas.worldCamera = stage3Cam;
            stage2Cam.enabled = false;
            camMain.enabled = false;
        }
    }
}