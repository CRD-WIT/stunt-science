using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSuv : MonoBehaviour
{
    private Animator myAnimator;
    public bool fade;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        myAnimator.SetBool("fade", fade);
         if(fade == true)
        {
           StartCoroutine(fadeout());
        }
    }
    IEnumerator fadeout()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        fade = false;
    }
}
