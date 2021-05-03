using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccMediumOne : MonoBehaviour
{
    public GameObject hangingRagdoll, ropeTip, playerInTruck, ragdollPrefab, stickmanPoint;
    public Player thePlayer;
    public Hellicopter theChopper;
    public TruckManager theTruck;
    public bool chase;
    public float timer, correctAnswer;
    public float velocity, accelaration;
    public float chopperPos, truckPos, answer;
    public bool readyToJump;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        truckPos = theTruck.transform.position.x;
        chopperPos = theChopper.transform.position.x;
        correctAnswer = (2 * velocity) / accelaration;
        hangingRagdoll.transform.position = ropeTip.transform.position;
        if (chase)
        {
            theTruck.accelerating = true;
            theTruck.accelaration = accelaration;
            theChopper.flySpeed = velocity;

            thePlayer.transform.position = playerInTruck.transform.position;
            timer += Time.fixedDeltaTime;
            if (timer >= answer - .8f)
            {
                if (readyToJump)
                {

                    StartCoroutine(jump());
                }
            }
        }
    }
    IEnumerator jump()
    {
        readyToJump = false;
        thePlayer.toJump = true;
        yield return new WaitForSeconds(0.8f);
        thePlayer.toJump = false;
        thePlayer.gameObject.SetActive(false);
        if (answer == correctAnswer)
        {
            hangingRagdoll.SetActive(true);
        }
        if (answer != correctAnswer)
        {
            ragdollSpawn();
        }

    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(ragdollPrefab);
        stick.transform.position = stickmanPoint.transform.position;
    }
        
}
