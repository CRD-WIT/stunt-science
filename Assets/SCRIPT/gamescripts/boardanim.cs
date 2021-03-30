using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boardanim : MonoBehaviour
{
    public int transit;
     private Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        myAnimator.SetInteger("transition", transit);
    }
}
