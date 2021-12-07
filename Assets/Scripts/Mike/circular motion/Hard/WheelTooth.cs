using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTooth : MonoBehaviour
{
    // Start is called before the first frame update
    MechaManager mm;
    float time, distanceT, distanceX, distanceY;
    void Start()
    {
        mm = FindObjectOfType<MechaManager>();
        if (this.transform.parent.name == "tooth1"){
            distanceX = 8.45f;
            distanceY = 2.24f;
        }
        else if(this.transform.parent.name == "tooth2"){
            distanceX = 1.429f;
            distanceY = 1.8f;
        }
        else{
            distanceX = 10;
            distanceY = 0;
        }
        distanceT = Mathf.Sqrt((distanceX*distanceX)+(distanceY*distanceY));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time = distanceT/mm.velocity.x;
        if(this.transform.parent.name == "tooth1")
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2((distanceX/time) + mm.velocity.x, distanceY/time);
        else if(this.transform.parent.name == "tooth2")
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2((distanceX/time) + mm.velocity.x, -distanceY/time);
        else
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2((distanceX/time) + mm.velocity.x,0);
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
        else if(other.transform.parent.name == "tooth1"){
            if(other.gameObject.name == "exit"){
                GameObject parent = GameObject.Find("tooth1");
                this.gameObject.transform.position = new Vector2(parent.transform.Find("entry").position.x - 0.1f, parent.transform.Find("entry").position.y -0.1f);
            }
            else{
                GameObject parent = GameObject.Find("tooth1");
                this.gameObject.transform.position = new Vector2(parent.transform.Find("exit").position.x + 0.1f, parent.transform.Find("exit").position.y+ 0.1f);
            }
        }
        else if(other.transform.parent.name == "tooth2"){
            if(other.gameObject.name == "exit"){
                GameObject parent = GameObject.Find("tooth2");
                this.gameObject.transform.position = new Vector2(parent.transform.Find("entry").position.x - 0.1f, parent.transform.Find("entry").position.y +0.1f);
            }
            else{
                GameObject parent = GameObject.Find("tooth2");
                this.gameObject.transform.position = new Vector2(parent.transform.Find("exit").position.x + 0.1f, parent.transform.Find("exit").position.y- 0.1f);
            }
        }
    }
}
