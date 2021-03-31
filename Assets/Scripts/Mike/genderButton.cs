using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenderButton : MonoBehaviour
{
    public void Gender(){
        switch(this.gameObject.name){
            case "maleButton":
                RegistrationManager.playerGender = "Male";
                this.gameObject.transform.localScale = new Vector2(1.5f, 1.5f);
            break;
            case "femaleButton":
                RegistrationManager.playerGender = "Female";
                this.gameObject.transform.localScale = new Vector2(1.5f, 1.5f);
            break;
        }
    }
}
