using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollAccMid : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    public float moveSpeedforward, rotateSpeed;
    public float moveSpeedupward;
    bool doRotate = true;
    private AccMidSimulation theSimulate;
    public GameObject stick, stickloc;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        theSimulate = FindObjectOfType<AccMidSimulation>();
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeedforward -= 2 * Time.deltaTime;
        //moveSpeedupward -= 3 * Time.deltaTime;
        myRigidbody.velocity = new Vector2(moveSpeedforward, myRigidbody.velocity.y);
        myRigidbody.rotation -= rotateSpeed;
        if (doRotate)
        {
            StartCoroutine(rotating());
        }

        if (moveSpeedforward <= 0)
        {
            moveSpeedforward = 0;
            myRigidbody.rotation = 0;
            
        }
        if (moveSpeedupward <= 0)
        {
            //moveSpeedupward = 0;
            //moveSpeedupward = myRigidbody.velocity.y;                
        }
        if (AccMidSimulation.playerDead == true)
            {
                StartCoroutine(playerSpawn());
            }
    }
    IEnumerator rotating()
    {
        doRotate = false;
        yield return new WaitForSeconds(1f);
        rotateSpeed = 20;
        yield return new WaitForSeconds(1f);
        rotateSpeed = 0;
    }
    IEnumerator playerSpawn()
    {
        AccMidSimulation.playerDead = false;
        if (theSimulate.stage == 1)
        {
            yield return new WaitForSeconds(3f);
        }
        if (theSimulate.stage == 2)
        {
            yield return new WaitForSeconds(4);
        }
        if (theSimulate.stage == 3)
        {
            yield return new WaitForSeconds(2.5f);
        }
        theSimulate.thePlayer.gameObject.SetActive(true);
        theSimulate.thePlayer.transform.position = stickloc.transform.position;
        Destroy(stick.gameObject);

    }
}
