using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Register: MonoBehaviour
{
    public GameObject pName;
    public Text popUp;
    public void StoreProfile(){  
        if(pName.GetComponent<Text>().text == ""){
            popUp.text = "You must Enter you name!!!";
            StartCoroutine(LateCall());
        }else if(RegistrationManager.playerGender == null){   
            popUp.text = "You must choose Gender!!!";
            StartCoroutine(LateCall());                       
        }else{
            RegistrationManager.playerName = pName.GetComponent<Text>().text;
            PlayerPrefs.SetString("Name", RegistrationManager.playerName);
            PlayerPrefs.SetString("Gender", RegistrationManager.playerGender);
            SceneManager.LoadScene("LevelSelection");
        }
    }
     IEnumerator LateCall()
     {
         popUp.gameObject.SetActive(true); 
         yield return new WaitForSeconds(3f); 
         popUp.gameObject.SetActive(false);
     }
}