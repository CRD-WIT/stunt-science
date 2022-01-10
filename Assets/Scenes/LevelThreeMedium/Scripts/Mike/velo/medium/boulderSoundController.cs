using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boulderSoundController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(this.gameObject.GetComponent<Rigidbody2D>().velocity.x) > 1){
            this.gameObject.GetComponent<AudioSource>().enabled =true;
        }else
            this.gameObject.GetComponent<AudioSource>().enabled =false;
    }
}
