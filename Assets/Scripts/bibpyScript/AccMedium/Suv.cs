using UnityEngine;

public class Suv : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float moveSpeed, accelaration;
    public bool accelarating;
    public bool deaccelarating;
    public Collider2D myCollider;
    // Start is called before the first frame update
    void Start()
    {
         myRigidbody = GetComponent<Rigidbody2D>();
         myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        if(accelarating)
        {
            moveSpeed -= accelaration * Time.fixedDeltaTime;
        }
        if(deaccelarating)
        {
            moveSpeed += (accelaration/4) * Time.fixedDeltaTime;
        }
       
    }
}
