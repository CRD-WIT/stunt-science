using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class regManager : MonoBehaviour
{
     public Button mButton, fButton;
     public static string playerName, playerGender;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerGender); 
        if(playerGender == "Male"){
            mButton.transform.localScale = new Vector3(1.2f, 1.2f,0);
            fButton.transform.localScale = new Vector2(1, 1);
        }else if(playerGender == "Female"){
            mButton.transform.localScale = new Vector2(1, 1);
            fButton.transform.localScale = new Vector2(1.2f, 1.2f);
        }
    }
}
