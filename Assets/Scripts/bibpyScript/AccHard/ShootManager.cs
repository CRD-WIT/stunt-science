using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
    public GameObject bullet, blastprefab, bulletPos;
    public float speed;
    
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
        GameObject ArrowIns = Instantiate(bullet, transform.position, transform.rotation);
        ArrowIns.GetComponent<Rigidbody2D>().velocity = transform.right * speed;


        GameObject explosion = Instantiate(blastprefab);
        explosion.transform.position = transform.position;


    }
   
}