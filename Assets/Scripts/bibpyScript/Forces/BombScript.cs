using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private float distanceToMoveX;
    private Player thePlayer;
    private float distanceToMoveY;
    private ragdollForces theRagdoll;
    private Vector2 ragDollLastPOs;
    private BombManager theBomb;
    public GameObject bombPos;
    public bool inPlayer;
   

    // Start is called before the first frame update
    void Start()
    {
        
        
        
        ragDollLastPOs = new Vector2(22, -0.6f);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        thePlayer = FindObjectOfType<Player>();
        theBomb = FindObjectOfType<BombManager>();
        theRagdoll = FindObjectOfType<ragdollForces>();
        if (inPlayer)
        {
            transform.position = bombPos.transform.position;
            transform.rotation = bombPos.transform.rotation;
        }
        if (theBomb.followRagdoll == true)
        {
            transform.position = theRagdoll.transform.position;
            inPlayer = false;
            /*distanceToMoveX = theRagdoll.transform.position.x - ragDollLastPOs.x;
            distanceToMoveY = theRagdoll.transform.position.y - ragDollLastPOs.y;
            transform.position = new Vector2(transform.position.x + distanceToMoveX, transform.position.y + distanceToMoveY);
            ragDollLastPOs = theRagdoll.transform.position;*/
        }
    }
}
