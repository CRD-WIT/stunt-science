using UnityEngine;
public class ConveyorTooth : MonoBehaviour
{
    // Vector2 lowerSpeed, upperSpeed;
    // string parentName;
    // Start is called before the first frame update
    void Start()
    {
        // parentName = this.transform.parent.parent.gameObject.name;
        // this.enabled =true;
        
        // if (parentName == "Conveyor(Clone)")
        // {
        //     upperSpeed = new Vector2(-ConveyorManager.conveyorSpeed, 0);
        //     lowerSpeed = new Vector2(ConveyorManager.conveyorSpeed, 0);
        // }
        // else
        // {
        //     upperSpeed = new Vector2(ConveyorManager.conveyorSpeed, 0);
        //     lowerSpeed = new Vector2(-ConveyorManager.conveyorSpeed * 0.9917f, -ConveyorManager.conveyorSpeed * 0.1285f);
        // }
    }
    // Update is called once per frame
    void Update()
    {
        // // Debug.Log(parentName);
        // if (this.transform.parent.name == "UpperTooth")
        //     this.gameObject.GetComponent<Rigidbody2D>().velocity = upperSpeed;
        // else
        //     this.gameObject.GetComponent<Rigidbody2D>().velocity = lowerSpeed;
        
        if (this.transform.parent.name == "UpperTooth")
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-ConveyorManager.conveyorSpeed, 0);
        else
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(ConveyorManager.conveyorSpeed, 0);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "UpperToothReset")
        {
            GameObject parent = this.transform.parent.gameObject;
            // Destroy(this.gameObject);
            // GameObject tooth = Instantiate(this.gameObject);
            // tooth.transform.position = parent.transform.Find("UpperSpawnPoint").position;
            // tooth.GetComponent<Collider2D>().enabled = true;
            // tooth.GetComponent<Rigidbody2D>().velocity = new Vector2(-ConveyorManager.conveyorSpeed, 0);
            // tooth.transform.SetParent(parent.transform);
            this.gameObject.transform.position = parent.transform.Find("UpperSpawnPoint").position;
        }
        else
        {
            GameObject parent = this.transform.parent.gameObject;
            // Destroy(this.gameObject);
            // GameObject tooth = Instantiate(this.gameObject);
            // tooth.transform.position = parent.transform.Find("LowerSpawnPoint").position;
            // tooth.GetComponent<Collider2D>().enabled = true;
            // tooth.GetComponent<Rigidbody2D>().velocity = new Vector2(ConveyorManager.conveyorSpeed, 0);
            // tooth.transform.SetParent(parent.transform);
            this.gameObject.transform.position = parent.transform.Find("LowerSpawnPoint").position;
        }
    }
}
