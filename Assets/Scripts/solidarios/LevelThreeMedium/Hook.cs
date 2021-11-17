using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public bool isCollided, isTrailing, getAngle;
    public bool isCollidedToFailCollider;
    public Rigidbody2D rb;
    public float RopePullAngle, ropePullVelocity;
    public GameObject[] rope;
    public playerProjectileMed thePlayer;
    public QuestionContProjMed theQuestion;
    public GameObject puller, hookLauncher,hit, missed ,indicator;
    public float correctAnswer;
    Quaternion lastRot;
    float mylastrotX,
           mylastrotY,
           angle,
           newAngle, time;
    int ropeNum;
    public AudioSource hitImpact,maneuverGearSfx,gearOxygenSfx;


    public GameObject target, hookLine, trail, wall,playerShadow;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        if (!getAngle)
        {
            angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            lastRot = transform.rotation;
        }
        if (isTrailing)
        {
            trail.transform.position = transform.position;
        }
        if (theQuestion.GetPlayerAnswer() < correctAnswer)
        {
            ropeNum = 1;
        }
        else
        {
            ropeNum = 0;
        }

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("HookTarget"))
        {
            if (!isCollided)
            {
                maneuverGearSfx.Stop();
                hitImpact.Play();
                time = 0;
                transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                transform.GetComponent<Rigidbody2D>().Sleep();
                getAngle = true;
                StartCoroutine(ropePull());
                hit.SetActive(true);
                indicator.transform.position = transform.position;
                isCollided = true;
            }


        }
         if (collision.gameObject.tag == ("ground"))
        {
            if (!isCollided)
            {
                maneuverGearSfx.Stop();
                hitImpact.Play();
                time = 3000;
                transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                transform.GetComponent<Rigidbody2D>().Sleep();
                getAngle = true;
               indicator.transform.position = transform.position;
                missed.SetActive(true);
               
                //StartCoroutine(ropePull());
                isCollided = true;
            }


        }

        if (collision.gameObject.tag == ("HookFailedCollider"))
        {
            if (isCollidedToFailCollider == false)
            {
                maneuverGearSfx.Stop();
                hitImpact.Play();
                wall.SetActive(false);
                time = 0;
                target.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(ropeFail());
                isCollidedToFailCollider = true;
                isTrailing = false;
                missed.SetActive(true);
               indicator.transform.position = transform.position;
                getAngle = true;
                isCollided = true;

            }

        }
        if (collision.gameObject.tag == ("wall2"))
        {
            if (isCollidedToFailCollider == false)
            {
                maneuverGearSfx.Stop();
                hitImpact.Play();
                time = .7f;
                wall.SetActive(false);
                target.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(ropeFail());
                isCollidedToFailCollider = true;
                isTrailing = false;
                getAngle = true;
                missed.SetActive(true);
                indicator.transform.position = transform.position;
                isCollided = true;

            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("wall"))
        {
            time = 0;
            hookLine.SetActive(false);
            rope[ropeNum].SetActive(true);
            isCollided = true;

            //StartCoroutine(ropeFail());
        }

    }
    public IEnumerator ropeFail()
    {
        yield return new WaitForSeconds(time);
        hookLine.SetActive(false);
        rope[ropeNum].SetActive(true);



        //rope.SetActive(false);


    }
    public IEnumerator ropePull()
    {
        yield return new WaitForSeconds(2f);
        gearOxygenSfx.Play();
        hookLauncher.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, RopePullAngle);
        puller.GetComponent<Rigidbody2D>().velocity = hookLauncher.transform.right * (ropePullVelocity);
        playerShadow.SetActive(true);
        thePlayer.aim = false;
        thePlayer.airdive = true;
        maneuverGearSfx.Play();


    }
}
