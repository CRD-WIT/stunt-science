using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class cameraman : MonoBehaviour
{
     private Animator myAnimator;
     public bool action;
     public Text timerAction;
     
     //public GameObject dialoguebox;
     public GameObject flash;
     public AudioSource actionvoice;
     public AudioSource three;
     public AudioSource two;
     public AudioSource one;
     
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        myAnimator.SetBool("action", action);
      
        
    }
    public void playAction()
    {
         StartCoroutine("dialogue");
    }
    IEnumerator dialogue()
    {
        //dialoguebox.SetActive(true);
        //timerAction.text = ("3");
        three.Play(0);
        flash.SetActive(true);
        yield return new WaitForSeconds(1);
        //timerAction.text = ("2");
        two.Play(0);
        yield return new WaitForSeconds(1);
        //timerAction.text = ("Action!");
        actionvoice.Play(0);
        yield return new WaitForSeconds(1);
        //dialoguebox.SetActive(false);
        flash.SetActive(false);
        action = false;
    }
}
