using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster : MonoBehaviour
{
    private Animator myAnimator;
     public bool attack;
    // Start is called before the first frame update
    void Start()
    {
         myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
         myAnimator.SetBool("attack", attack);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
      
        if (collision.gameObject.tag == ("Player"))
        {
            attack = false;
        }
    }
   
}
