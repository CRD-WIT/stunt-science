using UnityEngine;

public class SpotlightManagerV1 : MonoBehaviour
{
    Vector3 pos, scale, targetPos;
    Vector3 rotation;
    Transform sLight;
    public float angle;
    Transform target;
    DirectorManagerV2 dm;
    void Start()
    {
        dm = FindObjectOfType<DirectorManagerV2>();
        sLight = transform.Find("Light");
    }

    void Update()
    {
        target = dm.target;

        //initialization of values
        rotation.z = 0;
        pos.z = 0;

        if (target)
        {
            target.position = new Vector2(target.position.x, target.position.y);
        }



        pos = this.gameObject.transform.position;

        if(target){
            targetPos = target.transform.position - pos;
        }        

        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;


        if (dm.platformIsOn == DirectorManagerV2.ToThe.Right)  //this is for platform adjustment
            angle = angle + 180;


        if (this.gameObject.name == "SpotLight")  //for spotlight
        {
            scale.y = ((Mathf.Sqrt((targetPos.y * targetPos.y) + (targetPos.x * targetPos.x))) / 3);
            scale.x = (1 + (scale.y / 100));

            sLight.localScale = scale;
            this.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else   //for camera
        {
            if (this.transform.parent.localScale.x == -1)
                this.gameObject.transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
            else
                this.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
