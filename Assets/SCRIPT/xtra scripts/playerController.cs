using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public CharacterController controller;
    //public Animator animator;

    public Joystick joystick;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    float verticalMove = 0f;
    public Rigidbody2D myRb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myRb.velocity = new Vector2(horizontalMove,verticalMove);
        horizontalMove = joystick.Horizontal * runSpeed;
        verticalMove = joystick.Vertical * runSpeed;
        //Transform.eulerAngles = new Vector3( 0, 0, Mathf.Atan2( joystick.Horizontal, joystick.Vertical) * 180 / Mathf.PI );
        /*if (joystick.Horizontal >= .2f)
        {
            horizontalMove = runSpeed;
            
            
            
        }
        else if (joystick.Horizontal <= -.2f)
        {
            horizontalMove = -runSpeed;
        }
        else
        {
            horizontalMove = 0f;
        }

        
        if (joystick.Vertical >= .2)
        {
            verticalMove = runSpeed;
        }
        else if (joystick.Vertical <= -.2f)
        {
            verticalMove = -runSpeed;
        }
        else
        {
            verticalMove = 0f;
        }*/
    }
}
