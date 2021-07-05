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
            GameObject parent = this.transform.parent.gameObject;
            Destroy(this.gameObject);
            GameObject tooth = Instantiate(this.gameObject);
            tooth.transform.position = parent.transform.Find("UpperSpawnPoint").position;
            tooth.GetComponent<Collider2D>().enabled = true;
            tooth.GetComponent<Rigidbody2D>().velocity = new Vector2(-ConveyorManager.conveyorSpeed, 0);
            tooth.transform.SetParent(parent.transform);
        }
        else
        {
            GameObject parent = this.transform.parent.gameObject;
            Destroy(this.gameObject);
            GameObject tooth = Instantiate(this.gameObject);
            tooth.transform.position = parent.transform.Find("LowerSpawnPoint").position;
            tooth.GetComponent<Collider2D>().enabled = true;
            tooth.GetComponent<Rigidbody2D>().velocity = new Vector2(ConveyorManager.conveyorSpeed, 0);
            tooth.transform.SetParent(parent.transform);
        }
    }
}
