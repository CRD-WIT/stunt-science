using UnityEngine;

public class Arrow : MonoBehaviour
{
    public ProjectileHardOne theManagerOne;
    public ProjSimulationManager theSimulate;
    public Rigidbody2D rb;
    float mylastrotX, mylastrotY, angle, newAngle;
    public bool getAngle;
    Quaternion lastRot;
    public GameObject trail, line;
     public bool generateLine;
    // Start is called before the first frame update
    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!getAngle)
        {
            angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            lastRot = transform.rotation;
        }
        if (theSimulate.stage == 1)
        {
            if (theManagerOne.answerIsCorrect == true)
            {
                GetComponent<Collider2D>().isTrigger = true;
            }
        }
        if(generateLine)
        {
            trail.transform.position = this.transform.position;
        }



    }
    void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.tag == ("wall") || other.gameObject.tag == ("ground"))
        {
            getAngle = true;
            rb.bodyType = RigidbodyType2D.Static;
            transform.rotation = lastRot;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("boss") || collision.gameObject.tag == ("ground"))
        {
            generateLine = false;
            getAngle = true;
            line.SetActive(false);
        }

    }
}
