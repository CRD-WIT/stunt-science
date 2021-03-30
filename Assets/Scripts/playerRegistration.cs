using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerRegistration : MonoBehaviour
{
        //public Dropdown ;
        public GameObject pName, gender;          
        //public toggleSwitch TandC;
        public Text popUp;
        public Canvas profileSignUp, profile, home;
    private void Start() {
        //TandC = FindObjectOfType<toggleSwitch>();
    }
    public void StoreProfile(){  
        if(pName.GetComponent<Text>().text == ""){
            popUp.text = "You must Enter you name!!!";
            StartCoroutine(LateCall());
        }
        else if(toggleSwitch.goodToGo){   
            GameMAnager.playerName = pName.GetComponent<Text>().text;
            GameMAnager.playerGender = gender.GetComponent<Text>().text;
            SceneMan.pName = pName.GetComponent<Text>().text;
            SceneMan.gender = gender.GetComponent<Text>().text;  
        } 
        else{
            popUp.text = "You must accept the terms and conditions!!!";
            StartCoroutine(LateCall());
        }
    }
     IEnumerator LateCall()
     {
         popUp.gameObject.SetActive(true); 
         yield return new WaitForSeconds(3f); 
         popUp.gameObject.SetActive(false);
     }
}