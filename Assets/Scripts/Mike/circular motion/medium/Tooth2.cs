using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooth2 : MonoBehaviour
{
    void Update()
    {
        if (this.transform.parent.name == "UpperTooth")
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-ConveyorManager.conveyorSpeed, 0);
        else
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(ConveyorManager.conveyorSpeed * 0.9925f, ConveyorManager.conveyorSpeed * 0.1225f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "UpperToothReset")
        {
            GameObject parent = this.transform.parent.gameObject;
            this.gameObject.transform.position = parent.transform.Find("UpperSpawnPoint").position;
            // Destroy(this.gameObject);
            // GameObject tooth = Instantiate(this.gameObject);
            // tooth.transform.position = parent.transform.Find("UpperSpawnPoint").position;
            // tooth.GetComponent<Collider2D>().enabled = true;
            // tooth.GetComponent<Rigidbody2D>().velocity = new Vector2(-ConveyorManager.conveyorSpeed, 0);
            // tooth.transform.SetParent(parent.transform);
        }
        else
        {
            GameObject parent = this.transform.parent.gameObject;
            this.transform.position = parent.transform.Find("LowerSpawnPoint").position;
            // Destroy(this.gameObject);
            // GameObject tooth = Instantiate(this.gameObject);
            // tooth.transform.Rotate(0,0,7);
            // tooth.transform.position = parent.transform.Find("LowerSpawnPoint").position;
            // tooth.GetComponent<Collider2D>().enabled = true;
            // tooth.GetComponent<Rigidbody2D>().velocity = new Vector2(ConveyorManager.conveyorSpeed * 0.9925f, ConveyorManager.conveyorSpeed * 0.1225f);
            // tooth.transform.SetParent(parent.transform);
        }
    }
}
