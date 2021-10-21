using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    public GameObject pName;
    public InputField pCode;
    public Text popUp;
    public WarningErrorUI warningErrorUI;
    public FirebaseManager firebaseManager;

    void test(){
        Debug.Log("Called test");
    }

    public async void StoreProfile()
    {
        firebaseManager.SetKeyCode(pCode.text);
        // Error checkup
        bool error = false;
        string errorMessages = "";
        if (pName.GetComponent<Text>().text.Length < 2)
        {
            error = true;
            errorMessages += "Invalid name. Please review your name.\n";
        }

        if (RegistrationManager.playerGender == null)
        {
            error = true;
            errorMessages += "Missing gender selection.\n";
        }

        if (pCode.text.Length < 3)
        {
            error = true;
            errorMessages += "Invalid code. Please check or ask your teacher.\n";
        }

        firebaseManager.CheckIfKeyCodeValid(test);

        if (error)
        {
            warningErrorUI.message = errorMessages;
            warningErrorUI.togglePanel();
        }
        else
        {
            PlayerPrefs.SetInt("Life", 3);
            RegistrationManager.playerName = pName.GetComponent<Text>().text;
            RegistrationManager.playerCode = pCode.GetComponent<InputField>().text;
            PlayerPrefs.SetString("Name", RegistrationManager.playerName);
            PlayerPrefs.SetString("IDCode", RegistrationManager.playerCode);
            PlayerPrefs.SetString("Gender", RegistrationManager.playerGender);
            SceneManager.LoadScene("LevelSelectV2");
        }
    }
    IEnumerator LateCall()
    {
        popUp.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        popUp.gameObject.SetActive(false);
    }
}