using UnityEngine;

public class RunningPlayer : MonoBehaviour
{
    public bool isCollided;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Animator>().SetBool("grounded", true);
        transform.GetComponent<Animator>().SetFloat("speed", 6);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.tag == ("ShootPositionTrigger"))
        {
            isCollided = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(3, transform.GetComponent<Rigidbody2D>().velocity.y);        
    }





}
