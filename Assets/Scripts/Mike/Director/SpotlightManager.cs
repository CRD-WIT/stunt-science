using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightManager : MonoBehaviour
{
    Vector3 pos, scale, targetPos;
    Vector3 rotation;
    Transform sLight;
    public float angle;
    Transform target;
    DirectorManager dm;
    void Start()
    {
        dm = FindObjectOfType<DirectorManager>();
        sLight = transform.Find("Light");
    }

    void Update()
    {
        target = dm.target;

        //initialization of values
        rotation.z = 0;
        pos.z = 0;
        target.position = new Vector2(target.position.x, target.position.y);


        pos = this.gameObject.transform.position;
        targetPos = target.transform.position - pos;

        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;


        if(dm.platformIsOn == DirectorManager.To.Right)  //this is for platform adjustment
            angle = angle +180;


        if (this.gameObject.name == "SpotLight")  //for spotlight
        {
            scale.y = ((Mathf.Sqrt((targetPos.y * targetPos.y) + (targetPos.x * targetPos.x)) + 3) / 15)*3.75f;
            scale.x = (1 + (scale.y / 100));

            sLight.localScale = scale;
            this.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else   //for camera
        {
            Debug.Log(angle);
            if (this.transform.parent.localScale.x == -1)
                this.gameObject.transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
            else
                this.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
