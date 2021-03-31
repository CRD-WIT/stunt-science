using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenderButton : MonoBehaviour
{
    public void Gender(){
        switch(this.gameObject.name){
            case "maleButton":
                RegistrationManager.playerGender = "Male";
            break;
            case "femaleButton":
                RegistrationManager.playerGender = "Female";
            break;
        }
    }
}
