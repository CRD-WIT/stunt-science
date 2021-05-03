using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollAccMid : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    public float moveSpeedforward;
    public float moveSpeedupward;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
            moveSpeedforward -= 6 * Time.deltaTime;
            //moveSpeedupward -= 3 * Time.deltaTime;
            myRigidbody.velocity = new Vector2(moveSpeedforward, myRigidbody.velocity.y);
            if (moveSpeedforward <= 0)
            {
                moveSpeedforward = 0;                
            }
            if (moveSpeedupward <= 0)
            {
                //moveSpeedupward = 0;
                //moveSpeedupward = myRigidbody.velocity.y;                
            }
    }
}
