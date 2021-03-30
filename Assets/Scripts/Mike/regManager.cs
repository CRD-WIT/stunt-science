using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class regManager : MonoBehaviour
{
     public Button mButton, fButton;
     public static string playerName, playerGender;
     public Animator myAnim;
     public bool male;
 
    void Update()
    {
        myAnim.SetBool("maleSelected", male);
        Debug.Log(male);
        Debug.Log(playerGender); 
        if(playerGender == "Male"){
            mButton.transform.localScale = new Vector3(1.2f, 1.2f,0);
            fButton.transform.localScale = new Vector2(1, 1);
            male=true;
        }else if(playerGender == "Female"){
            mButton.transform.localScale = new Vector2(1, 1);
            fButton.transform.localScale = new Vector2(1.2f, 1.2f);
            male=false;
        }
    }
}
