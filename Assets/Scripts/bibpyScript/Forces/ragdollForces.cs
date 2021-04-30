using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollForces : MonoBehaviour
{
    public float moveSpeedforward;
    public float moveSpeedbackward;
    public float moveSpeedupward;
    private Rigidbody2D myRigidbody;
    public GameObject stick, napsack;
    public GameObject stickloc;
    private ForceSimulation theSimulate;
    private ForceManagerOne theManagerOne;
    private ForceManagerTwo theManagerTwo;
    private ForceManagerThree theManagerThree;




    bool forward;
    bool backward;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        theManagerOne = FindObjectOfType<ForceManagerOne>();
        theManagerTwo = FindObjectOfType<ForceManagerTwo>();
        theManagerThree = FindObjectOfType<ForceManagerThree>();
        if (theSimulate.stage == 1 || theSimulate.stage == 3)
        {
            moveSpeedforward = 8;
            moveSpeedbackward = -6;
        }
        if (theSimulate.stage == 2)
        {
            moveSpeedforward = 8;
            moveSpeedbackward = -8;
        }
         if (theSimulate.stage == 3)
         {
            napsack.SetActive(true);
         }

    }

    // Update is called once per frame
    void Update()
    {
        if (theSimulate.playerDead == true)
        {
            StartCoroutine(playerSpawn());
        }
        if (theSimulate.stage == 1)
        {
            if (theManagerOne.tooWeak)
            {
                backward = true;
            }
            if (theManagerOne.tooStrong)
            {
                forward = true;
            }
        }
        if (theSimulate.stage == 2)
        {
            if (theManagerTwo.tooStrong)
            {
                backward = true;
            }
            if (theManagerTwo.tooWeak)
            {
                forward = true;
            }
        }
        if (theSimulate.stage == 3)
        {
            if (theManagerThree.tooStrong)
            {
               forward = true;
            }
            if (theManagerThree.tooWeak)
            {
                backward = true;
            }
        }


        if (forward)
        {
            moveSpeedforward -= 6 * Time.deltaTime;
            myRigidbody.velocity = new Vector2(moveSpeedforward, myRigidbody.velocity.y);
            if (moveSpeedforward <= 0)
            {
                moveSpeedforward = 0;
                forward = false;
                
            }
        }
        if (backward)
        {
            myRigidbody.velocity = new Vector2(moveSpeedbackward, myRigidbody.velocity.y);
            moveSpeedbackward += 6 * Time.deltaTime;
            if (moveSpeedbackward >= 0)
            {
                moveSpeedbackward = 0;
                backward = false;
            }
        }
    }
    IEnumerator driverSpawn()
    {
        accSimulation.playerDead = false;
        yield return new WaitForSeconds(5);
        Destroy(stick.gameObject);
        theSimulate.thePlayer.gameObject.SetActive(true);
        //theSimulate.thePlayer.transform.position = new Vector2()






        //theplayer.gameObject.SetActive(true);
        //theplayer.transform.position = stick.transform.position;
    }
    IEnumerator playerSpawn()
    {
        SimulationManager.playerDead = false;
        if(theSimulate.stage == 1)
        {
            yield return new WaitForSeconds(4f); 
        }
        if(theSimulate.stage == 2)
        {
            yield return new WaitForSeconds(4); 
        }
        if(theSimulate.stage == 3)
        {
            yield return new WaitForSeconds(2.5f); 
        }
        theSimulate.thePlayer.gameObject.SetActive(true);
        theSimulate.thePlayer.transform.position = stickloc.transform.position;
        Destroy(stick.gameObject);

    }

}
