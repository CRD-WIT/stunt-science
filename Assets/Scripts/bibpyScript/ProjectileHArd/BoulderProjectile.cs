using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderProjectile : MonoBehaviour
{
    public GameObject boulder, player;
    public float force,distanceX, distanceY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y,135);
        distanceX = transform.position.x - player.transform.position.x;
        distanceY =  player.transform.position.y - transform.position.y;
        force = ((Mathf.Sqrt((Mathf.Abs(Physics2D.gravity.y * 2) / (2 * ((distanceX * (Mathf.Tan((45) * Mathf.Deg2Rad))) - distanceY))))) * distanceX) / (Mathf.Cos((45) * Mathf.Deg2Rad));
    }
    public void boulderThrow()
    {
        boulder.SetActive(true);
        boulder.transform.position = transform.position;
        boulder.GetComponent<Rigidbody2D>().velocity = transform.right * force;
    }
}
