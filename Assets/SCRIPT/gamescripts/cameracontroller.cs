using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cameracontroller : MonoBehaviour
{
     private Animator myAnimator;
    public Player thePlayer;

    private Vector3 lastPlayerPosition;
    private float distanceToMove;
    public bool playercam; 
    public int actioncam;
    public bool cameraAnim;
    bool selectedchar;
    

    // Start is called before the first frame update
    void Start()
    {
         myAnimator = GetComponent<Animator>();
        thePlayer = FindObjectOfType<Player>();
        lastPlayerPosition = thePlayer.transform.position;
        selectedchar = true;
    }

    // Update is called once per frame
    void Update()
    { 
        if (selectedchar == true)
        {
            thePlayer = FindObjectOfType<Player>();
            selectedchar = false;
        }
        if (playercam == true)
        {
           
           
            distanceToMove = thePlayer.transform.position.x - lastPlayerPosition.x;

            transform.position = new Vector3(transform.position.x + distanceToMove, transform.position.y, transform.position.z);

            lastPlayerPosition = thePlayer.transform.position;
        }
        if (actioncam > 0)
        {
         myAnimator.SetInteger("actioncam", actioncam);
        }
        if (actioncam == 3)
        {
             gameObject.GetComponent<Animator>().enabled = false;
        }
         if (actioncam < 3)
        {
             gameObject.GetComponent<Animator>().enabled = true;
        }
    }
    public void Action()
    {
        StartCoroutine(cameraAction());
    }
    IEnumerator cameraAction()
    {
        actioncam = 2;
        yield return new WaitForSeconds(1);
        actioncam = 3;
    }
}


