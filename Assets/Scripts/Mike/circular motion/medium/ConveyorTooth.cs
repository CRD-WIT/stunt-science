using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTooth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.parent.name == "UpperTooth")
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-ConveyorManager.conveyorSpeed, 0);
        else
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(ConveyorManager.conveyorSpeed, 0);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "UpperToothReset")
        {
            GameObject parent = GameObject.Find("UpperTooth");
            this.gameObject.transform.position = parent.transform.Find("UpperSpawnPoint").position;
        }
        else
        {
            GameObject parent = GameObject.Find("LowerTooth");
            this.gameObject.transform.position = parent.transform.Find("LowerSpawnPoint").position;
        }
    }
}
