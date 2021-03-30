using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class register: MonoBehaviour
{
        public GameObject pName;
        public Text popUp;

    public void StoreProfile(){  
        if(pName.GetComponent<Text>().text == ""){
            popUp.text = "You must Enter you name!!!";
            StartCoroutine(LateCall());
        }else if(regManager.playerGender == null){   
            popUp.text = "You must choose Gender!!!";
            StartCoroutine(LateCall());                       
        }else{
            regManager.playerName = pName.GetComponent<Text>().text;
            PlayerPrefs.SetString("Name", regManager.playerName);
        }
    }
     IEnumerator LateCall()
     {
         popUp.gameObject.SetActive(true); 
         yield return new WaitForSeconds(3f); 
         popUp.gameObject.SetActive(false);
     }
}