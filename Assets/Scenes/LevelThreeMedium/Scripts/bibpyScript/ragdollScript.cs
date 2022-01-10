using System.Collections;
using UnityEngine;

public class ragdollScript : MonoBehaviour
{
    public float moveSpeedforward;
    public float moveSpeedbackward;
    public float moveSpeedupward;
    private Rigidbody2D myRigidbody;
    public GameObject stick;
    public GameObject stickloc;
       bool forward = true;
    bool backward;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
       

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
        
        
        if (forward)
        {
            moveSpeedforward -= 2 * Time.deltaTime;
            moveSpeedupward -= 2 * Time.deltaTime;
            myRigidbody.velocity = new Vector2(moveSpeedforward, moveSpeedupward);
            if (moveSpeedforward <= 0)
        {
            moveSpeedforward = myRigidbody.velocity.x;
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
    }
    IEnumerator driverSpawn()
    {
        accSimulation.playerDead = false;
        yield return new WaitForSeconds(5);
        Destroy(stick.gameObject);
       




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
