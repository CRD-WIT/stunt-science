using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollScript : MonoBehaviour
{
    public float moveSpeedforward;
    public float moveSpeedbackward;
    private Rigidbody2D myRigidbody;
    public GameObject stick;
    public GameObject stickloc;
     private SimulationManager theSimulation;
     private accSimulation theAccsimulate;
     private BikeManager theBike;
     bool forward;
     bool backward;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        theSimulation = FindObjectOfType<SimulationManager>();
        theBike = FindObjectOfType<BikeManager>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeedforward -= 2 * Time.deltaTime;
        moveSpeedbackward += 2 * Time.deltaTime;
        
        if (moveSpeedforward <= 0 || moveSpeedbackward >= 0)
        {
            moveSpeedforward = myRigidbody.velocity.x;
            moveSpeedbackward = myRigidbody.velocity.x;
        }
        if (SimulationManager.playerDead == true)
        {
            StartCoroutine(playerSpawn());
        }
        if ( accSimulation.playerDead == true)
        {
            StartCoroutine(driverSpawn());
        }
        if(theBike.stopBackward)
        {
            backward = true;
        }
        if(forward)
        {
            myRigidbody.velocity = new Vector2(moveSpeedforward, myRigidbody.velocity.y);
        }
        if(backward)
        {
            myRigidbody.velocity = new Vector2(moveSpeedbackward, myRigidbody.velocity.y);
        }
    }
    IEnumerator driverSpawn()
    {
        accSimulation.playerDead = false;
        yield return new WaitForSeconds(5);
        Destroy(stick.gameObject);
        theBike.driverPrefab.SetActive(true);
        theBike.driverPrefab.transform.position = stickloc.transform.position;
        
       
        
        

        
        //theplayer.gameObject.SetActive(true);
        //theplayer.transform.position = stick.transform.position;
    }
    IEnumerator playerSpawn()
    {
        SimulationManager.playerDead = false;
        yield return new WaitForSeconds(3);
        Destroy(stick.gameObject);
        theSimulation.thePlayer.gameObject.transform.position = stickloc.transform.position;
        theSimulation.thePlayer.gameObject.SetActive(true);

    }
}
