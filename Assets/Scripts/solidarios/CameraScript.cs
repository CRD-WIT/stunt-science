using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject cameraImage;
    GameObject spotLight;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        cameraImage = transform.Find("CameraTop").gameObject;
        spotLight = transform.Find("SpotLight").gameObject;

        cameraImage.GetComponent<LookAtConstraint2D>().SetTarget(target);
        spotLight.GetComponent<LookAtConstraint2D>().SetTarget(target);
    }
}
