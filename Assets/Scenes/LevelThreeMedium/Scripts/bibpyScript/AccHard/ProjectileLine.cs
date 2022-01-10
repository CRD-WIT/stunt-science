using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    private SpriteRenderer mycolor;
    // Start is called before the first frame update
    void Start()
    {
        mycolor = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(transform.position.y <= -2)
        {
            mycolor.color = new Color32(0, 0, 0, 0);
        }
        if(transform.position.y > -2)
        {
            mycolor.color = new Color32(87, 0, 255, 255);
        }
    }
}
