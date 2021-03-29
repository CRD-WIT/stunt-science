using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    public float moveSpeedforward;
    public float moveSpeedbackward;
    private float speedTime;
    private GameManager thegmanager;
    float disolve = 6f;
    private level2Manager themanager2;
    public level3Manager thelevel3;
    public GameObject stick;
    public GameObject stickloc;
    public Player theplayer;
    
    
    // Start is called before the first frame update
    void Start()
    {
        themanager2 = FindObjectOfType<level2Manager>();
        thegmanager = FindObjectOfType<GameManager>();
        thelevel3 = FindObjectOfType<level3Manager>();
        myRigidbody = GetComponent<Rigidbody2D>();
        theplayer = FindObjectOfType<Player>();
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
       /* disolve -= Time.deltaTime;
        if (disolve <= 0)
        {
           
            gameObject.SetActive(false);
            thegmanager.died = true;
           
            thegmanager.respawnposx = gameObject.transform.position.x;
            thegmanager.respawnposy = gameObject.transform.position.y;

        }*/
        if (thegmanager.forwardstunt == true)
        { 
            
            
             if (moveSpeedforward >= 0)
             {
                moveSpeedforward -= 2* Time.deltaTime;
                myRigidbody.velocity = new Vector2(moveSpeedforward, myRigidbody.velocity.y);
                //moveSpeedy -= 2 * Time.deltaTime;

             }
        }
        if (themanager2.backwardstunt == true)
        {
            
            if (moveSpeedbackward < 0)
         {

            myRigidbody.velocity = new Vector2(moveSpeedbackward, myRigidbody.velocity.y);
            moveSpeedbackward += 3 * Time.deltaTime;
            //moveSpeedy -= 2 * Time.deltaTime;
            
         }
        }
        if (thegmanager.playerDead == true);
        {
            StartCoroutine(playerSpawn());
        }
        /*
         if (thegmanager.destroystick == true)
        {
            Destroy(stick.gameObject);
        }
        if (themanager2.destroystick == true)
        {
            Destroy(stick.gameObject);
        }
         if (thelevel3.destroystick == true)
        {
            Destroy(stick.gameObject);
        }*/
        
         // StartCoroutine(playerSpawn());
       
        
        

      

    }
    public void stillAlive()
    {
        StartCoroutine(playerSpawn());
    }
    IEnumerator playerSpawn()
     {
        thegmanager.playerDead = false;
        yield return new WaitForSeconds(2);
        Destroy(stick.gameObject);
       
        thegmanager.thePlayer.gameObject.transform.position = stickloc.transform.position;
        thegmanager.thePlayer.gameObject.SetActive(true);
        //theplayer.gameObject.SetActive(true);
        //theplayer.transform.position = stick.transform.position;
     }
}
