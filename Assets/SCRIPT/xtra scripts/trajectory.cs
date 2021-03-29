using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trajectory : MonoBehaviour
{
    public GameObject Arrow;
    public float speed;
    public float weight;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shoot()
    {
        GameObject ArrowIns = Instantiate(Arrow,transform.position, transform.rotation);
        weight = ArrowIns.GetComponent<Rigidbody2D>().mass;
        ArrowIns.GetComponent<Rigidbody2D>().velocity = transform.right * speed / weight;

    }
}
