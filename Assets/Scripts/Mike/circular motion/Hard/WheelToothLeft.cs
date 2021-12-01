using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelToothLeft : MonoBehaviour
{
    [SerializeField]EdgeCollider2D entry, exit;
    MechaManager mm;
    float forwardVelocityCatcher, velocity;
    // Start is called before the first frame update
    void Start()
    {
        mm = FindObjectOfType<MechaManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.transform.parent.name == "tooth"){
            if(other.gameObject.name == "exit"){
                GameObject parent = GameObject.Find("tooth");
                this.gameObject.transform.position = new Vector2(parent.transform.Find("entry").position.x + 0.1f, parent.transform.Find("entry").position.y);
            }
            else{
                GameObject parent = GameObject.Find("tooth");
                this.gameObject.transform.position = new Vector2(parent.transform.Find("exit").position.x - 0.1f, parent.transform.Find("exit").position.y);
            }
        }
    }
}
