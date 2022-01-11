using UnityEngine;

public class TruckManager : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float moveSpeed;
    public float accelaration;
    public bool accelerating, deaccelerating;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        if(accelerating)
        {
            moveSpeed  += accelaration * Time.fixedDeltaTime;
        }
         if(deaccelerating)
        {
            moveSpeed  -= accelaration * Time.fixedDeltaTime;
        }

    }
}
