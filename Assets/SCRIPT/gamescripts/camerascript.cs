using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerascript : MonoBehaviour
{
     private Animator myAnimator;
    public Player thePlayer;

    private Vector3 lastPlayerPosition;
    private float distanceToMove;
    public bool camActive;
    public bool stuntScene;
    
    

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        thePlayer = FindObjectOfType<Player>();
        lastPlayerPosition = thePlayer.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    { 
       
           
        distanceToMove = thePlayer.transform.position.x - lastPlayerPosition.x;

        transform.position = new Vector3(transform.position.x + distanceToMove, transform.position.y, transform.position.z);

        lastPlayerPosition = thePlayer.transform.position;
        myAnimator.SetBool("camActive", camActive);
        myAnimator.SetBool("stuntScene", stuntScene);
       
    }
   
}


