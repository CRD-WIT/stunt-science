using UnityEngine;

public class test : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float endPos;
    float currentPos;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(0, moveSpeed);
        currentPos = transform.position.y;
        if(currentPos <= endPos)
        {
            moveSpeed = 0;
            transform.position = new Vector2(0, endPos);
        }
    }
}
