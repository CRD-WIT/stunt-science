using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelarationRagdoll : MonoBehaviour
{
    public float moveSpeedforward;
    public float moveSpeedbackward;
    public float moveSpeedupward;
    private Rigidbody2D myRigidbody;
    public GameObject stick;
    public GameObject stickloc;
    private SimulationManager theSimulation;
    private accSimulation theAccsimulate;
    private BikeManager theBike;
    bool forward = false;
    bool backward;
    bool respawn = true;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        theSimulation = FindObjectOfType<SimulationManager>();
        theBike = FindObjectOfType<BikeManager>();
        theAccsimulate = FindObjectOfType<accSimulation>();


    }

    // Update is called once per frame
    void Update()
    {
        if (SimulationManager.playerDead == true)
        {
            StartCoroutine(playerSpawn());
        }
        if (accSimulation.playerDead == true)
        {
            StartCoroutine(driverSpawn());
        }
        if (theBike.stopBackward)
        {
            backward = true;
            forward = false;
        }

        if (forward)
        {
            moveSpeedforward -= 3 * Time.deltaTime;
            moveSpeedupward -= 6 * Time.deltaTime;
            myRigidbody.velocity = new Vector2(moveSpeedforward, moveSpeedupward);
            if (moveSpeedupward <= 0)
            {
                moveSpeedforward = 0;
                moveSpeedupward = 0;
                moveSpeedupward = myRigidbody.velocity.y;
                moveSpeedforward = myRigidbody.velocity.x;
                forward = false;
            }
        }
        if (backward)
        {
            myRigidbody.velocity = new Vector2(moveSpeedbackward, myRigidbody.velocity.y);
            moveSpeedbackward += 6 * Time.deltaTime;
            if (moveSpeedbackward >= 0)
            {
                moveSpeedbackward = myRigidbody.velocity.x;
            }
        }
        if (theAccsimulate.playButton.interactable == true)
        {
            Destroy(stick.gameObject);
        }

    }
    IEnumerator driverSpawn()
    {
        accSimulation.playerDead = false;
        yield return new WaitForSeconds(3);
        if (respawn)
        {
            Destroy(stick.gameObject);
            theBike.driverPrefab.SetActive(true);
            theBike.driverPrefab.transform.position = stickloc.transform.position;
        }






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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("water"))
        {
            respawn = false;
        }
    }
}


