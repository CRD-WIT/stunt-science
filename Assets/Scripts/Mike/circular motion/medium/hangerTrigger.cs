using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hangerTrigger : MonoBehaviour
{
    public Transform hanger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.transform.parent.name == "Wheel1"){
            other.gameObject.GetComponent< HingeJoint2D>().enabled = true;
            // TODO: For fixing
            // hanger.SetParent(other.gameObject);
            this.gameObject.SetActive(true);
        }
    }
}
