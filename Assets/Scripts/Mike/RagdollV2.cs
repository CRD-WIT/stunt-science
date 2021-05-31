using UnityEngine;

public class RagdollV2 : MonoBehaviour
{
    public float moveSpeedforward;
    private Rigidbody2D myRigidbody;
    public GameObject stick;
    public GameObject stickloc;
    private SimulationManager theSimulation;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        theSimulation = FindObjectOfType<SimulationManager>();
    }

    void Update()
    {
        moveSpeedforward -= 2 * Time.deltaTime;
        myRigidbody.velocity = new Vector2(moveSpeedforward, myRigidbody.velocity.y);
        if (moveSpeedforward <= 0)
        {
            moveSpeedforward = 0;
        }
        // if (SimulationManager.playerDead == true)
        // {
        //     SimulationManager.isRagdollActive = true;
        //     StartCoroutine(playerSpawn());
        // }
    }
    // IEnumerator playerSpawn()
    // {
    //     SimulationManager.playerDead = false;
    //     yield return new WaitForSeconds(3);
    //     Destroy(stick.gameObject);
    //     theSimulation.thePlayer.gameObject.transform.position = stickloc.transform.position;
    //     theSimulation.thePlayer.gameObject.SetActive(true);
    // }
    
}