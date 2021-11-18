using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlFiveHardManager : MonoBehaviour
{
    private HingeJoint2D grabJoint;
    public Rigidbody2D hangingPlayer;
    bool hanging= false;
    
    // Start is called before the first frame update
    void Start()
    {
        grabJoint = GetComponent<HingeJoint2D>();
        grabJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hanging){
            this.grabJoint.enabled =true;

        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("okay");
        if(other.gameObject.tag == "Player"){
            hangingPlayer.velocity = new Vector2(0,0);
            hangingPlayer.gravityScale =0;
            hanging = true;
            hangingPlayer.gameObject.GetComponent<Animator>().SetBool("dive", false);
        }
    }
}
