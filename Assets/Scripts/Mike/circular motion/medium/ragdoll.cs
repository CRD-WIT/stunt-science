using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdoll : MonoBehaviour
{
    Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // if(this.transform.parent.gameObject.activeSelf){
        //     this.gameObject.transform.position = startPos;
        // }
        
    }
    void OnWake(){
        this.gameObject.transform.position = startPos;
    }
}
