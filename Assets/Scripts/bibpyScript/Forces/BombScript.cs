using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private float distanceToMoveX;
    private float distanceToMoveY;
    private ragdollForces theRagdoll;
    private Vector2 ragDollLastPOs;
    private BombManager theBomb;

    // Start is called before the first frame update
    void Start()
    {
        
        theBomb = FindObjectOfType<BombManager>();
        ragDollLastPOs = new Vector2(22, -0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        theRagdoll = FindObjectOfType<ragdollForces>();
        if (theBomb.followRagdoll == true)
        {
            transform.position = theRagdoll.transform.position;
            /*distanceToMoveX = theRagdoll.transform.position.x - ragDollLastPOs.x;
            distanceToMoveY = theRagdoll.transform.position.y - ragDollLastPOs.y;
            transform.position = new Vector2(transform.position.x + distanceToMoveX, transform.position.y + distanceToMoveY);
            ragDollLastPOs = theRagdoll.transform.position;*/
        }
    }
}
