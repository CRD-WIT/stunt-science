using UnityEngine;

public class AngularAnnotation : MonoBehaviour
{

    float x;
    float y;
    public float angle;
    public Vector2 origin;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetAngle(float angle)
    {
        this.angle = angle;
    }

    public void Hide(){
        transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = new Vector3(origin.x, origin.y, 0);
    }
}
