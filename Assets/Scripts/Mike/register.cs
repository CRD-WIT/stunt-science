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
        }else if(termsAndCons.goodToGo){   
            regManager.playerName = pName.GetComponent<Text>().text;
            PlayerPrefs.SetString("Name", regManager.playerName);           
        }else{
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

/*public Toggle TandC;
    public static bool goodToGo;
    public bool check;
    // Start is called before the first frame update
    void Start()
    {
        TandC = GetComponent<Toggle>();
    }
    private void Update() {
        termAndCon();
        check = goodToGo;
    }
    public void termAndCon(){
        if(TandC.isOn){
            goodToGo = true;
        }
        else{
            goodToGo = false;
        }
    }*/