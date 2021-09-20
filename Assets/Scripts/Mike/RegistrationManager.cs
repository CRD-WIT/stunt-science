using UnityEngine;
using UnityEngine.UI;

public class RegistrationManager : MonoBehaviour
{
    public Settings settingsUI;
    public Button mButton, fButton;
    public static string playerName, playerGender;
    public Animator maleAnimator;
    public Animator femaleAnimator;
    public bool male;

    void Start(){
        settingsUI.ResetSettings();
    }

    void Update()
    {

        if (playerGender == "Male")
        {
            mButton.transform.localScale = new Vector3(1.2f, 1.2f, 0);
            fButton.transform.localScale = new Vector2(1, 1);
            maleAnimator.SetBool("maleGenderSelected", true);
            femaleAnimator.SetBool("maleGenderSelected", false);
        }
        else if (playerGender == "Female")
        {
            mButton.transform.localScale = new Vector2(1, 1);
            fButton.transform.localScale = new Vector2(1.2f, 1.2f);
            femaleAnimator.SetBool("maleGenderSelected", true);
            maleAnimator.SetBool("maleGenderSelected", false);
        }
    }
}
