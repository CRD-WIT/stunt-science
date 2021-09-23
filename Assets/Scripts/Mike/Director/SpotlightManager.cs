using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightManager : MonoBehaviour
{
    Vector3 pos, scale, targetPos;
    Vector3 rotation;
    Transform sLight;
    float angle;
    public Transform target;
    // [SerializeField] SpotLightPyramidShape spotLightPyramidShape;
    // Start is called before the first frame update
    void Start()
    {
        sLight = transform.Find("Light");
        rotation.z = 0;
        pos.z = 0;
        target.position = new Vector2(target.position.x, target.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        pos = this.gameObject.transform.position;
        targetPos = target.transform.position - pos;

        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        if (this.gameObject.name == "spot_light")
        {
            scale.y = (Mathf.Sqrt((targetPos.y * targetPos.y) + (targetPos.x * targetPos.x)) + 3) / 15;
            scale.x = 1 + (scale.y / 100);

            sLight.localScale = scale;
            this.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            if (this.transform.parent.localScale.x == -1)
                this.gameObject.transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
            else
                this.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }
}
