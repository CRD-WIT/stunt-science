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
            Destroy(this.gameObject);
            GameObject tooth = Instantiate(this.gameObject);
            tooth.transform.position = new Vector2(11.239f, 0.96f);
            tooth.GetComponent<Collider2D>().enabled = true;
            tooth.GetComponent<Rigidbody2D>().velocity = new Vector2(-ConveyorManager.conveyorSpeed, 0);
            tooth.transform.SetParent(parent.transform);
        }
        else
        {
            GameObject parent = GameObject.Find("LowerTooth");
            Destroy(this.gameObject);
            GameObject tooth = Instantiate(this.gameObject);
            tooth.transform.position = new Vector2(-11.1318f, -0.92f);
            tooth.GetComponent<Collider2D>().enabled = true;
            tooth.GetComponent<Rigidbody2D>().velocity = new Vector2(ConveyorManager.conveyorSpeed, 0);
            tooth.transform.SetParent(parent.transform);
        }
    }
}
