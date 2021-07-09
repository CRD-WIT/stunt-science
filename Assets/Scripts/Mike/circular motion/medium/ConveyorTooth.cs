using UnityEngine;
public class ConveyorTooth : MonoBehaviour
{
    Vector2 lowerSpeed, upperSpeed;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (MediumManager.stage == 1)
        {
            upperSpeed = new Vector2(-ConveyorManager.conveyorSpeed, 0);
            lowerSpeed = new Vector2(ConveyorManager.conveyorSpeed, 0);
        }
        else if (MediumManager.stage == 2)
        {
            upperSpeed = new Vector2(ConveyorManager.conveyorSpeed, 0);
            lowerSpeed = new Vector2(-ConveyorManager.conveyorSpeed * 0.9917f, -ConveyorManager.conveyorSpeed * 0.1285f);
        }
        if (this.gameObject.transform.parent.name == "UpperTooth")
            this.gameObject.GetComponent<Rigidbody2D>().velocity = upperSpeed;
        else
            this.gameObject.GetComponent<Rigidbody2D>().velocity = lowerSpeed;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        this.enabled = true;
        if (other.gameObject.name == "UpperToothReset")
        {
            GameObject parent = this.transform.parent.gameObject;
            Destroy(this.gameObject);
            GameObject tooth = Instantiate(this.gameObject);
            tooth.transform.position = parent.transform.Find("UpperSpawnPoint").position;
            tooth.GetComponent<Collider2D>().enabled = true;
            tooth.GetComponent<Rigidbody2D>().velocity = upperSpeed;
            tooth.transform.SetParent(parent.transform);
        }
        else
        {
            GameObject parent = this.transform.parent.gameObject;
            Destroy(this.gameObject);
            GameObject tooth = Instantiate(this.gameObject);
            tooth.transform.position = parent.transform.Find("LowerSpawnPoint").position;
            tooth.GetComponent<Collider2D>().enabled = true;
            tooth.GetComponent<Rigidbody2D>().velocity = lowerSpeed;
            tooth.transform.SetParent(parent.transform);
        }
    }
}
