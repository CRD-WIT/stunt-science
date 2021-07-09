using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooth2 : MonoBehaviour
{
    public float speed;
    void Update()
    {
        speed = ConveyorManager.conveyorSpeed* 0.865f;
        if (this.gameObject.transform.parent.name == "UpperTooth")
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);//*1.005f
        else
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed*0.953f, -speed * 0.31f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "UpperToothReset")
        {
            GameObject parent = this.transform.parent.gameObject;
            Destroy(this.gameObject);
            GameObject tooth = Instantiate(this.gameObject);
            tooth.transform.position = parent.transform.Find("UpperSpawnPoint").position;
            tooth.GetComponent<Collider2D>().enabled = true;
            tooth.GetComponent<Rigidbody2D>().velocity = new Vector2(speed*1.005f, 0);
            tooth.transform.SetParent(parent.transform);
        }
        else
        {
            GameObject parent = this.transform.parent.gameObject;
            Destroy(this.gameObject);
            GameObject tooth = Instantiate(this.gameObject);
            tooth.transform.Rotate(0,0,18.25f);
            tooth.transform.position = parent.transform.Find("LowerSpawnPoint").position;
            tooth.GetComponent<Collider2D>().enabled = true;
            tooth.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed*0.953f, -speed * 0.31f);
            tooth.transform.SetParent(parent.transform);
        }
    }
}
