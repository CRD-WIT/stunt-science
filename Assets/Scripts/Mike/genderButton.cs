using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genderButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void Gender(){
        switch(this.gameObject.name){
            case "maleButton":
                regManager.playerGender = "Male";
                PlayerPrefs.SetString("Name", regManager.playerGender);
                this.gameObject.transform.localScale = new Vector2(1.5f, 1.5f);
                
            break;
            case "femaleButton":
                regManager.playerGender = "Female";
                PlayerPrefs.SetString("Name", regManager.playerGender);
                this.gameObject.transform.localScale = new Vector2(1.5f, 1.5f);
            break;
        }
    }
}
