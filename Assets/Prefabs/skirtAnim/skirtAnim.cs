using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skirtAnim : MonoBehaviour
{
    public GameObject myPlayer;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", myPlayer.GetComponent<Rigidbody2D>().velocity.x);
        
    }
}
