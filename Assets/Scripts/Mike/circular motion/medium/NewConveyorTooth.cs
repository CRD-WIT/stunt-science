using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewConveyorTooth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (NewConveyorManager.conveyorSpeed == 0)
            Debug.Log("okay");
        else
        {
            if (this.gameObject.transform.parent.name == "UpperTooth")
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(
                    -NewConveyorManager.conveyorSpeed,
                    0
                );
            else
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(
                    NewConveyorManager.conveyorSpeed,
                    0
                );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "UpperToothReset")
        {
            Transform parent = this.gameObject.transform.parent;
            this.gameObject.transform.position = parent.transform.Find("UpperSpawnPoint").position;
        }
        else
        {
            Transform parent = this.gameObject.transform.parent;
            this.gameObject.transform.position = parent.transform.Find("LowerSpawnPoint").position;
        }
    }
}
