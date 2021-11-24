using UnityEngine;
using TMPro;

public class GyroParallax : MonoBehaviour
{
    public TMP_Text debugText;
    public Transform[] parallaxLayers;
    public Transform[] backgrounds;
    public float[] parallaxScales;
    public Transform cameraObject;
    private Vector3 previousCamPos;
    // Start is called before the first frame update
    void Awake(){
        cameraObject = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cameraObject.position;

        parallaxScales = new float[backgrounds.Length];
        
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z*-1;
        }








        if(!Input.gyro.enabled){
            Input.gyro.enabled = true;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = "input.gyro: " + Input.gyro.attitude;   
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x -cameraObject.position.x) * parallaxScales[i];

            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            
        }
    }

     private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
