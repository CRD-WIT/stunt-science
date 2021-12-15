using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BckGndManager : MonoBehaviour
{
    MechaManager mm;
    [SerializeField] Transform loopPnt;
    // Start is called before the first frame update
    void Start()
    {
        mm = FindObjectOfType<MechaManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(LvlFiveHardManager.stage != 1){
            this.gameObject.GetComponent<Rigidbody2D>().velocity = -mm.velocity;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "BckgndReset"){
            this.gameObject.transform.position = loopPnt.position;
        }
    }
}
