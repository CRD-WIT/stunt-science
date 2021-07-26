using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectile : MonoBehaviour
{
     private Animator myAnimator;
     public bool shooting, airdive, slash;
     public GameObject puller,mGear, sword,arrow;
     public ProjectileHardOne theManagerOne;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         myAnimator.SetBool("shooting", shooting);
         myAnimator.SetBool("airdive", airdive);
        myAnimator.SetBool("slash", slash);
    }
    void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.tag == ("wall2"))
        {  
            StartCoroutine(slowMo());
            sword.SetActive(true);
            slash = true;
            theManagerOne.generateAngle += 10;
            theManagerOne.reShoot();
        }
    }
    IEnumerator slowMo()
    {
        Time.timeScale = .4f;
        yield return new WaitForSeconds(.7f);
        Time.timeScale = 1;
        arrow.GetComponent<LineRenderer>().enabled = false;
    }
}
