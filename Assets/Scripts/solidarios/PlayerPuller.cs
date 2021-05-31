using UnityEngine;


public class PlayerPuller : MonoBehaviour
{
    float initVelocity = 5f;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (initVelocity != 0)
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, initVelocity * -1, 0);
            initVelocity -= 1;
        }

    }
}
