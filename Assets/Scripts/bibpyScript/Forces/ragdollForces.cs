using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollForces : MonoBehaviour
{
    public float moveSpeedforward;
    public float moveSpeedbackward;
    public float moveSpeedupward;
    private Rigidbody2D myRigidbody;
    public GameObject stick;
    public GameObject stickloc;
    private ForceSimulation theSimulate;
    private ForceManagerOne theManagerOne;
    
    
    
    
    bool forward;
    bool backward;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        theManagerOne = FindObjectOfType<ForceManagerOne>();

    }

    // Update is called once per frame
    void Update()
    {
       if(theSimulate.playerDead == true)
       {
            StartCoroutine(driverSpawn());
       }
        if(theManagerOne.tooWeak)
        {
            backward = true;
        }
        if(theManagerOne.tooStrong)
        {
            forward = true;
        }
        
        
        if (forward)
        {
            moveSpeedforward -= 3 * Time.deltaTime;
            myRigidbody.velocity = new Vector2(moveSpeedforward, myRigidbody.velocity.y);
            if (moveSpeedforward <= 0)
        {
            
            forward = false;
            moveSpeedforward = 0;
            if(moveSpeedforward == 0)
            {
                moveSpeedforward = myRigidbody.velocity.x;
            }
        }
        }
        if (backward)
        {
            myRigidbody.velocity = new Vector2(moveSpeedbackward, myRigidbody.velocity.y);
            moveSpeedbackward += 6 * Time.deltaTime;
            if (moveSpeedbackward >= 0)
            {
                moveSpeedbackward = 0;
                if(moveSpeedbackward == 0)
                {
                    moveSpeedbackward = myRigidbody.velocity.x;
                }
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
        yield return new WaitForSeconds(3);
        Destroy(stick.gameObject);
       
    }
    
}
