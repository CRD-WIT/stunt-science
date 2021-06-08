using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollAccMid : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    public float moveSpeedforward, rotateSpeed, moveSpeedbackward;
    public float moveSpeedupward;

    private AccMidSimulation theSimulate;
    public GameObject stick, stickloc;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        theSimulate = FindObjectOfType<AccMidSimulation>();

        if (theSimulate.stage == 1)
        {
            moveSpeedforward -= 2 * Time.deltaTime;
            //moveSpeedupward -= 3 * Time.deltaTime;
            myRigidbody.velocity = new Vector2(moveSpeedforward, myRigidbody.velocity.y);

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
        if (theSimulate.stage == 2)
        {
            if (AccMidSimulation.playerDead == true)
            {
                StartCoroutine(playerSpawn());
            }

        }
        if (theSimulate.stage == 3)
        {
            myRigidbody.velocity = new Vector2(moveSpeedbackward, myRigidbody.velocity.y);
            moveSpeedbackward += 10 * Time.deltaTime;
            if (moveSpeedbackward >= 0)
            {
                moveSpeedbackward = 0;
            }
        }
        if (theSimulate.playButton.interactable == true)
        {
            Destroy(stick.gameObject);
        }
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
            yield return new WaitForSeconds(2);
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
