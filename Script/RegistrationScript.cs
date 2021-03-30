using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerProfile : MonoBehaviour
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
            GameMAnager.level = 1;
            GameMAnager.stage = 1;
            SceneMan.level = 1;
            SceneMan.stage = 1;
            PlayerPrefs.SetString("pageOut","1,"+GameMAnager.playerName+","+GameMAnager.playerGender+","+GameMAnager.level.ToString()+","+GameMAnager.stage.ToString()+","+GameMAnager.talentFee.ToString());         
            SceneManager.LoadScene("levelOneScene");         
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
