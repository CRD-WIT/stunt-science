using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genderButton : MonoBehaviour
{
    public void Gender(){
        switch(this.gameObject.name){
            case "maleButton":
                regManager.playerGender = "Male";
                this.gameObject.transform.localScale = new Vector2(1.5f, 1.5f);
            break;
            case "femaleButton":
                regManager.playerGender = "Female";
                this.gameObject.transform.localScale = new Vector2(1.5f, 1.5f);
            break;
        }
    }
}
